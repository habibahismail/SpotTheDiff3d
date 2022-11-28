using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that past Island Size
/// </summary>

[CreateAssetMenu(fileName = "New Island Size Event Channel", menuName = "Events/Island Size Changed Event")]
public class IslandSizeEventChannelSO : ScriptableObject
{
    public UnityAction<SIZE> OnEventRaised;

    public void RaiseEvent(SIZE islandSize)
    {
        OnEventRaised?.Invoke(islandSize);
    }
}
