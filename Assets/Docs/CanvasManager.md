# CanvasManager

Maneja la visibilidad y transiciones de todos los paneles de UI del juego.

## Funcionalidad Principal

- **Gestión de paneles**: Controla qué UI mostrar en cada momento
- **Transiciones automáticas**: Cambia UI según contexto del juego
- **Configuración centralizada**: Un punto de control para toda la UI
- **Persistencia**: Mantiene UI entre escenas

## Paneles Gestionados

```csharp
[Header("UI Panels")]
[SerializeField] private GameObject mainPanel;      // Fondo del menú
[SerializeField] private GameObject pausePanel;     // Botones principales
[SerializeField] private GameObject gamePanel;      // Timer y monedas
[SerializeField] private GameObject endGamePanel;   // Victoria/derrota
[SerializeField] private GameObject audioSettingsPanel; // Configuración audio
[SerializeField] private GameObject levelSelectionPanel; // Selección nivel
```

## Eventos que Escucha

- `UIEvents.onShowMainMenu` - Mostrar menú principal
- `UIEvents.onShowGameUI` - Mostrar UI del juego
- `UIEvents.onShowPauseMenu` - Mostrar menú de pausa
- `UIEvents.onShowEndGamePanel` - Mostrar pantalla final
- `UIEvents.onShowLevelSelectionPanel` - Mostrar grilla de niveles
- `UIEvents.onHideAllPanels` - Ocultar todos los paneles

## Configuraciones de UI

### Menú Principal

- ✅ **MainPanel**: Fondo activo
- ✅ **PausePanel**: Botones (Play, Level, Exit)
- ❌ **GamePanel**: Inactivo
- ❌ **EndGamePanel**: Inactivo

### Durante el Juego

- ❌ **MainPanel**: Inactivo
- ❌ **PausePanel**: Inactivo
- ✅ **GamePanel**: Timer y monedas
- ❌ **EndGamePanel**: Inactivo

### Pausa

- ✅ **MainPanel**: Fondo activo
- ✅ **PausePanel**: Botones (Resume, Main Menu, Exit)
- ❌ **GamePanel**: Inactivo

### Fin de Nivel

- ✅ **MainPanel**: Fondo activo
- ❌ **PausePanel**: Inactivo
- ❌ **GamePanel**: Inactivo
- ✅ **EndGamePanel**: Victoria/derrota

## Uso

1. **Persistente**: Mantiene UI entre escenas con `DontDestroyOnLoad`
2. **Singleton**: Una instancia global
3. **Automático**: Responde a eventos del sistema
4. **Contextual**: UI apropiada para cada situación

## Integración

```csharp
// Mostrar menú principal
UIEvents.onShowMainMenu?.Invoke();

// Mostrar UI del juego
UIEvents.onShowGameUI?.Invoke();

// Mostrar menú de pausa
UIEvents.onShowPauseMenu?.Invoke();
```

## Métodos Principales

- `ShowMainMenu()` - Configura UI del menú principal
- `ShowGameUI()` - Configura UI del juego
- `ShowPauseMenu()` - Configura UI de pausa
- `ShowEndGamePanel()` - Configura UI de fin de nivel
- `ShowLevelSelectionPanel()` - Configura grilla de niveles
- `HideAllPanels()` - Oculta todos los paneles

## Notas Técnicas

- **Event-driven**: Responde solo a eventos, no polling
- **Stateless**: No mantiene estado, solo reacciona
- **Centralized**: Punto único de control para UI
- **Flexible**: Fácil agregar nuevos paneles
