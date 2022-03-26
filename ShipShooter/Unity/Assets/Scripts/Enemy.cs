using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyBullet;
    [SerializeField]
    private float minimumFireRate = 1;
    [SerializeField]
    private float maximumFireRate;

    private float timeLeftToFire = 0;

    void Start()
    {
        GameManager.Instance.RegisterEnemy();
        //Δίνουμε 0.1f για να αφήσουμε χρόνο στο παίκτη να αντιδράσει όταν κάνει spawn
        timeLeftToFire = Random.Range(0.1f, 1 / minimumFireRate);
    }

    void Update()
    {
        if (timeLeftToFire <= 0)
        {
            Shoot();
            timeLeftToFire = Random.Range(1 / maximumFireRate, 1 / minimumFireRate);

        }
        timeLeftToFire -= Time.deltaTime;
    }

    void OnDestroy()
    {
        GameManager.Instance.EnemyKilled();
    }

    private void Shoot()
    {
        Instantiate(enemyBullet, transform.position + new Vector3(0, -1, 0), enemyBullet.transform.rotation);
    }
}
