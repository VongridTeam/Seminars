using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private Transform enemies;

    private bool hasWon = false;
    private int enemiesAlive = 0;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singletonObject = new GameObject();
                DontDestroyOnLoad(singletonObject);
                singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                instance = singletonObject.AddComponent<GameManager>();
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        enemiesAlive = 0;
        SpawnEnemies();
    }

    void Update()
    {
        if (enemiesAlive == 0 && !hasWon)
        {
            Instantiate(winScreen);
            hasWon = true;
        }
    }

    private void SpawnEnemies()
    {
        Vector3 startPos = new Vector3(-7, 4.8f, 0);
        Vector3 step = new Vector3(2, 0, 0);
        for (int i = 0; i < 8; i++)
        {
            Instantiate(enemy, startPos + step * i, enemy.transform.rotation, enemies);
        }
        startPos = new Vector3(-8, 3.8f, 0);
        for (int i = 0; i < 9; i++)
        {
            Instantiate(enemy, startPos + step * i, enemy.transform.rotation, enemies);
        }
    }

    public void RegisterEnemy()
    {
        enemiesAlive++;
    }

    public void EnemyKilled()
    {
        enemiesAlive--;
    }
}
