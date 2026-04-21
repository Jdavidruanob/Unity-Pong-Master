using UnityEngine;
using System.Collections;

public class ControlPaleta : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private bool esJugador1;
    [SerializeField] private float rangoMovimiento = 4.0f;

    private Rigidbody2D rb;
    private float yInicial;

    // --- NUEVAS VARIABLES PARA EL AURA ---
    private ParticleSystem aura;
    public bool tienePowerShot = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        yInicial = transform.position.y;

        // Buscamos el sistema de partículas
        aura = GetComponent<ParticleSystem>();
        if (aura != null)
        {
            var emission = aura.emission;
            emission.enabled = false; // Empezamos con el aura apagada
        }
    }

    void Update()
    {
        // Tu lógica de movimiento original...
        float movimiento = esJugador1 ? Input.GetAxisRaw("Vertical") : Input.GetAxisRaw("Vertical2");
        rb.linearVelocity = new Vector2(0, movimiento * velocidad);
        float yLimitado = Mathf.Clamp(transform.position.y, yInicial - rangoMovimiento, yInicial + rangoMovimiento);
        transform.position = new Vector3(transform.position.x, yLimitado, transform.position.z);
    }

    public void ActivarPowerShot()
    {
        if (aura == null) return;
        StartCoroutine(DuracionPowerShot());
    }

    IEnumerator DuracionPowerShot()
    {
        tienePowerShot = true;

        // Encender el aura
        var emission = aura.emission;
        emission.enabled = true;
        aura.Play();

        yield return new WaitForSeconds(3f);

        // Apagar el aura
        tienePowerShot = false;
        emission.enabled = false;
    }
}