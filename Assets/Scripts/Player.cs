using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class Player : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rb;
    PlayerUI playerUI;
    float axisH;
    float jumpTime = 0f;
    bool saltar = false;
    Vector2 hitDirection;      // direccion en la que sale despedido el player al recibir un impacto
    bool hitReceived = false;  // indica si el player ha recibido un impacto
    float recoveryTime = 0f;   // tiempo de invulnerabilidad tras recibir un impacto
    bool derecha = true;       // el player esta mirando a la derecha o a la izquierda?
    Transform meleeRight;
    Transform meleeLeft;
    float meleeTime;
    float magicTime;

    [SerializeField]
    Transform magic;

    [SerializeField]
    Transform bullet;

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
    float magicMaxTime = 0.5f; // tiempo del ataque magico | disparo

    [SerializeField]
    Transform groundTest;

    [SerializeField]
    float groundTestRadius = 0.2f;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    LayerMask bulletLayer;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerUI = GetComponent<PlayerUI>();
        meleeRight = transform.Find("MeleeRight");
        meleeLeft = transform.Find("MeleeLeft");

        // actualizamos UI
        playerUI.ActualizarBarraDeVida(GameState.gameData.life);
        playerUI.ActualizarContadorMonedas(GameState.gameData.coins);
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
            } else
            {
                // si cambia de direccion, cambiamos el arma de sitio
                if ((axisH < 0f) && (meleeRight.gameObject.activeSelf))
                {
                    meleeRight.gameObject.SetActive(false);
                    meleeLeft.gameObject.SetActive(true);
                }
                if ((axisH > 0f) && (meleeLeft.gameObject.activeSelf))
                {
                    meleeRight.gameObject.SetActive(true);
                    meleeLeft.gameObject.SetActive(false);
                }

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

        // ya estamos atacando?
        if (magicTime > 0f)
        {
            magicTime = Mathf.Clamp(magicTime - Time.deltaTime, 0f, magicMaxTime);
        }

        if (Input.GetButtonDown("Fire2") && (magicTime == 0f))
        {
            Transform go;
            Magic m;
            if (derecha)
                go = Instantiate(magic, meleeRight.position, Quaternion.identity);                
            else
                go = Instantiate(magic, meleeLeft.position, Quaternion.identity);
            m = go.GetComponent<Magic>();
            if (derecha)
                m.Dir = Vector2.right;
            else
                m.Dir = Vector2.left;
            magicTime = magicMaxTime;
        }

        // disparo con arma de fuego
        if (Input.GetButtonDown("Fire3") && (magicTime == 0f))
        {
            RaycastHit2D rh;
            Transform bulletTransform;
            if (derecha)
            {
                rh = Physics2D.Raycast(rb.position, Vector2.right, Mathf.Infinity, bulletLayer);
                bulletTransform = Instantiate(bullet, meleeRight.position, Quaternion.Euler(-180f, -90f, 0f));
            }                
            else
            {
                rh = Physics2D.Raycast(rb.position, Vector2.left, Mathf.Infinity, bulletLayer);
                bulletTransform = Instantiate(bullet, meleeLeft.position, Quaternion.Euler(180f, 90f, 0f));
            }
            Destroy(bulletTransform.gameObject, 1f);
                
            if (rh.transform)
            {
                EnemigoX eX = rh.transform.GetComponent<EnemigoX>();
                if (eX)
                {
                    eX.ReceiveDamage();
                }
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
        GameState.gameData.life -= q;
        playerUI.ActualizarBarraDeVida(GameState.gameData.life);

        if (GameState.gameData.life <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void HitReceived(int damage)
    {
        if (recoveryTime <= 0f)
        {
            ReceiveDamage(damage);

            // hemos recibido daño!
            // tenemos un periodo de invulnerabilidad temporal
            recoveryTime = recoveryMaxTime;
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
                HitReceived(enemy.GetDamage());                
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
            GameState.gameData.coins++;
            // actualizamos el UI
            playerUI.ActualizarContadorMonedas(GameState.gameData.coins);
        }

        if (collision.gameObject.tag == "Door")
        {
            Puerta door = collision.gameObject.GetComponent<Puerta>();
            door.Open();
        }

        if (collision.gameObject.tag == "EvilMagic")
        {
            // recibimos daño
            HitReceived(10);
            // destruimos la bola de energia
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "SavePoint")
        {
            print("partida guardada!");
            GameState.Save();
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
