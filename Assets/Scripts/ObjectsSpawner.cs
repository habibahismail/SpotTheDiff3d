using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(RandomizeScaleRotation))]
public class ObjectsSpawner : MonoBehaviour
{
    [SerializeField] private List<ObjectsSO> ObjectsSOList;
    [SerializeField] protected int numItemsToSpawn = 10;

    [Header("Event Channels")]
    [SerializeField] private IslandSizeEventChannelSO islandCreated;
    [SerializeField] private ThemeChangeEventChannelSO themeChanged;

    [Space]
    [SerializeField] protected Transform spawnerOriginLand01;
    [SerializeField] protected Transform spawnerOriginLand02;
    [SerializeField] protected GameObject objectParentLand01;
    [SerializeField] protected GameObject objectParentLand02;
    [SerializeField] private LayerMask groundLayer;

    [Header("Object Spread Properties")]
    [SerializeField] protected float itemXSpread = 10;
    [SerializeField] protected float itemYSpread = 0;
    [SerializeField] protected float itemZSpread = 10;
    [SerializeField] private float itemSpreadDivisor = 2f;

    [Header ("Overlapped Checking Properties")]
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private float overlapTestBoxSize = 1f;
    [SerializeField] private LayerMask spawnedObjectLayer;

    protected GameObject theObjectToSpawn;
    protected Quaternion randYRotation;
    protected Vector3 randObjectScale;
    protected SIZE currentIslandSize;
    protected THEME currentIslandTheme;
    protected RandomizeScaleRotation randomizeScaleRotation;

    //temp variable
    protected int spawnedCount = 0;
    private int attempCount = 0;
    private readonly int maxAttemp = 5;


    private void Start()
    {
        randomizeScaleRotation = GetComponent<RandomizeScaleRotation>();

        themeChanged.OnEventRaised += SetCurrentIslandTheme;
        islandCreated.OnEventRaised += SpawnObjects;
    }

    private void OnDestroy()
    {
        themeChanged.OnEventRaised -= SetCurrentIslandTheme;
        islandCreated.OnEventRaised -= SpawnObjects;
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

    private void SpawnObjects(SIZE islandSize)
    {
        attempCount = 0;
        currentIslandSize = islandSize;
        float spreadValue = SetNewSpreadValue();
        
        itemXSpread = spreadValue;
        itemZSpread = spreadValue;

        DestroyAllInstanceOfObjects();

        Debug.Log("SpawnObjects() - current Island theme: " + currentIslandTheme);
        theObjectToSpawn = ChooseObjectToSpawn();

        while(attempCount < maxAttemp && spawnedCount == 0) {

            for (int i = 0; i < numItemsToSpawn; i++)
            {
                SpawnTheObjects();
            }

            attempCount++;
            //Debug.Log("Spawned Objects = " + spawnedCount + " | attempCount = " + attempCount);
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

    virtual protected void SpawnTheObjects()
    {

        //Generate the position to spawn on each island
        Vector3 randPosition = 
            new Vector3(Random.Range(-itemXSpread, itemXSpread), Random.Range(-itemYSpread, itemYSpread), Random.Range(-itemZSpread, itemZSpread));

        Vector3 land01SpawnPos = randPosition + spawnerOriginLand01.position;
        Vector3 land02SpawnPos = randPosition + spawnerOriginLand02.position;

        randomizeScaleRotation = GetComponent<RandomizeScaleRotation>();

        randYRotation = randomizeScaleRotation.RandomizeYRotation();
        randObjectScale = randomizeScaleRotation.RandomizeObjectScale(theObjectToSpawn);

        //Spawn on Land 01
        SpawnOnLand(land01SpawnPos, objectParentLand01.transform);

        //Spawn on Land 02
        SpawnOnLand(land02SpawnPos, objectParentLand02.transform);

    }

    protected void SpawnOnLand(Vector3 spawnPos, Transform objectParent)
    {

        RaycastHit hit;

        if (Physics.Raycast(spawnPos, Vector3.down, out hit, raycastDistance, groundLayer))
        {

            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            //Collider[] collidersInsideOverlapBox = new Collider[1];
            //int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapTestBoxScale, collidersInsideOverlapBox, spawnRotation, spawnedObjectLayer);

            //if (numberOfCollidersFound == 0)
            //{
            //    Pick(hit.point, spawnRotation, objectParent);
            //    spawnedCount++;
            //}

            Collider[] hitColliders = Physics.OverlapBox(hit.point, overlapTestBoxScale, spawnRotation, spawnedObjectLayer);

            if(hitColliders.Length == 0)
            {
                Pick(hit.point, spawnRotation, objectParent);
                spawnedCount++;
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
        List<ObjectsSO> objectSOtemp = new List<ObjectsSO>();
        GameObject objectPrefab = null;

        for (int i = 0; i < ObjectsSOList.Count; i++)
        {
     
            if (ObjectsSOList[i].Theme.CompareTo(currentIslandTheme) == 0)
            {
                objectSOtemp.Add(ObjectsSOList[i]);
            }
        }

        if (objectSOtemp.Count != 0)
        {

            int index = Random.Range(0, objectSOtemp.Count);
            int prefabIndex = Random.Range(0, objectSOtemp[index].Prefabs.Count);

            objectPrefab = objectSOtemp[index].Prefabs[prefabIndex];
        
        }

        return objectPrefab;
    }


    private float SetNewSpreadValue()
    {
        float spawnSpreadValue = 10f;

        switch ((int)currentIslandSize)
        {
            case 0: spawnSpreadValue = 60f / itemSpreadDivisor; break;
            case 1: spawnSpreadValue = 35f / itemSpreadDivisor; break;
            case 2: spawnSpreadValue = 20f / itemSpreadDivisor; break;
            default: break;

        }

        return spawnSpreadValue;

    }

    private void SetCurrentIslandTheme(THEME islandTheme)
    {
        currentIslandTheme = islandTheme;
    }
}