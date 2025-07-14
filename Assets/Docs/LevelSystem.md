# Sistema de Gestión de Niveles - Drift Rush

## Descripción General

El sistema de gestión de niveles permite cargar escenas de manera asíncrona y aditiva, inicializar el jugador en puntos específicos, y manejar la progresión entre niveles.

## Componentes Principales

### 1. LevelManager

- **Propósito**: Gestiona la carga asíncrona de escenas y la progresión de niveles
- **Características**:
  - Carga aditiva de escenas
  - Inicialización automática del jugador en SpawnPoints
  - Gestión de estados de juego
  - Singleton pattern para acceso global

### 2. LevelData (ScriptableObject)

- **Propósito**: Configuración individual de cada nivel
- **Propiedades**:
  - `levelName`: Nombre del nivel
  - `sceneName`: Nombre de la escena de Unity
  - `targetCoins`: Monedas necesarias para ganar
  - `timeLimit`: Tiempo límite en segundos
  - `description`: Descripción del nivel
  - `isUnlocked`: Si el nivel está desbloqueado
  - `requiredLevel`: Nivel requerido para desbloquear

### 3. SpawnPoint

- **Propósito**: Marca donde debe aparecer el jugador al iniciar un nivel
- **Características**:
  - Gizmos visuales en el editor
  - Posicionamiento y rotación automáticos
  - Reinicio de física del jugador

### 4. CanvasManager

- **Propósito**: Hace el Canvas persistente entre escenas
- **Características**:
  - Singleton pattern con DontDestroyOnLoad
  - Mantiene la UI activa durante todo el juego

### 5. PersistentObjectsManager

- **Propósito**: Gestiona la inicialización de objetos persistentes
- **Características**:
  - Setup automático de GameManager, LevelManager y Canvas
  - Conversión de objetos existentes a persistentes
  - Configuración por prefabs o detección automática

## Flujo de Trabajo

### 1. Configuración Inicial

1. Crear LevelData assets en `Assets/Data/`
2. Configurar el LevelManager con los datos de nivel
3. Agregar SpawnPoints en cada escena de nivel

### 2. Progresión de Niveles

1. El jugador completa objetivos (monedas + tiempo)
2. GameManager dispara eventos de victoria/derrota
3. LevelManager carga el siguiente nivel o maneja el fallo
4. UI se actualiza automáticamente

### 3. Gestión de Escenas

- **Carga Normal**: Las escenas se cargan normalmente (no aditivamente)
- **Objetos Persistentes**: GameManager, LevelManager y Canvas persisten entre escenas usando `DontDestroyOnLoad`
- **Transiciones Fluidas**: Los objetos persistentes mantienen el estado entre cambios de escena
- **Optimización**: Solo una escena está activa a la vez, pero los managers persisten

### 6. Objetos Persistentes (DontDestroyOnLoad)

#### ✅ **Objetos que Persisten Entre Escenas**

- **GameManager**: Maneja lógica de juego, monedas, tiempo - persiste automáticamente
- **LevelManager**: Gestiona transiciones entre escenas - persiste automáticamente  
- **Canvas**: UI completa (si tiene CanvasManager) - persiste opcionalmente
- **Cualquier objeto configurado con DontDestroyOnLoad**

#### 🔄 **Objetos que se Recrean en Cada Escena**

- **Player/Car**: Se posiciona en SpawnPoint de cada nivel
- **Monedas**: Específicas de cada nivel
- **Decoración del nivel**: Elementos visuales únicos de cada escena
- **SpawnPoints**: Específicos de cada nivel

### 7. Eventos del Sistema

- `onLevelCompleted`: Nivel completado exitosamente
- `onLevelFailed`: Nivel fallido (sin monedas o tiempo agotado)
- `onRestartLevel`: Reiniciar el nivel actual
- `onAllLevelsCompleted`: Todos los niveles completados
- `onLevelLoadStarted`: Comenzó la carga de nivel
- `onLevelLoadCompleted`: Terminó la carga de nivel
- `onLevelLoadProgress`: Progreso de carga (0-1)

## Integración con UI

### Gestión Automática de Paneles

El sistema configura automáticamente qué panel de UI debe estar activo según la escena:

#### En MainMenu

