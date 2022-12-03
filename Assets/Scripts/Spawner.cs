using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected List<ObjectsSO> ObjectsSOList;
    [SerializeField] protected IslandSizeEventChannelSO newIslandCreated;
    [SerializeField] protected ThemeChangeEventChannelSO themeChanged;

    [SerializeField] protected Transform spawnerOriginLand01;
    [SerializeField] protected Transform spawnerOriginLand02;
    [SerializeField] protected Transform objectParentL1;
    [SerializeField] protected Transform objectParentL2;
    [SerializeField] protected LayerMask groundLayer;

    [Header("Large Island Poisson Sampling Attributes")]
    [SerializeField] protected Vector2 zone_L = Vector2.one;
    [SerializeField] protected float radius_L = 1;
    [SerializeField] protected int k_L = 30;
    [SerializeField] protected float displayRadius_L;

    [Header("Medium Island Poisson Sampling Attributes")]
    [SerializeField] protected Vector2 zone_M = Vector2.one;
    [SerializeField] protected float radius_M = 1;
    [SerializeField] protected int k_M = 30;
    [SerializeField] protected float displayRadius_M;

    [Header("Small Island Poisson Sampling Attributes")]
    [SerializeField] protected Vector2 zone_S = Vector2.one;
    [SerializeField] protected float radius_S = 1;
    [SerializeField] protected int k_S = 30;
    [SerializeField] protected float displayRadius_S;

    [Header("Overlapped Checking Properties")]
    [SerializeField] protected float raycastDistance = 100f;
    [SerializeField] protected float overlapTestBoxSize = 1f;
    [SerializeField] protected LayerMask spawnedObjectLayer;

    [Header("Randomize Scaling Properties")]
    [SerializeField] protected bool scaleUniformly;

    [Space]
    [SerializeField] protected float uniformScaleMin = .1f;
    [SerializeField] protected float uniformScaleMax = 1f;

    [Space]
    [SerializeField] protected float xScaleMin = .1f;
    [SerializeField] protected float xScaleMax = 3f;
    [SerializeField] protected float yScaleMin = .1f;
    [SerializeField] protected float yScaleMax = 3f;
    [SerializeField] protected float zScaleMin = .1f;
    [SerializeField] protected float zScaleMax = 3f;


    private List<Vector2> samples;
    private GameObject theObjectToSpawn;

    private Vector3 largeIslandSpawnPoint = new Vector3(-25, 30, -25);
    private Vector3 mediumIslandSpawnPoint = new Vector3(-12, 30, -12);
    private Vector3 smallIslandSpawnPoint = new Vector3(-7, 30, -7);

    private SIZE currentIslandSize;

    private Quaternion randYRotation;
    private Vector3 randObjectScale;
    private Vector3 currentSpawnPoint;

    private Vector2 zone;
    private float radius;
    private int k;
    private float displayRadius;

    protected THEME currentIslandTheme;

    private void Start()
    {
        //SpawnObjects();

        themeChanged.OnEventRaised += SetCurrentIslandTheme;
        newIslandCreated.OnEventRaised += SpawnObjects;
     
    }

    private void SpawnObjects(SIZE islandSize)
    {
        DestroyAllInstanceOfObjects();

        currentIslandSize = islandSize;
        
        SetSpawnPoint();
        SetPoissonSamplingValues();

        samples = PoissonDiscSampling.GeneratePoint(radius, zone, k);

        if (samples != null)
        {

            theObjectToSpawn = ChooseObjectToSpawn();

            foreach (Vector2 sample in samples)
            {
                
                Vector3 land01SpawnPos = new Vector3(sample.x, 0, sample.y) + spawnerOriginLand01.position;
                Vector3 land02SpawnPos = new Vector3(sample.x, 0, sample.y) + spawnerOriginLand02.position;

                randYRotation = RandomizeYRotation();
                randObjectScale = RandomizeObjectScale();

                //Spawn on Land 01
                SpawnOnLand(land01SpawnPos, objectParentL1.transform);

                //Spawn on Land 02
                SpawnOnLand(land02SpawnPos, objectParentL2.transform);
            }
        }
    }

 
    private void DestroyAllInstanceOfObjects()
    {
        if (objectParentL1.transform.childCount > 0)
        {
            for (int i = 0; i < objectParentL1.transform.childCount; i++)
            {
                Destroy(objectParentL1.transform.GetChild(i).gameObject);
            }
        }

        if (objectParentL2.transform.childCount > 0)
        {
            for (int i = 0; i < objectParentL2.transform.childCount; i++)
            {
                Destroy(objectParentL2.transform.GetChild(i).gameObject);
            }
        }

    }

    private void SetSpawnPoint()
    {
        switch ((int)currentIslandSize)
        {

            case 0:
                spawnerOriginLand01.localPosition = largeIslandSpawnPoint;
                spawnerOriginLand02.localPosition = largeIslandSpawnPoint;
                break;
            case 1:
                spawnerOriginLand01.localPosition = mediumIslandSpawnPoint;
                spawnerOriginLand02.localPosition = mediumIslandSpawnPoint;
                break;
            case 2:
                spawnerOriginLand01.localPosition = smallIslandSpawnPoint;
                spawnerOriginLand02.localPosition = smallIslandSpawnPoint;
                break;
            default:
                spawnerOriginLand01.localPosition = largeIslandSpawnPoint;
                spawnerOriginLand02.localPosition = largeIslandSpawnPoint;
                break;

        }

    }

    private void SetPoissonSamplingValues()
    {
        switch ((int)currentIslandSize)
        {

            case 0:
                zone = zone_L;
                radius = radius_L;
                k = k_L;
                displayRadius = displayRadius_L;
                break;

            case 1:
                zone = zone_M;
                radius = radius_M;
                k = k_M;
                displayRadius = displayRadius_M;
                break;

            case 2:
                zone = zone_S;
                radius = radius_S;
                k = k_S;
                displayRadius = displayRadius_S;
                break;

            default:
                zone = zone_L;
                radius = radius_L;
                k = k_L;
                displayRadius = displayRadius_L;
                break;

        }

    }


    private void SpawnOnLand(Vector3 spawnPos, Transform objectParent)
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
            //}
           

            Collider[] hitColliders = Physics.OverlapBox(hit.point, overlapTestBoxScale, spawnRotation, spawnedObjectLayer);

            if (hitColliders.Length == 0)
            {
                Pick(hit.point, spawnRotation, objectParent);
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

            int index = Random.Range(0, objectSOtemp.Count-1);
            int prefabIndex = Random.Range(0, objectSOtemp[index].Prefabs.Count);

            objectPrefab = objectSOtemp[index].Prefabs[prefabIndex];

        }

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

    private void SetCurrentIslandTheme(THEME islandTheme)
    {
        currentIslandTheme = islandTheme;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((new Vector3(zone_L.x, 0, zone_L.y) / 2) + transform.position, new Vector3(zone_L.x, 0, zone_L.y));

    }

}