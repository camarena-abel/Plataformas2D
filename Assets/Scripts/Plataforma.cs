using UnityEngine;

public class Plataforma : MonoBehaviour
{
    [SerializeField]
    float speed = 1.0f; // unidades (metros?) por segundo

    Transform sprite;    
    Transform puntoA;    
    Transform puntoB;
    Rigidbody2D rb;
    bool adelante = true; // la plataforma va hacia adelante o hacia atras?

    void Start()
    {
        sprite = transform.Find("Sprite");
        puntoA = transform.Find("PuntoA");
        puntoB = transform.Find("PuntoB");
        rb = sprite.GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        // calculamos el vector diferencia entre los 2 puntos
        Vector3 diff;
        if (adelante)
        {
            diff = puntoB.position - sprite.position;
        } else
        {
            diff = puntoA.position - sprite.position;
        }
        

        // esta muy cerca de su destino?
        if (diff.magnitude < 0.01f)
        {
            adelante = !adelante;
        } else
        {
            Vector3 dir = diff.normalized;
            rb.MovePosition(sprite.position + dir * speed * Time.fixedDeltaTime);
        }
    }
}
