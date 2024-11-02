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

            var newBrick = Instantiate(brick, iconObj.transform);

            newBrick.gameObject.layer = LayerMask.NameToLayer("UI");

            newBrick.rigidbody.isKinematic = true;

            newBrick.transform.localScale = new Vector3(70, 70, 70);
        }
    }

    public void RefreshEnemyBrickQueue()
    {
        ClearGrid(enemyBrickGrid.transform);

        var enemyQueue = _manager.EnemyBricks;

        foreach (var brick in enemyQueue)
        {
            var iconObj = Instantiate(brickIconPref, enemyBrickGrid.transform);

            var newBrick = Instantiate(brick, iconObj.transform);

            newBrick.gameObject.layer = LayerMask.NameToLayer("UI");

            newBrick.rigidbody.isKinematic = true;

            newBrick.transform.localScale = new Vector3(70, 70, 70);
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
