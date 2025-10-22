using UnityEngine;
using UnityEngine.U2D;

public class Player : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rb;
    PlayerUI playerUI;
    float axisH;
    float jumpTime = 0f;
    int monedas = 0;
    int vida = 100;
    bool saltar = false;
    Vector2 hitDirection;      // direccion en la que sale despedido el player al recibir un impacto
    bool hitReceived = false;  // indica si el player ha recibido un impacto
    float recoveryTime = 0f;   // tiempo de invulnerabilidad tras recibir un impacto
    bool derecha = true;       // el player esta mirando a la derecha o a la izquierda?
    Transform meleeRight;
    Transform meleeLeft;
    float meleeTime;

    [SerializeField]
    Transform magic;

    [SerializeField]
    float speedF = 15f;

    [SerializeField]
    float jumpF = 15f;

    [SerializeField]
    float jumpMaxTime = 0.3f; // tiempo maximo para el salto mas grande

    [SerializeField]
    float hitF = 15f;

    [SerializeField]
    float recoveryMaxTime = 2.0f; // tiempo maximo de invulnerabilidad tras recibir un impacto

    [SerializeField]
    float meleeMaxTime = 0.5f; // tiempo del ataque melee

    [SerializeField]
    Transform groundTest;

    [SerializeField]
    float groundTestRadius = 0.2f;

    [SerializeField]
    LayerMask groundLayer;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerUI = GetComponent<PlayerUI>();
        meleeRight = transform.Find("MeleeRight");
        meleeLeft = transform.Find("MeleeLeft");
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal") * 2f; // esto un valor entre -2 y 2
        axisH = Mathf.Clamp(h, -1f, 1f);            // limito los valores a de -1 y 1

        bool dir = derecha;
        if (axisH > 0f)
        {
            derecha = true;
        }
        else if (axisH < 0f)
        {
            derecha = false;
        }
        if (dir != derecha)
        {
            sprite.flipX = !derecha;
        }

        if (Input.GetButton("Jump"))
        {
            jumpTime += Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            bool puedeSaltar = Physics2D.OverlapCircle(groundTest.position, groundTestRadius, groundLayer);
            if (puedeSaltar)
            {
                saltar = true;
            }
                
        }

        // ataque melee

        // ya estamos atacando?
        if (meleeTime > 0f)
        {
            // reducimos contador de tiempo
            meleeTime -= Time.deltaTime;

            // se ha terminado el ataque?
            if (meleeTime < 0f)
            {
                meleeRight.gameObject.SetActive(false);
                meleeLeft.gameObject.SetActive(false);
                meleeTime = 0f;
            }
        }

        if (Input.GetButtonDown("Fire1") && (meleeTime == 0f))
        {
            meleeTime = meleeMaxTime;
            if (derecha)
            {
                meleeRight.gameObject.SetActive(true);
            } 
            else
            {
                meleeLeft.gameObject.SetActive(true);
            }
        }

        // ataque magico
        if (Input.GetButtonDown("Fire2"))
        {
            if (derecha)
            {
                Instantiate(magic, meleeRight.position, Quaternion.identity);
            }
            else
            {
                Instantiate(magic, meleeLeft.position, Quaternion.identity);
            }
        }

        // vamos a "pintar" cuando el player es invulnerable
        if (recoveryTime > 0f)
        {
            recoveryTime = Mathf.Clamp( recoveryTime - Time.deltaTime, 0f, recoveryMaxTime);
            float t = (recoveryTime / recoveryMaxTime); // valor de 0 a 1            
            t = (1f - Mathf.Repeat(t * 5f, 1f));
            if (t == 1f)
            {
                sprite.color = Color.white;
            } else
            {
                sprite.color = new Color(t, t, t);
            }
        }
    }

    private void FixedUpdate()
    {
        
        rb.AddForceX(axisH * speedF);
        if (saltar)
        {
            // en t tendremos un valor 0 a 1
            float t = Mathf.Clamp01( (jumpTime / jumpMaxTime) );
            // ahora lo convertimos en un valor que va desde 0.5 a 1.0
            t = Mathf.Lerp(0.5f, 1.0f, t);

            rb.AddForceY(jumpF * t, ForceMode2D.Impulse);
            saltar = false;
            jumpTime = 0f;
        }

        // impulso por impacto con un enemigo
        if (hitReceived)
        {
            rb.AddForce(hitDirection * hitF, ForceMode2D.Impulse);
            hitReceived = false;
        }
        
    }

    private void ReceiveDamage(int q)
    {
        vida -= q;
        playerUI.ActualizarBarraDeVida(vida);

        if (vida <= 0)
        {
            print("Game over");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemigoX enemy = collision.gameObject.GetComponent<EnemigoX>();

            // calculamos la direccion en la que tiene que salir volando el player al producirse el impacto
            hitDirection = new Vector2();
            foreach (var c in collision.contacts)
            {
                hitDirection += c.normal;
            }
            hitDirection.Normalize();           

            // segun la vertical podemos hacer daño al enemigo o recibirlo nosotros
            if (hitDirection.y > 0.5f)
            {
                enemy.ReceiveDamage();
            } else
            {
                // si no somos invulnerables, recibimos daño
                if (recoveryTime <= 0f)
                {
                    int danyo = enemy.GetDamage();
                    ReceiveDamage(danyo);

                    // hemos recibido daño!
                    // tenemos un periodo de invulnerabilidad temporal
                    recoveryTime = recoveryMaxTime;
                }
            }

            hitReceived = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            // obtenemos el transform del sprite con la moneda
            Transform transformCollider = collision.gameObject.transform;
            // destruimos el padre (el gameobject moneda)
            Destroy(transformCollider.parent.gameObject);
            // aumentamos el contador de monedas
            monedas++;
            // actualizamos el UI
            playerUI.ActualizarContadorMonedas(monedas);
        }

        if (collision.gameObject.tag == "Door")
        {
            Puerta door = collision.gameObject.GetComponent<Puerta>();
            door.Open();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            Puerta door = collision.gameObject.GetComponent<Puerta>();
            door.Close();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundTest.position, groundTestRadius);
    }
}
