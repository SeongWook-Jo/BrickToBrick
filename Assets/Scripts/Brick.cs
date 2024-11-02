using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Brick : MonoBehaviour
{
    public Rigidbody Rigidbody { get => _rigidbody;}

    public bool IsLaunched { get; private set; }

    private MeshCollider _meshCollider;
    private Rigidbody _rigidbody;

    public void Init()
    {
        IsLaunched = false;

        _meshCollider = GetComponent<MeshCollider>();

        _rigidbody = GetComponent<Rigidbody>();

        InitDetail();
    }

    public virtual void InitDetail()
    {

    }

    public void Launch(Vector2 dir, float power)
    {
        IsLaunched = true;

        dir = dir.normalized;

        _rigidbody.AddForce(dir * power, ForceMode.Impulse);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick") == false)
            return;

        OnTrigger();
    }

    protected void OnTrigger()
    {
        if (IsLaunched == false)
            return;

        OnTriggerDetail();
    }

    protected virtual void OnTriggerDetail()
    {

    }

    protected Collider[] GetNearbyBrickCollider(float radius)
    {
        return Physics.OverlapSphere(transform.position, radius);
    }

    public void ChageBrick()
    {

    }
}
