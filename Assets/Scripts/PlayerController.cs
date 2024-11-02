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

    [SerializeField]
    protected GameObject curBrickPrefab;

    [SerializeField] protected TargetArrow targetArrow;       //���� ȭ��ǥ
    [SerializeField] protected Gauge powerGauge;              //�Ŀ� ������
    [SerializeField] protected Gauge coolTimeGauge;           //��Ž ������

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
                    //���콺 ���� ȭ��ǥ �����ϱ�
                    curTargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetArrow.RotateToTargetPos(curTargetPos);

                    //Ŭ���ϸ� �Ŀ� ������ �����ֱ�
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

                    //Ŭ���ϸ� �߻�
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
