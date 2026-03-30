# ShapeAR Module

An Android AR application built with Unity and ARFoundation. Users select a 3D shape, assign a behaviour, then spawn and interact with it on a real-world surface.

Submitted for the Catrobat mARine AR module task.

- Screen capture video: [view recording](https://drive.google.com/drive/folders/1EYX4FLdd2XXmoKjumJ8K5DF8mGh6c2aJ?usp=sharing)

**Beyond the minimum requirements:**
- Distance-based auto-scaling — shape size clamped relative to camera distance at spawn
- Live parameter control panel — adjust behaviour speed, axis, and height post-spawn
- Android back navigation — hardware/gesture back correctly navigates between scenes
- Unity Test Runner suites (21 tests across EditMode and PlayMode) — included to fulfil the TDD requirement in the proposal

---

## Features

- **4 shapes** — Cube, Sphere, Cylinder, Pyramid
- **4 behaviours** — Spin, Move, Bounce, Scale
- **AR spawn** — tap a detected plane to place the shape, world-locked via AR anchor
- **Live parameter control** — adjust behaviour parameters from an on-screen panel after spawn
- **Distance-based auto-scaling** — shape size is clamped relative to camera distance
- **Tappable cube** — cube is UV mapped with a custom texture showing Catrobat, GSoC, and profile links; tapping it opens the Catrobat org page
- **Android back navigation** — hardware/gesture back button navigates between scenes correctly

---

## Design & Assets

| | |
|---|---|
| **UI** | All buttons and screen elements custom designed in Figma — [view designs](https://www.figma.com/design/VXGOiJzONjdMwACpVtjgQi/GSoC-Entry-Task-Assets?node-id=57-2&t=ye0otEiq02xnJrDo-1) |
| **3D Models** | Cube and Pyramid modelled in Blender; Sphere and Cylinder use Unity primitives |
| **Cube Texture** | UV mapped in Blender with a custom texture made in Figma — displays Catrobat branding, GSoC, and profile links; tapping the cube opens the Catrobat org page |

---

## User Flow

```
ShapeSelectionScene → BehaviorSelectScene → ARPlayScene
```

1. **Select a shape** — toggle between shapes; selection persists if you navigate back
2. **Select a behaviour** — tap a behaviour button to proceed to AR
3. **Spawn in AR** — tap a detected surface to place the shape
4. **Control the animation** — adjust parameters live using the on-screen panel
5. **Tap the cube** — opens an external link (cube only)

---

## Architecture

| Script | Responsibility |
|---|---|
| `ShapeModuleCache` | Static cache passing shape/behaviour selection between scenes |
| `ShapeModuleData` | Data model holding `shapeName` and `behaviourName` |
| `ShapeSelector` | Shape toggle UI, seeds cache from default-on toggle, restores state on back |
| `BehaviourSelector` | Behaviour button UI, navigates to AR scene |
| `ARShapeSpawner` | Plane detection, shape instantiation, anchor creation, behaviour attachment, tappable link raycast |
| `ARPlayUIController` | Wires animation control UI to the active behaviour component post-spawn |
| `TappableLink` | Data component on prefab — holds URL, opened when cube is tapped post-spawn |
| `SpinBehaviour` | Rotates shape around a chosen axis at adjustable speed |
| `MoveBehaviour` | Oscillates shape along a chosen direction using a sine wave |
| `BounceBehaviour` | Bounces shape vertically using `Abs(Sin)` with adjustable height and speed |
| `ScaleBehaviour` | Pulses shape scale between min/max values in three modes |

## Key Technical Decisions

- **Behaviours attached at runtime** via `AddComponent` rather than baked into prefabs — keeps prefabs clean and decoupled from animation logic
- **AR Anchor created programmatically** at the hit pose so the shape stays world-locked as the device moves
- **Static cache for inter-scene data** — simple and sufficient for this scope, avoids `DontDestroyOnLoad` complexity; cache is seeded from the default-on toggle on first load so no shape is ever unset
- **Tappable link via collider raycast** — post-spawn taps are routed through `Camera.main.ScreenPointToRay` against the shape's collider; only shapes with a `TappableLink` component respond, so other shapes are unaffected
- **Android back navigation** — predictive back disabled in Player Settings so Android delivers back as a standard key event; caught via `Keyboard.current[Key.Escape].wasPressedThisFrame` in each scene controller
- **Editor/simulator support** — mouse click handling via `#if UNITY_EDITOR` for testing without a physical device

---

## Tech Stack

- Unity 6 (6000.1.9f1)
- ARFoundation 6.1.1
- ARCore 6.1.1
- Universal Render Pipeline (URP) 17.1.0
- Unity Input System 1.14.0
- Target Platform: Android (min SDK 30)

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
    ├── AR/              # ARShapeSpawner, ARPlayUIController, TappableLink, SelectableButton
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

## Testing

The project follows a TDD workflow for the speed button limit behaviour in `ARPlayUIController`. Tests live in `Assets/Tests/` split across two assemblies.

### Test Suites

| Suite | Assembly | Runner | Tests |
|---|---|---|---|
| `ShapeModuleCacheTests` | `ShapeARModule.Tests.EditMode` | EditMode | 6 |
| `ARShapeSpawnerTests` | `ShapeARModule.Tests.PlayMode` | PlayMode | 5 |
| `SpeedButtonLimitTests` | `ShapeARModule.Tests.PlayMode` | PlayMode | 10 |

### SpeedButtonLimitTests — TDD Walkthrough

The speed +/− buttons on the AR control panel must disable at their limits (min/max) and re-enable when back in range. This behaviour was driven entirely by tests before the implementation existed.

**Red phase** — tests written first against `internal_AdjustSpinSpeed` and `internal_AdjustScaleSpeed` methods that didn't exist yet. Unity's console showed compilation errors blocking the run:

![Red phase — compilation errors](Documentation/Test%20Images/redPhase.png)

**Minimal implementation** — the two `internal_*` methods were added to `ARPlayUIController` with just enough logic to clamp speed and set `button.interactable`:

![Minimal implementation — diff in editor](Documentation/Test%20Images/minimalImplementation.png)

**Green phase** — all 10 tests pass in the Unity Test Runner (PlayMode), completing the cycle:

![Green phase — all 10 tests passing](Documentation/Test%20Images/greenPhase.png)

### What the 10 tests cover

| Test | Assertion |
|---|---|
| `SpinSpeed_AtMinimum_DecreaseButtonIsNotInteractable` | Decrease button disabled when spin speed hits 10 |
| `SpinSpeed_AtMinimum_IncreaseButtonRemainsInteractable` | Increase button stays enabled at min |
| `SpinSpeed_AtMaximum_IncreaseButtonIsNotInteractable` | Increase button disabled when spin speed hits 360 |
| `SpinSpeed_AtMaximum_DecreaseButtonRemainsInteractable` | Decrease button stays enabled at max |
| `SpinSpeed_InMiddle_BothButtonsInteractable` | Both buttons enabled when speed is mid-range |
| `ScaleSpeed_AtMinimum_DecreaseButtonIsNotInteractable` | Decrease button disabled when scale speed hits 0.5 |
| `ScaleSpeed_AtMinimum_IncreaseButtonRemainsInteractable` | Increase button stays enabled at min |
| `ScaleSpeed_AtMaximum_IncreaseButtonIsNotInteractable` | Increase button disabled when scale speed hits 10 |
| `ScaleSpeed_AtMaximum_DecreaseButtonRemainsInteractable` | Decrease button stays enabled at max |
| `ScaleSpeed_InMiddle_BothButtonsInteractable` | Both buttons enabled when speed is mid-range |

Testability is enabled via `internal_AdjustSpinSpeed` / `internal_AdjustScaleSpeed` / `internal_SetSpinBehaviour` / `internal_SetScaleBehaviour` methods on `ARPlayUIController`, exposed to the test assembly through `[assembly: InternalsVisibleTo("ShapeARModule.Tests.PlayMode")]`.
