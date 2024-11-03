using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BoomBrick : Brick
{
    public override BrickType Type { get => BrickType.Boom; }

    public Texture tex;

    public float force;
    public float radius;

    private bool _isBoom;

    public override void InitDetail()
    {
        renderer.materials[0].SetTexture("_MainTex", tex);
    }

    protected override void OnCollisionDetail()
    {
        if (_isBoom)
            return;

        var colliders = GetNearbyBrickCollider(radius);

        HashSet<Brick> tempBricks = new HashSet<Brick>();

        foreach (var col in colliders)
        {
            var brick = col.GetComponentInParent<Brick>();

            if (brick == this)
                continue;

            if (tempBricks.Contains(brick) == false)
                tempBricks.Add(brick);
        }

        SoundManager.Instance.PlaySFX(SoundManager.SFX.Boom, false);
        FXManager.Instance.ShowFX(FXManager.FX.Boom, transform.position);        

        foreach(var brick in tempBricks)
            brick.rigidbody.AddExplosionForce(force, transform.position, radius, 10f, ForceMode.Impulse);

        _isBoom = true;
    }

    protected override void SetFoceTex(Texture tex)
    {
        renderer.materials[0].SetTexture("_MainTex", tex);
    }
}
