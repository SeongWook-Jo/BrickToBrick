using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUi : MonoBehaviour
{
    public GameObject brickIconPref;

    public GridLayoutGroup playerBrickGrid;
    public GridLayoutGroup enemyBrickGrid;

    private StageManager _manager;

    public void Init(StageManager manager)
    {
        _manager = manager;
    }

    public void RefreshPlayerBrickQueue()
    {
        ClearGrid(playerBrickGrid.transform);

        var playerQueue = _manager.PlayerBricks;

        foreach (var brick in playerQueue)
        {
            var iconObj = Instantiate(brickIconPref, playerBrickGrid.transform);

            var newBrick = Instantiate(brick.Item1, iconObj.transform);

            newBrick.gameObject.layer = LayerMask.NameToLayer("UI");

            newBrick.rigidbody.isKinematic = true;

            newBrick.transform.localScale = Vector3.one * 70f * newBrick.iconScaleFactor;

            newBrick.Init();

            newBrick.SetColor(brick.Item2);
        }
    }

    public void RefreshEnemyBrickQueue()
    {
        ClearGrid(enemyBrickGrid.transform);

        var enemyQueue = _manager.EnemyBricks;

        foreach (var brick in enemyQueue)
        {
            var iconObj = Instantiate(brickIconPref, enemyBrickGrid.transform);

            var newBrick = Instantiate(brick.Item1, iconObj.transform);

            newBrick.gameObject.layer = LayerMask.NameToLayer("UI");

            newBrick.rigidbody.isKinematic = true;

            newBrick.transform.localScale = Vector3.one * 70f * newBrick.iconScaleFactor;

            newBrick.Init();

            newBrick.SetColor(brick.Item2);
        }
    }

    private void ClearGrid(Transform tran)
    {
        for (int i = tran.childCount - 1; i >= 0; i--)
        {
            var child = tran.GetChild(i);

            Destroy(child.gameObject);
        }
    }
}
