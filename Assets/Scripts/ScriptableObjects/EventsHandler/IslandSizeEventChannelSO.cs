using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that past Island Size
/// </summary>

[CreateAssetMenu(fileName = "New Island Size Event Channel", menuName = "Events/Island Size Changed Event")]
public class IslandSizeEventChannelSO : ScriptableObject
{
    public UnityAction<Size> OnEventRaised;

    public void RaiseEvent(Size islandSize)
    {
        OnEventRaised?.Invoke(islandSize);
    }
}
