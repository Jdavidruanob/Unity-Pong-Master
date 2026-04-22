# Retro Pong - Power Edition 🏓🔥

Este es un proyecto de Pong moderno desarrollado en **Unity**, enfocado en mejorar el **Game Feel**, la respuesta física y la implementación de sistemas de progresión visual y mecánica (fases de velocidad y poderes especiales).

## 🚀 Características Principales

### 1. Sistema de Fases Dinámicas
La pelota no mantiene una velocidad constante. El juego evoluciona a través de 3 fases basadas en el número de golpes, lo cual aumenta la tensión competitiva:
- **Fase Verde (Calentamiento):** Velocidad base controlada.
- **Fase Amarilla (Tensión):** Incremento de velocidad y cambio visual.
- **Fase Roja (Clímax):** Velocidad máxima y reinicio de ciclo tras alcanzar el límite.

### 2. Físicas de Rebote Avanzado
Se implementó un sistema de **Rebote Relativo** para evitar ángulos muertos y rebotes infinitos:
- **Control de Ángulo:** El ángulo de salida de la pelota depende del punto exacto de impacto en la paleta (bordes vs. centro).
- **Anti-Estancamiento:** Lógica programada para detectar si la pelota se mueve demasiado lento en el eje horizontal, forzando un impulso para mantener el dinamismo.

### 3. Sistema de Poderes y Energía
- **Gestión de Energía:** Cada jugador acumula energía al golpear la pelota. La cantidad de energía ganada depende de la fase actual (Verde, Amarilla o Roja).
- **Power Shot:** Al activar el poder, la paleta entra en un estado de "Aura" (Particle System) que duplica la velocidad de la pelota al siguiente contacto.

### 4. Efectos Visuales (VFX)
- **Aura de Poder:** Sistema de partículas configurado en modo *Local* con ráfagas (*Bursts*) explosivas para feedback inmediato al jugador.
- **Trail Renderers:** Estelas de movimiento aplicadas a la pelota y paletas para enfatizar la velocidad.
- **Pixel Art UI:** Interfaz diseñada con fuentes Pixel Art para mantener una estética retro coherente.

## 🛠️ Tecnologías Utilizadas
- **Motor:** Unity 6 (o tu versión actual)
- **Lenguaje:** C#
- **UI:** TextMeshPro para tipografía nítida.
- **Gráficos:** Sprites 2D con configuraciones de filtrado "Point" para estética Pixel Art.

## 📂 Estructura del Código
- `ControlPelota.cs`: Gestiona la física, detección de colisiones, cálculo de ángulos y cambios de fase.
- `ControlPaleta.cs`: Maneja el movimiento de los jugadores y la activación del estado de poder/aura.
- `GameManager.cs`: Administra el sistema de puntos, la energía de los jugadores y la lógica de los menús.

## 🎮 Cómo Jugar
- **Jugador 1:** W/S para moverse. Tecla **E** para activar poder.
- **Jugador 2:** Flechas Arriba/Abajo para moverse. Tecla **K** (o la que asignaras) para activar poder.
- **Objetivo:** Acumular puntos haciendo que la pelota pase la paleta del rival. ¡Usa los bordes de tu paleta para dirigir la bola a ángulos difíciles!

---
**Desarrollado por:** Jose David Ruano Burbano  
*Estudiante de Ingeniería de Sistemas - Pontificia Universidad Javeriana*