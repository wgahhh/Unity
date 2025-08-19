using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskState
{
    Waiting,
    Executing,
    Completed,
    End
}

[CreateAssetMenu()]
public class Task : ScriptableObject
{
    public TaskState State;

    public string[] dialogueWaiting;

    public ItemSO startReward;
    public ItemSO endReward;

    public int enemyCountNeeded = 10;
    public int currentEnemyCount = 0;

    public void Start()
    {
        State = TaskState.Executing;
        InventoryManager.Instance.AddItem(startReward);
        currentEnemyCount = 0;
        EventCenter.OnEnemyDied += OnEnemyDied;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        if (State == TaskState.Completed) return;
        currentEnemyCount++;
        if (currentEnemyCount >= enemyCountNeeded)
        {
            State = TaskState.Completed;
            MessageUI.Instance.Show("任务完成条件已达成！");
        }
    }

    public void End()
    {
        State = TaskState.End;
        InventoryManager.Instance.AddItem(endReward);
        EventCenter.OnEnemyDied -= OnEnemyDied;
    }
}
