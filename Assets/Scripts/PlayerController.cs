using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Init,
        Idle,               //���
        ArrowTargeting,     //����
        PowerGauge,         //�Ŀ� ����
        Fire                //�߻�        
    }

    [SerializeField]
    GameObject brickPrefab;

    [SerializeField] TargetArrow targetArrow;       //���� ȭ��ǥ
    [SerializeField] Gauge powerGauge;              //�Ŀ� ������
    [SerializeField] Gauge coolTimeGauge;           //��Ž ������

    [SerializeField] float fireCoolTime;
    [SerializeField] float firePower;

    public PlayerState curPlayerState = PlayerState.Idle;
        
    Vector3 curMousePos = Vector3.zero;
    void Update()
    {
        switch (curPlayerState)
        {
            case PlayerState.Init:
                {
                    targetArrow.gameObject.SetActive(false);
                    powerGauge.gameObject.SetActive(false);
                    coolTimeGauge.gameObject.SetActive(true);
                    coolTimeGauge.InitGauage(fireCoolTime);
                    curPlayerState = PlayerState.Idle;
                }
                break;
            case PlayerState.Idle:
                {
                    //��Ÿ�� ������ �� �������� ���
                    if(coolTimeGauge.GetCurGauge() < 1)
                    {
                        break;
                    }
                    
                    targetArrow.gameObject.SetActive(true);
                    curPlayerState = PlayerState.ArrowTargeting;
                }
                break;
            case PlayerState.ArrowTargeting:
                {
                    //���콺 ���� ȭ��ǥ �����ϱ�
                    curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetArrow.RotateToTargetPos(curMousePos);

                    //Ŭ���ϸ� �Ŀ� ������ �����ֱ�
                    if (Input.GetMouseButtonDown(0))
                    {
                        curPlayerState = PlayerState.PowerGauge;
                    }
                }
                break;
            case PlayerState.PowerGauge:
                {
                    if (powerGauge.gameObject.activeSelf == false)
                        powerGauge.gameObject.SetActive(true);

                    //Ŭ���ϸ� �߻�
                    if (Input.GetMouseButtonDown(0))
                    {
                        curPlayerState = PlayerState.Fire;
                    }
                }
                break;
            case PlayerState.Fire:
                {
                    float tempGaugePower = powerGauge.GetCurGauge();
                    Vector3 targetDir = targetArrow.CurDir;

                    //�긯 ���� �Ǵ� Pool���� ��������
                    GameObject brickObj = PoolManager.Instance.Get(brickPrefab, transform.position);

                    //������
                    Brick tempBrick = brickObj.GetComponent<Brick>();
                    tempBrick.Launch(targetDir, firePower * tempGaugePower);

                    curPlayerState = PlayerState.Init;
                }
                break;
        }
    }

}
