using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Brick : MonoBehaviour
{
    public MeshCollider meshCollider;
    public Rigidbody rigidbody;

    public bool IsLaunched { get; private set; }

    public float testX;
    public float testY;
    public float testPower;

    [ContextMenu("test")]
    public void TestCode()
    {
        Launch(new Vector2(testX, testY), testPower);
    }

    public void Init()
    {
        IsLaunched = false;

        InitDetail();
    }

    public virtual void InitDetail()
    {

    }

    public void Launch(Vector2 dir, float power)
    {
        IsLaunched = true;

        dir = dir.normalized;

        rigidbody.AddForce(dir * power, ForceMode.Impulse);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick") == false)
            return;

        OnTrigger();
    }

    protected virtual void OnTrigger()
    {

    }
}
