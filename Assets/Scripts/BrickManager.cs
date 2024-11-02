using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BrickManager : Singleton<BrickManager>
{
    public Brick[] bricks;

    private void Awake()
    {
        bricks = Resources.LoadAll<Brick>("NormalBricks");
    }

    public Brick GetNewNormalBrick()
    {
        return bricks[Random.Range(0, bricks.Length)];
    }
}
