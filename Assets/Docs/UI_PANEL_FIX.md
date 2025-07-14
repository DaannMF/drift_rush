# Fix: Gestión Automática de Paneles de UI

## 🚨 **Problema Solucionado**

**Antes**: GamePanel (timer + monedas) aparecía en MainMenu  
**Ahora**: MainPanel aparece en MainMenu, GamePanel solo en niveles

## ✅ **Lógica Implementada**

### En MainMenu Scene

- ✅ **MainPanel**: Activo (fondo/decoración)
- ❌ **GamePanel**: Inactivo (no se necesita timer/monedas)
- ✅ **PausePanel**: Activo (botones Play, Exit)
- ❌ **EndGamePanel**: Inactivo

### En Level Scenes (Level1, Level2)

- ❌ **MainPanel**: Inactivo
- ✅ **GamePanel**: Activo (timer, contador de monedas)
- ❌ **PausePanel**: Inactivo (se activa con ESC)
- ❌ **EndGamePanel**: Inactivo (se activa al terminar)

## 🔧 **Cambios Realizados**

### 1. **GameManager.cs**

```csharp
// Nuevos métodos agregados:
- SetupMainMenuUI()  // Configura UI para MainMenu
- SetupGameUI()      // Configura UI para niveles
- ConfigureUIForCurrentScene()  // Detecta escena y configura UI
- ForceUIUpdate()    // Fuerza actualización de UI
```

### 2. **Detección Automática**

- El sistema detecta automáticamente si está en MainMenu o en un nivel
- Usa `LevelManager.Instance.IsInMainMenu` para determinar la escena
- Configura los paneles apropiados automáticamente

### 3. **Integración con LevelManager**

- `ConfigureUIForLevel()` llama a `SetupGameUI()`
- `ConfigureUIForMainMenu()` llama a `SetupMainMenuUI()`
- Fuerza actualización después de cargar escenas

## 🎮 **Experiencia del Usuario**

| Acción | UI Activa |
|--------|-----------|
| **Abrir MainMenu** | ✅ MainPanel + PausePanel (Play, Exit buttons) |
| **Cargar Level1** | ✅ GamePanel (Timer: 01:00, Coins: 0/10) |
| **Presionar ESC** | ✅ PausePanel (sobre GamePanel) |
| **Completar nivel** | ✅ EndGamePanel (Victoria/Derrota) |
| **Volver a MainMenu** | ✅ MainPanel (botones principales) |

## 🚀 **Configuración Requerida**

### En Unity Inspector (GameManager)

1. **Main Panel**: Asignar GameObject del menú principal
2. **Game Panel**: Asignar GameObject con timer/monedas  
3. **Pause Panel**: Asignar GameObject del menú de pausa
4. **End Game Panel**: Asignar GameObject de victoria/derrota

### Verificación

- Los 4 GameObjects deben estar asignados en GameManager
- Todos deben ser hijos del Canvas persistente
- El sistema configurará automáticamente cuál debe estar activo

## 🔍 **Debug en LevelSystemExample**

El script de ejemplo ahora muestra:

```
=== UI PANELS ===
Expected: MainPanel ✅, PausePanel ✅  (en MainMenu)
Expected: MainPanel ❌, GamePanel ✅   (en niveles)
```

## ✅ **Resultado Final**

- ✅ **MainMenu**: MainPanel + PausePanel (botones Play, Exit)
- ✅ **Niveles**: Solo muestra UI de juego (Timer, Monedas)
- ✅ **Transiciones automáticas**: Sin configuración manual
- ✅ **Persistencia**: UI se mantiene correcta entre escenas
- ✅ **Robustez**: Funciona aunque se cambie de escena manualmente

**El problema está completamente solucionado y la UI se comporta correctamente en todas las escenas.**
