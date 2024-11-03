using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUi : MonoBehaviour
{
    public GameObject howtoplayObj;
    public GameObject creditObj;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        howtoplayObj.gameObject.SetActive(false);
        creditObj.gameObject.SetActive(false);
    }

    public void OnClickShowHowToPlay()
    { 
        howtoplayObj.gameObject.SetActive(true);
    }

    public void OnClickHideHowToPlay()
    {
        howtoplayObj.gameObject.SetActive(false);
    }

    public void OnClickGameStart()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnClickShowCredit()
    {
        creditObj.gameObject.SetActive(true);
    }

    public void OnClickHideCredit()
    {
        creditObj.gameObject.SetActive(false);
    }
}
