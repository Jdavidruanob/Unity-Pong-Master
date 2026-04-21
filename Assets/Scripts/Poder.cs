using UnityEngine;

[System.Serializable] // Esto permite que lo veas en el Inspector
public class Poder
{
    public string nombre;
    public Sprite iconoCard; // La imagen que generaste con la IA
    public float costoEnergia = 100f;

    // Aquí podrías agregar un ID para saber qué hace cada uno
    public enum TipoPoder { PowerShot, Escudo, RestaPuntos }
    public TipoPoder tipo;
}