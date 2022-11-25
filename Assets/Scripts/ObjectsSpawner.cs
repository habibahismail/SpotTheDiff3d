using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    [SerializeField] private List<ObjectsSO> ObjectsSOList;
    [SerializeField] private int numItemsToSpawn = 10;

    [SerializeField] private FloatEventChannelSO islandCreated;

    [SerializeField] private GameObject objectParentLand01;
    [SerializeField] private GameObject objectParentLand02;
    [SerializeField] private LayerMask groundLayer;

    [Space]
    [SerializeField] private Transform spawnerOriginLand01;
    [SerializeField] private Transform spawnerOriginLand02;

    [Space]
    [SerializeField] private float itemXSpread = 10;
    [SerializeField] private float itemYSpread = 0;
    [SerializeField] private float itemZSpread = 10;

    [Header ("Overlapped Checking")]
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private float overlapTestBoxSize = 1f;
    [SerializeField] private LayerMask spawnedObjectLayer;

    private GameObject theObjectToSpawn;
    private Quaternion randYRotation;

    void Start()
    {
        islandCreated.OnEventRaised += SpawnObjects;
    }

    private void SpawnObjects(float spreadValue)
    {
        itemXSpread = spreadValue;
        itemZSpread = spreadValue;

        DestroyAllInstanceOfObjects();
        theObjectToSpawn = ChooseObjectToSpawn();

        for (int i = 0; i < numItemsToSpawn; i++)
        {
            SpawnTheObjects();
        }
    }

    private void DestroyAllInstanceOfObjects()
    {
        if (objectParentLand01.transform.childCount > 0)
        {
            for (int i = 0; i < objectParentLand01.transform.childCount; i++)
            {
                Destroy(objectParentLand01.transform.GetChild(i).gameObject);
            }
        }

        if (objectParentLand02.transform.childCount > 0)
        {
            for (int i = 0; i < objectParentLand02.transform.childCount; i++)
            {
                Destroy(objectParentLand02.transform.GetChild(i).gameObject);
            }
        }
    }

    void SpawnTheObjects()
    {

        //Generate the position to spawn on each island
        Vector3 randPosition = 
            new Vector3(Random.Range(-itemXSpread, itemXSpread), Random.Range(-itemYSpread, itemYSpread), Random.Range(-itemZSpread, itemZSpread));

        Vector3 land01SpawnPos = randPosition + spawnerOriginLand01.position;
        Vector3 land02SpawnPos = randPosition + spawnerOriginLand02.position;

        randYRotation = RandomizeYRotation();

        //Spawn on Land 01
        SpawnOnLand(land01SpawnPos, objectParentLand01.transform);

        //Spawn on Land 02
        SpawnOnLand(land02SpawnPos, objectParentLand02.transform);

    }

    private void SpawnOnLand(Vector3 spawnPos, Transform objectParent)
    {

        RaycastHit hit;

        if (Physics.Raycast(spawnPos, Vector3.down, out hit, raycastDistance, groundLayer))
        {

            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            Collider[] collidersInsideOverlapBox = new Collider[1];
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapTestBoxScale, collidersInsideOverlapBox, spawnRotation, spawnedObjectLayer);

            if (numberOfCollidersFound == 0)
            {
                Pick(hit.point, spawnRotation, objectParent);
            }
            else
            {
                Debug.Log("name of collider 0 found " + collidersInsideOverlapBox[0].name);
            }
        }
    }

    private void Pick(Vector3 positionToSpawn, Quaternion rotationToSpawn, Transform objectParent)
    {
        GameObject gO = Instantiate(theObjectToSpawn, positionToSpawn, rotationToSpawn);
        gO.transform.parent = objectParent;
        
        gO.transform.localRotation = Quaternion.identity;
        gO.transform.localRotation = randYRotation;

    }

    private GameObject ChooseObjectToSpawn()
    {
        int index = Random.Range(0, ObjectsSOList.Count);
        int prefabIndex = Random.Range(0, ObjectsSOList[index].Prefabs.Count);

        GameObject objectPrefab = ObjectsSOList[index].Prefabs[prefabIndex];

        return objectPrefab;
    }

    private Quaternion RandomizeYRotation()
    {
        return Quaternion.Euler(0, Random.Range(0, 360), 0);
        
    }
}