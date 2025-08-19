using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;


    //场景加载请求
    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScene)//要加载的场景，player的坐标，是否渐入渐出
    {
        LoadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScene);

    }
}