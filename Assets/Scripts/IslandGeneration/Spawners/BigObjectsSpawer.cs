using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigObjectsSpawer : ObjectsSpawner
{

    public override void SpawnObjects(Size islandSize, Theme islandTheme)
    {
        Debug.Log("is island small true? = " + islandSize.CompareTo(Size.SMALL));

        if (islandSize.CompareTo(Size.SMALL) != 0)
        {

            attempCount = 0;
            float spreadValue = SetNewSpreadValue(islandSize);

            itemXSpread = spreadValue;
            itemZSpread = spreadValue;

            DestroyAllInstanceOfObjects();

            ObjectType _objectType = SetCurrentObjectType(islandSize);            
            theObjectToSpawn = ChooseObjectToSpawn(islandTheme, _objectType);

            Debug.Log("SpawnObjects(size,theme) - current Island theme: " + islandTheme);
            Debug.Log("SpawnObjects(size,theme) - _objectType: " + _objectType);

            while (attempCount < maxAttemp && spawnedCount == 0)
            {

                for (int i = 0; i < numItemsToSpawn; i++)
                {
                    SpawnTheObjects();
                }

                attempCount++;
            }

        }

    }

    private ObjectType SetCurrentObjectType(Size islandSize)
    {
        ObjectType _objectType = ObjectType.MED_TYPE;

        switch ((int)islandSize)
        {
            case 0: _objectType = ObjectType.BIG_TYPE; break;
            case 1: _objectType = ObjectType.MED_TYPE; break;
            case 2: break;
            default: break;
        }

        return _objectType;
    }

  
    protected override GameObject ChooseObjectToSpawn(Theme islandTheme, ObjectType objectType)
    {
        List<ObjectsSO> objectSOtemp = new List<ObjectsSO>();
        GameObject objectPrefab = null;

        for (int i = 0; i < ObjectsSOList.Count; i++)
        {

            if (ObjectsSOList[i].IslandTheme.CompareTo(islandTheme) == 0 && ObjectsSOList[i].ObjectType.CompareTo(objectType) == 0 )
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
}
