# Seyren Game Framework

Seyren is a game development framework for Unity that provides a robust foundation for building complex games with features like ability systems, state management, damage calculations, spatial partitioning, and more.

## Features

### Core Systems

- **Universe System**: A flexible universe simulation system with time and space management
  - IUniverse interface for managing game world state
  - Modular space and time components
  - Event-based state synchronization

- **Ability System**: Comprehensive ability and skill framework
  - Support for different ability types (Active, Channel, etc.)
  - Cooldown management
  - Targeting system (No Target, Unit Target, Point Target)
  - Ability status tracking and events

- **Damage System**: Sophisticated damage calculation engine
  - Pre-hit modifiers
  - Damage type system
  - Defense and resistance calculations
  - Hit effect triggers
  - Event-based damage flow

### Game Features

- **Force System**: Team/faction management system
  - Alliance and enemy relationships
  - Force-based unit interactions
  - Editor support for force configuration

- **Quest System**: Flexible quest framework
  - Quest conditions and progress tracking
  - Quest dependencies
  - Event-based quest completion

- **State System**: Unit state management
  - HP/MP/Shield tracking
  - State change events
  - Modifier system

### Utility Systems

- **Spatial Systems**:
  - Octree implementation for efficient spatial queries
  - Fog of war system with visibility calculations
  - Grid-based map system with cell properties

- **UI Framework**:
  - Loading screen system
  - HUD management
  - Inventory system
  - Listview implementation

### Technical Features

- **Action System**: Action management framework
  - Action queueing and priorities
  - Action constraints
  - Event-based action flow

- **Serialization**: Custom serialization support
  - SerializableDictionary implementation
  - Unity-compatible data structures

## Installation

1. Open your Unity project
2. Add the package through the Package Manager using the git URL:
   ```
   https://github.com/yourusername/seyren.git
   ```
   Or copy the package directly into your Assets folder

## Usage

### Basic Setup

1. Import the necessary namespaces:
```csharp
using Seyren.Universe;
using Seyren.System.Units;
using Seyren.System.Abilities;
// ... other namespaces as needed
```

2. Initialize the core systems:
```csharp
// Initialize universe
IUniverse universe = new Universe();

// Setup forces
Force playerForce = Force.CreateForce("PlayerTeam");
Force enemyForce = Force.CreateForce("EnemyTeam");

// Create units
IUnit playerUnit = universe.SpawnUnit("player", position, rotation);
```

### Creating Abilities

```csharp
public class FireballAbility : ActiveAbility 
{
    public FireballAbility(int level) : base(level) 
    {
        Targeting = TargetingType.UnitTarget;
        Cooldown = 5f;
        ManaCost = 100;
    }

    protected override void DoCastAbility() 
    {
        // Implement ability logic
    }
}
```

### Setting Up Quests

```csharp
var killCondition = new QuestCondition(5); // Kill 5 enemies
var quest = new Quest("Hunt", "Kill 5 enemies", killCondition);
quest.Completed += OnQuestCompleted;
```

## Architecture

The framework follows a modular, event-driven architecture with clear separation of concerns:

- **Universe Layer**: Top-level game world management
- **Systems Layer**: Core gameplay systems (Abilities, Forces, Damage, etc.)
- **Unit Layer**: Entity management and state
- **Infrastructure Layer**: Utilities and technical services

## Contributing

1. Fork the repository
2. Create a feature branch
3. Submit a pull request

## License

[Your License Here]

## Support

[Contact Information/Support Details]