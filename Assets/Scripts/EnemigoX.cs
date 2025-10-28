using UnityEngine;

public abstract class EnemigoX : MonoBehaviour
{
    [SerializeField]
    string guid;

    [SerializeField]
    int vida = 30;

    [SerializeField]
    int danyo = 10;

    [SerializeField]
    Transform explosion;

    public int GetLife()
    {
        return vida;
    }

    public int GetDamage()
    {
        return danyo;
    }

    public void CheckIfAlreadyDefeated()
    {
        // si el enemigo ya ha sido destruido en una partida anterior, lo destruimos
        if (GameState.gameData.defeatedEnemies.Contains(guid))
        {
            Destroy(gameObject);
        }
    }

    public void ChangeLife(int damage)
    {
        // reducimos la vida
        vida -= damage;

        // si se queda sin vida
        if (vida <= 0)
        {
            // destruimos el enemigo
            Destroy(gameObject);

            // efecto de explosion
            if (explosion)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }

            // marcamos el enemigo como destruido en la informacion de la partida
            GameState.gameData.AddDefeatedEnemy(guid);
        }
    }

    public abstract void ReceiveDamage();
    
}
