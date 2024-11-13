using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    public KeyCode Up { get; private set; }
    public KeyCode Down { get; private set; }
    public KeyCode Shot { get; private set; }

    public PlayerInput(KeyCode up, KeyCode down, KeyCode shot)
    {
        Up = up;
        Down = down;
        Shot = shot;
    }
}
