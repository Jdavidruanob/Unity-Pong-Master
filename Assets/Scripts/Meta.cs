using UnityEngine;

public class Meta : MonoBehaviour
{
    [SerializeField] private int jugadorQueAnota; // 1 para la meta derecha, 2 para la izquierda
    [SerializeField] private GameManager gameManager;

    // Este método es exclusivo de Unity y se activa SOLO si marcaste "Is Trigger"
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si lo que atravesó la meta fue la pelota
        if (collision.gameObject.name == "Pelota")
        {
            gameManager.AnotarGol(jugadorQueAnota);
        }
    }
}