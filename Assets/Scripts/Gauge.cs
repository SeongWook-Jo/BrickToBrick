using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    GameObject fill;

    //최초 로컬 Y 위치 및 스케일
    float initialLocalScale_Y;
    float initialLocalScale_Y_Half;

    [SerializeField] float curPosY;
    [SerializeField] float curScaleY;
    [SerializeField] float powerUptotalTime = 1;
    [SerializeField] float timer = 0;

    //게이지 위아래로 스위칭할떄 사용
    [SerializeField] bool isGaugeUpDown;

    bool isScaleDown = true;

    private void Awake()
    {
        fill = transform.GetChild(1).gameObject;
        initialLocalScale_Y = fill.transform.localScale.y;
        initialLocalScale_Y_Half = initialLocalScale_Y / 2.0f;
    }


    void Update()
    {
        if(isGaugeUpDown == true)
        {
            if (isScaleDown == true)
                timer += Time.deltaTime;
            else
                timer -= Time.deltaTime;

            curPosY = -Mathf.Lerp(0, initialLocalScale_Y_Half, timer / powerUptotalTime);
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
            timer += Time.deltaTime;

            curPosY = -Mathf.Lerp(initialLocalScale_Y_Half, 0, timer / powerUptotalTime);
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
        float tempCurPowerRatio = (initialLocalScale_Y_Half - Mathf.Abs(curPosY)) / initialLocalScale_Y_Half;
        return tempCurPowerRatio;
    }
}
