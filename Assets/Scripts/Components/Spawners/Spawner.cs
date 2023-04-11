using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Variables
    public enum SpawnerType
    {
        LandAnimal,
        WaterAnimal,
        Tribesman,
        Player,
        Terminal,
        CodeStructure,
        CodelessStructure
    }

    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private List<GameObject> objectsToSpawn;

    #endregion Variables

    #region MonoBehaviours
    private void Start()
    {
        Debug.Log("Adding self to a spawner list");
        SpawnerManager.instance.AddSelfToSpawnerList(gameObject);
    }

    #endregion MonoBehaviours

    #region GetSet
    public SpawnerType GetSpawnerType()
    {
        return spawnerType;
    }

    #endregion GetSet

    #region SpawnerFunctions
    public void SpawnObject(int index)
    {
        if (objectsToSpawn.Count == 0 || index > objectsToSpawn.Count)
        {
            return;
        }

        Instantiate(objectsToSpawn[index], gameObject.transform.position, gameObject.transform.rotation);
    }

    public void SpawnRandomObject()
    {
        if (objectsToSpawn.Count == 0)
        {
            return;
        }

        int randIndex = Random.Range(0, objectsToSpawn.Count);

        Instantiate(objectsToSpawn[randIndex], gameObject.transform.position, gameObject.transform.rotation);
    }

    #endregion SpawnerFunctions
}