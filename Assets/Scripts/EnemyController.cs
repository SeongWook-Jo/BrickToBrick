using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PlayerController
{
    [SerializeField] float arrowTargetingDelay_Max;
    [SerializeField] float arrowTargetingDelay_Min;
    float remainArrowTargetingDelay; //화살표 방향 정하는 딜레이

    [SerializeField] float powerGaugeMin;
    [SerializeField] float powerGaugeMax;    
    float targetPowerGauge;      //파워 게이지 정하는 딜레이

    float targetAngle_Min = 140f;
    float targetAngle_AddMax = 60;
    float curTargetAngle;
    float angleTimer = 0;

    public void SetDifficulty(StageManager.DifficultyLevel difficulty)
    {
        switch (difficulty)
        {
            case StageManager.DifficultyLevel.Easy:
                {
                    fireCoolTime = 1.0f;
                    additionalSpeedMax = 1.0f;
                }
                break;
            case StageManager.DifficultyLevel.Hard:
                {
                    fireCoolTime = 0.4f;
                    additionalSpeedMax = 2.5f;
                }
                break;
        }
    }

    public override void OnUpdate()
    {
        //스테이지 시간에 따라 가속도 주기 
        //적군은 점점 빠르게 해주자
        speedMultiplier = 1 + (_stageManager.GetRemainTimeRatio() * additionalSpeedMax);
        powerGauge.speedMultiplier = speedMultiplier;
        coolTimeGauge.speedMultiplier = speedMultiplier;

        coolTimeGauge.OnUpdate();

        switch (curState)
        {
            case State.Init:
                {
                    targetArrow.gameObject.SetActive(false);
                    powerGauge.Hide();
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

                    //시작 앵글 랜덤
                    angleTimer = Random.Range(0, 181);

                    curState = State.ArrowTargeting;
                }
                break;
            case State.ArrowTargeting:
                {
                    //위아래로 화살표 조준하기
                    angleTimer += Time.deltaTime * 90 * speedMultiplier;  //초당 90도
                    if (angleTimer >= 180)
                        angleTimer = 0;

                    curTargetAngle = targetAngle_Min + (Mathf.Sin(angleTimer * Mathf.Deg2Rad) * targetAngle_AddMax);
                    
                    //각도로 회전시키고 방향값 가져오기
                    curTargetPos = targetArrow.RotateToTargetAngle(curTargetAngle);

                    //시간 다 지나면 자동으로 다음 단계로 이동
                    remainArrowTargetingDelay -= Time.deltaTime * speedMultiplier;
                    if (remainArrowTargetingDelay <= 0)
                    {
                        powerGauge.Show();
                        curState = State.PowerGauge;
                    }
                }
                break;
            case State.PowerGauge:
                {
                    powerGauge.OnUpdate();
                    //파워 게이지 타겟범위 안에 들어오면 발사 오차 +- 0.02f
                    float tempCurPowerGauge = powerGauge.GetCurGauge();
                    if (targetPowerGauge - 0.02f <= tempCurPowerGauge && tempCurPowerGauge <= targetPowerGauge + 0.02f)
                    {
                        if (characterAnimator != null)
                        {
                            characterAnimator.SetTrigger("Throw");
                        }

                        curGaugePower = powerGauge.GetCurGauge();
                        curTargetDir = targetArrow.CurDir;
                        powerGauge.Hide();

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
}
