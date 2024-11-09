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

    [SerializeField] protected GameObject curBrickPrefab;

    [SerializeField] protected Animator characterAnimator;

    [SerializeField] protected TargetArrow targetArrow;       //���� ȭ��ǥ
    [SerializeField] protected Gauge powerGauge;              //�Ŀ� ������
    [SerializeField] protected Gauge coolTimeGauge;           //��Ž ������

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

    public void Init(StageManager manager)
    {
        _stageManager = manager;

        powerGauge.Init(manager);

        coolTimeGauge.Init(manager);

        coolTimeGauge.Show();

        powerGauge.Hide();

        powerGauge.speedMultiplier = 2;
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
                    //���콺 ���� ȭ��ǥ �����ϱ�
                    curTargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetArrow.RotateToTargetPos(curTargetPos);
                    powerGauge.OnUpdate();

                    //Ŭ���ϸ� �߻�
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (characterAnimator != null)
                        {
                            characterAnimator.SetTrigger("Throw");
                        }

                        curGaugePower = powerGauge.GetCurGauge();
                        curTargetDir = targetArrow.CurDir;

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

    protected virtual Brick GetNewBrick()
    {
        var newBrick = _stageManager.GetPlayerBrick();

        var brick = Instantiate(newBrick.Item1, transform);

        brick.Init(_stageManager);

        if (brick.Type == Brick.BrickType.Chnage || 
            brick.Type == Brick.BrickType.Boom)
        {
            brick.SetUseAction(_stageManager.ShakeCamera);
        }

        brick.SetColor(newBrick.Item2);

        return brick;
    }
}
