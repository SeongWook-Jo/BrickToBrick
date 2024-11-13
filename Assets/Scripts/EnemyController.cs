using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PlayerController
{
    [SerializeField] float arrowTargetingDelay_Max;
    [SerializeField] float arrowTargetingDelay_Min;
    float remainArrowTargetingDelay; //ȭ��ǥ ���� ���ϴ� ������

    [SerializeField] float powerGaugeMin;
    [SerializeField] float powerGaugeMax;    
    float targetPowerGauge;      //�Ŀ� ������ ���ϴ� ������

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
        //�������� �ð��� ���� ���ӵ� �ֱ� 
        //������ ���� ������ ������
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
                    //��Ÿ�� ������ �� �������� ���
                    if (coolTimeGauge.GetCurGauge() < 1)
                    {
                        break;
                    }

                    //���� ���ϴ� ������
                    remainArrowTargetingDelay = Random.Range(arrowTargetingDelay_Min, arrowTargetingDelay_Max);
                    //�Ŀ� ���ϱ�
                    targetPowerGauge = Random.Range(powerGaugeMin, powerGaugeMax);

                    targetArrow.gameObject.SetActive(true);

                    //���� �ޱ� ����
                    angleTimer = Random.Range(0, 181);

                    curState = State.ArrowTargeting;
                }
                break;
            case State.ArrowTargeting:
                {
                    //���Ʒ��� ȭ��ǥ �����ϱ�
                    angleTimer += Time.deltaTime * 90 * speedMultiplier;  //�ʴ� 90��
                    if (angleTimer >= 180)
                        angleTimer = 0;

                    curTargetAngle = targetAngle_Min + (Mathf.Sin(angleTimer * Mathf.Deg2Rad) * targetAngle_AddMax);
                    
                    //������ ȸ����Ű�� ���Ⱚ ��������
                    curTargetPos = targetArrow.RotateToTargetAngle(curTargetAngle);

                    //�ð� �� ������ �ڵ����� ���� �ܰ�� �̵�
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
                    //�Ŀ� ������ Ÿ�ٹ��� �ȿ� ������ �߻� ���� +- 0.02f
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
