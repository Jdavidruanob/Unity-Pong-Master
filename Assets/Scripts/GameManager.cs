using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI de Puntaje")]
    [SerializeField] private TextMeshProUGUI textoPuntosJ1;
    [SerializeField] private TextMeshProUGUI textoPuntosJ2;

    [Header("Referencias de Paletas")]
    [SerializeField] public ControlPaleta scriptPaletaJ1;
    [SerializeField] public ControlPaleta scriptPaletaJ2;

    [Header("UI de Energía")]
    [SerializeField] private Image barraJ1;
    [SerializeField] private Image barraJ2;

    [Header("Referencias")]
    [SerializeField] private ControlPelota pelota;

    [Header("Repertorio de Poderes")]
    public List<Poder> listaDePoderes; // La "bolsa" de donde salen los poderes

    [Header("Slots de UI (3 por jugador)")]
    public Image[] slotsUI_J1;
    public Image[] slotsUI_J2;

    // Variables de estado
    private int puntosJ1 = 0, puntosJ2 = 0;
    private float energiaJ1 = 0f, energiaJ2 = 0f;
    private float maxEnergia = 100f;

    // Inventarios lógicos
    private List<Poder> inventarioJ1 = new List<Poder>();
    private List<Poder> inventarioJ2 = new List<Poder>();

    // Índice de qué poder tiene seleccionado el jugador (0, 1 o 2)
    private int indexSeleccionadoJ1 = 0;
    private int indexSeleccionadoJ2 = 0;

    

    void Start()
    {
        LimpiarUI();
        ActualizarInterfaz();
    }

    void Update()
    {
        // --- INPUT JUGADOR 1 ---
        if (Input.GetKeyDown(KeyCode.Q)) CiclarSeleccion(1);
        if (Input.GetKeyDown(KeyCode.E)) IntentarUsarPoder(1);

        // --- INPUT JUGADOR 2 ---
        if (Input.GetKeyDown(KeyCode.Period)) CiclarSeleccion(2); // Tecla "."
        if (Input.GetKeyDown(KeyCode.Return)) IntentarUsarPoder(2); // Enter
    }

    // --- SISTEMA DE ENERGÍA Y GOLES ---
    public void AnotarGol(int jugadorQueAnota)
    {
        if (jugadorQueAnota == 1) puntosJ1++;
        else puntosJ2++;

        ActualizarInterfaz();
        pelota.ReiniciarPelota();
    }

    public void GanarEnergia(int jugador, int faseBola)
    {
        float cantidad = (faseBola == 0) ? 10f : (faseBola == 1) ? 20f : 30f;

        if (jugador == 1) energiaJ1 += cantidad;
        else energiaJ2 += cantidad;

        energiaJ1 = Mathf.Clamp(energiaJ1, 0, maxEnergia);
        energiaJ2 = Mathf.Clamp(energiaJ2, 0, maxEnergia);

        // Si llega a 100, intentamos dar un poder
        if (jugador == 1 && energiaJ1 >= 100) EntregarPoderAleatorio(1);
        if (jugador == 2 && energiaJ2 >= 100) EntregarPoderAleatorio(2);

        ActualizarInterfaz();
    }

    // --- SISTEMA DE PODERES DINÁMICO ---
    void EntregarPoderAleatorio(int jugador)
    {
        if (listaDePoderes.Count == 0) return;

        if (jugador == 1 && inventarioJ1.Count < 3)
        {
            Poder p = listaDePoderes[Random.Range(0, listaDePoderes.Count)];
            inventarioJ1.Add(p);
            energiaJ1 = 0; // Reset energía al recibir poder
        }
        else if (jugador == 2 && inventarioJ2.Count < 3)
        {
            Poder p = listaDePoderes[Random.Range(0, listaDePoderes.Count)];
            inventarioJ2.Add(p);
            energiaJ2 = 0;
        }
        ActualizarInterfaz();
    }

    void CiclarSeleccion(int jugador)
    {
        if (jugador == 1 && inventarioJ1.Count > 0)
            indexSeleccionadoJ1 = (indexSeleccionadoJ1 + 1) % inventarioJ1.Count;
        else if (jugador == 2 && inventarioJ2.Count > 0)
            indexSeleccionadoJ2 = (indexSeleccionadoJ2 + 1) % inventarioJ2.Count;

        ActualizarInterfaz();
    }

    void IntentarUsarPoder(int jugador)
    {
        if (jugador == 1 && inventarioJ1.Count > 0)
        {
            Poder p = inventarioJ1[indexSeleccionadoJ1];
            EjecutarEfectoPoder(p, 1);
            inventarioJ1.RemoveAt(indexSeleccionadoJ1);
            if (indexSeleccionadoJ1 >= inventarioJ1.Count) indexSeleccionadoJ1 = 0;
        }
        else if (jugador == 2 && inventarioJ2.Count > 0)
        {
            Poder p = inventarioJ2[indexSeleccionadoJ2];
            EjecutarEfectoPoder(p, 2);
            inventarioJ2.RemoveAt(indexSeleccionadoJ2);
            if (indexSeleccionadoJ2 >= inventarioJ2.Count) indexSeleccionadoJ2 = 0;
        }
        ActualizarInterfaz();
    }

    void EjecutarEfectoPoder(Poder p, int jugador)
    {
        // Validación de seguridad: ¿El poder existe?
        if (p == null)
        {
            Debug.LogError("Error: Se intentó usar un poder que es NULL");
            return;
        }

        Debug.Log("Jugador " + jugador + " usó " + p.nombre);

        if (p.tipo == Poder.TipoPoder.PowerShot)
        {
            if (jugador == 1)
            {
                // Verificamos si la referencia a la paleta existe antes de llamarla
                if (scriptPaletaJ1 != null)
                    scriptPaletaJ1.ActivarPowerShot();
                else
                    Debug.LogError("¡Falta asignar la Paleta J1 en el Inspector del GameManager!");
            }
            else if (jugador == 2)
            {
                if (scriptPaletaJ2 != null)
                    scriptPaletaJ2.ActivarPowerShot();
                else
                    Debug.LogError("¡Falta asignar la Paleta J2 en el Inspector del GameManager!");
            }
        }
    }

    // --- INTERFAZ ---
    private void ActualizarInterfaz()
    {
        textoPuntosJ1.text = puntosJ1.ToString();
        textoPuntosJ2.text = puntosJ2.ToString();
        barraJ1.fillAmount = energiaJ1 / maxEnergia;
        barraJ2.fillAmount = energiaJ2 / maxEnergia;

        ActualizarSlots(inventarioJ1, slotsUI_J1, indexSeleccionadoJ1);
        ActualizarSlots(inventarioJ2, slotsUI_J2, indexSeleccionadoJ2);
    }

    private void ActualizarSlots(List<Poder> inventario, Image[] slots, int seleccionado)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventario.Count)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].sprite = inventario[i].iconoCard;
                // Feedback visual de selección (Escala o Color)
                slots[i].transform.localScale = (i == seleccionado) ? new Vector3(1.2f, 1.2f, 1) : Vector3.one;
                slots[i].color = (i == seleccionado) ? Color.white : new Color(0.7f, 0.7f, 0.7f);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void LimpiarUI()
    {
        foreach (var s in slotsUI_J1) s.gameObject.SetActive(false);
        foreach (var s in slotsUI_J2) s.gameObject.SetActive(false);
    }
}