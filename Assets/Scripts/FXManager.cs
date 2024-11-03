using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : Singleton<FXManager>
{
    public enum FX
    {
        Boing = 0,
        Boom,
        BlackHole,
    }

    [SerializeField]
    GameObject[] fxPrefabs;

    Dictionary<GameObject, List<GameObject>> objPoolDic = new Dictionary<GameObject, List<GameObject>>();


    public GameObject ShowFX(FX _fxName, Vector3 _spawnGlobalPos)
    {
        int tempTargetIndex = (int)_fxName;
        GameObject fxObj = Get(fxPrefabs[tempTargetIndex], _spawnGlobalPos);

        return fxObj;
    }

    public GameObject ShowFX(FX _fxName, Transform _parentTransform)
    {
        int tempTargetIndex = (int)_fxName;
        GameObject fxObj = Get(fxPrefabs[tempTargetIndex], _parentTransform);

        return fxObj;
    }

    GameObject Get(GameObject _obj, Vector3 _globalStartPos)
    {
        GameObject selected = null;
        List<GameObject> tempList;
        bool ret = objPoolDic.TryGetValue(_obj, out tempList);
        if (ret == false)
        {
            //아직 딕셔너리에 생성한적 없으면 새로 생성해주기
            tempList = new List<GameObject>();
            objPoolDic.Add(_obj, tempList);
        }

        foreach (GameObject item in tempList)
        {
            if (item != null && item.activeSelf == false)
            {
                selected = item;
                selected.transform.position = _globalStartPos;

                selected.SetActive(true);
                break;
            }
        }

        if (!selected)
        {
            if (_obj != null)
                selected = Instantiate(_obj, transform);

            if (selected != null)
            {
                tempList.Add(selected);
                selected.transform.position = _globalStartPos;

                selected.SetActive(true);
            }
        }

        return selected;
    }

    GameObject Get(GameObject _obj, Transform _parentTransform)
    {
        GameObject selected = null;
        List<GameObject> tempList;
        bool ret = objPoolDic.TryGetValue(_obj, out tempList);
        if (ret == false)
        {
            //아직 딕셔너리에 생성한적 없으면 새로 생성해주기
            tempList = new List<GameObject>();
            objPoolDic.Add(_obj, tempList);
        }

        foreach (GameObject item in tempList)
        {
            if (item != null && item.activeSelf == false)
            {
                selected = item;
                selected.SetActive(false);
                selected.transform.SetParent(_parentTransform);
                selected.transform.localPosition = Vector3.zero;
                selected.transform.localRotation = Quaternion.identity;

                selected.SetActive(true);
                break;
            }
        }

        if (!selected)
        {
            if (_obj != null)
                selected = Instantiate(_obj, transform);

            if (selected != null)
            {
                tempList.Add(selected);
                selected.transform.SetParent(_parentTransform);
                selected.transform.localPosition = Vector3.zero;
                selected.transform.localRotation = Quaternion.identity;

                selected.SetActive(true);
            }
        }

        return selected;
    }

    public void CollectAllFXs()
    {
        foreach (var dic in objPoolDic)
        {
            for (int i = 0; i < dic.Value.Count; i++)
            {
                dic.Value[i].gameObject.SetActive(false);
                dic.Value[i].transform.SetParent(transform);
            }
        }
    }
}
