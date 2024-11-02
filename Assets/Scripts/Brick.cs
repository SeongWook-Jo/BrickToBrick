using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Brick : MonoBehaviour
{
    public readonly Color[] RandomColors = new Color[] { Color.blue, Color.red, Color.yellow };

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

    public bool IsLaunched { get; private set; }

    public Rigidbody rigidbody;

    protected Renderer renderer;

    public float iconScaleFactor;

    public void Init()
    {
        IsLaunched = false;

        renderer = GetComponent<MeshRenderer>();

        if (Type == BrickType.Normal)
        {
            var randomIdx = Random.Range(0, RandomColors.Length);

            var randomColor = RandomColors[randomIdx];

            renderer.materials[0].SetColor("_Color", randomColor);
        }

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
        return Physics.OverlapSphere(transform.position, radius, 1 << LayerMask.NameToLayer("Brick"));
    }

    public void ChageBrick()
    {
        if (Type != BrickType.Normal)
            return;

        
    }
}
