using UnityEngine;

public class EnemigoB : EnemigoX
{
    SpriteRenderer sprite;
    Rigidbody2D rb;
    bool derecha = true;       // el player esta mirando a la derecha o a la izquierda?
    float magicTime;

    [SerializeField]
    float speedF = 15f;

    [SerializeField]
    float chageDirNearWallDist = 0.5f; // a que distancia de una pared se da la vuelta

    [SerializeField]
    Transform groundTest;

    [SerializeField]
    float groundTestRadius = 0.2f;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    Transform magic;

    [SerializeField]
    float magicMaxTime = 0.5f; // tiempo del ataque magico | disparo

    [SerializeField]
    LayerMask lookForPlayerLayer;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        CheckIfAlreadyDefeated();
    }

    public override void ReceiveDamage()
    {
        ChangeLife(10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // movimiento
        bool puedeCaminar = Physics2D.OverlapCircle(groundTest.position, groundTestRadius, groundLayer);
        if (puedeCaminar)
        {
            if (derecha)
            {
                rb.linearVelocityX = speedF;  // rb.AddForceX(speedF);
            }
            else
            {
                rb.linearVelocityX = -speedF;  // rb.AddForceX(-speedF);
            }
        }

        // detectar al player
        RaycastHit2D rh;
        if (derecha)
        {
            rh = Physics2D.Raycast(rb.position, Vector2.right, Mathf.Infinity, lookForPlayerLayer);
        }
        else
        {
            rh = Physics2D.Raycast(rb.position, Vector2.left, Mathf.Infinity, lookForPlayerLayer);
        }

        magicTime = Mathf.Clamp(magicTime - Time.fixedDeltaTime, 0f, magicMaxTime);

        if (rh.transform)
        {
            Transform magicBall;
            if (rh.transform.gameObject.tag == "Player")
            {
                // podemos disparar al player?
                if (magicTime == 0f)
                {
                    // disparamos!
                    magicBall = Instantiate(magic, rb.position, Quaternion.identity);
                    EvilMagic em = magicBall.GetComponent<EvilMagic>();
                    if (derecha)
                        em.Dir = Vector2.right;
                    else
                        em.Dir = Vector2.left;
                    // iniciamos el contador del cool down
                    magicTime = magicMaxTime;
                }
                
            } else
            {
                // esta cerca de una pared?
                if (rh.distance < chageDirNearWallDist)
                {
                    // damos la vuelta!
                    derecha = !derecha;
                    sprite.flipX = !derecha;
                }
            }
                
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerMelee")
        {
            ReceiveDamage();
        }
        if (collision.gameObject.tag == "Magic")
        {
            ReceiveDamage();
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            print("Aplastado!");
            ChangeLife(GetLife()); // le quitamos toda la vida
        }
    }
}
