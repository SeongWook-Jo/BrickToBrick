using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class Singleton <T>: MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null )
                {
                    var obj = Resources.Load<T>($"Prefabs/{typeof(T).Name}");
                    var tObj = Instantiate(obj);
                    instance = tObj;
                }
            }

            return instance;
        }
    }
}
