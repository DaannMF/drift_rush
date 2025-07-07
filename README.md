# Drift Rush

Juego de carreras arcade donde debes conseguir un número de monedas y llegar a la meta antes de que se acabe el tiempo.

**Características principales:**

- Sistema de control arcade optimizado para drift
- Nuevo Unity Input System con eventos
- Soporte completo para múltiples dispositivos (keyboard, gamepad)
- Físicas arcade sin WheelColliders para máxima diversión

Para la materia Práctica Profesional juego en Unity - Image Campus

## 🎮 Sistema de Control Moderno

DriftRush utiliza el **nuevo Unity Input System** con un sistema de eventos para máximo rendimiento y flexibilidad:

### Controles

- **Teclado**: W/S (acelerar/frenar), A/D (girar), Espacio (freno de mano)
- **Gamepad**: Stick izquierdo (movimiento), botones (freno de mano)
- **Auto-detección** de dispositivos conectados
- **Sensibilidad y deadzone** configurables

### Arquitectura

- **InputManager**: Maneja todos los inputs con eventos estáticos
- **ArcadeCarController**: Sistema arcade optimizado para drift
- **CarSettings**: Configuración centralizada con ScriptableObject

## 📚 Documentación

- **[ArcadeWheelCarController](./Assets/Docs/ArcadeWheelCarController.md)** - **🏆 RECOMENDADO** Sistema híbrido: WheelColliders + Control Arcade
- **[ArcadeCarController](./Assets/Docs/ArcadeCarController.md)** - Sistema arcade puro sin WheelColliders  
- **[CarController Guide](./Assets/Docs/CarControllerGuide.md)** - Sistema legacy realista

### Documentos del Proyecto

- [Consigna TP 01](./Assets/Docs/parcial_1.md)
- [Game Design Document](./Assets/Docs/GDD%20Drift%20Rush.pdf)

## PENDIENTES

Agregar dos niveles mas
Checkpoints
Obstáculos tipo conos
Sonidos
UI con velocímetro
Obstáculo que restartee al player o lo ralentice
Cheat code (que complete el siguiente checkpoint o invencibilidad)
