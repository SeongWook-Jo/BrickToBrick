using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ChangeBrick : Brick
{
    public override BrickType Type { get => BrickType.Chnage; }

    public Texture tex;

    public float radius;

    public bool _isChange;

    public override void InitDetail()
    {
        renderer.materials[0].SetTexture("_MainTex", tex);

        _isChange = false;
    }

    protected override void OnCollisionDetail()
    {
        if (_isChange)
            return;

        var colliders = GetNearbyBrickCollider(radius);

        var tempBricks = new HashSet<Brick>();

        foreach (var col in colliders)
        {
            var brick = col.GetComponentInParent<Brick>();

            if (tempBricks.Contains(brick) == false)
                tempBricks.Add(brick);
        }

        foreach(var brick in tempBricks)
        {
            var pos = brick.transform.position;

            Destroy(brick.gameObject);

            var newBrick = BrickManager.Instance.GetNormalBrick();

            var instance = Instantiate(newBrick.Item1);

            instance.transform.position = pos;

            instance.Init();

            instance.SetColor(newBrick.Item2);

            instance.ChageBrick();
        }

        _isChange = true;
    }
}
