# Seyren Framework Overview

## System Architecture Flowchart

```mermaid
%%{init: {'theme': 'forest'}}%%
flowchart TD
    %% Main components
    A[Start] --> B[Initialize]
    B --> C[Main Process]
    C --> D{Decision Point}
    C --> Visualization[Visualization]
    
    %% Branches
    D -->|Condition 1| E[Universe]
    E --> U[Ability Manager]
    S -.->|Executes on tick| U
    
    %% Define independent Ability node
    AbilityBase((Ability))
    
    %% Define independent AbilityInstance node
    InstanceBase((AbilityInstance))
    
    %% Define independent Unit node
    UnitBase((Unit))
    
    %% Define independent Damage Engine node
    DamageEngine((Damage Engine))
    
    %% Define independent Force node
    ForceNode((Force))
    
    %% Define independent Buff node
    BuffNode((Buff))
    
    %% Define independent Resource Manager node
    ResourceManager((Resource Manager))
    
    %% Resource types
    ResourceManager --> AttributePoint[Attribute Point]
    ResourceManager --> SkillPoint[Skill Point]
    ResourceManager --> Money[Money]
    
    %% Define independent Payment Processor node
    PaymentProcessor((Payment Processor))
    
    %% Connect ResourceManager and PaymentProcessor
    ResourceManager -->|Pay| PaymentProcessor
    PaymentProcessor -->|Refund| ResourceManager
    
    %% Define independent Inventory nodes
    InventoryBase((Inventory))
    StashInventoryNode((StashInventory))
    EquipmentInventoryNode((EquipmentInventory))
    
    %% Connect Unit and Inventory
    UnitBase -->|Has| InventoryBase
    
    %% Connect Unit and ResourceManager
    UnitBase -->|Has| ResourceManager
    
    %% Connect inventory types to base inventory
    InventoryBase -.->|Including| StashInventoryNode
    InventoryBase -.->|Including| EquipmentInventoryNode
    
    %% Equipment inventory contains items
    EquipmentInventoryNode -->|Equip| Weapon[Weapon]
    EquipmentInventoryNode -->|Equip| Armor[Armor]
    EquipmentInventoryNode -->|Equip| Accessory[Accessory]
    
    %% Define independent Item node
    ItemNode((Item))
    
    %% Connect item types to base item
    ItemNode -.->|Inherits| Weapon
    ItemNode -.->|Inherits| Armor
    ItemNode -.->|Inherits| Accessory
    
    %% Connect Force to Unit
    ForceNode -.->|Classifies allegiance| UnitBase
    
    %% Connect Damage Engine to Unit
    DamageEngine -.->|Inflict Damage| UnitBase
    
    %% Connect Unit and Buff
    UnitBase -.->|Affected by| BuffNode
    
    %% AbilityInstance contains properties
    InstanceBase --> Target[Target]
    InstanceBase --> Level[Level]
    InstanceBase --> Caster[Caster]
    
    %% Add "Contains" comments to connections
    InstanceBase -->|Contains| Target
    InstanceBase -->|Contains| Level
    InstanceBase -->|Contains| Caster
    
    %% Ability Manager manages abilities
    U -.->|Manages| AbilityBase
    U -.->|Register| AbilityBase
    U -.->|Update on tick| AbilityBase
    
    %% Ability creates instances
    AbilityBase -.->|Creates| InstanceBase
    
    %% Ability Manager contains abilities
    U -->|Register| V1[Ability 1]
    U -->|Register| V2[Ability 2]
    U -->|Register| V3[Ability 3]
    
    %% Connect abilities to base ability
    AbilityBase -.->|Inherits| V1
    AbilityBase -.->|Inherits| V2
    AbilityBase -.->|Inherits| V3
    
    %% Each ability has instances
    V1 --> W1[Instance 1.1]
    V1 --> W2[Instance 1.2]
    V2 --> W3[Instance 2.1]
    V2 --> W4[Instance 2.2]
    V3 --> W5[Instance 3.1]
    
    %% Connect instances to base instance
    InstanceBase -.->|Inherits| W1
    InstanceBase -.->|Inherits| W2
    InstanceBase -.->|Inherits| W3
    InstanceBase -.->|Inherits| W4
    InstanceBase -.->|Inherits| W5
    
    %% Subprocess details
    E --> H[Space]
    H -->|Find units in range| T[Spatial]
    H --> UX[UnitIndexer]
    UX -.->|Manages| UnitBase
    E --> I[Time]
    I --> S[Update Loop]
    
    %% Final state
    %% No results section, removed Result 1 and its connectors
    
    %% Styling
    classDef process fill:#f9f,stroke:#333,stroke-width:2px
    classDef decision fill:#bbf,stroke:#333,stroke-width:2px
    classDef result fill:#bfb,stroke:#333,stroke-width:2px
    
    class A,B,C,E,F,G,H,I,J,K,L,Visualization process
    class D decision
    class M,N,O,P,Q,R result
```

## Component Relationships

```mermaid
classDiagram
    class CoreSystem {
        +Initialize()
        +Process()
        +Terminate()
    }
    
    class ComponentA {
        +propertyA
        +methodA()
    }
    
    class ComponentB {
        +propertyB
        +methodB()
    }
    
    class ComponentC {
        +propertyC
        +methodC()
    }
    
    CoreSystem --> ComponentA
    CoreSystem --> ComponentB
    ComponentA --> ComponentC
    ComponentB --> ComponentC
```

## Process Sequence

```mermaid
sequenceDiagram
    participant User
    participant System
    participant Component
    participant Database
    
    User->>System: Action
    System->>Component: Process
    Component->>Database: Query
    Database-->>Component: Response
    Component-->>System: Result
    System-->>User: Feedback
```

## State Transitions

```mermaid
stateDiagram-v2
    [*] --> State1
    State1 --> State2: Transition
    State1 --> State3: Alternative
    State2 --> State4: Process
    State3 --> State4: Process
    State4 --> [*]: Complete
```
stateDiagram-v2
    [*] --> State1
    State1 --> State2: Transition
    State1 --> State3: Alternative
    State2 --> State4: Process
    State3 --> State4: Process
    State4 --> [*]: Complete
```
    State1 --> State3: Alternative
    State2 --> State4: Process
    State3 --> State4: Process
    State4 --> [*]: Complete
```
    State2 --> State4: Process
    State3 --> State4: Process
    State4 --> [*]: Complete
```
