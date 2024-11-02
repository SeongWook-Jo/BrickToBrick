using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BoomBrick : Brick
{
    public override BrickType Type { get => BrickType.Boom; }

    public float force;
    public float radius;

    protected override void OnTriggerDetail()
    {
        var colliders = GetNearbyBrickCollider(radius);

        foreach (var col in colliders)
        {
            var brick = col.GetComponentInParent<Brick>();

            brick.rigidbody.AddExplosionForce(force, transform.position, radius, 0f, ForceMode.Impulse);
        }
    }
}
