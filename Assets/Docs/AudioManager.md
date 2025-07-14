# AudioManager

Maneja música de fondo, efectos de sonido y configuración de volumen de forma centralizada.

## Funcionalidad Principal

- **Música dinámica**: Cambia automáticamente entre menú y juego
- **Efectos de sonido**: Auto, UI y gameplay
- **Control de volumen**: Configuración por categorías
- **Persistencia**: Mantiene configuración entre sesiones

## Configuración

```csharp
[Header("Audio Settings")]
[SerializeField] private float masterVolume = 1f;
[SerializeField] private float musicVolume = 0.7f;
[SerializeField] private float uiVolume = 1f;
[SerializeField] private float sfxVolume = 1f;
```

## Tipos de Audio

### Música

- **Menu Music**: `menu_loop.mp3` - Reproduce en MainMenu
- **Game Music**: `game_loop.mp3` - Reproduce en niveles
- **Control**: Play, stop, pause, resume automático

### Efectos de Sonido

- **Auto**: Aceleración, frenado, drift, idle
- **UI**: Hover, click, menú open/close
- **Gameplay**: Monedas, victoria, derrota

## Eventos que Escucha

### Música

- `AudioEvents.onPlayMenuMusic` - Reproduce música de menú
- `AudioEvents.onPlayGameMusic` - Reproduce música de juego
- `AudioEvents.onPauseMusic` - Pausa música actual
- `AudioEvents.onResumeMusic` - Reanuda música pausada

### Efectos

- `AudioEvents.onCarAccelerate` - Sonido de aceleración
- `AudioEvents.onCarBrake` - Sonido de frenado
- `AudioEvents.onCarDrift` - Sonido de drift
- `AudioEvents.onCoinCollected` - Sonido de moneda

### Volumen

- `AudioEvents.onSetMasterVolume` - Volumen maestro
- `AudioEvents.onSetMusicVolume` - Volumen música
- `AudioEvents.onSetUIVolume` - Volumen UI
- `AudioEvents.onSetSFXVolume` - Volumen efectos

## Uso

1. **Persistente**: Mantiene configuración entre escenas
2. **Singleton**: Una instancia global
3. **Automático**: Carga clips desde Resources si no están asignados
4. **Pool de fuentes**: 10 AudioSources para efectos simultáneos

## Integración

```csharp
// Reproducir efecto
AudioEvents.onCoinCollected?.Invoke();

// Cambiar música
AudioEvents.onPlayMenuMusic?.Invoke();

// Configurar volumen
AudioEvents.onSetMasterVolume?.Invoke(0.8f);
```

## Estructura de Archivos

```
Assets/Resources/Art/Audio/
├── menu_loop.mp3    # Música del menú
├── game_loop.mp3    # Música del juego
├── coin.mp3         # Efecto de moneda
├── acceleration.wav # Sonido de aceleración
├── brake.wav        # Sonido de frenado
└── ...
```

## Configuración de Volumen

- **Master Volume**: Controla todo el audio
- **Music Volume**: Solo música de fondo
- **UI Volume**: Efectos de interfaz
- **SFX Volume**: Efectos de gameplay y auto
