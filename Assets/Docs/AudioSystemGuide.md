# 🎵 Sistema de Audio Completo - Drift Rush

## Resumen

Sistema completo de audio que maneja música de fondo, efectos de sonido de UI y auto, usando eventos y persistencia entre escenas.

## 🏗️ Arquitectura

### AudioManager

- **Singleton**: Persistente usando DontDestroyOnLoad
- **Event-Based**: Sin referencias directas, todo por eventos
- **Pool de AudioSources**: 10 fuentes para efectos simultáneos
- **Configuración desde Inspector**: Todos los clips asignables

### Componentes Principales

```
AudioManager (Persistent)
├── Music System
│   ├── Menu Music
│   └── Game Music
├── SFX System
│   ├── UI Sounds (hover, click, menu)
│   ├── Car Sounds (acelerar, frenar, drift, crash)
│   └── Game Sounds (monedas, victoria, derrota)
└── Volume Control
    ├── Music Volume
    └── SFX Volume
```

## 🎮 Eventos de Audio

### UI Events

```csharp
GameEvents.onButtonHover      // Hover sobre botón
GameEvents.onButtonClick      // Click en botón
GameEvents.onMenuOpen         // Abrir menú
GameEvents.onMenuClose        // Cerrar menú
```

### Car Events

```csharp
GameEvents.onCarAccelerate    // Acelerar
GameEvents.onCarBrake         // Frenar
GameEvents.onCarIdle          // Auto parado
GameEvents.onCarDrift         // Drifting
GameEvents.onCarCrash         // Crash/recuperación forzada
GameEvents.onCarReset         // Reset del auto
```

### Game Events

```csharp
GameEvents.onCoinCollected    // Moneda colectada
GameEvents.onLevelWin         // Victoria
GameEvents.onLevelLose        // Derrota
GameEvents.onCountdown        // Cuenta regresiva
```

### Music Events

```csharp
GameEvents.onPlayMenuMusic    // Música de menú
GameEvents.onPlayGameMusic    // Música de juego
GameEvents.onStopMusic        // Detener música
GameEvents.onSetMusicVolume   // Ajustar volumen música
GameEvents.onSetSFXVolume     // Ajustar volumen efectos
```

## 📋 Setup Instructions

### 1. Crear AudioManager GameObject

```
1. Crear Empty GameObject → AudioManager
2. Agregar script AudioManager
3. Asignar AudioClips en Inspector
4. Agregar a PersistentObjectsManager
```

### 2. Configurar UI Audio

```csharp
// Automático: agregar AutoUIAudioSetup a Canvas
public class AutoUIAudioSetup : MonoBehaviour {
    [SerializeField] private bool setupOnStart = true;
    [SerializeField] private bool includeChildButtons = true;
}

// Manual: agregar UIAudioHandler a cada botón
public class UIAudioHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
```

### 3. Configurar Controles de Volumen

```csharp
// Agregar AudioSettings a UI
public class AudioSettings : MonoBehaviour {
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
}
```

## 🔧 Configuración Técnica

### AudioManager Settings

```csharp
[Header("Audio Sources")]
[SerializeField] private int sfxSourcesCount = 10;   // Pool de fuentes SFX

[Header("Audio Settings")]
[SerializeField] private float musicVolume = 0.7f;   // Volumen música
[SerializeField] private float sfxVolume = 1f;       // Volumen efectos
```

### AudioClips por Categoría

```csharp
[Header("Music Clips")]
- menuMusic: Música del menú principal
- gameMusic: Música durante el juego

[Header("UI Sound Effects")]
- buttonHoverClip: Hover sobre botón
- buttonClickClip: Click en botón
- menuOpenClip: Abrir menú
- menuCloseClip: Cerrar menú

[Header("Car Sound Effects")]
- carAccelerateClip: Acelerar
- carBrakeClip: Frenar
- carIdleClip: Auto parado
- carDriftClip: Drift
- carCrashClip: Crash
- carResetClip: Reset del auto

[Header("Game Sound Effects")]
- coinCollectedClip: Moneda colectada
- levelWinClip: Victoria
- levelLoseClip: Derrota
- countdownClip: Cuenta regresiva
```

## 🎯 Integración con Scripts Existentes

### ArcadeCarController

```csharp
// Detección de drift
private void CheckDrift() {
    if (isDrifting && !wasDrifting) {
        GameEvents.onCarDrift?.Invoke();
    }
}

// Sonidos de movimiento
if (_input.Throttle > 0) {
    GameEvents.onCarAccelerate?.Invoke();
}
```

### GameManager

```csharp
// Música de fondo
public void InitializeLevel() {
    GameEvents.onPlayGameMusic?.Invoke();
}

// Sonidos de victoria/derrota
if (isGameWon) {
    GameEvents.onLevelWin?.Invoke();
}
```

### Coin

```csharp
void OnTriggerEnter(Collider other) {
    GameEvents.onCoinCollected?.Invoke();
    GameEvents.onAddCoin?.Invoke();
}
```

## 📱 Características Avanzadas

### Pool de AudioSources

- 10 fuentes para efectos simultáneos
- Round-robin cuando todas están ocupadas
- Búsqueda de fuentes disponibles

### Persistencia de Configuración

- PlayerPrefs para volúmenes
- Carga automática al iniciar
- Reset a valores por defecto

### Detección de Drift

- Basada en dot product entre forward y velocity
- Threshold configurable
- Previene spam de eventos

## 🐛 Troubleshooting

### Audio no se reproduce

1. Verificar que AudioManager esté en la escena
2. Comprobar que los clips estén asignados
3. Revisar que los eventos se estén disparando

### Múltiples AudioManagers

- El sistema usa Singleton pattern
- Solo uno persistirá (DontDestroyOnLoad)
- Los duplicados se destruyen automáticamente

### Volumen no persiste

- Verificar que AudioSettings esté funcionando
- Comprobar PlayerPrefs en Registry/Unix

## 🚀 Próximas Mejoras

1. **Audio Zones**: Diferentes ambientes por área
2. **Dynamic Music**: Cambios basados en gameplay
3. **3D Audio**: Efectos posicionales
4. **Audio Mixing**: Grupos de audio separados
5. **Fade Transitions**: Transiciones suaves entre música

## 📁 Archivos del Sistema

```
Assets/Scripts/
├── Managers/
│   ├── AudioManager.cs          # Manager principal
│   ├── AudioSettings.cs         # Configuración UI
│   └── PersistentObjectsManager.cs # Setup automático
├── UI/
│   └── UIAudioHandler.cs        # Sonidos de botones
├── Events/
│   └── GameEvents.cs            # Eventos de audio
└── Controllers/
    ├── Car/ArcadeCarController.cs # Integración auto
    └── Coin.cs                  # Integración monedas
```

## 💡 Ejemplo de Uso

```csharp
// Reproducir efecto específico
GameEvents.onCoinCollected?.Invoke();

// Cambiar volumen
GameEvents.onSetMusicVolume?.Invoke(0.5f);

// Cambiar música
GameEvents.onPlayMenuMusic?.Invoke();
```

---

Este sistema proporciona audio completo y flexible para Drift Rush, manteniendo el patrón event-based y la persistencia entre escenas.
