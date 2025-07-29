# Payment System Documentation

The Seyren payment system provides a flexible framework for handling in-game resource transactions. This system allows for any kind of resource payment such as gold, skill points, attribute points, and more.

## Component Relationships

```mermaid
classDiagram
    class IPaymentProcessor {
        <<interface>>
        +ProcessPayment(ICost) bool
        +CanProcessPayment(ICost) bool
        +RefundPayment(ICost) void
    }
    
    class ICost {
        <<interface>>
        +CanSatisfy(IResourceManager) bool
        +Apply(IResourceManager) bool
        +Refund(IResourceManager) void
        +GetDescription() string
    }
    
    class IResourceManager {
        <<interface>>
        +GetResourceAmount(string) int
        +HasResource(string, int) bool
        +AddResource(string, int) void
        +RemoveResource(string, int) bool
        +OnResourceChanged event
    }
    
    class ICostProvider {
        <<interface>>
        +GetCost() ICost
    }
    
    class StandardPaymentProcessor {
        -IResourceManager resourceManager
        +StandardPaymentProcessor(IResourceManager)
        +ProcessPayment(ICost) bool
        +CanProcessPayment(ICost) bool
        +RefundPayment(ICost) void
    }
    
    class DefaultResourceManager {
        -Dictionary~string,int~ resources
        +GetResourceAmount(string) int
        +HasResource(string, int) bool
        +AddResource(string, int) void
        +RemoveResource(string, int) bool
        +OnResourceChanged event
    }
    
    class SimpleCost {
        -string resourceId
        -int amount
        +SimpleCost(string, int)
        +CanSatisfy(IResourceManager) bool
        +Apply(IResourceManager) bool
        +Refund(IResourceManager) void
        +GetDescription() string
    }
    
    class CompositeCost {
        -List~SimpleCost~ costs
        +CompositeCost()
        +CompositeCost(SimpleCost[])
        +AddCost(SimpleCost) void
        +CanSatisfy(IResourceManager) bool
        +Apply(IResourceManager) bool
        +Refund(IResourceManager) void
        +GetDescription() string
    }
    
    class PaymentUtility {
        <<static>>
        +TryPurchase(ICostProvider, IPaymentProcessor) bool
        +CanPurchase(ICostProvider, IPaymentProcessor) bool
    }
    
    class PurchasableItem {
        <<example>>
        -ICost cost
        +GetCost() ICost
    }
    
    IPaymentProcessor <|.. StandardPaymentProcessor
    IResourceManager <|.. DefaultResourceManager
    ICost <|.. SimpleCost
    ICost <|.. CompositeCost
    CompositeCost o-- SimpleCost : contains
    StandardPaymentProcessor --> IResourceManager : uses
    ICostProvider <|.. PurchasableItem
    PaymentUtility --> ICostProvider : uses
    PaymentUtility --> IPaymentProcessor : uses
    IPaymentProcessor --> ICost : processes
    ICost --> IResourceManager : manipulates
    PurchasableItem --> ICost : has
```

## Flow Diagram

```mermaid
sequenceDiagram
    participant Client
    participant PaymentUtility
    participant CostProvider as Purchasable Item
    participant PaymentProcessor
    participant Cost
    participant ResourceManager
    
    Client->>PaymentUtility: TryPurchase(item, processor)
    PaymentUtility->>CostProvider: GetCost()
    CostProvider-->>PaymentUtility: ICost
    PaymentUtility->>PaymentProcessor: ProcessPayment(cost)
    PaymentProcessor->>Cost: CanSatisfy(resourceManager)
    Cost->>ResourceManager: HasResource(resourceId, amount)
    ResourceManager-->>Cost: true/false
    Cost-->>PaymentProcessor: true/false
    
    alt Cost can be satisfied
        PaymentProcessor->>Cost: Apply(resourceManager)
        Cost->>ResourceManager: RemoveResource(resourceId, amount)
        ResourceManager-->>Cost: true
        ResourceManager->>ResourceManager: Trigger OnResourceChanged
        Cost-->>PaymentProcessor: true
        PaymentProcessor-->>PaymentUtility: true
        PaymentUtility-->>Client: true
    else Cost cannot be satisfied
        PaymentProcessor-->>PaymentUtility: false
        PaymentUtility-->>Client: false
    end
```

## Resource Flow

```mermaid
flowchart LR
    Player[Player] --> Resources[Resource Pool]
    Resources --> Actions[Game Actions]
    
    subgraph Payment System
        ICostProvider --> ICost
        ICost --> IResourceManager
        IPaymentProcessor --> ICost
    end
    
    Actions --> ICostProvider
    Resources --- IResourceManager
    
    style Payment System fill:#f9f,stroke:#333,stroke-width:2px
```

## Common Usage Patterns

### Simple Resource Payment

```mermaid
sequenceDiagram
    participant Game
    participant Skill as Skill (ICostProvider)
    participant Payment as PaymentProcessor
    participant Resources as ResourceManager
    
    Game->>Resources: Check if enough skill points
    Resources-->>Game: Has sufficient points
    Game->>Payment: ProcessPayment(skillPointCost)
    Payment->>Resources: Remove skill points
    Resources-->>Payment: Success
    Payment-->>Game: Payment successful
    Game->>Skill: Unlock skill
```

### Composite Resource Cost

```mermaid
sequenceDiagram
    participant Game
    participant CraftingRecipe as Recipe (ICostProvider)
    participant Payment as PaymentProcessor
    participant CompCost as CompositeCost
    participant Resources as ResourceManager
    
    Game->>CraftingRecipe: GetCost()
    CraftingRecipe-->>Game: CompositeCost
    Game->>Payment: CanProcessPayment(compositeCost)
    Payment->>CompCost: CanSatisfy(resourceManager)
    CompCost->>Resources: Check for multiple resources
    Resources-->>CompCost: All resources available
    CompCost-->>Payment: Can satisfy
    Payment-->>Game: Can process payment
    Game->>Payment: ProcessPayment(compositeCost)
    Payment->>CompCost: Apply(resourceManager)
    CompCost->>Resources: Remove multiple resources
    Resources-->>CompCost: Success
    CompCost-->>Payment: Success
    Payment-->>Game: Payment successful
    Game->>Game: Craft item
```

## Implementation Notes

- The payment system is designed to be flexible and extensible, allowing for any type of in-game resource to be managed.
- Resources are identified by string IDs, making the system easily adaptable to different game requirements.
- The separation of concerns between costs, providers, and processors ensures good software architecture.
- The system supports both simple payments (one resource) and composite payments (multiple resources).
- Refund functionality is available for cases where operations need to be reversed.