using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ChangeBrick : Brick
{
    public override BrickType Type { get => BrickType.Chnage; }

    public float radius;

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
