using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    public static GameController controllerInstance;

    public List<Transform> enemySpawnPoints;
    public List<Transform> keySpawnPoints;
    public List<Transform> shopPortalSpawnPoints;
    public List<Transform> shopItemSpawnPoints;
    public List<GameObject> enemyPrefabs;
    public List<GameObject> lootPrefabs;
    public GameObject keyPrefab;
    public PlayerStats playerStats;
    public Portal shopPortal;
    //Fields needed for save/load functionality and scene changes
    public GameData gameData;


    // Start is called before the first frame update
    void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        shopPortal = GameObject.FindGameObjectWithTag("ShopPortal").GetComponentInChildren<Portal>();
        if (controllerInstance == null) 
        {
            controllerInstance = this;
        }
        if (PlayerPrefs.GetInt("Load") != 0) 
        {
            Load();
            PlayerPrefs.SetInt("Load", 0);
        }

        gameData.activeSceneName = SceneManager.GetActiveScene().name;
        SpawnEnemies();
        SpawnKeys();
        SpawnShopPortal();
        SpawnShopItems();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnEnemies() 
    {
        for (int i = 0; i < gameData.numberOfEnemies; i++)
        {
            int randEnemy = Random.Range(0, enemyPrefabs.Count);
            int randSpawnPoint = Random.Range(0, enemySpawnPoints.Count);

            if (enemySpawnPoints.Count > 0) 
            { 
                GameObject enemy = Instantiate(enemyPrefabs[randEnemy], enemySpawnPoints[randSpawnPoint].position, transform.rotation);
                float getLootNumber = Random.Range((float)0, (float)1);
                EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                if (getLootNumber < gameData.lootOdds)
                {
                    enemyStats.rewards = 1;
                }
                enemyStats.SetCurrencyReward(gameData.currencyRewardMultiplier);
                enemySpawnPoints.RemoveAt(randSpawnPoint);
            }
        }
    }

    public void SpawnKeys() 
    {
        for (int i = 0; i < gameData.numberOfKeys; i++)
        {
            if (keySpawnPoints.Count > 0) 
            {
                int randSpawnPoint = Random.Range(0, keySpawnPoints.Count);

                GameObject key = Instantiate(keyPrefab, keySpawnPoints[randSpawnPoint].position, transform.rotation);
                key.GetComponent<KeyItem>().keyItemNumber = i;
                keySpawnPoints.RemoveAt(randSpawnPoint);
            }
        }
    }

    public void SpawnShopPortal() 
    { 
        int randSpawnPoint = Random.Range(0, shopPortalSpawnPoints.Count);

        shopPortal.transform.position = shopPortalSpawnPoints[randSpawnPoint].position;
    }

    public void SpawnShopItems()
    {
        for (int i = 0; i < shopItemSpawnPoints.Count; i++) 
        {
            DropLoot(shopItemSpawnPoints[i].position, gameData.shopItemCost);
        }

    }

    public void DropLoot(Vector3 lootPosition) 
    {
        int randLoot = Random.Range(0, lootPrefabs.Count);

       Instantiate(lootPrefabs[randLoot], lootPosition , transform.rotation);
    }

    public void DropLoot(Vector3 lootPosition, int cost)
    {
        int randLoot = Random.Range(0, lootPrefabs.Count);

        GameObject loot = Instantiate(lootPrefabs[randLoot], lootPosition, transform.rotation);
        loot.GetComponent<StatBoost>().cost = cost;
    }

    public void EnemyKilled() 
    {
        gameData.numberOfEnemies -= 1;
    }

    public void Save()
    {
        GetCurrentPlayerData();
        string destination =  Path.Combine(Application.persistentDataPath, "save.json");
        string jsonString = JsonUtility.ToJson(gameData);
        File.WriteAllText(destination, jsonString);
    }

    public void Load()
    {
        string destination = Path.Combine(Application.persistentDataPath, "save.json");

        if (!File.Exists(destination)) 
        {
            Debug.LogError("File not found");
            return;
        }

        string fileContents = File.ReadAllText(destination);
        JsonUtility.FromJsonOverwrite(fileContents, gameData);
        SetCurrentPlayerData();
    }

    public void LoadScene(string sceneName, bool loadScene)
    {
        if (loadScene) 
        {
            PlayerPrefs.SetInt("Load", 1);
        }
        Time.timeScale = 1;
        Scene scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.LoadScene(scene.name);
    }

    public void LoadNextLevel() 
    {
        gameData.numberOfTotalEnemies = (int)(gameData.numberOfTotalEnemies * gameData.endOfLevelMultiplier);
        gameData.numberOfTotalKeys = (int)(gameData.numberOfTotalKeys * gameData.endOfLevelMultiplier);
        gameData.numberOfEnemies = gameData.numberOfTotalEnemies;
        gameData.numberOfKeys = gameData.numberOfTotalKeys;
        Save();
        LoadScene("GroundPlay", true);
    }

    public void GetCurrentPlayerData() 
    {
        gameData.baseMaxJumpHeight      = playerStats.baseMaxJumpHeight;
        gameData.jumpHeightMultipler    = playerStats.jumpHeightMultipler;
        gameData.minJumpHeight          = playerStats.minJumpHeight;
        gameData.maxJumpHeight          = playerStats.maxJumpHeight;

        gameData.numberOfJumps          = playerStats.numberOfJumps;
        gameData.timeToJumpApex         = playerStats.timeToJumpApex;

        gameData.wallJumpPower          = playerStats.wallJumpPower;
        gameData.wallJumpHeight         = playerStats.wallJumpHeight;
        gameData.wallSlideSpeedDampener = playerStats.wallSlideSpeedDampener ;

        gameData.numberOfDashes         = playerStats.numberOfDashes;
        gameData.fullDashTime           = playerStats.fullDashTime;
        gameData.dashSpeedMultiplier    = playerStats.dashSpeedMultiplier;
        gameData.dashChargeCooldownTime = playerStats.dashChargeCooldownTime;

        gameData.baseMoveSpeed          = playerStats.baseMoveSpeed;
        gameData.moveSpeedMultipler     = playerStats.moveSpeedMultipler;
        gameData.moveSpeed              = playerStats.moveSpeed;

        gameData.damageMultipler        = playerStats.damageMultipler;

        gameData.knockbackMultiplier    = playerStats.knockbackMultiplier;

        gameData.health                 = playerStats.GetHealth();
        gameData.baseMaxHealth          = playerStats.baseMaxHealth;
        gameData.maxHealthMultiplier    = playerStats.maxHealthMultiplier;
        gameData.maxHealth              = playerStats.GetMaxHealth();

        gameData.meleeDamageMultiplier  = playerStats.meleeDamageMultiplier;
        gameData.rangedDamageMultiplier = playerStats.rangedDamageMultiplier;
        gameData.currency               = playerStats.currency;
    }

    public void SetCurrentPlayerData()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerStats.baseMaxJumpHeight       = gameData.baseMaxJumpHeight;
        playerStats.jumpHeightMultipler     = gameData.jumpHeightMultipler;
        playerStats.minJumpHeight           = gameData.minJumpHeight;
        playerStats.maxJumpHeight           = gameData.maxJumpHeight;

        playerStats.numberOfJumps           = gameData.numberOfJumps;
        playerStats.timeToJumpApex          = gameData.timeToJumpApex;

        playerStats.wallJumpPower           = gameData.wallJumpPower;
        playerStats.wallJumpHeight          = gameData.wallJumpHeight;
        playerStats.wallSlideSpeedDampener  = gameData.wallSlideSpeedDampener;

        playerStats.numberOfDashes          = gameData.numberOfDashes;
        playerStats.fullDashTime            = gameData.fullDashTime;
        playerStats.dashSpeedMultiplier     = gameData.dashSpeedMultiplier;
        playerStats.dashChargeCooldownTime  = gameData.dashChargeCooldownTime;

        playerStats.baseMoveSpeed           = gameData.baseMoveSpeed;
        playerStats.moveSpeedMultipler      = gameData.moveSpeedMultipler;
        playerStats.moveSpeed               = gameData.moveSpeed;

        playerStats.damageMultipler         = gameData.damageMultipler;

        playerStats.knockbackMultiplier     = gameData.knockbackMultiplier;
        playerStats.baseMaxHealth           = gameData.baseMaxHealth;
        playerStats.maxHealthMultiplier     = gameData.maxHealthMultiplier;
        playerStats.SetMaxHealth(gameData.maxHealth);
        playerStats.SetHealth(gameData.health);

        playerStats.meleeDamageMultiplier   = gameData.meleeDamageMultiplier;
        playerStats.rangedDamageMultiplier  = gameData.rangedDamageMultiplier;

        playerStats.currency                = gameData.currency;
    }   
}

