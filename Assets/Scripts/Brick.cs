using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Brick : MonoBehaviour
{
    public enum BrickType
    {
        Normal,
        Boom,
        Blackhole,
        Chnage,
        RandomBox,
        TangTang,
    }

    public virtual BrickType Type { get => BrickType.Normal; }

    public Rigidbody Rigidbody { get => _rigidbody; }

    public bool IsLaunched { get; private set; }

    private MeshCollider _meshCollider;
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    private MeshFilter _meshFilter;

    public void Init()
    {
        IsLaunched = false;

        _meshCollider = GetComponent<MeshCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _meshFilter = GetComponent<MeshFilter>();

        InitDetail();
    }

    public virtual void InitDetail()
    {
        var mesh = BrickManager.Instance.GetNormalBrickMesh();

        _meshCollider.sharedMesh = mesh;

        _meshFilter.mesh = mesh;
    }

    public void Launch(Vector2 dir, float power)
    {
        IsLaunched = true;

        dir = dir.normalized;

        _rigidbody.AddForce(dir * power, ForceMode.Impulse);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
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
        if (Type != BrickType.Normal)
            return;

        
    }
}
