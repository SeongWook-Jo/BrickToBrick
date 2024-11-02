using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BrickManager : Singleton<BrickManager>
{
    public Brick[] bricks;
    public Brick[] specialBricks;

    private void Awake()
    {
        bricks = Resources.LoadAll<Brick>("Prefabs/NormalBricks");
        specialBricks = Resources.LoadAll<Brick>("Prefabs/SpecialBricks");
    }

    public (Brick, int) GetBrick()
    {
        var random = Random.Range(0, 100);

        if (random < 20)
            return GetSpecialBrick();
        else
            return GetNormalBrick();
    }

    public (Brick, int) GetSpecialBrick()
    {
        return (specialBricks[Random.Range(0, specialBricks.Length)], 0);
    }

    public (Brick, int) GetNormalBrick()
    {
        return (bricks[Random.Range(0, bricks.Length)], Random.Range(0, Brick.RandomColors.Length));
    }
}
