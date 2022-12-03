using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object" , menuName = "Island/Objects")]
public class ObjectsSO : ScriptableObject
{
    public string ObjectName;
    public Theme IslandTheme;
    public ObjectType ObjectType;
    public List<GameObject> Prefabs;
}

public enum ObjectType { ANY, BIG_TYPE, MED_TYPE }


