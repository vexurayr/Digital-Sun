using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    #region Variables
    public static SpawnerManager instance { get; private set; }

    [SerializeField] private int maxLandAnimalsInScene;
    [SerializeField] private int maxWaterAnimalsInScene;
    [SerializeField] private int maxTribesmanInScene;
    [SerializeField] private int maxCodelessStructuresInScene;
    [SerializeField] private float distanceFromPlayerToSpawn;

    private int landAnimalsLeft;
    private int waterAnimalsLeft;
    private int tribesmanLeft;

    private List<GameObject> landAnimalSpawners;
    private List<GameObject> waterAnimalSpawners;
    private List<GameObject> tribesmanSpawners;
    private List<GameObject> playerSpawners;
    private List<GameObject> terminalSpawners;
    private List<GameObject> codeStructureSpawners;
    private List<GameObject> codelessStructureSpawners;

    #endregion Variables

    #region MonoBehaviours
    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        landAnimalsLeft = maxLandAnimalsInScene;
        waterAnimalsLeft = maxWaterAnimalsInScene;
        tribesmanLeft = maxTribesmanInScene;
        landAnimalSpawners = new List<GameObject>();
        waterAnimalSpawners = new List<GameObject>();
        tribesmanSpawners = new List<GameObject>();
        playerSpawners = new List<GameObject>();
        terminalSpawners = new List<GameObject>();
        codeStructureSpawners = new List<GameObject>();
        codelessStructureSpawners = new List<GameObject>();
    }

    #endregion MonoBehaviours

    #region SpawnerFunctions
    public void SpawnOnNewZone()
    {
        SpawnLandAnimals();
        SpawnWaterAnimals();
        SpawnTribesman();
        SpawnTerminal();
        SpawnCodeStructures();
        SpawnCodelessStructures();
        SpawnPlayer();
    }

    public void SpawnOnDifficultyIncrease()
    {
        Debug.Log("Spawning more from difficulty increase!");
        SpawnLandAnimals();
        SpawnWaterAnimals();
        SpawnTribesman();
    }

    // Randomly choosing X number of spawners and spawning a random item at each
    public void SpawnLandAnimals()
    {
        // There aren't any of these spawners in the scene
        if (landAnimalSpawners.Count == 0)
        {
            return;
        }
        Debug.Log("Spawning land animals");
        // Shuffles the list for random spawner order
        for (int i = 0; i < landAnimalSpawners.Count; i++)
        {
            GameObject temp = landAnimalSpawners[i];
            int randomIndex = Random.Range(i, landAnimalSpawners.Count);
            landAnimalSpawners[i] = landAnimalSpawners[randomIndex];
            landAnimalSpawners[randomIndex] = temp;
        }

        int spawnCount = ReturnLowerNumber(landAnimalSpawners.Count, landAnimalsLeft);

        if (landAnimalsLeft > landAnimalSpawners.Count)
        {
            landAnimalsLeft = landAnimalSpawners.Count;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            if (landAnimalSpawners[i].GetComponent<Spawner>())
            {
                landAnimalSpawners[i].GetComponent<Spawner>().SpawnRandomObject();
                landAnimalsLeft--;
            }
        }
    }

    public void SpawnWaterAnimals()
    {
        if (waterAnimalSpawners.Count == 0)
        {
            return;
        }
        Debug.Log("Spawning water animals");
        for (int i = 0; i < waterAnimalSpawners.Count; i++)
        {
            GameObject temp = waterAnimalSpawners[i];
            int randomIndex = Random.Range(i, waterAnimalSpawners.Count);
            waterAnimalSpawners[i] = waterAnimalSpawners[randomIndex];
            waterAnimalSpawners[randomIndex] = temp;
        }

        int spawnCount = ReturnLowerNumber(waterAnimalSpawners.Count, waterAnimalsLeft);

        if (waterAnimalsLeft > waterAnimalSpawners.Count)
        {
            waterAnimalsLeft = waterAnimalSpawners.Count;
        }
        Debug.Log("WaterAnimalSpawners.Count: " + waterAnimalSpawners.Count + "\nWaterAnimalsLeft: " + waterAnimalsLeft + "\nSpawn Count: " + spawnCount);
        for (int i = 0; i < spawnCount; i++)
        {
            if (waterAnimalSpawners[i].GetComponent<Spawner>())
            {
                // If a player is in the scene
                if (GameManager.instance.GetCurrentPlayerController() != null)
                {
                    // Get the distance from this spawner to the player
                    GameObject player = GameManager.instance.GetCurrentPlayerController().gameObject;
                    Vector3 vectorFromPlayerToSpawn = player.transform.position - waterAnimalSpawners[i].gameObject.transform.position;
                    float distance = vectorFromPlayerToSpawn.sqrMagnitude;

                    // Get the angle between the distance vector and player.forward
                    float angle = Vector3.Angle(vectorFromPlayerToSpawn, player.transform.forward);

                    // Spawn thing if far enough away from the player
                    if (distance >= distanceFromPlayerToSpawn)
                    {
                        waterAnimalSpawners[i].GetComponent<Spawner>().SpawnRandomObject();
                        waterAnimalsLeft--;
                    }
                    else if (distance > distanceFromPlayerToSpawn / 2 && angle > GameManager.instance.GetCurrentPlayerController().GetCameraFOV())
                    {
                        waterAnimalSpawners[i].GetComponent<Spawner>().SpawnRandomObject();
                        waterAnimalsLeft--;
                    }
                }
                // Spawn the thing regardless if there is no player yet
                else
                {
                    waterAnimalSpawners[i].GetComponent<Spawner>().SpawnRandomObject();
                    waterAnimalsLeft--;
                }
            }
        }
    }

    public void SpawnTribesman()
    {
        if (tribesmanSpawners.Count == 0)
        {
            return;
        }
        Debug.Log("Spawning tribesman");
        for (int i = 0; i < tribesmanSpawners.Count; i++)
        {
            GameObject temp = tribesmanSpawners[i];
            int randomIndex = Random.Range(i, tribesmanSpawners.Count);
            tribesmanSpawners[i] = tribesmanSpawners[randomIndex];
            tribesmanSpawners[randomIndex] = temp;
        }

        int spawnCount = ReturnLowerNumber(tribesmanSpawners.Count, tribesmanLeft);

        if (tribesmanLeft > tribesmanSpawners.Count)
        {
            tribesmanLeft = tribesmanSpawners.Count;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            if (tribesmanSpawners[i].GetComponent<Spawner>())
            {
                tribesmanSpawners[i].GetComponent<Spawner>().SpawnRandomObject();
                tribesmanLeft--;
            }
        }
    }

    // Only spawning 1 player at a randomly chosen spawn point
    public void SpawnPlayer()
    {
        if (playerSpawners.Count == 0)
        {
            return;
        }
        Debug.Log("Spawning player");
        int randSpawnPoint = Random.Range(0, playerSpawners.Count);

        if (playerSpawners[randSpawnPoint].GetComponent<Spawner>())
        {
            playerSpawners[randSpawnPoint].GetComponent<Spawner>().SpawnObject(0);
        }
    }

    public void SpawnTerminal()
    {
        if (terminalSpawners.Count == 0)
        {
            return;
        }
        Debug.Log("Spawning terminal");
        int randSpawnPoint = Random.Range(0, terminalSpawners.Count);

        if (terminalSpawners[randSpawnPoint].GetComponent<Spawner>())
        {
            terminalSpawners[randSpawnPoint].GetComponent<Spawner>().SpawnObject(0);
        }
    }

    public void SpawnCodeStructures()
    {
        if (codeStructureSpawners.Count == 0)
        {
            return;
        }
        Debug.Log("Spawning code structures");
        for (int i = 0; i < codeStructureSpawners.Count; i++)
        {
            GameObject temp = codeStructureSpawners[i];
            int randomIndex = Random.Range(i, codeStructureSpawners.Count);
            codeStructureSpawners[i] = codeStructureSpawners[randomIndex];
            codeStructureSpawners[randomIndex] = temp;
        }

        int spawnCount = ReturnLowerNumber(codeStructureSpawners.Count, 4);

        for (int i = 0; i < spawnCount; i++)
        {
            if (codeStructureSpawners[i].GetComponent<Spawner>())
            {
                codeStructureSpawners[i].GetComponent<Spawner>().SpawnRandomObject();
            }
        }
    }

    public void SpawnCodelessStructures()
    {
        if (codelessStructureSpawners.Count == 0)
        {
            return;
        }
        Debug.Log("Spawning codeless structures");
        for (int i = 0; i < codelessStructureSpawners.Count; i++)
        {
            GameObject temp = codelessStructureSpawners[i];
            int randomIndex = Random.Range(i, codelessStructureSpawners.Count);
            codelessStructureSpawners[i] = codelessStructureSpawners[randomIndex];
            codelessStructureSpawners[randomIndex] = temp;
        }

        int spawnCount = ReturnLowerNumber(codelessStructureSpawners.Count, maxCodelessStructuresInScene);

        for (int i = 0; i < spawnCount; i++)
        {
            if (codelessStructureSpawners[i].GetComponent<Spawner>())
            {
                codelessStructureSpawners[i].GetComponent<Spawner>().SpawnRandomObject();
            }
        }
    }

    #endregion SpawnerFunctions

    #region HelperFunctions
    public void AddSelfToSpawnerList(GameObject obj)
    {
        if (obj.GetComponent<Spawner>().GetSpawnerType() == Spawner.SpawnerType.LandAnimal)
        {
            landAnimalSpawners.Add(obj);
        }
        else if (obj.GetComponent<Spawner>().GetSpawnerType() == Spawner.SpawnerType.WaterAnimal)
        {
            waterAnimalSpawners.Add(obj);
        }
        else if (obj.GetComponent<Spawner>().GetSpawnerType() == Spawner.SpawnerType.Tribesman)
        {
            tribesmanSpawners.Add(obj);
        }
        else if (obj.GetComponent<Spawner>().GetSpawnerType() == Spawner.SpawnerType.Player)
        {
            playerSpawners.Add(obj);
        }
        else if (obj.GetComponent<Spawner>().GetSpawnerType() == Spawner.SpawnerType.Terminal)
        {
            terminalSpawners.Add(obj);
        }
        else if (obj.GetComponent<Spawner>().GetSpawnerType() == Spawner.SpawnerType.CodeStructure)
        {
            codeStructureSpawners.Add(obj);
        }
        else if (obj.GetComponent<Spawner>().GetSpawnerType() == Spawner.SpawnerType.CodelessStructure)
        {
            codelessStructureSpawners.Add(obj);
        }
    }

    public void ClearAllLists()
    {
        landAnimalSpawners.Clear();
        waterAnimalSpawners.Clear();
        tribesmanSpawners.Clear();
        playerSpawners.Clear();
        terminalSpawners.Clear();
        codeStructureSpawners.Clear();
        codelessStructureSpawners.Clear();
    }

    public void IncrementLandAnimalsLeft()
    {
        if (landAnimalsLeft < maxLandAnimalsInScene)
        {
            landAnimalsLeft++;
        }
    }

    public void IncrementWaterAnimalsLeft()
    {
        if (waterAnimalsLeft < maxWaterAnimalsInScene)
        {
            waterAnimalsLeft++;
        }
    }

    public void IncrementTribesmanLeft()
    {
        if (tribesmanLeft < maxTribesmanInScene)
        {
            tribesmanLeft++;
        }
    }

    public int ReturnLowerNumber(int firstNumber, int secondNumber)
    {
        if (firstNumber < secondNumber)
        {
            return firstNumber;
        }
        else
        {
            return secondNumber;
        }
    }

    #endregion HelperFunctions
}