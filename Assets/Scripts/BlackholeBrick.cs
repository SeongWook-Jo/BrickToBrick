using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BlackholeBrick : Brick
{
    public override BrickType Type { get => BrickType.Blackhole; }

    public Texture tex;

    public float blackholeTime;
    public float pullForce;
    public float radius;

    private float _currTime;

    public override void InitDetail()
    {
        renderer.materials[0].SetTexture("_MainTex", tex);
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

            brick.rigidbody.AddForce(dir * pullForce);
        }
    }

    protected override void SetFoceTex(Texture tex)
    {
        renderer.materials[0].SetTexture("_MainTex", tex);
    }


    AudioSource blackHoleAudio = null;
    protected override void LaunchDetail()
    {
        FXManager.Instance.ShowFX(FXManager.FX.BlackHole, transform);
        blackHoleAudio = SoundManager.Instance.PlaySFX(SoundManager.SFX.BlackHole, true);
        if(blackHoleAudio != null)
        {
            Invoke("StopAudioSound", blackholeTime);
        }
    }

    private void OnDisable()
    {
        StopAudioSound();
    }

    void StopAudioSound()
    {
        if (blackHoleAudio != null)
            blackHoleAudio.Stop();
    }
}
