# GameManager

Maneja la lógica principal del juego incluyendo tiempo, monedas, victoria/derrota y pausa.

## Funcionalidad Principal

- **Contador de tiempo**: Cuenta regresiva hasta 0
- **Sistema de monedas**: Rastrea monedas recolectadas vs objetivo
- **Estados del juego**: Pausa, reanuda, victoria, derrota
- **Configuración de niveles**: Establece objetivos de cada nivel

## Configuración

```csharp
[Header("Game Settings")]
[SerializeField] private int targetCoins = 10;
[SerializeField] private float timeLimit = 60;
```

## Eventos que Escucha

- `GameEvents.onPauseGame` - Pausa el juego
- `GameEvents.onResumeGame` - Reanuda el juego
- `GameEvents.onAddCoin` - Suma una moneda
- `GameEvents.onInitializeLevel` - Inicializa nuevo nivel

## Eventos que Dispara

- `GameEvents.onCurrentTimeChanged` - Actualiza UI del tiempo
- `GameEvents.onCurrentCoinsChanged` - Actualiza UI de monedas
- `GameEvents.onGameFinished` - Nivel terminado
- `AudioEvents.onPlayGameMusic` - Reproduce música de juego

## Uso

1. **Persistente**: Usa `DontDestroyOnLoad()` para mantener estado entre escenas
2. **Singleton**: Solo una instancia activa
3. **Automático**: Se inicializa automáticamente al cargar
4. **Configuración**: Recibe datos desde `LevelData` via eventos

## Estados del Juego

- **gameStarted**: `false` hasta que se recoge la primera moneda
- **isGameWon**: `true` si se alcanza el objetivo de monedas
- **Time.timeScale**: `0f` pausado, `1f` corriendo

## Integración

```csharp
// Pausar juego
GameEvents.onPauseGame?.Invoke();

// Agregar moneda
GameEvents.onAddCoin?.Invoke();

// Consultar victoria
GameEvents.onGetIsGameWon?.Invoke(callback);
```
