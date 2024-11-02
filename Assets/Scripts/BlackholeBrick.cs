using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BlackholeBrick : Brick
{
    public override BrickType Type { get => BrickType.Blackhole; }

    public float blackholeTime;
    public float pullForce;
    public float radius;

    private float _currTime;

    public override void InitDetail()
    {
        _currTime = 0;
    }

    private void FixedUpdate()
    {
        if (IsLaunched == false)
            return;

        if (_currTime > blackholeTime)
            return;

        _currTime += Time.fixedDeltaTime;

        var colliders = GetNearbyBrickCollider(radius);

        foreach (var col in colliders)
        {
            var brick = col.GetComponentInParent<Brick>();

            var dir = transform.position - col.transform.position;

            dir.Normalize();

            brick.Rigidbody.AddForce(dir * pullForce);
        }
    }
}
