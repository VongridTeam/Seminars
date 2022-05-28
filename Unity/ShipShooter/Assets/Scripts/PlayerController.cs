using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float fireRate = 1.15f;
    [SerializeField]
    private GameObject bullet;

    private Vector2 screenBorders;
    private float playerHalfWidth;
    private Vector3 currentPosition;
    private float timeLeftToFire = 0;

    void Start()
    {
        screenBorders = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        playerHalfWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    void Update()
    {
        Move();
        Shoot();
    }

    void OnDestroy()
    {
        SceneManager.LoadScene("Game");
    }

    private void Shoot()
    {
        if (Input.GetKey(KeyCode.Space) && timeLeftToFire <= 0)
        {
            Instantiate(bullet, transform.position + new Vector3(0, 1.1f, 0), bullet.transform.rotation);
            timeLeftToFire = 1 / fireRate;
        }
        timeLeftToFire -= Time.deltaTime;
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        //Keep the player inside the screen
        currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, (screenBorders.x * -1) + playerHalfWidth, screenBorders.x - playerHalfWidth);
        transform.position = currentPosition;
    }
}
