using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text contadorMonedas;

    public void ActualizarContadorMonedas(int cantidad)
    {
        contadorMonedas.text = $"Monedas: {cantidad}";
    }
}
