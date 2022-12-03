using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : ObjectsSpawner
{
    protected override void SpawnTheObjects(Size islandSize)
    {

        if(islandSize == Size.LARGE) {

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

    }
}
