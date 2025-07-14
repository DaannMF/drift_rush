# Documentación Técnica - Drift Rush

Documentación técnica de los sistemas principales del juego.

## 🎮 Managers del Sistema

### [GameManager](./GameManager.md)

Maneja la lógica principal del juego: tiempo, monedas, victoria/derrota y estados de pausa.

### [LevelManager](./LevelManager.md)

Controla la carga de escenas, transiciones entre niveles y posicionamiento del jugador.

### [AudioManager](./AudioManager.md)

Gestiona música de fondo, efectos de sonido y configuración de volumen.

### [InputManager](./InputManager.md)

Maneja entrada del usuario con Unity Input System y la convierte en eventos.

### [CanvasManager](./CanvasManager.md)

Controla la visibilidad y transiciones de todos los paneles de UI.

## 🚗 Controladores de Gameplay

### [ArcadeCarController](./ArcadeCarController.md)

Controlador de físicas arcade del auto con sistema de drift y recuperación automática.

## 📋 Documentos del Proyecto

### [Parcial 1](./parcial_1.md)

Consigna del primer parcial del proyecto.

### [Parcial 2](./parcial_2.md)

Consigna del primer parcial del proyecto.

### [Game Design Document](./GDD%20Drift%20Rush.pdf)

Documento de diseño completo del juego.

---

## 🏗️ Arquitectura General

El proyecto utiliza un **sistema de eventos desacoplado** donde:

- **Managers**: Sistemas persistentes que manejan lógica global
- **Controllers**: Componentes que manejan gameplay específico
- **Events**: Sistema de comunicación sin referencias directas
- **UI**: Interfaz gestionada centralmente

## 🎯 Patrones Utilizados

- **Singleton**: Para managers persistentes
- **Observer**: Para sistema de eventos
- **Event Bus**: Para comunicación desacoplada
- **Scriptable Objects**: Para configuración de datos

## 🔧 Configuración del Proyecto

1. **Managers**: Agregar prefabs de managers persistentes
2. **Input System**: Configurar PlayerInput.inputactions
3. **Audio**: Colocar clips en Assets/Resources/Art/Audio/
4. **Levels**: Crear LevelData assets en Assets/Resources/Data/
5. **UI**: Asignar paneles en CanvasManager

---

**Mantener esta documentación actualizada con cada cambio significativo del sistema.**
