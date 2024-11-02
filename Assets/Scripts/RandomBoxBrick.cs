using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RandomBoxBrick : Brick
{
    public override BrickType Type { get => BrickType.RandomBox; }

    public Texture tex;

    public override void InitDetail()
    {
        renderer.materials[0].SetTexture("_MainTex", tex);
    }

}
