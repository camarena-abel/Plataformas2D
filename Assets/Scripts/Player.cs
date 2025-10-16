using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerUI playerUI;
    float axisH;
    float jumpTime = 0f;
    int monedas = 0;
    bool saltar = false;

    [SerializeField]
    float speedF = 15f;

    [SerializeField]
    float jumpF = 15f;

    [SerializeField]
    float jumpMaxTime = 0.3f; // tiempo maximo para el salto mas grande

    [SerializeField]
    Transform groundTest;

    [SerializeField]
    float groundTestRadius = 0.2f;

    [SerializeField]
    LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerUI = GetComponent<PlayerUI>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal") * 2f; // esto un valor entre -2 y 2
        axisH = Mathf.Clamp(h, -1f, 1f);            // limito los valores a de -1 y 1

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

    }

    private void FixedUpdate()
    {
        // rb.linearVelocityX = axisH * speedF;
        rb.AddForceX(axisH * speedF);
        if (saltar)
        {
            // en t tendremos un valor 0 a 1
            float t = Mathf.Clamp01( (jumpTime / jumpMaxTime) );
            // ahora lo convertimos en un valor que va desde 0.5 a 1.0
            t = Mathf.Lerp(0.5f, 1.0f, t);
            print(t);

            rb.AddForceY(jumpF * t, ForceMode2D.Impulse);
            saltar = false;
            jumpTime = 0f;
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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundTest.position, groundTestRadius);
    }
}
