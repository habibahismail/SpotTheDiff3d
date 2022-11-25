using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that past float argument
/// </summary>

[CreateAssetMenu(fileName = "New Float Event Channel", menuName = "Events/Float Change Event Channel")]
public class FloatEventChannelSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void RaiseEvent(float amount)
    {
        OnEventRaised?.Invoke(amount);
    }
}
