# CarController - Guía de Uso

## Resumen de la Refactorización

El CarController ha sido completamente refactorizado para ofrecer un manejo más realista y amigable del vehículo. Se han separado las responsabilidades en un ScriptableObject (CarSettings) y se han agregado múltiples mejoras físicas.

## Nuevas Funcionalidades

### 1. Sistema de Configuración con ScriptableObject

- **CarSettings**: Todas las configuraciones ahora están en un ScriptableObject reutilizable
- **Presets incluidos**: Arcade, Balanceado, Realista, y Carreras
- **Configuración visual**: Parámetros organizados en categorías en el Inspector

### 2. Manejo de Inputs Mejorado

- **Teclado**: WASD o flechas direccionales
- **Gamepad**: Soporte nativo para joysticks Xbox/PlayStation (mediante InputManager)
- **Controles**:
  - W/↑ o stick hacia adelante: Acelerar
  - S/↓ o stick hacia atrás: Frenar/Reversa
  - A/D o ←/→ o stick lateral: Dirección
  - Espacio: Freno de mano

### 3. Física del Vehículo Mejorada

#### Parámetros de Física (`Car Physics`)

- **Motor Torque**: Potencia del motor (1200f por defecto)
- **Max Steer Angle**: Ángulo máximo de dirección (25° por defecto)
- **Brake Torque**: Fuerza de frenado (2000f por defecto)
- **Handbrake Torque**: Fuerza del freno de mano (4000f por defecto)

#### Configuración de Manejo (`Handling`)

- **Steering Smoothness**: Suavizado de la dirección (5f por defecto)
- **Throttle Smoothness**: Suavizado del acelerador (3f por defecto)
- **Downforce**: Fuerza hacia abajo a alta velocidad (100f por defecto)
- **Anti Roll**: Resistencia al balanceo en curvas (5000f por defecto)

#### Control de Velocidad (`Speed Control`)

- **Max Speed**: Velocidad máxima en km/h (50 por defecto)
- **Speed Limit Factor**: Factor de limitación de velocidad (0.8f por defecto)

#### Sistemas de Estabilidad (`Stability`)

- **Traction Control**: Control de tracción (0.8f por defecto)
- **Stability Control**: Control de estabilidad (0.5f por defecto)

## Configuración de CarSettings

### Crear un nuevo CarSettings

1. Click derecho en Project → Create → DriftRush → Car Settings
2. Nombra el archivo (ej: "DefaultCarSettings")
3. Configura los parámetros según necesidades
4. Asigna el asset al CarController

### Usar Presets

1. En el CarSettings, marca "Use Preset"
2. Selecciona el preset deseado
3. Los valores se aplicarán automáticamente

### Presets Disponibles

#### Arcade (Fácil de Manejar)

- Motor Torque: 1500f
- Steering Smoothness: 8f
- Traction Control: 0.9f
- Max Speed: 60 km/h

#### Balanceado (Por Defecto)

- Motor Torque: 1200f
- Steering Smoothness: 5f
- Traction Control: 0.8f
- Max Speed: 50 km/h

#### Realista (Más Desafiante)

- Motor Torque: 1000f
- Steering Smoothness: 3f
- Traction Control: 0.4f
- Max Speed: 45 km/h

#### Carreras (Alta Velocidad)

- Motor Torque: 1800f
- Steering Smoothness: 4f
- Traction Control: 0.6f
- Max Speed: 80 km/h

## Configuración de Ruedas

### Propiedades de la Clase Wheel

Cada rueda ahora tiene las siguientes propiedades configurables:

- **Is Front Wheel**: Define si es rueda delantera o trasera
- **Steer**: Si la rueda puede girar para direccionar
- **Power**: Si la rueda recibe potencia del motor
- **Invert Steer**: Invertir la dirección (útil para ruedas traseras direccionales)

### Configuración Típica

- **Ruedas Delanteras**: IsFrontWheel = true, Steer = true, Power = false/true
- **Ruedas Traseras**: IsFrontWheel = false, Steer = false, Power = true

## InputManager (Clase Separada)

Se incluye una clase `InputManager` completamente funcional que puede ser usada para proyectos más avanzados.

### Características del InputManager

- Detección automática de gamepad
- Soporte para múltiples tipos de input
- Configuración de sensibilidad
- Deadzone configurable para gamepads
- Detección de Xbox y PlayStation controllers

### Uso del InputManager

```csharp
// Obtener input actual
InputData input = inputManager.GetInput();

// Configurar sensibilidad
inputManager.SetSensitivity(steerSensitivity, throttleSensitivity);

// Cambiar tipo de input manualmente
inputManager.SetInputType(InputType.Gamepad);
```

## Métodos Públicos del CarController

### Propiedades de Solo Lectura

- `CurrentSpeed`: Velocidad actual en km/h
- `CurrentSteerAngle`: Ángulo actual de dirección
- `IsBraking`: Si está frenando actualmente

### Métodos Útiles

- `ResetCar()`: Resetea velocidad y rotación del vehículo
- `SetMaxSpeed(float speed)`: Cambia la velocidad máxima
- `SetMotorTorque(float torque)`: Cambia la potencia del motor

## Configuración en Unity

### Setup Básico

1. **CarController**: Asignar CarSettings y Center of Mass
2. **Wheels**: Configurar como Front/Rear, Steer/Power según posición
3. **Physics**: Ajustar Rigidbody mass (recomendado: 1000-1500)
4. **WheelColliders**: Configurar suspensión y fricción apropiadas

### Tips de Configuración

- **Centro de Masa**: Colocar ligeramente hacia atrás y abajo para mejor estabilidad
- **Masa**: 1000-1500 para carros normales, 2000+ para camiones
- **Suspensión**: Distance 0.3-0.5, Spring 35000, Damper 4500

## Mejoras Implementadas

1. **ScriptableObject**: Configuraciones reutilizables y organizadas
2. **Presets**: Configuraciones predefinidas para diferentes estilos
3. **Suavizado de Inputs**: Los inputs ahora se suavizan para evitar cambios bruscos
4. **Sistema de Frenos Separado**: Freno normal y freno de mano funcionan independientemente
5. **Control de Velocidad**: Limitación progresiva de potencia cerca de la velocidad máxima
6. **Downforce**: Mejor agarre a altas velocidades
7. **Traction Control**: Reduce la potencia cuando las ruedas patinan
8. **Stability Control**: Mantiene el auto estable en situaciones extremas
9. **Anti-Roll Bars**: Reduce el balanceo en curvas
10. **Centro de Masa Configurable**: Para ajustar la estabilidad del vehículo

## Notas Adicionales

- El sistema es completamente retrocompatible con configuraciones existentes
- Se recomienda ajustar los parámetros según el tipo de juego deseado
- Para uso con InputManager, simplemente reemplaza el código de input en HandleInput()
- Todos los valores están optimizados para Unity con escala normal (1 unidad = 1 metro)
- Los CarSettings pueden ser compartidos entre múltiples vehículos
- Usar presets como punto de partida y luego ajustar según necesidades específicas
