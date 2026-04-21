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

    [Header("Configuración de Fases (Límite de golpes)")]
    [SerializeField] private int limiteVerde = 2;      // Se acaba lo verde en el golpe 2
    [SerializeField] private int limiteAmarillo = 6;   // Se acaba lo amarillo en el golpe 6
    [SerializeField] private int limiteReinicio = 8;   // En qué golpe vuelve a ser verde

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Paleta"))
        {
            // --- 1. TU LÓGICA ORIGINAL DE FASES Y ENERGÍA ---
            contadorGolpes++;
            ActualizarEstadoVisual();

            // Identificamos al jugador por el nombre del objeto o posición
            int idJugador = (col.transform.position.x < 0) ? 1 : 2;

            // Determinamos la fase usando nuestras nuevas variables configurables
            int fase = 0;
            if (contadorGolpes > limiteVerde && contadorGolpes <= limiteAmarillo) fase = 1;
            else if (contadorGolpes > limiteAmarillo) fase = 2;

            gameManager.GanarEnergia(idJugador, fase);

            // --- 2. MAGIA PONG: EL ÁNGULO RELATIVO ---
            // A. ¿Dónde golpeó la pelota respecto al centro de la paleta?
            float yPelota = transform.position.y;
            float yPaleta = col.transform.position.y;
            float alturaPaleta = col.collider.bounds.size.y;

            // Esto nos da un valor entre -1 (borde inferior) y 1 (borde superior)
            float factorImpacto = (yPelota - yPaleta) / (alturaPaleta / 2f);

            // B. Anti-línea recta: Si golpea justo en el centro, forzamos un mínimo ángulo
            if (Mathf.Abs(factorImpacto) < 0.15f)
            {
                factorImpacto = 0.15f * Mathf.Sign(factorImpacto == 0 ? Random.Range(-1f, 1f) : factorImpacto);
            }

            // C. Calculamos la nueva dirección. 
            // Multiplicamos por 0.8f para limitar qué tan vertical puede salir y evitar rebotes infinitos.
            float dirX = (col.transform.position.x < 0) ? 1 : -1;
            Vector2 nuevaDireccion = new Vector2(dirX, factorImpacto * 0.8f).normalized;

            // D. Aplicamos la nueva dirección pero mantenemos la velocidad que ya traía el juego
            float velocidadActual = rb.linearVelocity.magnitude;
            rb.linearVelocity = nuevaDireccion * velocidadActual;

            // --- 3. LÓGICA DE TRANSFERENCIA DEL POWERSHOT ---
            ControlPaleta paleta = col.gameObject.GetComponent<ControlPaleta>();
            if (paleta != null && paleta.tienePowerShot)
            {
                rb.linearVelocity *= 2.0f; // Multiplicador de velocidad del poder

                TrailRenderer trailPelota = GetComponent<TrailRenderer>();
                if (trailPelota != null)
                {
                    trailPelota.emitting = true;
                    trailPelota.startColor = Color.red;
                    Invoke("ApagarTrailPelota", 1.5f);
                }
            }
        }
        else
        {
            // --- 4. ANTI-ESTANCAMIENTO EN PAREDES (Techo/Suelo) ---
            // Si la pelota rebota en una pared pero avanza muy lento hacia los jugadores, la aceleramos en X
            if (Mathf.Abs(rb.linearVelocity.x) < 2f)
            {
                float empujeX = (rb.linearVelocity.x > 0 ? 2f : -2f);
                // Mantenemos la magnitud general pero mejoramos el ángulo horizontal
                rb.linearVelocity = new Vector2(empujeX, rb.linearVelocity.y).normalized * rb.linearVelocity.magnitude;
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
        if (contadorGolpes <= limiteVerde) // FASE VERDE
        {
            spriteRenderer.color = colorVerde;
            rb.linearVelocity = rb.linearVelocity.normalized * velocidadBase;
        }
        else if (contadorGolpes <= limiteAmarillo) // FASE AMARILLA
        {
            spriteRenderer.color = colorAmarillo;
            rb.linearVelocity = rb.linearVelocity.normalized * (velocidadBase * multiplicadorAmarillo);
        }
        else // FASE ROJA
        {
            spriteRenderer.color = colorRojo;
            rb.linearVelocity = rb.linearVelocity.normalized * (velocidadBase * multiplicadorRojo);

            // Reinicio de ciclo
            if (contadorGolpes > limiteReinicio)
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