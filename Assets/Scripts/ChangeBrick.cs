using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ChangeBrick : Brick
{
    public override BrickType Type { get => BrickType.Chnage; }

    public float radius;

    public Texture tex;

    public override void InitDetail()
    {
        renderer.materials[0].SetTexture("_MainTex", tex);
    }

    protected override void OnTriggerDetail()
    {
        var colliders = GetNearbyBrickCollider(radius);

        foreach (var col in colliders)
        {
            var brick = col.GetComponentInParent<Brick>();

            brick.ChageBrick();
        }
    }
}
