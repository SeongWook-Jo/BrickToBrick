using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCollision : MonoBehaviour
{
    private Brick _manager;

    public void Init(Brick manager)
    {
        _manager = manager;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _manager.OnCollision(collision);
    }
}
