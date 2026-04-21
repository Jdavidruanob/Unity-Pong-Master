using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject botonMenu;

    void Start()
    {
        Pausar(); // Empezar pausado
    }

    public void Reanudar()
    {
        Time.timeScale = 1f; // Reanuda el tiempo del motor físico
        botonMenu.SetActive(false); // Esconde el botón
    }

    public void Pausar()
    {
        Time.timeScale = 0f; // Congela el juego
        botonMenu.SetActive(true); // Muestra el botón
    }
}