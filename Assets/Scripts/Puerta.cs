using UnityEngine;

public class Puerta : MonoBehaviour
{
    [SerializeField]
    float speed = 1.0f; // velocidad a la que se abre la puerta (1/speed = tiempo)
    enum DoorStatus { Idle, Opening, Closing }
    DoorStatus status = DoorStatus.Idle;

    Vector2 initPos;
    Vector2 endPos;

    Transform sprite;
    Rigidbody2D rb;

    float posicion = 0.0f; // 0 = cerrada, 1 = abierta

    public void Open()
    {
        status = DoorStatus.Opening;
    }

    public void Close()
    {
        status = DoorStatus.Closing;
    }
    
    void Start()
    {
        sprite = transform.Find("Sprite");
        rb = sprite.GetComponent<Rigidbody2D>();

        initPos = transform.localPosition;
        endPos = new Vector2(initPos.x, initPos.y + 3f);
    }

    void FixedUpdate()
    {
        // se abre, se cierra, o esta quieta?
        switch (status)
        {
            case DoorStatus.Idle:
                return;
            case DoorStatus.Opening:
                posicion += speed * Time.fixedDeltaTime;
                if (posicion > 1f)
                {
                    status = DoorStatus.Idle;
                    posicion = 1f;
                }
                break;
            case DoorStatus.Closing:
                posicion -= speed * Time.fixedDeltaTime;
                if (posicion < 0f)
                {
                    status = DoorStatus.Idle;
                    posicion = 0f;
                }
                break;
        }

        // movemos de verdad
        Vector2 posFinal = Vector2.Lerp(initPos, endPos, posicion);
        rb.MovePosition(posFinal);
    }
}
