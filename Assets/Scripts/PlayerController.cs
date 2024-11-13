using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected readonly float FireWaitTime = 0.35f;

    public enum State
    {
        Init,
        Idle,               //���
        ArrowTargeting,     //����
        PowerGauge,         //�Ŀ� ����
        Fire                //�߻�        
    }

    public Gauge powerGauge;              //�Ŀ� ������
    public Gauge coolTimeGauge;           //��Ž ������

    [SerializeField] protected GameObject curBrickPrefab;

    [SerializeField] protected Animator characterAnimator;

    [SerializeField] protected TargetArrow targetArrow;       //���� ȭ��ǥ

    [SerializeField] protected float fireCoolTime;
    [SerializeField] protected float firePower;

    [SerializeField] protected float additionalSpeedMax;
    protected float speedMultiplier = 1f;

    protected Vector3 curTargetPos = Vector3.zero;

    protected StageManager _stageManager;

    public State curState = State.Idle;

    protected float curGaugePower;
    protected Vector3 curTargetDir;

    protected float _currFireWaitTime;

    protected Queue<(Brick, int)> _bricks;

    private PlayerInput _playerInput;

    public void Init(StageManager manager, Queue<(Brick, int)> bricks)
    {
        _stageManager = manager;

        _bricks = bricks;

        powerGauge.Init(manager);

        coolTimeGauge.Init(manager);

        coolTimeGauge.Show();

        powerGauge.Hide();

        targetArrow.Init(-25f, 60f);

        powerGauge.speedMultiplier = 2;
    }

    public void SetPlayerInput(PlayerInput input)
    {
        _playerInput = input;
    }

    public virtual void OnUpdate()
    {
        //�������� �ð��� ���� ���ӵ� �ֱ� 
        //�÷��̾�� �ϴ� 1�� ����
        speedMultiplier = 1 + (_stageManager.GetRemainTimeRatio() * additionalSpeedMax);
        coolTimeGauge.speedMultiplier = speedMultiplier;

        coolTimeGauge.OnUpdate();

        switch (curState)
        {
            case State.Init:
                {
                    powerGauge.Hide();
                    coolTimeGauge.InitGauage(fireCoolTime);
                    curState = State.Idle;
                }
                break;
            case State.Idle:
                {
                    //��Ÿ�� ������ �� �������� ���
                    if(coolTimeGauge.GetCurGauge() < 1)
                    {
                        break;
                    }

                    targetArrow.gameObject.SetActive(true);
                    curState = State.ArrowTargeting;
                }
                break;
            case State.ArrowTargeting:
                {
                    powerGauge.Show();
                    curState = State.PowerGauge;
                }
                break;
            case State.PowerGauge:
                {
                    powerGauge.OnUpdate();

                    if (Input.GetKey(_playerInput.Up))
                    {
                        targetArrow.Up();
                    }

                    if (Input.GetKey(_playerInput.Down))
                    {
                        targetArrow.Down();
                    }

                    if (Input.GetKeyDown(_playerInput.Shot))
                    {
                        if (characterAnimator != null)
                        {
                            characterAnimator.SetTrigger("Throw");
                        }

                        curGaugePower = powerGauge.GetCurGauge();
                        curTargetDir = targetArrow.CustomCurDir;

                        powerGauge.Hide();
                        targetArrow.gameObject.SetActive(false);

                        _currFireWaitTime = 0f;

                        curState = State.Fire;
                    }
                }
                break;
            case State.Fire:
                {
                    _currFireWaitTime += Time.deltaTime;
                    
                    if (FireWaitTime > _currFireWaitTime)
                        break;

                    Brick tempBrick = GetNewBrick();
                    tempBrick.Launch(curTargetDir, firePower * curGaugePower);

                    _stageManager.ShowBrickList.Add(tempBrick);

                    curState = State.Init;
                }
                break;
        }
    }

    protected Brick GetNewBrick()
    {
        var newBrick = GetNextBrick();

        var brick = Instantiate(newBrick.Item1, transform.position, newBrick.Item1.transform.rotation);

        brick.Init(_stageManager);

        if (brick.Type == Brick.BrickType.Chnage || 
            brick.Type == Brick.BrickType.Boom)
        {
            brick.SetUseAction(_stageManager.ShakeCamera);
        }

        brick.SetColor(newBrick.Item2);

        return brick;
    }

    public (Brick, int) GetNextBrick()
    {
        var brick = _bricks.Dequeue();

        _bricks.Enqueue(BrickManager.Instance.GetBrick());

        _stageManager.StageUi.RefreshPlayerBrickQueue();

        _stageManager.StageUi.RefreshEnemyBrickQueue();

        return brick;
    }
}
