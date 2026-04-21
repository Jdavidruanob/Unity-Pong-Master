using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoPuntajeJ1;
    [SerializeField] private TextMeshProUGUI textoPuntajeJ2;
    [SerializeField] private ControlPelota scriptPelota; // Referencia a la pelota

    private int puntosJ1 = 0;
    private int puntosJ2 = 0;

    public void AnotarGol(int jugadorQueAnoto)
    {
        if (jugadorQueAnoto == 1)
        {
            puntosJ1++;
            textoPuntajeJ1.text = puntosJ1.ToString();
        }
        else
        {
            puntosJ2++;
            textoPuntajeJ2.text = puntosJ2.ToString();
        }

        // Llamamos al método de reinicio de la pelota
        scriptPelota.ReiniciarPelota();
    }
}