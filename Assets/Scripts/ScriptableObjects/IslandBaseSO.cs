using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New IslandBase", menuName = "Island/IslandBase")]
public class IslandBaseSO : ScriptableObject
{
    [SerializeField] private string objectName;

    public string ObjectName { get { return objectName; } }
    public Theme IslandTheme;
    public Size IslandSize;
    public List<GameObject> Prefabs;
}

public enum Size { LARGE, MEDIUM, SMALL }
public enum Theme { SUMMER, SPRING, AUTUMN, WINTER }
