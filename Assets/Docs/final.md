# Tecnicatura Superior en Programación de videojuegos con motores - Res- 2022/1157

Práctica Profesional: Proyecto de Juego con Unity
Año 2025
Final: Entrega de Juego

## Objetivo

El objetivo del final consiste en que los estudiantes entreguen una versión avanzada del
juego que estuvieron desarrollando durante el cuatrimestre.
Este juego debe cumplir con las características previamente acordadas con el docente,
perfeccionando la mecánica principal y puliendo todos los aspectos del videojuego.

## Consigna Principal

El final consistirá en una defensa del proyecto presentado.
Todos los puntos deben estar completos (a menos que un reemplazo haya sido confirmado
por el profesor).

Se evaluará tanto la calidad de cada punto entregado como la presentación general, y
estos estarán sujetos a criterio del docente.

## Requisitos Generales

- El juego debe ser desarrollado en Unity 3D (2022.3.15f1).
- El juego debe poder ejecutarse desde el editor y desde la build.
Entrega
- Enlace al repositorio público en GitHub, con la build subida como release y
utilizando un tag con el siguiente formato:
Final Intento (Número de intento).
Ejemplo: Final Intento 1.

- Enlace a la página de Itch.io donde se pueda jugar o descargar el juego.
La página debe tener un nivel de presentación adecuado, incluyendo:
  - Descripción del juego
  - Imágenes y/o videos de gameplay
  - Enlace en los metadatos al repositorio del proyecto
- La build debe estar entregada en un archivo .zip, compilada únicamente para
Windows, e incluir solo los archivos necesarios para su ejecución.

## Criterios de Evaluación

### Requisitos para Nota 0 a 4

No está permitido el uso de packages extras que agreguen funcionalidad o sistemas al juego
para los siguientes puntos del examen, solo Assets visibles y sonoros.

- Deben cumplirse todas las consignas requeridas para obtener un 4 en los parciales
anteriores.
- El juego debe contar con feedbacks positivos, negativos y neutrales para todas las
interacciones posibles, incluyendo la UI.
- Debe incluir efectos de postprocesado estáticos.
- Debe contar con un menú que permita cambiar la resolución y el framerate máximo.
- El juego no debe contener errores, excepciones ni bugs que impidan el progreso
(game-breaking bugs).
- El juego debe contener una boot screen con su nombre del estudiante, usuario o
empresa, luego del logo de "Made with Unity".
- El juego debe ser completamente jugable con teclado y mouse o gamepad (Xbox,
PlayStation, Nintendo o genérico), incluyendo UI.
Ambas opciones deben permitir el acceso completo a todas las funcionalidades, con un
nivel de pulido equivalente.
- Debe haber una pantalla de victoria y una de derrota, que no aparezcan de forma
abrupta, sino con una transición o retardo.
- Todos los NPCs y armas deben tener alguna animación o feedback asociado a sus
-cciones.

### Requisitos para Nota 4 a 7

Se permite el uso de paquetes externos que agreguen funcionalidad o sistemas. Además de
los requisitos anteriores, el juego debe:

- Usar Cinemachine para el sistema de cámaras.
- Incluir shaders personalizados en al menos una mecánica, objeto del mundo o UI.
- Implementar una función de guardado entre partidas.
- Utilizar un sistema centralizado de eventos para la comunicación entre elementos del
gameplay.

### Requisitos para Nota 7 a 10

Se permite el uso de paquetes externos. Además de los requisitos anteriores el juego debe
contener:

- Efectos de postprocesado reactivos o por zonas, que mejoren la experiencia de juego.
- Un skybox personalizado acorde al tono del juego.
- Uno de los siguientes sistemas:

1. Tienda de cosméticos
    - Se pueden comprar cosméticos con dinero del juego (In-Game
    Currency).
    - Los cosméticos pueden afectar visuales, gameplay o sonido.
    - El dinero y los cosméticos deben guardarse entre partidas.
2. Sistema de logros
    - Logros desbloqueables por completar niveles o realizar acciones
    desafiantes.
    - Deben mostrarse en una UI dentro del menú principal.
    - Notificaciones al momento de desbloquear logros, similar a plataformas
    como Steam/PS/Xbox.
3. Integración con Discord
    - El juego debe mostrar actividad del jugador en Discord: nivel actual,
    ícono del juego y datos relevantes.
4. Sistema de diálogos
    - Permite interactuar con NPCs mediante opciones de diálogo.
    - La cámara se enfoca en el NPC durante la conversación.
    - El NPC debe tener animaciones relacionadas al diálogo.
