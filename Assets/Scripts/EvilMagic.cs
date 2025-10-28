using UnityEngine;

public class EvilMagic : MonoBehaviour
{
    Vector2 dir;
    Rigidbody2D rb;
    public Vector2 Dir { get => dir; set => dir = value; }

    [SerializeField]
    float speed = 1f; // 1m/s

    void Start()
    {
        // pillamos el rigidbody
        rb = GetComponent<Rigidbody2D>();
        // tiempo maximo de existencia de la bola de energia
        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }

    }
}
