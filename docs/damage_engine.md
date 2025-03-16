```mermaid
flowchart TD
    A[Damage Source] --> B[Create Damage Event]
    B --> C[Apply Pre-Hit Modifiers]
    C --> G[Apply Damage Resistance]
    G --> CD[Calculate Final Damage]
    CD --> L{Damage > 0 and !Immune}
    L -->|No| M[No Damage]
    L -->|Yes| N[Apply Damage to Target]
    N --> N1{Can trigger On-Hit Effects}
    N1 -->|Yes| R[Trigger On-Hit Effects]
    M --> N1
    R --> S[End Damage Process]
    N1 -->|No| S

    style A fill:#ff9999
    style CD fill:#99ff99
    style N fill:#9999ff
```