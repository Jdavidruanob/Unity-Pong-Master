using UnityEngine;

public class ControlPelota : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [Header("Configuración de Velocidad")]
    [SerializeField] private float velocidadBase = 8f;
    [SerializeField] private float multiplicadorAmarillo = 1.4f;
    [SerializeField] private float multiplicadorRojo = 1.8f;

    [Header("Configuración de Colores")]
    [SerializeField] private Color colorVerde = Color.green;
    [SerializeField] private Color colorAmarillo = Color.yellow;
    [SerializeField] private Color colorRojo = Color.red;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int contadorGolpes = 0;
    private Vector2 posicionInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        posicionInicial = transform.position;

        // Nos aseguramos de que la pelota empiece sin estela
        TrailRenderer trail = GetComponent<TrailRenderer>();
        if (trail != null) trail.emitting = false;

        ReiniciarPelota();
    }

    public void Lanzar()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;

        // Empezamos con velocidad base y color verde
        rb.linearVelocity = new Vector2(x * velocidadBase, y * velocidadBase);
        ActualizarEstadoVisual();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Paleta"))
        {
            // --- 1. TU LÓGICA ORIGINAL DE FASES Y ENERGÍA ---
            contadorGolpes++;
            ActualizarEstadoVisual(); // El método que ya cambia colores y velocidad

            int idJugador = (col.transform.position.x < 0) ? 1 : 2;

            int fase = 0;
            if (contadorGolpes > 4 && contadorGolpes <= 8) fase = 1;
            else if (contadorGolpes > 8) fase = 2;

            gameManager.GanarEnergia(idJugador, fase);

            // --- 2. LÓGICA NUEVA: TRANSFERENCIA DEL POWERSHOT ---
            ControlPaleta paleta = col.gameObject.GetComponent<ControlPaleta>();
            if (paleta != null && paleta.tienePowerShot)
            {
                // Multiplicamos la velocidad actual (sea verde, amarilla o roja) por 2
                rb.linearVelocity *= 2.0f;

                // Encendemos la estela de fuego en la pelota
                TrailRenderer trailPelota = GetComponent<TrailRenderer>();
                if (trailPelota != null)
                {
                    trailPelota.emitting = true;
                    trailPelota.startColor = Color.red; // Color de fuego intenso

                    // La estela de la pelota dura 1.5 segundos encendida
                    Invoke("ApagarTrailPelota", 1.5f);
                }
            }
        }
    }

    void ApagarTrailPelota()
    {
        TrailRenderer trailPelota = GetComponent<TrailRenderer>();
        if (trailPelota != null) trailPelota.emitting = false;
    }

    void ActualizarEstadoVisual()
    {
        if (contadorGolpes <= 4) // FASE VERDE
        {
            spriteRenderer.color = colorVerde;
            rb.linearVelocity = rb.linearVelocity.normalized * velocidadBase;
        }
        else if (contadorGolpes <= 8) // FASE AMARILLA
        {
            spriteRenderer.color = colorAmarillo;
            rb.linearVelocity = rb.linearVelocity.normalized * (velocidadBase * multiplicadorAmarillo);
        }
        else // FASE ROJA
        {
            spriteRenderer.color = colorRojo;
            rb.linearVelocity = rb.linearVelocity.normalized * (velocidadBase * multiplicadorRojo);

            if (contadorGolpes > 10)
            {
                contadorGolpes = 0;
                ActualizarEstadoVisual();
            }
        }
    }

    public void ReiniciarPelota()
    {
        contadorGolpes = 0;
        rb.linearVelocity = Vector2.zero;
        transform.position = posicionInicial;

        // Apagar la estela al reiniciar si hubo gol
        ApagarTrailPelota();

        Invoke("Lanzar", 1.5f);
    }
}