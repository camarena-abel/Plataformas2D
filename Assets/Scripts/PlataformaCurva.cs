using UnityEngine;

public class PlataformaCurva : MonoBehaviour
{
    [SerializeField]
    float speed = 0.2f; 

    Transform sprite;
    Transform puntoA;
    Transform puntoB;
    Transform puntoC;
    Transform puntoD;
    Rigidbody2D rb;
    bool adelante = true;  // la plataforma va hacia adelante o hacia atras?
    [SerializeField]
    float posicion = 0.5f; // posicion dentro de la curva 0 = inicio, 1 = final

    void Start()
    {
        sprite = transform.Find("Sprite");
        puntoA = transform.Find("PuntoA");
        puntoB = transform.Find("PuntoB");
        puntoC = transform.Find("PuntoC");
        puntoD = transform.Find("PuntoD");
        rb = sprite.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 puntoA1 = Vector3.Lerp(puntoA.position, puntoB.position, posicion);
        Vector3 puntoA2 = Vector3.Lerp(puntoB.position, puntoC.position, posicion);
        Vector3 puntoA3 = Vector3.Lerp(puntoC.position, puntoD.position, posicion);
        Vector3 puntoB1 = Vector3.Lerp(puntoA1, puntoA2, posicion);
        Vector3 puntoB2 = Vector3.Lerp(puntoA2, puntoA3, posicion);
        Vector3 pos = Vector3.Lerp(puntoB1, puntoB2, posicion);
        rb.MovePosition(pos);
        
        if (adelante)
        {
            if (posicion < 1f)
            {
                posicion += speed * Time.fixedDeltaTime;
            } else
            {
                posicion = 1f;
                adelante = !adelante;
            }
        } else
        {
            if (posicion > 0f)
            {
                posicion -= speed * Time.fixedDeltaTime;
            } else
            {
                posicion = 0f;
                adelante = !adelante;
            }
        }
   
    }
}
