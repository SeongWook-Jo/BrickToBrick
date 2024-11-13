using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUi : MonoBehaviour
{
    public GameObject mainObj;
    public GameObject difficultyObj;
    public GameObject howtoplayObj;
    public GameObject creditObj;
    public GameObject bricksRootObj;

    public List<Brick> brickList;

    private void Awake()
    {
        Init();
        SoundManager.Instance.PlayBGM(SoundManager.BGM.Looby, true);
    }

    private void Init()
    {
        brickList = new List<Brick>();

        howtoplayObj.SetActive(false);
        creditObj.SetActive(false);
        difficultyObj.SetActive(false);

        var childCnt = bricksRootObj.transform.childCount;

        for (int i = 0; i < childCnt; i++)
        {
            var brick = bricksRootObj.transform.GetChild(i).GetComponent<Brick>();
            brick.SetColor(Random.Range(0, Brick.RandomColors.Length));
            brick.rigidbody.drag = Random.Range(0, 6);
            brick.transform.localPosition = new Vector3(Random.Range(-800f, 800f), 720, 0);

            if (i != 0)
            {
                var currRot = brick.transform.rotation.eulerAngles;
                brick.transform.rotation = Quaternion.Euler(currRot.x, Random.Range(0f, 360f), currRot.z);
                brick.transform.rotation = Quaternion.Euler(currRot.x, currRot.y, Random.Range(0f, 360f));
            }

            brickList.Add(brick);
        }
    }

    public void OnClickShowHowToPlay()
    { 
        howtoplayObj.SetActive(true);
    }

    public void OnClickHideHowToPlay()
    {
        howtoplayObj.SetActive(false);
    }

    public void OnClickGameStart()
    {
        mainObj.SetActive(false);
        difficultyObj.SetActive(true);
    }

    public void OnClickEasy()
    {
        StageManager.MultiType = StageManager.GameMultiType.Single;
        StageManager.Difficulty = StageManager.DifficultyLevel.Easy;
        SceneManager.LoadScene("Main");
    }

    public void OnClickHard()
    {
        StageManager.MultiType = StageManager.GameMultiType.Single;
        StageManager.Difficulty = StageManager.DifficultyLevel.Hard;
        SceneManager.LoadScene("Main");
    }

    public void OnClickMultiPlay()
    {
        StageManager.MultiType = StageManager.GameMultiType.Multi;
        SceneManager.LoadScene("Main");
    }

    public void OnClickBackBtn()
    {
        mainObj.SetActive(true);
        difficultyObj.SetActive(false);
    }

    public void OnClickShowCredit()
    {
        creditObj.SetActive(true);
    }

    public void OnClickHideCredit()
    {
        creditObj.SetActive(false);
    }

    private void Update()
    {
        foreach(var brick in brickList)
        {
            if (brick.transform.localPosition.y < -650)
            {
                var currPos = brick.transform.localPosition;
                brick.transform.localPosition = new Vector3(currPos.x, 720f, 0);
                brick.rigidbody.velocity = Vector3.zero;
            }
        }
    }
}
