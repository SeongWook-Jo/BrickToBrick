using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAnimationFrameController : MonoBehaviour
{
    bool isReadyToThrow = false;
    public bool IsReadyToThrow { get { return isReadyToThrow; } }

    public void ReadyToThrow()
    {
        isReadyToThrow = true;
    }

    public void FinishThrowing()
    {
        isReadyToThrow = false;
    }
}
