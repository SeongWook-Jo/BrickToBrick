using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PlayerController
{
    [SerializeField] float arrowTargetingDelay_Max;
    [SerializeField] float arrowTargetingDelay_Min;
    [SerializeField] float remainArrowTargetingDelay; //화살표 방향 정하는 딜레이

    [SerializeField] float powerGaugeMin;
    [SerializeField] float powerGaugeMax;    
    [SerializeField] float targetPowerGauge;      //파워 게이지 정하는 딜레이

    float targetAngle_Min = 110f;
    float curTargetAngle;
    float angleTimer = 0;

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
                    if (coolTimeGauge.GetCurGauge() < 1)
                    {
                        break;
                    }

                    //방향 정하는 딜레이
                    remainArrowTargetingDelay = Random.Range(arrowTargetingDelay_Min, arrowTargetingDelay_Max);
                    //파워 정하기
                    targetPowerGauge = Random.Range(powerGaugeMin, powerGaugeMax);

                    targetArrow.gameObject.SetActive(true);

                    curState = State.ArrowTargeting;
                }
                break;
            case State.ArrowTargeting:
                {
                    //위아래로 화살표 조준하기
                    angleTimer += Time.deltaTime * 90;  //초당 90도
                    if (angleTimer >= 180)
                        angleTimer = 0;

                    curTargetAngle = targetAngle_Min + (Mathf.Sin(angleTimer * Mathf.Deg2Rad) * 70);
                    
                    //각도로 회전시키고 방향값 가져오기
                    curTargetPos = targetArrow.RotateToTargetAngle(curTargetAngle);

                    //시간 다 지나면 자동으로 다음 단계로 이동
                    remainArrowTargetingDelay -= Time.deltaTime;
                    if (remainArrowTargetingDelay <= 0)
                    {
                        curState = State.PowerGauge;
                    }
                }
                break;
            case State.PowerGauge:
                {
                    if (powerGauge.gameObject.activeSelf == false)
                        powerGauge.gameObject.SetActive(true);

                    //파워 게이지 타겟범위 안에 들어오면 발사 오차 +- 0.02f
                    float tempCurPowerGauge = powerGauge.GetCurGauge();
                    if (targetPowerGauge - 0.02f <= tempCurPowerGauge && tempCurPowerGauge <= targetPowerGauge + 0.02f)
                    {
                        curState = State.Fire;
                    }
                }
                break;
            case State.Fire:
                {
                    float tempGaugePower = powerGauge.GetCurGauge();
                    Vector3 targetDir = targetArrow.CurDir;

                    //브릭 생성 또는 Pool에서 가져오기
                    GameObject brickObj = PoolManager.Instance.Get(curBrickPrefab, transform.position);

                    //던지기
                    Brick tempBrick = brickObj.GetComponent<Brick>();
                    tempBrick.Init();
                    tempBrick.Launch(targetDir, firePower * tempGaugePower);

                    curState = State.Init;
                }
                break;
        }
    }
}
