# Drift Rush

Juego de carreras arcade donde debes recolectar monedas y completar cada nivel antes de que se acabe el tiempo.

## 🎮 Sobre el Juego

**Drift Rush** es un juego de carreras arcade con físicas de drift donde el objetivo es recolectar todas las monedas necesarias en cada nivel dentro del tiempo límite. El juego cuenta con múltiples niveles, sistema de pausa, música dinámica y efectos de sonido inmersivos.

### Características Principales

- 🏎️ **Sistema de drift arcade** optimizado para diversión
- 🎵 **Música dinámica** que cambia entre menú y niveles
- 🎮 **Controles intuitivos** con teclado y gamepad
- ⏱️ **Sistema de tiempo límite** con contador regresivo
- 🪙 **Recolección de monedas** como objetivo principal
- 🎯 **Múltiples niveles** con diferentes desafíos

## 🎮 Controles

### Teclado

- **W/S**: Acelerar/Frenar
- **A/D**: Girar izquierda/derecha
- **Espacio**: Freno de mano (drift)
- **Escape**: Pausar/Reanudar (solo en niveles)
- **R**: Resetear auto

### Gamepad

- **Stick izquierdo**: Movimiento
- **Gatillos**: Acelerar/Frenar
- **Botones**: Freno de mano
- **Botón Start**: Pausar/Reanudar

## 🏗️ Arquitectura del Sistema

El juego utiliza un sistema de eventos desacoplado con managers persistentes:

### Managers Principales

- **[GameManager](./Assets/Docs/GameManager.md)** - Lógica del juego, tiempo y monedas
- **[LevelManager](./Assets/Docs/LevelManager.md)** - Carga de escenas y niveles
- **[AudioManager](./Assets/Docs/AudioManager.md)** - Música y efectos de sonido
- **[InputManager](./Assets/Docs/InputManager.md)** - Entrada del usuario
- **[CanvasManager](./Assets/Docs/CanvasManager.md)** - Manejo de UI

### Componentes Principales

- **[ArcadeCarController](./Assets/Docs/ArcadeCarController.md)** - Controlador del auto con drift

📚 **[Ver Documentación Técnica Completa](./Assets/Docs/README.md)**

## 🚀 Cómo Jugar

1. **Menú Principal**: Selecciona "Level" para elegir un nivel
2. **Durante el Juego**: Recolecta todas las monedas antes de que se acabe el tiempo
3. **Pausa**: Presiona Escape para pausar/reanudar
4. **Victoria**: Completa el objetivo para avanzar al siguiente nivel
5. **Derrota**: Si se acaba el tiempo, puedes reintentar

## 🎵 Sistema de Audio

- **Música de fondo**: Cambia automáticamente entre menú y juego
- **Efectos de auto**: Aceleración, frenado, drift, idle
- **Efectos de UI**: Hover, click, menú
- **Efectos de juego**: Monedas, victoria, derrota

## 📁 Estructura del Proyecto

```bash
Assets/
├── Scripts/
│   ├── Managers/          # Sistemas principales
│   ├── Controllers/       # Controladores de gameplay
│   ├── Events/           # Sistema de eventos
│   └── UI/               # Interfaz de usuario
├── Prefabs/              # Prefabs reutilizables
├── Scenes/               # Escenas del juego
└── Docs/                 # Documentación técnica
```

---

**Para la materia Práctica Profesional - Unity Development**  
**Image Campus**
Cheat code (que complete el siguiente checkpoint o invencibilidad)
