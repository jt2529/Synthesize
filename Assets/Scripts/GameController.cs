using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public List<Transform> enemySpawnPoints;
    public List<Transform> keySpawnPoints;
    public List<GameObject> enemyPrefabs;
    public List<GameObject> lootPrefabs;
    public GameObject keyPrefab;
    public int numberOfEnemies;
    public int numberOfKeys;
    public float lootOdds;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int randEnemy = Random.Range(0, enemyPrefabs.Count);
            int randSpawnPoint = Random.Range(0, enemySpawnPoints.Count);

            GameObject enemy = Instantiate(enemyPrefabs[randEnemy], enemySpawnPoints[randSpawnPoint].position, transform.rotation);
            float getLootNumber = Random.Range((float)0, (float)1);
            if (getLootNumber < lootOdds) 
            {
                enemy.GetComponent<EnemyStats>().rewards = 1;
            }
            enemySpawnPoints.RemoveAt(randSpawnPoint);
        }

        for (int i = 0; i < numberOfKeys; i++)
        {
            int randSpawnPoint = Random.Range(0, keySpawnPoints.Count);

            GameObject key = Instantiate(keyPrefab, keySpawnPoints[randSpawnPoint].position, transform.rotation);
            key.GetComponent<KeyItem>().keyItemNumber = i;
            keySpawnPoints.RemoveAt(randSpawnPoint);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropLoot(Vector3 lootPosition) 
    {
        int randLoot = Random.Range(0, lootPrefabs.Count);

        Instantiate(lootPrefabs[randLoot], lootPosition , transform.rotation);
    }
}
