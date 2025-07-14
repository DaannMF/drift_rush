# ArcadeCarController

Controlador de físicas arcade del auto con sistema de drift y recuperación automática.

## Funcionalidad Principal

- **Físicas arcade**: Controles responsivos optimizados para diversión
- **Sistema de drift**: Activado con freno de mano
- **Detección de suelo**: Raycast en 4 puntos para estabilidad
- **Recuperación automática**: Endereza el auto cuando se voltea
- **Efectos integrados**: Audio y visuales automáticos

## Configuración

```csharp
[Header("Car Settings")]
[SerializeField] private Car_SO data;              // Configuración del auto
[SerializeField] private Rigidbody rb;             // Rigidbody del auto
[SerializeField] private LayerMask groundLayer;    // Capa del suelo
[SerializeField] private float groundCheckDistance = 0.5f;

[Header("Recovery Settings")]
[SerializeField] private float upRightForce = 300f;
[SerializeField] private float uprightTorque = 3000f;
[SerializeField] private float maxVerticalVelocity = 25f;
```

## Eventos que Escucha

- `CarEvents.onCarThrottleInput` - Entrada de aceleración
- `CarEvents.onCarSteerInput` - Entrada de dirección
- `CarEvents.onCarDriftInput` - Entrada de drift
- `CarEvents.onResetCar` - Resetear auto

## Eventos que Dispara

### Audio

- `AudioEvents.onCarAccelerate` - Inicia sonido de aceleración
- `AudioEvents.onCarBrake` - Inicia sonido de frenado
- `AudioEvents.onCarDrift` - Inicia sonido de drift
- `AudioEvents.onCarIdle` - Inicia sonido de idle

### Efectos

- `CarEvents.onDriftStarted` - Inicia efectos de drift
- `CarEvents.onDriftEnded` - Termina efectos de drift
- `CarEvents.onCarSpeedChanged` - Actualiza velocidad

## Sistemas Principales

### Detección de Suelo

```csharp
// Raycast en 4 puntos para detectar suelo
private void CheckIsGroundedAndRotate()
```

### Recuperación Automática

```csharp
// Endereza el auto cuando se voltea
private void HandleUprightRecovery()
```

### Sistema de Drift

```csharp
// Cambia ángulo de dirección cuando se activa drift
float currentSteerAngle = currentDrift ? data.driftSteerAngle : data.normalSteerAngle;
```

## Física del Auto

### En el Suelo

- **Drag**: Aplicado según configuración
- **Fuerza**: Aplicada hacia adelante
- **Dirección**: Basada en input del jugador

### En el Aire

- **Gravedad**: Fuerza hacia abajo
- **Drag reducido**: Menos resistencia
- **Recuperación**: Solo si está muy inclinado

## Parámetros del Car_SO

- `motorTorque` - Fuerza del motor
- `normalSteerAngle` - Ángulo de dirección normal
- `driftSteerAngle` - Ángulo de dirección en drift
- `maxWheelRotation` - Rotación máxima de ruedas
- `dragOnGround` - Resistencia en suelo
- `dragInAir` - Resistencia en aire
- `gravityForce` - Fuerza de gravedad

## Uso

1. **Configuración**: Asignar Car_SO y Rigidbody
2. **Automático**: Responde a eventos de input
3. **Físicas**: Utiliza FixedUpdate para física
4. **Efectos**: Integra automáticamente audio y visuales

## Integración

```csharp
// El auto responde automáticamente a:
CarEvents.onCarThrottleInput?.Invoke(throttle);
CarEvents.onCarSteerInput?.Invoke(steer);
CarEvents.onCarDriftInput?.Invoke(drift);
```

## Limitaciones de Seguridad

- **Velocidad vertical**: Máximo 25f para evitar disparos al cielo
- **Recuperación inteligente**: Solo se activa cuando es necesario
- **Detección de atasco**: Resetea automáticamente si está atascado

## Notas Técnicas

- **Event-driven**: Responde solo a eventos
- **Stateless input**: No mantiene estado de input
- **Física optimizada**: Usa ForceMode apropiado para cada situación
- **Recuperación suave**: Evita saltos bruscos
