using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will handle all the works to generate the island.
/// </summary>

public class IslandGenerator : MonoBehaviour
{
    [SerializeField] private GameObject island01;
    [SerializeField] private GameObject island02;
    [SerializeField] private FloatEventChannelSO islandChangeSizeEventChannel;
    [SerializeField] private FloatEventChannelSO spreadValueIslandChangeEC;

    private IslandBase islandBase;
    private GameObject chosenIslandBasePrefab;
    
    private GameObject baseIsland01;
    private GameObject baseIsland02;
    private SIZE currentIslandSize;


    private void Start()
    {
        islandBase = GetComponent<IslandBase>();
        GenerateIsland();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Destroy(baseIsland01);
            Destroy(baseIsland02);

            GenerateIsland();
        }
    }

    private void GenerateIsland()
    {
        chosenIslandBasePrefab = islandBase.ChooseIslandBase();

        baseIsland01 = Instantiate(chosenIslandBasePrefab, Vector3.zero, Quaternion.identity);
        baseIsland02 = Instantiate(chosenIslandBasePrefab, Vector3.zero, Quaternion.identity);

        baseIsland01.transform.parent = island01.transform;
        baseIsland02.transform.parent = island02.transform;

        baseIsland01.transform.localPosition = Vector3.zero;
        baseIsland02.transform.localPosition = Vector3.zero;

        baseIsland01.transform.localRotation = Quaternion.identity;
        baseIsland02.transform.localRotation = Quaternion.identity;

        currentIslandSize = islandBase.CurrentIslandBaseSO.IslandSize;

        ChangeCameraDistanceAccordingToIslandSize();
        PassNewSpawnSpreadValue();
        
    }

    private void ChangeCameraDistanceAccordingToIslandSize()
    {
        float _cameraDistance = 35f;

        switch ((int)currentIslandSize)
        {
            case 0: _cameraDistance = 50f; break;
            case 1: _cameraDistance = 35f; break;
            case 2: _cameraDistance = 20f; break;
            default: break;

        }
        
        islandChangeSizeEventChannel.RaiseEvent(_cameraDistance);
    }

    private void PassNewSpawnSpreadValue()
    {
        float spawnSpreadValue = 20f;

        switch ((int)currentIslandSize)
        {
            case 0: spawnSpreadValue = 60f - 6f; break;
            case 1: spawnSpreadValue = 35f - 4f; break;
            case 2: spawnSpreadValue = 20f - 2f ; break;
            default: break;

        }

        spreadValueIslandChangeEC.RaiseEvent(spawnSpreadValue);
    }
}
