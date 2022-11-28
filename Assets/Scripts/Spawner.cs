using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected List<ObjectsSO> ObjectsSOList;
    [SerializeField] protected IslandSizeEventChannelSO newIslandCreated;

    [SerializeField] protected Transform spawnerOriginLand01;
    [SerializeField] protected Transform spawnerOriginLand02;
    [SerializeField] protected Transform objectParentL1;
    [SerializeField] protected Transform objectParentL2;
    [SerializeField] protected LayerMask groundLayer;

    [Header("Poisson Sampling Attributes")]
    [SerializeField] protected Vector2 zone = Vector2.one;
    [SerializeField] protected float radius = 1;
    [SerializeField] protected int k = 30;
    [SerializeField] protected float display_radius;

    [Header("Overlapped Checking Properties")]
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


    private List<Vector2> samples;
    private GameObject theObjectToSpawn;

    private Vector3 largeIslandSpawnPoint = new Vector3(-25, 30, -25);
    private Vector3 mediumIslandSpawnPoint = new Vector3(-12, 30, -12);
    private Vector3 smallIslandSpawnPoint = new Vector3(-7, 30, -7);

    private SIZE currentIslandSize;

    private Quaternion randYRotation;
    private Vector3 randObjectScale;
    private Vector3 currentSpawnPoint;

    private void Start()
    {
        //SpawnObjects();

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
                zone = new Vector2(50, 50);
                radius = 15f;
                k = 20;
                display_radius = 3.8f;
                break;

            case 1:
                zone = new Vector2(25, 25);
                radius = 8f;
                k = 10;
                display_radius = 3.8f; 
                break;

            case 2:
                zone = new Vector2(14, 14);
                radius = 4.2f;
                k = 10;
                display_radius = 3.8f;
                break;
            default:
                zone = new Vector2(50, 50);
                radius = 15f;
                k = 10;
                display_radius = 3.8f;
                break;

        }

    }


    private void SpawnOnLand(Vector3 spawnPos, Transform objectParent)
    {

        RaycastHit hit;

        if (Physics.Raycast(spawnPos, Vector3.down, out hit, raycastDistance, groundLayer))
        {

            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            //Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            //Collider[] collidersInsideOverlapBox = new Collider[1];
            //int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapTestBoxScale, collidersInsideOverlapBox, spawnRotation, spawnedObjectLayer);

            //if (numberOfCollidersFound == 0 )
            //{
                Pick(hit.point, spawnRotation, objectParent);
            //}
            //else
            //{
            //    // Debug.Log("name of collider 0 found " + collidersInsideOverlapBox[0].name);

            //}
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((new Vector3(zone.x, 0, zone.y) / 2) + transform.position, new Vector3(zone.x, 0, zone.y));

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(currentSpawnPoint, 1f);

    }

}