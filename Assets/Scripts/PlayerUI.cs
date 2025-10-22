using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text contadorMonedas;

    [SerializeField]
    Slider barraDeVida;

    public void ActualizarContadorMonedas(int cantidad)
    {
        contadorMonedas.text = $"Monedas: {cantidad}";
    }

    public void ActualizarBarraDeVida(int cantidad)
    {
        barraDeVida.value = cantidad;
    }
}
