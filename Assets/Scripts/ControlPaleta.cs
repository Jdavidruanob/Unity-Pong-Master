using UnityEngine;
using System.Collections; // Necesario para la Corrutina (el temporizador de 3 seg)

public class ControlPaleta : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private bool esJugador1;
    [SerializeField] private float rangoMovimiento = 4.0f;

    private Rigidbody2D rb;
    private float yInicial;

    // --- VARIABLES DE PODER (Lo nuevo) ---
    private TrailRenderer trail;
    public bool tienePowerShot = false; // "public" para que el GameManager la vea

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        yInicial = transform.position.y;

        // --- INICIALIZAR TRAIL (Lo nuevo) ---
        trail = GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.emitting = false; // Empezamos apagados
        }
        else
        {
            Debug.LogError("¡Falta el componente Trail Renderer en " + gameObject.name + "!");
        }
    }

    void Update()
    {
        // Tu lógica original de movimiento (intacta)
        float movimiento;
        if (esJugador1)
        {
            movimiento = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movimiento = Input.GetAxisRaw("Vertical2");
        }

        rb.linearVelocity = new Vector2(0, movimiento * velocidad);

        float yLimitado = Mathf.Clamp(transform.position.y, yInicial - rangoMovimiento, yInicial + rangoMovimiento);
        transform.position = new Vector3(transform.position.x, yLimitado, transform.position.z);
    }

    // --- FUNCIONES DE PODER (Lo nuevo) ---
    public void ActivarPowerShot()
    {
        // Verificamos por seguridad que el componente exista
        if (trail == null) return;

        // Iniciamos el temporizador de 3 seg
        StartCoroutine(DuracionPowerShot());
    }

    IEnumerator DuracionPowerShot()
    {
        tienePowerShot = true;
        trail.emitting = true; // Encendemos la estela
        trail.startColor = new Color(1f, 0.5f, 0f); // Color naranja de fuego

        // Esperamos 3 segundos reales
        yield return new WaitForSeconds(3f);

        tienePowerShot = false;
        trail.emitting = false; // Apagamos la estela
    }
}