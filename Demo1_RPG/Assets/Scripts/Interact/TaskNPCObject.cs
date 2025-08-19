using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskNPCObject : InteractableObject
{
    public string npcName;
    public Task task;

    public string[] dialogueExecuting;
    public string[] dialogueCompleted;
    public string[] dialogueEnd;

    private void Start()
    {
        task.State = TaskState.Waiting;
    }

    protected override void Interact()
    {
        switch (task.State)
        {
            case TaskState.Waiting:
                DialogueUI.Instance.Show(npcName, task.dialogueWaiting, OnDialogueEnd);
                break;
            case TaskState.Executing:
                DialogueUI.Instance.Show(npcName, dialogueExecuting, OnDialogueEnd);
                break;
            case TaskState.Completed:
                DialogueUI.Instance.Show(npcName, dialogueCompleted, OnDialogueEnd);
                break;
            case TaskState.End:
                DialogueUI.Instance.Show(npcName, dialogueEnd, OnDialogueEnd);
                break;
            default:
                break;
        }
    }

    public void OnDialogueEnd()
    {
        switch (task.State)
        {
            case TaskState.Waiting:
                task.Start();
                MessageUI.Instance.Show("你接取了一个任务！");
                break;
            case TaskState.Executing:
                break;
            case TaskState.Completed:
                task.End();
                MessageUI.Instance.Show("任务已完成！");
                break;
            case TaskState.End:
                break;
            default:
                break;
        }
    }
}
