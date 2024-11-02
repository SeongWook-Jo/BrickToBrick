using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PlayerController
{
    [SerializeField] float arrowTargetingDelay_Max;
    [SerializeField] float arrowTargetingDelay_Min;
    [SerializeField] float remainArrowTargetingDelay; //ȭ��ǥ ���� ���ϴ� ������

    [SerializeField] float powerGaugeMin;
    [SerializeField] float powerGaugeMax;    
    [SerializeField] float targetPowerGauge;      //�Ŀ� ������ ���ϴ� ������

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

                    curState = State.ArrowTargeting;
                }
                break;
            case State.ArrowTargeting:
                {
                    //���Ʒ��� ȭ��ǥ �����ϱ�
                    angleTimer += Time.deltaTime * 90;  //�ʴ� 90��
                    if (angleTimer >= 180)
                        angleTimer = 0;

                    curTargetAngle = targetAngle_Min + (Mathf.Sin(angleTimer * Mathf.Deg2Rad) * 70);
                    
                    //������ ȸ����Ű�� ���Ⱚ ��������
                    curTargetPos = targetArrow.RotateToTargetAngle(curTargetAngle);

                    //�ð� �� ������ �ڵ����� ���� �ܰ�� �̵�
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

                    //�Ŀ� ������ Ÿ�ٹ��� �ȿ� ������ �߻� ���� +- 0.02f
                    float tempCurPowerGauge = powerGauge.GetCurGauge();
                    if (targetPowerGauge - 0.02f <= tempCurPowerGauge && tempCurPowerGauge <= targetPowerGauge + 0.02f)
                    {
                        if (characterAnimator != null)
                        {
                            if (characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Throw") == false)
                                characterAnimator.SetTrigger("Throw");
                        }

                        curGaugePower = powerGauge.GetCurGauge();
                        curTargetDir = targetArrow.CurDir;

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
                    tempBrick.Init();
                    tempBrick.Launch(curTargetDir, firePower * curGaugePower);
                    throwAnimationController.FinishThrowing();

                    curState = State.Init;
                }
                break;
        }
    }

    protected override Brick GetNewBrick()
    {
        var newBrick = _stageManager.GetEnemyBrick();

        return Instantiate(newBrick, transform);
    }
}
