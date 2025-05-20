# Tecnicatura Superior en Programación de videojuegos con motores - Res- 2022/1157

Práctica Profesional: Proyecto de Juego con Unity
Año 2025

Parcial 1: Entrega de Prototipo

## Objetivo

El objetivo del parcial consiste en que los alumnos entreguen un prototipo avanzado del
juego que van a desarrollar durante el cuatrimestre.
Este prototipo debe cumplir con ciertas características específicas dependiendo del juego
elegido. Estos requisitos se analizarán caso por caso con cada alumno.
En caso de no querer realizar un juego propio, se brindarán dos bases de juegos
predefinidos para desarrollar.

## Consigna Principal

Todos los puntos deben estar completos (a menos que un reemplazo haya sido confirmado
por el profesor).

Requisitos Generales

- ✅ El juego debe ser desarrollado en Unity 3D (2022.3.15f1).
- ✅ Las mecánicas deben contemplar completamente el uso de las 3 dimensiones.
Juegos en 2D no serán aceptados a menos que el profesor lo apruebe, y se
compensará su menor dificultad con otros requisitos.
Entrega
- Enlace a repositorio público de GitHub, con la build subida como release y el tag: Primer Parcial.
- Enlace a una página de Itch.io donde se pueda jugar o descargar el juego.
- El juego debe poder ejecutarse desde el editor y desde la build.

### Requisitos para Nota 0 a 4

- ✅ El juego debe tener un personaje principal (o una variante del tropo).
  - ✅ No se aceptan juegos de control de recursos sin aprobación previa del
    profesor.
- ✅ El personaje debe contar con un modelo (No primitivas de Unity).
- ✅ El personaje debe contar con animaciones o sfx de acuerdo al input.
- ✅ El juego contiene las características mecanicas necesarias para un prototipo.
- El juego debe contar con un inicio y un final.
  - No se aceptan juegos que se cierren automáticamente o que no permitan salir
del nivel.
- ✅ Debe tener una UI y un menú de pausa.
Estructura de Escenas
El prototipo debe contener al menos 3 escenas separadas:

1. Menú Principal
2. Pantalla de Créditos o Tutorial
3. Mundo del Juego

#### Transiciones obligatorias

- ✅ Desde el Menú Principal se debe poder acceder a:
  - La pantalla de Créditos o Tutorial.
  - El Mundo del Juego.
- ✅  Desde el Mundo del Juego se debe poder volver al Menú Principal.
- Todas las pantallas deben incluir un botón para cerrar el juego (no cuenta ALT +
F4).

### Requisitos para Nota 4 a 7

- Mecánica principal implementada correctamente, sin errores ni excepciones.
- Documento de diseño que explique las mecánicas principales.
- Todas las variables de balance deben ser editables mediante Scriptable Objects.
- El script que maneje al player no debe tener conocimiento de la UI ni del Input
Manager.

### Requisitos para Nota 7 a 10

- El juego no debe producir ningún tipo de error.
- Implementación de una función de guardado entre partidas.
- El juego debe ser completamente jugable con teclado y mouse o gamepad (Xbox,
PlayStation, Nintendo o genérico), incluyendo UI y gameplay.
- El juego debe tener Assets integrados en:
  - UI
  - Gameplay
  - Escenarios
