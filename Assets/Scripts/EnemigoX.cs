using UnityEngine;

public abstract class EnemigoX : MonoBehaviour
{
    [SerializeField]
    int vida = 30;

    [SerializeField]
    int danyo = 10;

    [SerializeField]
    Transform explosion;

    public int GetDamage()
    {
        return danyo;
    }

    public void ChangeLife(int damage)
    {
        vida -= damage;
        if (vida <= 0)
        {
            Destroy(gameObject);
            if (explosion)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
        }
    }

    public abstract void ReceiveDamage();
    
}
