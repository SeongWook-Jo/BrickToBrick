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

        transform.localRotation = Quaternion.Euler(0, 0, tempDegree);
    }

    public Vector3 RotateToTargetAngle(float _angle)
    {
        Vector3 startDir = Vector3.right;   //우측을 기준으로 0
        float tempRadian = _angle * Mathf.Deg2Rad;
        tempRadian = Mathf.Atan2(startDir.y, startDir.x) + tempRadian;
        dir = new Vector3(Mathf.Cos(tempRadian), Mathf.Sin(tempRadian), 0);

        transform.localRotation = Quaternion.Euler(0, 0, _angle);

        //회전한 방향 반환
        return dir;
    }
}
