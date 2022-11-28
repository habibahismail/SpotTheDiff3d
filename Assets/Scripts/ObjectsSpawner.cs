using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    [SerializeField] private List<ObjectsSO> ObjectsSOList;
    [SerializeField] private int numItemsToSpawn = 10;

    [SerializeField] private FloatEventChannelSO islandCreated;

    [Space]
    [SerializeField] private Transform spawnerOriginLand01;
    [SerializeField] private Transform spawnerOriginLand02;
    [SerializeField] private GameObject objectParentLand01;
    [SerializeField] private GameObject objectParentLand02;
    [SerializeField] private LayerMask groundLayer;

    [Header("Object Spread Properties")]
    [SerializeField] private float itemXSpread = 10;
    [SerializeField] private float itemYSpread = 0;
    [SerializeField] private float itemZSpread = 10;

    [Header ("Overlapped Checking Properties")]
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private float overlapTestBoxSize = 1f;
    [SerializeField] private LayerMask spawnedObjectLayer;

    [Header("Randomize Scaling Properties")]
    [SerializeField] private bool scaleUniformly;
    
    [Space]
    [SerializeField] private float uniformScaleMin = .1f;
    [SerializeField] private float uniformScaleMax = 1f;
    
    [Space]
    [SerializeField] private float xScaleMin = .1f;
    [SerializeField] private float xScaleMax = 3f;
    [SerializeField] private float yScaleMin = .1f;
    [SerializeField] private float yScaleMax = 3f;
    [SerializeField] private float zScaleMin = .1f;
    [SerializeField] private float zScaleMax = 3f;


    private GameObject theObjectToSpawn;
    private Quaternion randYRotation;
    private Vector3 randObjectScale;

    //temp variable
    private int spawnedCount = 0;

    void Start()
    {
        islandCreated.OnEventRaised += SpawnObjects;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Draw a yellow wire cube at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnerOriginLand01.position, itemXSpread);
        Gizmos.DrawWireSphere(spawnerOriginLand02.position, itemXSpread);

    }

#endif

    private void SpawnObjects(float spreadValue)
    {
        itemXSpread = spreadValue;
        itemZSpread = spreadValue;

        DestroyAllInstanceOfObjects();
        theObjectToSpawn = ChooseObjectToSpawn();

           
            for (int i = 0; i < numItemsToSpawn; i++)
            {
                SpawnTheObjects();
                Debug.Log("Spawned Objects = " + spawnedCount);
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

        spawnedCount = 0;
    }

    private void SpawnTheObjects()
    {

        //Generate the position to spawn on each island
        Vector3 randPosition = 
            new Vector3(Random.Range(-itemXSpread, itemXSpread), Random.Range(-itemYSpread, itemYSpread), Random.Range(-itemZSpread, itemZSpread));

        Vector3 land01SpawnPos = randPosition + spawnerOriginLand01.position;
        Vector3 land02SpawnPos = randPosition + spawnerOriginLand02.position;

        randYRotation = RandomizeYRotation();
        randObjectScale = RandomizeObjectScale();

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
                spawnedCount++;
            }
            else
            {
               // Debug.Log("name of collider 0 found " + collidersInsideOverlapBox[0].name);
                
            }
        }
    }

    private void Pick(Vector3 positionToSpawn, Quaternion rotationToSpawn, Transform objectParent)
    {
        GameObject gO = Instantiate(theObjectToSpawn, positionToSpawn, rotationToSpawn);
        gO.transform.parent = objectParent;
        
        gO.transform.localRotation = Quaternion.identity;
        gO.transform.localRotation = randYRotation;
        gO.transform.localScale = randObjectScale;

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

    private Vector3 RandomizeObjectScale()
    {
        Vector3 randomizedScale = Vector3.one;
        if (scaleUniformly)
        {
            float uniformScale = Random.Range(uniformScaleMin, uniformScaleMax);
            randomizedScale = new Vector3(uniformScale, uniformScale, uniformScale);
        }
        else
        {
            randomizedScale = new Vector3(Random.Range(xScaleMin, xScaleMax), Random.Range(yScaleMin, yScaleMax), Random.Range(zScaleMin, zScaleMax));
        }

        float objectCurrentScale = theObjectToSpawn.transform.localScale.x;
        
        return randomizedScale * objectCurrentScale;
    }
}