using UnityEngine;

public class ControlPelota : MonoBehaviour
{
    [SerializeField] private float velocidadInicial = 8f;
    private Rigidbody2D rb;
    private Vector2 posicionInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;
        Lanzar();
    }

    public void Lanzar()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.linearVelocity = new Vector2(x * velocidadInicial, y * velocidadInicial);
    }

    public void ReiniciarPelota()
    {
        rb.linearVelocity = Vector2.zero; // Detenerla
        transform.position = posicionInicial; // Llevarla al centro
        Invoke("Lanzar", 1f); // Esperar 1 segundo y lanzar
    }
}