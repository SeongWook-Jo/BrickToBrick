using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArrow : MonoBehaviour
{
    Vector3 dir = Vector3.zero;
    public Vector3 CurDir 
    {
        get
        {
            return dir.normalized;
        }
    }

    public void RotateToTargetPos(Vector3 _targetGlobalPos)
    {
        dir = _targetGlobalPos - transform.position;
        float tempDegree = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (tempDegree >= 90)
            tempDegree = 90;

        if (tempDegree <= 0)
            tempDegree = 0;

        transform.localRotation = Quaternion.Euler(0, 0, tempDegree);
    }
}
