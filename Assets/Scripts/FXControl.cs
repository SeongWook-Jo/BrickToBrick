using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXControl : MonoBehaviour
{
    [SerializeField] float deActivateTime_Sec;
    [SerializeField] bool scaleDownOnDeActivation;
    [SerializeField] float scaleDownTime;

    Vector3 originScale;
    
    private void Awake()
    {
        originScale = transform.localScale;
    }

    private void OnEnable()
    {
        StartCoroutine(CoDeActivation());
    }

    IEnumerator CoDeActivation()
    {
        float scaleTimer = 0;

        yield return new WaitForSeconds(deActivateTime_Sec);

        if (scaleDownOnDeActivation)
        {
            while (scaleTimer < scaleDownTime)
            {
                scaleTimer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(originScale, Vector3.zero, scaleTimer / scaleDownTime);

                yield return null;
            }
        }

        gameObject.SetActive(false);
    }
}
