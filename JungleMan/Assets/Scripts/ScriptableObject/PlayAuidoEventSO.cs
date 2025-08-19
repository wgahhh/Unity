using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/PlayAudioEventSO")]
public class PlayAuidoEventSO : ScriptableObject
{
    public UnityAction<AudioClip> OnEventRaised;

    public void RaiseEvent(AudioClip audioClip)
    {
        OnEventRaised?.Invoke(audioClip);
    }
}
