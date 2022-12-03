using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that past Island Size
/// </summary>

[CreateAssetMenu(fileName = "New ThemeChange Event Channel", menuName = "Events/Theme Changed Event")]
public class ThemeChangeEventChannelSO : ScriptableObject
{
    public UnityAction<Theme> OnEventRaised;

    public void RaiseEvent(Theme theme)
    {
        OnEventRaised?.Invoke(theme);
    }
}
