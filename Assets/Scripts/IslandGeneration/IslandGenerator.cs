using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will handle all works to generate new island.
/// </summary>

public class IslandGenerator : MonoBehaviour
{
    [SerializeField] private GameObject island01;
    [SerializeField] private GameObject island02;

    [Header("Event Channels")]
    [SerializeField] private IslandSizeEventChannelSO islandChangeSizeEventChannel;
    [SerializeField] private ThemeChangeEventChannelSO themeChangeEventChannel;
    
    private GameObject baseIsland01;
    private GameObject baseIsland02;
    private Size currentIslandSize;
    private Theme currentIslandTheme;

    private IslandBase islandBase;
    private TerrainSpawner terrainSpawner;
    private BigObjectsSpawer bigObjectSpawner;

    private ObjectsSpawner treeSpawner;

    private GameObject chosenIslandBasePrefab;
 

    private void Start()
    {
        islandBase = GetComponent<IslandBase>();
        terrainSpawner = GetComponentInChildren<TerrainSpawner>();
        bigObjectSpawner = GameObject.Find("BigStuffObjectsSpawner").GetComponent<BigObjectsSpawer>();
        treeSpawner = GameObject.Find("TreeSpawner").GetComponent<ObjectsSpawner>();

        GenerateIsland();
        GenerateBigStuffs();
        GenerateTrees();

        RotateIslandTo45deg();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Destroy(baseIsland01);
            Destroy(baseIsland02);

            GenerateIsland();
            GenerateBigStuffs();
            GenerateTrees();

            RotateIslandTo45deg();
        }
    }

    private void GenerateIsland()
    {
        island01.transform.rotation = Quaternion.identity;
        island02.transform.rotation = Quaternion.identity;

        chosenIslandBasePrefab = islandBase.ChooseIslandBase();
        currentIslandSize = islandBase.CurrentIslandBaseSO.IslandSize;
        currentIslandTheme = islandBase.CurrentIslandBaseSO.IslandTheme;

        baseIsland01 = Instantiate(chosenIslandBasePrefab, Vector3.zero, Quaternion.identity);
        baseIsland02 = Instantiate(chosenIslandBasePrefab, Vector3.zero, Quaternion.identity);

        baseIsland01.transform.parent = island01.transform;
        baseIsland02.transform.parent = island02.transform;

        baseIsland01.transform.localPosition = Vector3.zero;
        baseIsland02.transform.localPosition = Vector3.zero;

        baseIsland01.transform.localRotation = Quaternion.identity;
        baseIsland02.transform.localRotation = Quaternion.identity;

        themeChangeEventChannel.RaiseEvent(currentIslandTheme);
        islandChangeSizeEventChannel.RaiseEvent(currentIslandSize);
        
    }

    private void GenerateBigStuffs()
    {
        terrainSpawner.SpawnObjects(currentIslandSize, currentIslandTheme);
        bigObjectSpawner.SpawnObjects(currentIslandSize,currentIslandTheme);
    }

    private void GenerateTrees()
    {
        treeSpawner.SpawnObjects(currentIslandSize, currentIslandTheme);
    }

    private void RotateIslandTo45deg()
    {
        island01.transform.rotation = Quaternion.Euler(0f, 45f, 0f);
        island02.transform.rotation = Quaternion.Euler(0f, 45f, 0f);

    }

    
}
