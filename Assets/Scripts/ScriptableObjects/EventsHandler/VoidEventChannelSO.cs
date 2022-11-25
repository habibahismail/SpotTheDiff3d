using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments
/// </summary>

[CreateAssetMenu(fileName = "New Void Event Channel", menuName = "Events/Void Event Channel")]
public class VoidEventChannelSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
