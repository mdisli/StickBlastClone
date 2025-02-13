# StickBlast

StickBlast is an engaging puzzle game developed as a case study project for Garawell Games. In this game, players connect dots to create squares and collect diamonds, combining strategic thinking with quick reflexes to create an addictive gameplay experience.

## 🎮 Game Features

- **Grid-Based Gameplay**: Players interact with a dynamic grid system where they can connect dots to form lines and squares
- **Diamond Collection**: Collect diamonds that appear on the board to progress through levels
- **Level Progression**: Multiple levels with increasing difficulty and unique challenges
- **Score System**: Track your progress with an intuitive scoring system
- **Modern UI**: Clean and responsive user interface with smooth animations
- **Level End Screen**: Detailed end-of-level feedback and progression tracking

## 🛠 Technical Features

### Core Components

- **Board System**
  - Dynamic grid generation
  - Edge and line management
  - Square completion detection
  - Diamond spawning system

- **Level Management**
  - Scriptable Object-based level design
  - Progressive difficulty scaling
  - Player progress persistence

- **UI System**
  - Real-time score updates
  - Level progress tracking
  - Diamond collection animations
  - End-level screen

### Architecture

The game is built using a modular architecture with the following key components:

- **Controllers**
  - `BoardController`: Manages the game board, edges, and gameplay mechanics
  - `UIController`: Handles all UI elements and animations
  - `LevelController`: Controls level progression and game state
  - `DiamondController`: Manages diamond spawning and collection

- **Event System**
  - Uses Scriptable Objects for event management
  - Decoupled communication between game systems
  - Efficient state management

## 🎯 Gameplay Mechanics

1. **Board Generation**
   - Dynamic grid system with customizable dimensions
   - Automatic edge and line generation

2. **Square Completion**
   - Connect edges to form squares
   - Automatic detection of completed shapes
   - Special effects and animations on completion

3. **Diamond Collection**
   - Random diamond spawning
   - Collection animations
   - Score tracking

## 🔧 Development Setup

1. Open the project in Unity (compatible with Unity 2020.3 or later)
2. Navigate to the `_Workspace` folder for core game scripts
3. Use the Scene view to test and modify levels
4. Modify `LevelSO` Scriptable Objects to create new levels

## 📁 Project Structure

```
Assets/
├── _Workspace/
│   ├── Scripts/
│   │   ├── Board Scripts/
│   │   ├── Diamond/
│   │   ├── Level Scripts/
│   │   ├── Line & Edge Scripts/
│   │   ├── Managers/
│   │   ├── Shape Scripts/
│   │   ├── SO Scripts/
│   │   └── UI Scripts/
│   ├── Prefabs/
│   ├── Scenes/
│   └── ScriptableObjects/
```

## 🎨 Visual Effects

The game includes various visual effects using the Epic Toon FX package:
- Explosion effects
- Particle systems for diamond collection
- Square completion animations

## 📝 Dependencies

- DOTween Pro: For smooth animations and transitions
- TextMeshPro: For high-quality text rendering
- NaughtyAttributes: For enhanced Unity inspector functionality
- UniTask: For efficient asynchronous operations
- Epic Toon FX: For particle effects and visual feedback 