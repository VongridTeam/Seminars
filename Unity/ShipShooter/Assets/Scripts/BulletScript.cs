using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 5f;

    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.AddForce(new Vector2(0, bulletSpeed));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Destroy(collision.gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
