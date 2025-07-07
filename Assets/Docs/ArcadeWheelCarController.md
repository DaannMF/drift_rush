# ArcadeWheelCarController - Sistema Híbrido

Sistema híbrido que combina **WheelColliders** (mejores físicas para terrenos) con **control arcade** y **sistema de eventos moderno**.

## 🎯 Lo Mejor de Ambos Mundos

### ✅ **WheelColliders** (Físicas Robustas)

- **Mejor adaptación** a terrenos complejos
- **Suspensión automática** y realista
- **Tracción predictible** en diferentes superficies
- **Física de ruedas** probada y optimizada

### ✅ **Control Arcade** (Diversión)

- **Sistema de eventos** moderno
- **Inputs suavizados** para mejor control
- **Configuración drift-friendly**
- **Controles instantáneos** y responsivos

### ✅ **Sistema de Eventos** (Arquitectura Moderna)

- **InputManager** con eventos estáticos
- **Desacoplamiento** total entre input y control
- **Escalable** para múltiples vehículos
- **Compatible** con el nuevo Unity Input System

## 🛠️ Setup Rápido

### 1. **Reemplazar Controller**

```
1. Quitar ArcadeCarController
2. Agregar ArcadeWheelCarController
3. El InputManager se agrega automáticamente
```

### 2. **Configurar WheelColliders**

```
1. Agregar 4 WheelColliders al auto:
   - Front Left Wheel
   - Front Right Wheel  
   - Rear Left Wheel
   - Rear Right Wheel

2. Posicionar WheelColliders:
   - En las esquinas del auto
   - Radius: 0.35f
   - Suspension Distance: 0.3f
```

### 3. **Configurar Visual Wheels (Opcional)**

```
1. Agregar Transforms de ruedas visuales
2. Asignar al ArcadeWheelCarController
3. Las ruedas se actualizan automáticamente
```

### 4. **Configurar Rigidbody**

```
Mass: 1300-1500
Drag: 0.25
Angular Drag: 6
Center of Mass: Manual (GameObject vacío)
```

## ⚙️ CarSettings Optimizados

### **Valores para WheelColliders:**

```csharp
Motor Torque: 1500f      // Mayor que arcade puro
Brake Torque: 3000f      // Para detener el auto eficientemente  
Handbrake Torque: 8000f  // Para drift potente
Max Steer Angle: 45f     // Ángulo cómodo
Traction Control: 1.2f   // Friction stiffness
Stability Control: 0.8f  // Drift-friendly
```

## 🎮 Características Arcade

### **Tracción Trasera**

- Motor solo en **ruedas traseras**
- Comportamiento drift-friendly
- Más predecible que AWD

### **Handbrake Drift**

- Reduce **friction lateral** automáticamente
- Solo frena **ruedas traseras**
- **Restaura friction** al soltar

### **Speed-Based Steering**

- Giros más cerrados a **baja velocidad**
- Estabilidad a **alta velocidad**

### **Downforce Automático**

- Se aplica a partir de 20 km/h
- Mantiene el auto **pegado al suelo**
- Evita saltos no deseados

### **Anti-Roll Bars**

- Reduce **volcado** en curvas
- Mantiene **estabilidad lateral**
- Configurable desde CarSettings

## 🔧 Configuración de WheelColliders

### **Forward Friction (Tracción):**

```
Extremum Slip: 0.4f
Extremum Value: 1f
Asymptote Slip: 0.8f
Asymptote Value: 0.5f
Stiffness: carSettings.tractionControl
```

### **Sideways Friction (Drift):**

```
Extremum Slip: 0.3f
Extremum Value: 1f
Asymptote Slip: 0.5f
Asymptote Value: 0.7f
Stiffness: carSettings.stabilityControl
```

## 🎯 Ventajas del Sistema Híbrido

### **vs Arcade Puro:**

- ✅ **Mejor adaptación** a terrenos irregulares
- ✅ **Suspensión realista** automática
- ✅ **Físicas más robustas** en rampas/saltos
- ✅ **Menos ajustes manuales** de raycast

### **vs WheelColliders Realistas:**

- ✅ **Control más arcade** y divertido
- ✅ **Drift más fácil** de controlar
- ✅ **Configuración simplificada**
- ✅ **Respuesta más instantánea**

## 🚀 Uso Recomendado

Este sistema es **perfecto** para:

- ✅ **Terrenos complejos** con rampas/saltos
- ✅ **Gameplay arcade** con drift
- ✅ **Múltiples superficies** (asfalto, tierra, césped)
- ✅ **Juegos que requieren física robusta** pero control arcade

## 🐛 Troubleshooting

### **Auto no se mueve:**

- Verificar que **CarSettings** está asignado
- **Motor Torque** debe ser ~1500f (no 22f)
- WheelColliders deben estar **tocando el suelo**

### **Auto muy lento:**

- Aumentar **Motor Torque** (1500-2500f)
- Reducir **Mass** del Rigidbody
- Verificar **Traction Control** (1.0-1.5f)

### **No drifta bien:**

- Reducir **Stability Control** (0.5-0.8f)
- Verificar que **Handbrake** funciona
- Asegurar **tracción trasera** únicamente

### **Auto inestable:**

- Bajar **Centro de Masa**
- Aumentar **Angular Drag** (6-8)
- Verificar **Anti-Roll** settings

¡Este sistema híbrido te da la robustez de WheelColliders con la diversión de controles arcade! 🏁
