using UnityEngine;

public class EnemigoX : MonoBehaviour
{
    [SerializeField]
    int vida = 30;

    [SerializeField]
    int danyo = 10;

    public int GetDamage()
    {
        return danyo;
    }
}
