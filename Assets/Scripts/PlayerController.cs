using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Init,
        Idle,               //평상
        ArrowTargeting,     //조준
        PowerGauge,         //파워 조절
        Fire                //발사        
    }

    [SerializeField]
    GameObject brickPrefab;

    [SerializeField] TargetArrow targetArrow;       //방향 화살표
    [SerializeField] Gauge powerGauge;              //파워 게이지
    [SerializeField] Gauge coolTimeGauge;           //쿨탐 게이지

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
                    //쿨타임 게이지 다 찰떄까지 대기
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
                    //마우스 따라서 화살표 조준하기
                    curMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetArrow.RotateToTargetPos(curMousePos);

                    //클릭하면 파워 게이지 보여주기
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

                    //클릭하면 발사
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

                    //브릭 생성 또는 Pool에서 가져오기
                    GameObject brickObj = PoolManager.Instance.Get(brickPrefab, transform.position);

                    //던지기
                    Brick tempBrick = brickObj.GetComponent<Brick>();
                    tempBrick.Launch(targetDir, firePower * tempGaugePower);

                    curPlayerState = PlayerState.Init;
                }
                break;
        }
    }

}