- ✅ **MainPanel**: Activo (fondo/decoración)
- ❌ **GamePanel**: Inactivo (no se necesita timer/monedas)
- ✅ **PausePanel**: Activo (botones Play/Exit)
- ❌ **EndGamePanel**: Inactivo

#### En Niveles (Level1, Level2)

- ❌ **MainPanel**: Inactivo
- ✅ **GamePanel**: Activo (timer, contador de monedas)
- ❌ **PausePanel**: Inactivo (se activa con ESC)
- ❌ **EndGamePanel**: Inactivo (se activa al terminar)

### EndGamePanel

- Muestra botón "Siguiente Nivel" solo si hay niveles disponibles
- Botón "Jugar Otra Vez" reinicia el nivel actual
- Botón "Menú Principal" regresa al menú principal

### MainPanel

- Botón "Jugar" carga el primer nivel desde el menú principal
- Botón "Volver" regresa al menú principal

## Uso en Código

```csharp
// Cargar un nivel específico
LevelManager.Instance.LoadLevel(1);

// Cargar el siguiente nivel
LevelManager.Instance.LoadNextLevel();

// Reiniciar el nivel actual
LevelManager.Instance.RestartCurrentLevel();

// Obtener información del nivel actual
LevelData currentLevel = LevelManager.Instance.CurrentLevel;

// Verificar estado del LevelManager
bool isInMainMenu = LevelManager.Instance.IsInMainMenu;
bool isInLevel = LevelManager.Instance.IsInLevel;
int totalLevels = LevelManager.Instance.TotalLevels;
bool isLoading = LevelManager.Instance.IsLoading;
```

## Configuración en Unity

### 1. Configurar LevelData

1. Right-click en Project → Create → Drift Rush → Level Data
2. Configurar las propiedades del nivel
3. Asignar al LevelManager

### 2. Configurar SpawnPoints

1. Crear GameObject vacío en la escena
2. Agregar componente SpawnPoint
3. Posicionar donde debe aparecer el jugador

### 3. Configurar LevelManager

1. Agregar el prefab LevelManager a la escena principal
2. Asignar los LevelData assets
3. Configurar el nombre de la escena del menú principal

### 4. Configurar Objetos Persistentes

#### Opción A: Usando PersistentObjectsManager

1. Agregar componente PersistentObjectsManager a la escena inicial
2. Asignar prefabs de GameManager, LevelManager y Canvas
3. Activar "Auto Setup On Awake"

#### Opción B: Manual

1. Agregar CanvasManager al Canvas principal
2. Verificar que GameManager y LevelManager tienen DontDestroyOnLoad en sus Awake()

## Solución de Problemas Comunes

### ❌ Problema: Los datos del juego se resetean al cambiar de escena

**Solución**: Verifica que el GameManager tenga `DontDestroyOnLoad` en su Awake(). Usa PersistentObjectsManager para asegurar configuración correcta.

### ❌ Problema: La UI desaparece al cambiar de escena

**Solución**: Agrega CanvasManager al Canvas principal para hacerlo persistente, o usa PersistentObjectsManager con un prefab de Canvas configurado.

### ❌ Problema: El jugador no aparece en el SpawnPoint

**Solución**: Verifica que el Car tenga el tag "Player" y que haya un SpawnPoint en la escena del nivel.

### ❌ Problema: No se puede cargar el siguiente nivel

**Solución**: Revisa que las escenas estén agregadas en Build Settings y que los LevelData assets tengan los nombres correctos de escena.

### ❌ Problema: GamePanel aparece en MainMenu en lugar de MainPanel

**Solución**: El sistema ahora configura automáticamente los paneles según la escena. Si persiste el problema, verifica que los GameObjects de los paneles estén correctamente asignados en el GameManager inspector.

## Notas Importantes

- El Car/Player debe tener el tag "Player"
- Las escenas deben estar agregadas en Build Settings
- **GameManager, LevelManager y Canvas** (opcional) persisten entre escenas usando `DontDestroyOnLoad`
- Los SpawnPoints son opcionales (se crea uno por defecto si no existe)
- **Las escenas se cargan normalmente** (no aditivamente) - los objetos persistentes se mantienen automáticamente
- **Use PersistentObjectsManager** para configuración automática de objetos persistentes
