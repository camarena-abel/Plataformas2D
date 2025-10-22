using UnityEngine;

public abstract class EnemigoX : MonoBehaviour
{
    [SerializeField]
    int vida = 30;

    [SerializeField]
    int danyo = 10;

    public int GetDamage()
    {
        return danyo;
    }

    public void ChangeLife(int damage)
    {
        vida -= damage;
        if (vida <= 0)
        {
            Destroy(gameObject, 1f);
        }
    }

    public abstract void ReceiveDamage();
    
}
