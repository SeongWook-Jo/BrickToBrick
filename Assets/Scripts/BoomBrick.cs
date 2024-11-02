using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BoomBrick : Brick
{
    public override BrickType Type { get => BrickType.Boom; }

    public float force;
    public float radius;

    private bool _isBoom;

    protected override void OnTriggerDetail()
    {
        if (_isBoom)
            return;

        var colliders = GetNearbyBrickCollider(radius);

        foreach (var col in colliders)
        {
            var brick = col.GetComponentInParent<Brick>();

            brick.rigidbody.AddExplosionForce(force, transform.position, radius, 3f, ForceMode.Impulse);
        }

        _isBoom = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
