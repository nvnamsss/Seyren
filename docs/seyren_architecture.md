# Seyren Architecture

```mermaid
graph TD
    subgraph "Seyren Project"
        Root["Seyren Root"]
        
        subgraph "Project Files"
            Root --> Sln["Seyren.sln"]
            Root --> Csproj["Seyren.csproj"]
            Root --> EditorProj["Seyren.Editor.csproj"]
            Root --> MsgPack["MessagePack Projects"]
        end
        
        subgraph "Source Code"
            Root --> Assets["Assets"]
            
            %% Main Code Structure
            Assets --> Seyren["Seyren/"]
            Assets --> Scripts["Scripts/"]
            Assets --> Game["Game/"]
            Assets --> Labs["Labs/"]
            
            %% Unity Specific
            Assets --> Scenes["Scenes/"]
            Assets --> Prefab["Prefab/"]
            Assets --> Resources["Resources/"]
            Assets --> Plugins["Plugins/"]
            Assets --> AOSFogWar["AOSFogWar/"]
            Assets --> StarterAssets["StarterAssets/"]
            
            %% Data
            Assets --> LevelData["LevelData/"]
        end
        
        subgraph "Build & Config"
            Root --> Packages["Packages/"]
            Root --> ProjectSettings["ProjectSettings/"]
            Root --> Library["Library/"]
            Root --> Logs["Logs/"]
            Root --> MsgPackGen["MessagePackGenerated/"]
        end
        
        %% Seyren Core Architecture
        subgraph "Core Architecture"
            Seyren --> Universe["Universe System"]
            Seyren --> Damages["Damage System"]
            Seyren --> Abilities["Abilities System"]
            Seyren --> States["State System"]
            Seyren --> Actions["Actions System"]
            Seyren --> Quests["Quests System"]
        end
        
        %% Universe System Breakdown
        Universe --> Maps["Map<TCell>"]
        Universe --> FogOfWar["Fog of War"]
        Universe --> Algorithms["Spatial Algorithms"]
        
        %% Damage System
        Damages --> DamageInfo["Damage Info"]
        Damages --> Modifiers["Damage Modifiers"]
        Damages --> Critical["Critical System"]
        
        %% Abilities System
        Abilities --> AbilityBase["Ability Base"]
        Abilities --> Targeting["Targeting System"]
        Abilities --> CastTypes["Cast Types"]
        
        %% Actions System
        Actions --> IAction["IAction Interface"]
        Actions --> ActionFlow["Action Flow Events"]
        
        %% State System
        States --> Attributes["Attributes"]
        States --> Stats["Stats"]
        
        %% Fog of War details
        FogOfWar --> FogV3["FOW V3"]
        FogOfWar --> FogV8["FOW V8"]
        FogOfWar --> FogComponents["FOW Components"]
        
        FogComponents --> Fog["Fog"]
        FogComponents --> Doodad["Doodad"]
        FogComponents --> Sight["Sight"]
    end

    classDef mainNode fill:#f9f,stroke:#333,stroke-width:2px;
    classDef subsystem fill:#bbf,stroke:#33f,stroke-width:1px;
    classDef component fill:#dfd,stroke:#3a3,stroke-width:1px;
    classDef data fill:#ffd,stroke:#aa3,stroke-width:1px;
    classDef unity fill:#ddd,stroke:#333,stroke-width:1px;
    
    class Root mainNode;
    class Seyren,Universe,Damages,Abilities,States,Actions,Quests subsystem;
    class Maps,FogOfWar,Algorithms,DamageInfo,Modifiers,Critical,AbilityBase,Targeting,CastTypes,IAction,ActionFlow,Attributes,Stats component;
    class LevelData data;
    class Scenes,Prefab,Resources,Plugins,AOSFogWar,StarterAssets unity;
```

## Key Components

### Universe System
- **Map<TCell>**: Generic rectangular grid system for the game world
- **Fog of War**: Visibility system with multiple implementations (V3-V8)
- **Spatial Algorithms**: Includes QuadTree, OcTree for efficient spatial queries

### Damage System
- **Damage Info**: Core damage calculation and application
- **Modifiers**: System for modifying damage (criticals, reductions, etc.)
- **Critical System**: Handles critical hit mechanics

### Abilities System
- **Ability Base**: Foundation for character abilities
- **Targeting System**: Different targeting types (self, unit, point)
- **Cast Types**: Various casting behaviors (channel, active, instant, aura)

### Actions System
- **IAction Interface**: Contract for game actions with constraints
- **Action Flow**: Event-based flow management (start, break, end)

### State System
- **Attributes**: Character attributes with base and modified values
- **Stats**: Gameplay statistics tracking

## Design Patterns Used
- Entity Component System
- Observer Pattern
- Factory Pattern
- Pipeline Pattern
- Strategy Pattern