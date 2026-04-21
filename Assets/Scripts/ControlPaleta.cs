using UnityEngine;

public class ControlPaleta : MonoBehaviour
{
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private bool esJugador1;
    [SerializeField] private float rangoMovimiento = 6.0f; // Cuánto puede subir/bajar desde su inicio

    private Rigidbody2D rb;
    private float yInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Guardamos la posición Y donde el programador puso la paleta manualmente
        yInicial = transform.position.y;
    }

    void Update()
    {
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

        // Limitamos el movimiento sumando el rango a la posición inicial
        float yLimitado = Mathf.Clamp(transform.position.y, yInicial - rangoMovimiento, yInicial + rangoMovimiento);
        transform.position = new Vector3(transform.position.x, yLimitado, transform.position.z);
    }
}