using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public enum GameMultiType
    {
        PvE,
        PvP,
    }

    public enum DifficultyLevel
    {
        Easy,
        Hard,
    }

    public static GameMultiType GameType { get; set; }

    public static DifficultyLevel Difficulty { get; set; }

    public Queue<(Brick, int)> PlayerBricks { get => _playerBricks; }
    public Queue<(Brick, int)> EnemyBricks { get => _enemyBricks; }

    private readonly int ShowBrickCount = 5;

    public bool IsEndGame { get; private set; }

    public PlayerController player;

    public EnemyController enemy;

    public BrickManager brickManager;

    public Transform cameraShakeTran;

    public List<Brick> ShowBrickList { get; private set; }

    public float totalPlayingTime;

    private float _currTime;

    private StageUi _stageUi;

    private Queue<(Brick, int)> _playerBricks;
    private Queue<(Brick, int)> _enemyBricks;

    private void Awake()
    {
        Init();
        SoundManager.Instance.PlayBGM(SoundManager.BGM.Main, true);
    }

    private void Init()
    {
        IsEndGame = false;

        _currTime = 0;

        ShowBrickList = new List<Brick>();

        _stageUi = FindObjectOfType<StageUi>();

        _stageUi.Init(this);

        brickManager.Init();

        _playerBricks = new Queue<(Brick, int)>();
        _enemyBricks = new Queue<(Brick, int)>();

        player.Init(this);

        enemy.Init(this);

        enemy.SetDifficulty(Difficulty);

        for (int i = 0; i < ShowBrickCount; i++)
        {
            _playerBricks.Enqueue(BrickManager.Instance.GetBrick());
            _enemyBricks.Enqueue(BrickManager.Instance.GetBrick());
        }

        _stageUi.RefreshPlayerBrickQueue();
        _stageUi.RefreshEnemyBrickQueue();

        StartCoroutine(CoSetPlayingTimeProgress());
    }

    private IEnumerator CoSetPlayingTimeProgress()
    {
        var delay = new WaitForSeconds(1.0f);

        while(_currTime < totalPlayingTime)
        {
            _stageUi.SetPlayingTimeProgress(_currTime, totalPlayingTime);

            yield return delay;

            _currTime += 1;
        }

        _stageUi.SetPlayingTimeProgress(_currTime, totalPlayingTime);

        EndGame();
    }

    private void EndGame()
    {
        IsEndGame = true;

        StartCoroutine(CoEndGame());
    }

    private IEnumerator CoEndGame()
    {
        var wantedTime = 3.0f;

        var delay = new WaitForSeconds(wantedTime / ShowBrickList.Count);

        //FX다른데 달려 있으면 회수해오기
        FXManager.Instance.CollectAllFXs();

        foreach (var brick in ShowBrickList)
            brick.rigidbody.isKinematic = true;

        var myAreaBrickCnt = 0;
        var enemyAreaBrickCnt = 0;

        _stageUi.ShowBrickCnt();

        var cam = Camera.main;

        for (int i = ShowBrickList.Count - 1; i >= 0; i--)
        {
            var brick = ShowBrickList[i];

            brick.gameObject.SetActive(false);

            var viewPortPoint = cam.WorldToViewportPoint(brick.transform.position);

            var soundSource = SoundManager.Instance.PlaySFX(SoundManager.SFX.Score, false);

            if (viewPortPoint.x < 0.5f)
                myAreaBrickCnt++;
            else
                enemyAreaBrickCnt++;

            _stageUi.SetBrickCnt(myAreaBrickCnt, enemyAreaBrickCnt);

            yield return delay;
        }

        _stageUi.ShowResult(myAreaBrickCnt, enemyAreaBrickCnt);
    }

    public (Brick, int) GetPlayerBrick()
    {
        var brick = _playerBricks.Dequeue();

        _playerBricks.Enqueue(BrickManager.Instance.GetBrick());

        _stageUi.RefreshPlayerBrickQueue();

        return brick;
    }

    public (Brick, int) GetEnemyBrick()
    {
        var brick = _enemyBricks.Dequeue();

        _enemyBricks.Enqueue(BrickManager.Instance.GetBrick());

        _stageUi.RefreshEnemyBrickQueue();

        return brick;
    }

    //시작 = 0,   끝 = 1
    public float GetRemainTimeRatio()
    {
        float tempRatio = _currTime / totalPlayingTime;
        return tempRatio;
    }

    private Coroutine _coShakeCamera;

    public void ShakeCamera()
    {
        if (_coShakeCamera  != null)
            return;

        _coShakeCamera = StartCoroutine(CoShakeCamera(0.3f, 0.4f));
    }

    private IEnumerator CoShakeCamera(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector2 shakePos = Random.insideUnitCircle * magnitude;

            cameraShakeTran.localPosition = shakePos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraShakeTran.localPosition = Vector3.zero;

        _coShakeCamera = null;
    }

    void Update()
    {
        if (IsEndGame)
            return;

        player.OnUpdate();
        enemy.OnUpdate();
    }
}
