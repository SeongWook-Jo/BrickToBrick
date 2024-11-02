using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class StageManager : MonoBehaviour
{
    public Queue<(Brick, int)> PlayerBricks { get => _playerBricks; }
    public Queue<(Brick, int)> EnemyBricks { get => _enemyBricks; }

    private readonly int ShowBrickCount = 5;

    private StageUi _stageUi;

    public PlayerController player;
    public EnemyController enemy;

    private Queue<(Brick, int)> _playerBricks;
    private Queue<(Brick, int)> _enemyBricks;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _stageUi = FindObjectOfType<StageUi>();

        _stageUi.Init(this);

        _playerBricks = new Queue<(Brick, int)>();
        _enemyBricks = new Queue<(Brick, int)>();

        player.Init(this);

        enemy.Init(this);

        for (int i = 0; i < ShowBrickCount; i++)
        {
            _playerBricks.Enqueue(BrickManager.Instance.GetBrick());
            _enemyBricks.Enqueue(BrickManager.Instance.GetBrick());
        }

        _stageUi.RefreshPlayerBrickQueue();
        _stageUi.RefreshEnemyBrickQueue();
    }

    public (Brick, int) GetPlayerBrick()
    {
        var brick = _playerBricks.Dequeue();

        _playerBricks.Enqueue(BrickManager.Instance.GetBrick());

        _stageUi.RefreshPlayerBrickQueue();

        return brick;
    }

    public (Brick, int) GetEnemyBrick()
    {
        var brick = _enemyBricks.Dequeue();

        _enemyBricks.Enqueue(BrickManager.Instance.GetBrick());

        _stageUi.RefreshEnemyBrickQueue();

        return brick;
    }
}
