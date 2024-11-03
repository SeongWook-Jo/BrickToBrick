using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    [SerializeField] protected ThrowAnimationFrameController throwAnimationController;

    [SerializeField] protected TargetArrow targetArrow;       //���� ȭ��ǥ
    [SerializeField] protected Gauge powerGauge;              //�Ŀ� ������
    [SerializeField] protected Gauge coolTimeGauge;           //��Ž ������

    [SerializeField] protected float fireCoolTime;
    [SerializeField] protected float firePower;

    protected Vector3 curTargetPos = Vector3.zero;

    protected StageManager _stageManager;

    public State curState = State.Idle;

    protected float curGaugePower;
    protected Vector3 curTargetDir;

    public void Init(StageManager manager)
    {
        _stageManager = manager;

        powerGauge.Init(manager);

        coolTimeGauge.Init(manager);
    }

    void Update()
    {
        if (_stageManager.IsEndGame)
            return;

        switch (curState)
        {
            case State.Init:
                {
                    powerGauge.gameObject.SetActive(false);
                    coolTimeGauge.gameObject.SetActive(true);
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
                    curState = State.PowerGauge;
                }
                break;
            case State.PowerGauge:
                {
                    //���콺 ���� ȭ��ǥ �����ϱ�
                    curTargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetArrow.RotateToTargetPos(curTargetPos);

                    if (powerGauge.gameObject.activeSelf == false)
                        powerGauge.gameObject.SetActive(true);

                    //Ŭ���ϸ� �߻�
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (characterAnimator != null)
                        {
                            if (characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Throw") == false)
                                characterAnimator.SetTrigger("Throw");
                        }

                        curGaugePower = powerGauge.GetCurGauge();
                        curTargetDir = targetArrow.CurDir;

                        powerGauge.gameObject.SetActive(false);
                        targetArrow.gameObject.SetActive(false);

                        curState = State.Fire;
                    }
                }
                break;
            case State.Fire:
                {
                    //�ִϸ��̼� ������ ���� ���缭 ���󰡱�
                    if (throwAnimationController.IsReadyToThrow == false)
                    {
                        break;
                    }                    

                    Brick tempBrick = GetNewBrick();
                    tempBrick.Launch(curTargetDir, firePower * curGaugePower);
                    throwAnimationController.FinishThrowing();

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

        brick.SetColor(newBrick.Item2);

        return brick;
    }
}
