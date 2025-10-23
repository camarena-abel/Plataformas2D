using UnityEngine;

public class EnemigoA : EnemigoX
{
    Animator animator;

    public override void ReceiveDamage()
    {
        ChangeLife(10);
        animator.SetTrigger("Hit");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
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
