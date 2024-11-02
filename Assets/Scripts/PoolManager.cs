using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    Dictionary<GameObject, List<GameObject>> objPoolDic = new Dictionary<GameObject, List<GameObject>>();


    public void Init_Pool()
    {
        foreach (var dic in objPoolDic)
        {
            //딕셔너리 value 제거
            for (int i = dic.Value.Count - 1; i >= 0 ; i--)
            {
                if (dic.Value[i] != null)
                {
                    dic.Value[i].SetActive(false);
                    Destroy(dic.Value[i]);
                }                
            }
            
            dic.Value.Clear();
            //key는 Prefab이니깐 삭제ㄴㄴ
            //dic.Key.SetActive(false);
            //Destroy(dic.Key);
        }

        objPoolDic.Clear();
    }

    public GameObject Get(GameObject _obj, Vector3 _globalStartPos)
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

    public GameObject Get(GameObject _obj, Transform _parentTransform)
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

    public List<GameObject> FindObjectListFromPool_ByName(string _objName)
    {
        List<GameObject> tempList = null;
        foreach (var dic in objPoolDic)
        {
            if (dic.Key.name == _objName)
            {
                tempList = dic.Value;
                break;
            }
        }

        return tempList;
    }

    public void RemoveFromPool(GameObject _gameObj)
    {
        foreach (var item in objPoolDic)
        {
            if(item.Key == _gameObj)
            {
                for (int i = item.Value.Count - 1; i >= 0; i--)
                {
                    item.Value[i].SetActive(false);
                    Destroy(item.Value[i]);
                }
                item.Value.Clear();
                objPoolDic.Remove(item.Key);
                break;
            }
        }
    }

    public void BackToPoolGroup(GameObject _obj)
    {
        _obj.transform.SetParent(transform);        
    }
}
