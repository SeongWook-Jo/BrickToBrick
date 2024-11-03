using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Init,
        Idle,               //평상
        ArrowTargeting,     //조준
        PowerGauge,         //파워 조절
        Fire                //발사        
    }

    [SerializeField] protected GameObject curBrickPrefab;

    [SerializeField] protected Animator characterAnimator;
    [SerializeField] protected ThrowAnimationFrameController throwAnimationController;

    [SerializeField] protected TargetArrow targetArrow;       //방향 화살표
    [SerializeField] protected Gauge powerGauge;              //파워 게이지
    [SerializeField] protected Gauge coolTimeGauge;           //쿨탐 게이지

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
                    //쿨타임 게이지 다 찰떄까지 대기
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
                    //마우스 따라서 화살표 조준하기
                    curTargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetArrow.RotateToTargetPos(curTargetPos);

                    if (powerGauge.gameObject.activeSelf == false)
                        powerGauge.gameObject.SetActive(true);

                    //클릭하면 발사
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
                    //애니메이션 던지는 동작 맞춰서 날라가기
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
