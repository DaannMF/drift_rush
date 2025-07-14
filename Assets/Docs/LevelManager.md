# LevelManager

Maneja la carga de escenas, transiciones entre niveles y posicionamiento del jugador.

## Funcionalidad Principal

- **Carga de escenas**: Transiciones suaves entre MainMenu y niveles
- **Gestión de datos**: Carga automática de datos de niveles desde Resources
- **Posicionamiento**: Coloca el jugador en el SpawnPoint de cada nivel
- **UI Configuration**: Configura paneles UI según el contexto

## Configuración

```csharp
[Header("Level Settings")]
[SerializeField] private List<LevelData> levels;
[SerializeField] private string mainMenuSceneName = "MainMenu";
```

## Métodos Públicos

- `LoadLevel(int levelIndex)` - Carga un nivel específico
- `LoadNextLevel()` - Carga el siguiente nivel
- `RestartCurrentLevel()` - Reinicia el nivel actual
- `LoadMainMenu()` - Regresa al menú principal

## Eventos que Escucha

- `LevelEvents.onLoadLevel` - Cargar nivel por índice
- `LevelEvents.onLoadNextLevel` - Cargar siguiente nivel
- `LevelEvents.onRestartLevel` - Reiniciar nivel actual
- `LevelEvents.onLoadMainMenu` - Cargar menú principal

## Eventos que Dispara

- `LevelEvents.onLevelLoadStarted` - Inicio de carga
- `LevelEvents.onLevelLoadProgress` - Progreso de carga
- `LevelEvents.onLevelLoadCompleted` - Fin de carga
- `UIEvents.onShowMainMenu` - Mostrar UI del menú
- `UIEvents.onShowGameUI` - Mostrar UI del juego

## Propiedades

- `CurrentLevel` - Datos del nivel actual
- `IsInMainMenu` - Si está en escena MainMenu
- `IsInLevel` - Si está en un nivel de juego
- `TotalLevels` - Número total de niveles

## Uso

1. **Persistente**: Mantiene estado entre escenas
2. **Singleton**: Una instancia global
3. **Automático**: Carga datos de niveles automáticamente
4. **Asíncrono**: Carga escenas de forma no bloqueante

## Integración

```csharp
// Cargar nivel 1
LevelEvents.onLoadLevel?.Invoke(0);

// Siguiente nivel
LevelEvents.onLoadNextLevel?.Invoke();

// Consultar nivel actual
LevelEvents.onGetCurrentLevelIndex?.Invoke(callback);
```

## Estructura de Niveles

Los datos de niveles se cargan automáticamente desde:

- `Assets/Resources/Data/Level1Data.asset`
- `Assets/Resources/Data/Level2Data.asset`
- `Assets/Resources/Data/Level3Data.asset`
