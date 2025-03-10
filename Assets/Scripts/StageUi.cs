using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

using TMPro;
using UnityEngine.SceneManagement;

public class StageUi : MonoBehaviour
{
    public GameObject brickIconPref;

    public TextMeshProUGUI progressTime;
    public TextMeshProUGUI myAreaBrickCnt;
    public TextMeshProUGUI enemyAreaBrickCnt;

    public GridLayoutGroup playerBrickGrid;
    public GridLayoutGroup enemyBrickGrid;

    public Image winImg;
    public Image loseImg;
    public Image drawImg;
    public Image exitImg;

    private StageManager _manager;

    public void Init(StageManager manager)
    {
        _manager = manager;

        myAreaBrickCnt.gameObject.SetActive(false);
        enemyAreaBrickCnt.gameObject.SetActive(false);

        winImg.gameObject.SetActive(false);
        loseImg.gameObject.SetActive(false);
        drawImg.gameObject.SetActive(false);
        exitImg.gameObject.SetActive(false);
    }

    public void RefreshPlayerBrickQueue()
    {
        ClearGrid(playerBrickGrid.transform);

        var playerQueue = _manager.PlayerBricks;

        foreach (var brick in playerQueue)
        {
            var iconObj = Instantiate(brickIconPref, playerBrickGrid.transform);

            var newBrick = Instantiate(brick.Item1, iconObj.transform);

            newBrick.gameObject.layer = LayerMask.NameToLayer("UI");

            newBrick.rigidbody.isKinematic = true;

            newBrick.transform.localScale = Vector3.one * 70f * newBrick.iconScaleFactor;

            newBrick.Init(_manager);

            newBrick.SetColor(brick.Item2);
        }
    }

    public void RefreshEnemyBrickQueue()
    {
        ClearGrid(enemyBrickGrid.transform);

        var enemyQueue = _manager.EnemyBricks;

        foreach (var brick in enemyQueue)
        {
            var iconObj = Instantiate(brickIconPref, enemyBrickGrid.transform);

            var newBrick = Instantiate(brick.Item1, iconObj.transform);

            newBrick.gameObject.layer = LayerMask.NameToLayer("UI");

            newBrick.rigidbody.isKinematic = true;

            newBrick.transform.localScale = Vector3.one * 70f * newBrick.iconScaleFactor;

            newBrick.Init(_manager);

            newBrick.SetColor(brick.Item2);
        }
    }

    private void ClearGrid(Transform tran)
    {
        for (int i = tran.childCount - 1; i >= 0; i--)
        {
            var child = tran.GetChild(i);

            Destroy(child.gameObject);
        }
    }

    public void SetPlayingTimeProgress(float currTime, float totalTime)
    {
        var reaminTime = (int)(totalTime - currTime);

        if (reaminTime < 10)
            progressTime.color = Color.red;

        progressTime.text = $"{reaminTime}";
    }

    public void ShowBrickCnt()
    {
        myAreaBrickCnt.gameObject.SetActive(true);
        enemyAreaBrickCnt.gameObject.SetActive(true);

        myAreaBrickCnt.text = "0";
        enemyAreaBrickCnt.text = "0";
    }

    public void SetBrickCnt(int myAreaCnt, int enemyAreaCnt)
    {
        myAreaBrickCnt.text = myAreaCnt.ToString();
        enemyAreaBrickCnt.text = enemyAreaCnt.ToString();
    }

    public void OnClickExit()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void ShowResult(int myAreaBrickCnt, int enemyAreaBrickCnt)
    {
        exitImg.gameObject.SetActive(true);

        if (myAreaBrickCnt < enemyAreaBrickCnt)
        {
            winImg.gameObject.SetActive(true);
            SoundManager.Instance.PlaySFX(SoundManager.SFX.Win, false);
        }
        else if (myAreaBrickCnt > enemyAreaBrickCnt)
        {
            loseImg.gameObject.SetActive(true);
            SoundManager.Instance.PlaySFX(SoundManager.SFX.Lose, false);
        }
        else
        {
            drawImg.gameObject.SetActive(true);
            SoundManager.Instance.PlaySFX(SoundManager.SFX.Draw, false);
        }
    }
}
