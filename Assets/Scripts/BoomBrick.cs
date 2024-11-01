using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BoomBrick : Brick
{
    public float force;
    public float radius;

    protected virtual void OnTrigger()
    {
        if (IsLaunched == false)
            return;

        var colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var col in colliders)
        {
            if (col.CompareTag("Brick") == false)
                continue;

            var brick = col.GetComponent<Brick>();

            brick.rigidbody.AddExplosionForce(force, transform.position, radius, 0f, ForceMode.Impulse);
        }
    }
}
