using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    private float spawnRate = 1f;
    private int numberOfEnemies = 2;

    private float timeBetweenWaves = 5f;

    public GameObject waveText;


    private int waveNumber = 0;
    private float spawnTimer = 0f;
    private int enemiesSpawned = 0;
    private int leftToSpawn = 0;

    private bool spawning = false;

    public static Spawner instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (Level.instance.isPaused)
        {
            return;
        }

        if (spawning)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate && enemiesSpawned < EnemiesPerWave())
            {
                spawnTimer = 0f;
                GameObject enemy = Instantiate(enemyPrefab, Level.instance.start.position, Quaternion.identity);
                enemiesSpawned++;
                enemy.name = "Enemy " + enemiesSpawned;
                enemy.GetComponent<EnemyMove>().SetSpeed(WaveSpeed());
                leftToSpawn--;
                Debug.Log("Left to spawn " + leftToSpawn);
                if (leftToSpawn == 0)
                {
                    Debug.Log("Wave " + waveNumber + " finished");
                    spawning = false;
                }
            }

        }
        else
        {
            StartWave();
        }
    }

    private IEnumerator WaitForEnemies()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
    }

    void StartWave()
    {
        spawning = true;
        waveNumber++;
        StartCoroutine(WaitForEnemies());

        Debug.Log("Starting wave " + waveNumber);
        waveText.GetComponent<TextMeshProUGUI>().text = "Wave: " + waveNumber;
        leftToSpawn = EnemiesPerWave();
        enemiesSpawned = 0;
    }

    private int EnemiesPerWave()
    {
        return (int)(waveNumber * numberOfEnemies * 1.5f);//todo: make this better
    }

    private float WaveSpeed()
    {
        return 1.0f + waveNumber;
    }

    public void Restart()
    {
        waveNumber = 0;
        //Destroy all enemies

        //Reset coins and lives
        Level.instance.Coins = 100;
        Level.instance.Lives = 10;

        StartWave();
    }
}
