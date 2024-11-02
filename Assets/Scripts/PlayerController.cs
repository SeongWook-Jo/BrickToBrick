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

    [SerializeField]
    protected GameObject curBrickPrefab;

    [SerializeField] protected TargetArrow targetArrow;       //방향 화살표
    [SerializeField] protected Gauge powerGauge;              //파워 게이지
    [SerializeField] protected Gauge coolTimeGauge;           //쿨탐 게이지

    [SerializeField] protected float fireCoolTime;
    [SerializeField] protected float firePower;

    protected Vector3 curTargetPos = Vector3.zero;

    protected StageManager _stageManager;

    public State curState = State.Idle;

    public void Init(StageManager manager)
    {
        _stageManager = manager;
    }

    void Update()
    {
        switch (curState)
        {
            case State.Init:
                {
                    targetArrow.gameObject.SetActive(false);
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
                    //마우스 따라서 화살표 조준하기
                    curTargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetArrow.RotateToTargetPos(curTargetPos);

                    //클릭하면 파워 게이지 보여주기
                    if (Input.GetMouseButtonDown(0))
                    {
                        curState = State.PowerGauge;
                    }
                }
                break;
            case State.PowerGauge:
                {
                    if (powerGauge.gameObject.activeSelf == false)
                        powerGauge.gameObject.SetActive(true);

                    //클릭하면 발사
                    if (Input.GetMouseButtonDown(0))
                    {
                        curState = State.Fire;
                    }
                }
                break;
            case State.Fire:
                {
                    float tempGaugePower = powerGauge.GetCurGauge();
                    Vector3 targetDir = targetArrow.CurDir;

                    Brick tempBrick = GetNewBrick();
                    tempBrick.Init();
                    tempBrick.Launch(targetDir, firePower * tempGaugePower);

                    curState = State.Init;
                }
                break;
        }
    }

    protected virtual Brick GetNewBrick()
    {
        var newBrick = _stageManager.GetPlayerBrick();

        return Instantiate(newBrick, transform);
    }
}
