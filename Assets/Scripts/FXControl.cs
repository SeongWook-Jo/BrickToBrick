using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXControl : MonoBehaviour
{
    [SerializeField] float deActivateTime_Sec;

    private void OnEnable()
    {
        StartCoroutine(CoDeActivation());
    }

    IEnumerator CoDeActivation()
    {
        yield return new WaitForSeconds(deActivateTime_Sec);
        gameObject.SetActive(false);
    }
}
