using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    [SerializeField] Vector3 startLocalPos;
    [SerializeField] Vector3 endLocalPos;
    [SerializeField] float totalMoveTime;

    Vector3 curLocalPos = Vector3.zero;

    float timer = 0;

    private void OnEnable()
    {
        curLocalPos = transform.localPosition;
        float totalDist = Vector2.Distance(startLocalPos, endLocalPos);
        float curDist = Vector2.Distance(curLocalPos, endLocalPos);
        timer = totalMoveTime - (totalMoveTime * (curDist / totalDist));
    }

    void Update()
    {
        timer += Time.deltaTime;

        curLocalPos = Vector3.Lerp(startLocalPos, endLocalPos, timer / totalMoveTime);
        transform.localPosition = curLocalPos;

        if (timer >= totalMoveTime)
            timer = 0;
    }
}
