# Theme Process Pipeline

The Theme system in Seyren provides a structured approach for creating and managing UI layouts across different resolutions and device types.

## Process Flow

```mermaid
flowchart TD
    A[Awake] --> B[Initialize]
    B --> C{Find Components}
    C --> D[Get UIComponents in children]
    C --> E[Find fields with UIComponentAttribute]
    D --> F[Match components by name]
    E --> F
    F --> G[Add to components list]
    G --> H[OnInitialized]
    
    H -.-> PostInit
    H --> I
    I[GetLayout]
    
    I --> J{Resolution Match?}
    J -->|Yes| K[Use matched layout]
    J -->|No| L[Use default layout]
    K --> M[Setup]
    L --> M
    M --> N[Position Components]
    N --> O[Apply scaling]
    N --> P[Set active state]

    subgraph Theme Lifecycle
    A
    B
    H
    I
    M
    end

    subgraph Component Discovery
    C
    D
    E
    F
    G
    end

    subgraph Layout Resolution
    J
    K
    L
    end

    subgraph Component Setup
    N
    O
    P
    end
    
    subgraph PostInit[Post Initialization]
        Z1[Load Layout]
        Z2[Load Theme]
    end
```

## Theme Implementation Steps

1. **Initialization**: The theme initializes by discovering all UI components in its children
2. **Layout Resolution**: The appropriate layout is selected based on current screen resolution
3. **Component Setup**: Each UI component is positioned and configured according to the layout
4. **Runtime Updates**: Components can be updated at runtime as needed

## Key Classes

- `Theme`: Abstract base class for all UI themes
- `UIResolutionConfig`: Contains layout configurations for different resolutions
- `UIResolutionLayout`: Defines component positions, scales, and states for a specific resolution
- `ComponentLayout`: Stores position, scale, and active state for a single component
- `DeviceResolutionMapper`: Maps device names to their screen resolutions
- `DeviceResolutionMapper`: Maps device names to their screen resolutions
