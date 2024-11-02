using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BrickManager : Singleton<BrickManager>
{
    public Mesh[] meshs;

    private void Awake()
    {
        var models = Resources.LoadAll<Mesh>("Model");

        meshs = new Mesh[models.Length];

        for (int i = 0; i < models.Length;i++)
            meshs[i] = models[i];
    }

    public Mesh GetNormalBrickMesh()
    {
        return meshs[Random.Range(0, meshs.Length)];
    }
}
