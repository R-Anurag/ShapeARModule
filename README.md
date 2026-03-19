# ShapeAR Module

An Android AR application built with Unity and ARFoundation that lets users place animated 3D shapes into the real world.

---

## Features

### Shapes
- Cube
- Sphere
- Cylinder
- Pyramid *(bonus)*

### Behaviours
- **Spin** — rotates continuously around X, Y, or Z axis with adjustable speed
- **Move** — oscillates in 4 directions (horizontal, vertical, left diagonal, right diagonal)
- **Bounce** — bounces up and down with adjustable height and speed *(bonus)*
- **Scale** — pulses size in 3 modes: pulse, grow only, shrink only *(bonus)*

---

## User Flow

```
ShapeSelectionScene → BehaviorSelectScene → ARPlayScene
```

1. **Select a shape** — pick from 4 shapes using toggle buttons
2. **Select a behaviour** — pick an animation to apply to the shape
3. **Spawn in AR** — tap on a detected real-world surface to place the shape
4. **Control the animation** — adjust parameters live using the on-screen panel

---

## Implementation

### Architecture

| Script | Responsibility |
|---|---|
| `ShapeModuleCache` | Static cache passing shape/behaviour selection between scenes |
| `ShapeModuleData` | Data model holding `shapeName` and `behaviourName` |
| `ShapeSelector` | Handles shape toggle UI and navigates to behaviour selection |
| `BehaviourSelector` | Handles behaviour button UI and navigates to AR scene |
| `ARShapeSpawner` | AR plane detection, shape instantiation, anchor creation, behaviour attachment |
| `ARPlayUIController` | Wires animation control UI to the active behaviour component post-spawn |
| `SpinBehaviour` | Rotates shape around a chosen axis |
| `MoveBehaviour` | Oscillates shape along a chosen direction using sine wave |
| `BounceBehaviour` | Bounces shape vertically using Abs(Sin) |
| `ScaleBehaviour` | Pulses shape scale between min/max values |

### Key Technical Decisions

- **Behaviours attached at runtime** via `AddComponent` rather than baked into prefabs — keeps prefabs clean and decoupled from animation logic
- **AR Anchor** created programmatically at the hit pose so the shape stays world-locked as the device moves
- **Distance-based auto-scaling** — shape scale is clamped based on distance from camera so it appears consistent regardless of where the user taps
- **Static cache** for inter-scene data — simple and sufficient for this scope, avoids DontDestroyOnLoad complexity
- **Editor/simulator support** — mouse click handling via `#if UNITY_EDITOR` for testing without a physical device

---

## Tech Stack

- Unity 6 (6000.x)
- ARFoundation 6.1.1
- ARCore 6.1.1
- Universal Render Pipeline (URP) 17.1.0
- Unity Input System 1.14.0
- Target Platform: Android

---

## Project Structure

```
Assets/
├── Models/
│   ├── Materials/       # All .mat material files
│   ├── Textures/        # All texture files
│   ├── ShapeCube.obj
│   └── ShapePyramid.obj
├── Prefabs/
│   ├── ShapeCube.prefab
│   ├── ShapeSphere.prefab
│   ├── ShapeCylinder.prefab
│   └── ShapePyramid.prefab
├── Scenes/
│   ├── ShapeSelectionScene.unity
│   ├── BehaviorSelectScene.unity
│   └── ARPlayScene.unity
└── Scripts/
    ├── AR/              # ARShapeSpawner, ARPlayUIController, SelectableButton
    ├── Behaviours/      # SpinBehaviour, MoveBehaviour, BounceBehaviour, ScaleBehaviour
    ├── Data/            # ShapeModuleCache, ShapeModuleData
    └── Selection/       # ShapeSelector, BehaviourSelector, ToggleSpriteSwap
```

---

## Setup & Running

### Requirements
- Unity 6 with Android Build Support
- Android device with ARCore support, or Unity AR Simulation for editor testing

### Steps
1. Clone the repository
2. Open the project in Unity 6
3. Open `Assets/Scenes/ShapeSelectionScene.unity`
4. For device: build and deploy to an ARCore-supported Android device
5. For editor: use the XR Simulation environment via the XR menu

---

## Submission

- Screen capture video: *(link here)*
- GitHub: *(link here)*
