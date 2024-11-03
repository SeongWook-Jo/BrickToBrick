using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TangTangBrick : Brick
{
    public override BrickType Type { get => BrickType.TangTang; }

    public Texture tex;

    public override void InitDetail()
    {
        renderer.materials[0].SetTexture("_MainTex", tex);
    }

    protected override void SetFoceTex(Texture tex)
    {
        renderer.materials[0].SetTexture("_MainTex", tex);
    }



    int hitCountToShowFX = 3;
    int curHitCount = 0;
    protected override void OnCollisionDetail()
    {
        curHitCount += 1;
        if(curHitCount % hitCountToShowFX == 0)
        {
            FXManager.Instance.ShowFX(FXManager.FX.Boing, transform.position);
        }
    }
}
