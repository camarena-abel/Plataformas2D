using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    float axisH;
    bool saltar = false;

    [SerializeField]
    float speedF = 15f;

    [SerializeField]
    float jumpF = 15f;

    [SerializeField]
    Transform groundTest;

    [SerializeField]
    float groundTestRadius = 0.2f;

    [SerializeField]
    LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal") * 2f; // esto un valor entre -2 y 2
        axisH = Mathf.Clamp(h, -1f, 1f);            // limito los valores a de -1 y 1


        if (Input.GetButtonDown("Jump"))
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
            rb.AddForceY(jumpF, ForceMode2D.Impulse);
            saltar = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundTest.position, groundTestRadius);
    }
}
