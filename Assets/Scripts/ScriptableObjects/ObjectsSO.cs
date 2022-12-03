using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object" , menuName = "Island/Objects")]
public class ObjectsSO : ScriptableObject
{
    public string ObjectName;
    public THEME Theme;
    public OBJECT_TYPE ObjectType;
    public List<GameObject> Prefabs;
}

public enum THEME { summer, spring, autumn, winter, beach, town }
public enum OBJECT_TYPE { tree, rock, structure, vegetation }
