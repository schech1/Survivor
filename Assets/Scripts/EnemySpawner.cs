using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemySpawner : MonoBehaviour
{
    Transform enemyPosition;
    GameObject enemy;
    [SerializeField] GameObject trianglePreFab;
    [SerializeField] GameObject circlePreFab;
    [SerializeField] GameObject squarePreFab;
    Enemy.Type enemyShape;
    [SerializeField] float spawnFrequency;
    [SerializeField] SOLevelData levelData;
    public enum Shape { Triangle, Circle, Square };
    public Shape shape;

    int i;
    [SerializeField] int EnemyCount;
    int waveCount;
    [SerializeField] bool timespawn;
    void Start()
    {
        if (!timespawn)
        {
            for (int i = 0; i < EnemyCount; i++)
            {

                SpawnEnemy(Random.Range(1, 3), 0);

            }
        }


        switch (shape)
        {
            case Shape.Circle:
                enemy = circlePreFab;
                enemyShape = Enemy.Type.Circle;

                break;

            case Shape.Triangle:
                enemy = trianglePreFab;
                enemyShape = Enemy.Type.Triangle;

                break;

            case Shape.Square:

                enemy = squarePreFab;
                enemyShape = Enemy.Type.Square;

                break;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (timespawn)
        {

            if (Time.time > i && waveCount < levelData.wavesize)
            {
                i += (int)spawnFrequency;
                SpawnEnemy(Random.Range(1, 3), levelData.wavesize);
                waveCount++;

            }
        }

    }



    public void SpawnEnemy(int level, int wavesize)
    {
        GameObject spawnedEnemy = Instantiate(enemy, new Vector3(gameObject.transform.position.x + Random.Range(-10, 10f), gameObject.transform.position.y + Random.Range(-10, 10f), 0), Quaternion.identity);
        spawnedEnemy.transform.localScale = new Vector3(level, level, 1);
        spawnedEnemy.GetComponent<Enemy>().level = level;
        spawnedEnemy.GetComponent<Enemy>().shape = enemyShape;
        spawnedEnemy.GetComponent<Enemy>().state = Enemy.State.Following;
    }
}
