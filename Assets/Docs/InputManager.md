# InputManager

Maneja toda la entrada del usuario usando Unity Input System y la convierte en eventos.

## Funcionalidad Principal

- **Unity Input System**: Soporte moderno para múltiples dispositivos
- **Conversión a eventos**: Traduce input a eventos del sistema
- **Soporte multi-dispositivo**: Teclado, gamepad, automático
- **Pausa inteligente**: Solo pausa durante niveles

## Configuración

Usa `Assets/Input/PlayerInput.inputactions` para configurar:

- **Throttle**: W/S, gatillos gamepad
- **Steer**: A/D, stick izquierdo
- **Drift**: Espacio, botones gamepad
- **Restart**: R, botón gamepad
- **Pause**: Escape, botón start

## Métodos de Input

```csharp
private void OnThrottle(InputValue value)
private void OnSteer(InputValue value)
private void OnDrift(InputValue value)
private void OnRestart(InputValue _)
private void OnPause(InputValue _)
```

## Eventos que Dispara

### Controles del Auto

- `CarEvents.onCarThrottleInput` - Aceleración/frenado
- `CarEvents.onCarSteerInput` - Dirección
- `CarEvents.onCarDriftInput` - Drift activado/desactivado
- `CarEvents.onResetCar` - Resetear posición

### Controles del Juego

- `GameEvents.onPauseGame` - Pausar juego
- `GameEvents.onResumeGame` - Reanudar juego
- `UIEvents.onShowPauseMenu` - Mostrar menú de pausa

## Lógica de Pausa

```csharp
// Solo pausa si estamos en un nivel
LevelEvents.onGetIsInLevel?.Invoke(isInLevel => {
    if (isInLevel) {
        if (Time.timeScale == 0f) {
            GameEvents.onResumeGame?.Invoke();
        } else {
            GameEvents.onPauseGame?.Invoke();
            UIEvents.onShowPauseMenu?.Invoke();
        }
    }
});
```

## Uso

1. **Automático**: Se conecta automáticamente con PlayerInput component
2. **Event-based**: No referencias directas, todo por eventos
3. **Contexto inteligente**: Pausa solo funciona en niveles
4. **Toggle**: Escape alterna entre pausa/reanuda

## Integración

El `InputManager` se conecta automáticamente con:

- `PlayerInput` component (Unity Input System)
- `PlayerInput.inputactions` asset
- Sistema de eventos del juego

## Configuración de Dispositivos

**Teclado**:

- W/S: Throttle
- A/D: Steer
- Espacio: Drift
- R: Restart
- Escape: Pause

**Gamepad**:

- Stick izquierdo: Steer
- Gatillos: Throttle
- Botones: Drift
- Start: Pause

## Notas Técnicas

- **Sin estado**: No mantiene referencias a valores
- **Inmediato**: Convierte input a eventos al instante
- **Ligero**: Procesamiento mínimo, solo traducción
- **Desacoplado**: No conoce otros sistemas directamente
