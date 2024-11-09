using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    GameObject fill;

    //최초 로컬 Y 위치 및 스케일
    float initialLocalScale_Y;
    float initialLocalScale_Y_x2;

    [SerializeField] float curPosY;
    [SerializeField] float curScaleY;
    [SerializeField] float powerUptotalTime = 1;
    [SerializeField] float timer = 0;

    //게이지 위아래로 스위칭할떄 사용
    [SerializeField] bool isGaugeUpDown;

    bool isScaleDown = true;

    private StageManager _manager;

    public float speedMultiplier = 1;

    public void Init(StageManager manager)
    {
        _manager = manager;

        fill = transform.GetChild(1).gameObject;
        initialLocalScale_Y = fill.transform.localScale.y;
        initialLocalScale_Y_x2 = initialLocalScale_Y * 2.0f;
    }

    public void OnUpdate()
    {
        if(isGaugeUpDown == true)
        {
            if (isScaleDown == true)
                timer += Time.deltaTime * speedMultiplier;
            else
                timer -= Time.deltaTime * speedMultiplier;

            curPosY = -Mathf.Lerp(0, initialLocalScale_Y_x2, timer / powerUptotalTime);
            curScaleY = Mathf.Lerp(initialLocalScale_Y, 0, timer / powerUptotalTime);

            fill.transform.localPosition = new Vector3(fill.transform.localPosition.x, curPosY, fill.transform.localPosition.z);
            fill.transform.localScale = new Vector3(fill.transform.localScale.x, curScaleY, fill.transform.localScale.z);

            if (isScaleDown == true)
            {
                if (curScaleY <= 0)
                    isScaleDown = false;
            }
            else
            {
                if (curScaleY >= initialLocalScale_Y)
                    isScaleDown = true;
            }
        }
        else
        {
            timer += Time.deltaTime * speedMultiplier;

            curPosY = -Mathf.Lerp(initialLocalScale_Y_x2, 0, timer / powerUptotalTime);
            curScaleY = Mathf.Lerp(0, initialLocalScale_Y, timer / powerUptotalTime);

            fill.transform.localPosition = new Vector3(fill.transform.localPosition.x, curPosY, fill.transform.localPosition.z);
            fill.transform.localScale = new Vector3(fill.transform.localScale.x, curScaleY, fill.transform.localScale.z);
        }   
    }

    public void InitGauage(float _totalTime)
    {
        timer = 0;
        powerUptotalTime = _totalTime;
    }

    public float GetCurGauge()
    {
        //0 ~ 1.0
        float tempCurPowerRatio = (initialLocalScale_Y_x2 - Mathf.Abs(curPosY)) / initialLocalScale_Y_x2;
        return tempCurPowerRatio;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
