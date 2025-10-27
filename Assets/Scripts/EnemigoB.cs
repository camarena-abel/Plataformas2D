using UnityEngine;

public class EnemigoB : EnemigoX
{
    bool derecha = true;       // el player esta mirando a la derecha o a la izquierda?

    [SerializeField]
    Transform groundTest;

    [SerializeField]
    LayerMask groundLayer;

    public override void ReceiveDamage()
    {
        ChangeLife(10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
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
}
