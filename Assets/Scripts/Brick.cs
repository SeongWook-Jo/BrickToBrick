using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public static readonly Color[] RandomColors = new Color[] { Color.blue, Color.red, Color.yellow };

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

    public float iconScaleFactor;

    protected Renderer renderer;

    public void Init()
    {
        IsLaunched = false;

        renderer = GetComponent<MeshRenderer>();

        if (Type == BrickType.Normal)
        {
            transform.GetChild(0).AddComponent<BrickCollision>().Init(this);
        }
        else
        {
            gameObject.AddComponent<BrickCollision>().Init(this);
        }

        InitDetail();
    }

    public void SetColor(int colorIdx)
    {
        if (Type != BrickType.Normal)
            return;

        renderer.materials[0].SetColor("_Color", RandomColors[colorIdx]);
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

    public void OnCollision(Collision collision)
    {
        if (Type == BrickType.Boom)
        {

        }

        if (IsLaunched == false)
            return;

        OnCollisionDetail();
    }

    protected virtual void OnCollisionDetail()
    {

    }

    protected Collider[] GetNearbyBrickCollider(float radius)
    {
        return Physics.OverlapSphere(transform.position, radius, 1 << LayerMask.NameToLayer("Brick"));
    }

    public void ChageBrick()
    {
        IsLaunched = true;
    }
}
