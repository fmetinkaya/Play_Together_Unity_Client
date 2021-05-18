using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdObstaclesManager : MonoBehaviour
{

    public GameObject birdObstaclePrefab;
    GameObject[] birdObstacles;

    public int obstaclesPoolSize;
    public float spawnRate;
    public float obstacleYMin;
    public float obstacleYMax;
    float spawnXPosition = 6f;
    int obstaclePoolSize = 5;
    int currentObstacle = 0;


    Vector2 obstaclePoolPosition = new Vector2(-15, -25);
    float timeSinceLastSpawned;

    public GameScreenManager gameScreenManagerScript;

    RandomManager randomManager;

    void Start()
    {
        randomManager = gameScreenManagerScript.GetNewRandomManager();

        timeSinceLastSpawned = spawnRate;
        birdObstacles = new GameObject[obstaclePoolSize];

        for (int i = 0; i < obstaclePoolSize; i++)
        {
            birdObstacles[i] = (GameObject)Instantiate(birdObstaclePrefab, obstaclePoolPosition, Quaternion.identity);
        }
    }

    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;
        if (timeSinceLastSpawned >= spawnRate)
        {
            StartObstacle();
        }
    }

    void StartObstacle()
    {
        timeSinceLastSpawned = 0f;

        float spawnYPosition = randomManager.RandomGenerate(obstacleYMin, obstacleYMax);

        birdObstacles[currentObstacle].transform.position = new Vector2(spawnXPosition, spawnYPosition);
        birdObstacles[currentObstacle].GetComponent<BirdObstacleScript>().isTrigger = true;

        currentObstacle += 1;
        if (currentObstacle == obstaclePoolSize)
            currentObstacle = 0;

    }
    public void restartPositionObstacle()
    {
        randomManager.resetSeed();
        for (int i = 0; i < obstaclePoolSize; i++)
        {
            birdObstacles[i].transform.position = obstaclePoolPosition;
            birdObstacles[i].GetComponent<BirdObstacleScript>().isTrigger = true;
        }
        timeSinceLastSpawned = spawnRate;
    }

}
