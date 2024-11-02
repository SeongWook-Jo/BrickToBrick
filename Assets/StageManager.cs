using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class StageManager : MonoBehaviour
{
    public Queue<Brick> PlayerBricks { get => _playerBricks; }
    public Queue<Brick> EnemyBricks { get => _enemyBricks; }

    private readonly int ShowBrickCount = 5;

    private StageUi _stageUi;

    public PlayerController player;
    public EnemyController enemy;

    private Queue<Brick> _playerBricks;
    private Queue<Brick> _enemyBricks;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _stageUi = FindObjectOfType<StageUi>();

        _stageUi.Init(this);

        _playerBricks = new Queue<Brick>();
        _enemyBricks = new Queue<Brick>();

        player.Init(this);

        enemy.Init(this);

        for (int i = 0; i < ShowBrickCount; i++)
        {
            _playerBricks.Enqueue(BrickManager.Instance.GetNewNormalBrick());
            _enemyBricks.Enqueue(BrickManager.Instance.GetNewNormalBrick());
        }

        _stageUi.RefreshPlayerBrickQueue();
        _stageUi.RefreshEnemyBrickQueue();
    }

    public Brick GetPlayerBrick()
    {
        var brick = _playerBricks.Dequeue();

        _playerBricks.Enqueue(BrickManager.Instance.GetNewNormalBrick());

        _stageUi.RefreshPlayerBrickQueue();

        return brick;
    }

    public Brick GetEnemyBrick()
    {
        var brick = _enemyBricks.Dequeue();

        _enemyBricks.Enqueue(BrickManager.Instance.GetNewNormalBrick());

        _stageUi.RefreshEnemyBrickQueue();

        return brick;
    }
}
