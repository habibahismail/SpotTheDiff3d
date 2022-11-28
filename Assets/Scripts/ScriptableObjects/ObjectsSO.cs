using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object" , menuName = "Island/Objects")]
public class ObjectsSO : ScriptableObject
{
    public string ObjectName;
    public SEASONS Seasons;
    public OBJECT_TYPE ObjectType;
    public List<GameObject> Prefabs;
}

public enum SEASONS { summer, spring, autumn, winter }
public enum OBJECT_TYPE { tree, rock, structure, vegetation }
