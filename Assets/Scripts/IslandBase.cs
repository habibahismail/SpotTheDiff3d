using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandBase : MonoBehaviour
{
    [SerializeField] private List<IslandBaseSO> allIslandBaseSO;

    public IslandBaseSO CurrentIslandBaseSO { get; private set; }

    public GameObject ChooseIslandBase()
    {
        int index = Random.Range(0, allIslandBaseSO.Count);
        int prefabIndex = Random.Range(0, allIslandBaseSO[index].Prefabs.Count);

        CurrentIslandBaseSO = allIslandBaseSO[index];

        GameObject islandBasePrefab = allIslandBaseSO[index].Prefabs[prefabIndex];

        return islandBasePrefab;
    }

}
