using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New IslandBase", menuName = "Island/IslandBase")]
public class IslandBaseSO : ScriptableObject
{
    [SerializeField] private string objectName;

    public string ObjectName { get { return objectName; } }
    public THEME Theme;
    public SIZE IslandSize;
    public List<GameObject> Prefabs;
}

public enum SIZE { Large, Medium, Small }
