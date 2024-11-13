using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArrow : MonoBehaviour
{
    public float rotSpeed = 100f;

    private float _minAngle;

    private float _maxAngle;

    Vector3 dir = Vector3.zero;

    public Vector3 CustomCurDir { get { return transform.right; } }

    public Vector3 CurDir 
    {
        get
        {
            return dir.normalized;
        }
    }

    public void Init(float minAngle, float maxAngle)
    {
        _minAngle = minAngle;

        _maxAngle = maxAngle;
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

    public void Up()
    {
        transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);

        ClampRot();
    }

    public void Down()
    {
        transform.Rotate(-Vector3.forward, rotSpeed * Time.deltaTime);

        ClampRot();
    }

    private void ClampRot()
    {
        var currZ = transform.localEulerAngles.z;

        if (currZ < 180)
            currZ = Mathf.Clamp(currZ, currZ, _maxAngle);
        else
        {
            currZ -= 360;

            currZ = Mathf.Clamp(currZ, _minAngle, 0);
        }

        transform.localEulerAngles = new Vector3(0, 0, currZ);
    }
}
