+---------------------------------------------------+
|                  Unity Scene                      |
|                                                   |
|  GameObject: UI                                   |
|   └── Component: UIDocument                       |
|         └── Source Asset: DiabloTheme.uxml        |
|                                                   |
|  GameObject: GameUIController                     |
|   └── Script: GameUIController.cs                 |
|         └── Access UIDocument.rootVisualElement   |
|               └── Get <DiabloThemeView>           |
|                                                   |
+---------------------------------------------------+

               ▼
+----------------------------------+
|          DiabloTheme.uxml        |
|   - Declares <DiabloThemeView>   |
|   - References USS stylesheet    |
+----------------------------------+
               │
               ▼
+----------------------------------+
|      DiabloThemeView.cs          |
|   - Inherits VisualElement       |
|   - Creates InventoryView        |
|   - Creates EquipmentView        |
|   - Register events (e.g., I key)|
+----------------------------------+
               │
               ▼
+----------------------------------+
|          USS Styles              |
|   - diablo-theme {}              |
|   - inventory {}                 |
|   - equipment {}                 |
|   - .hidden { display: none; }   |
+----------------------------------+
               │
               ▼
+----------------------------------+
|         Display on Screen        |
|   - UI Toolkit Renderer          |
+----------------------------------+
