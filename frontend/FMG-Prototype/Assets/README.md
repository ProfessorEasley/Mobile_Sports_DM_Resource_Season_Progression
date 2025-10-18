# FMG Prototype Documentation

## Project Overview
The FMG Prototype is an American Football mobile game designed to provide an engaging user experience through a well-structured UI and backend logic. This documentation outlines the project's structure, key components, and instructions for rebuilding the screens.

## Project Structure
- **Assets/**: Contains all game assets including scenes, scripts, settings, and UI components.
  - **Scenes/**: Houses the main game scenes.
    - `SampleScene.unity`: The primary scene where UI elements and game objects are arranged.
  - **Scripts/**: Contains all the scripts for backend logic and UI management.
    - **Backend/**: Scripts related to backend functionalities.
      - `FacilityDetailsHandler.cs`: Manages facility details, including data retrieval and updates.
    - **UI/**: Scripts for managing UI interactions and data.
      - `ProgressionUIController.cs`: Controls the UI flow for the progression system.
      - `SeasonManager.cs`: Manages season-related data and logic.
      - `TeamData.cs`: Contains data structures for team-related information.
  - **Settings/**: Contains settings for TextMesh Pro.
    - **TextMesh Pro/**: Settings related to text rendering in the UI.
  - **UI Toolkit/**: Intended for UI toolkit components (currently empty).
  - **UISample/**: Contains UI assets including fonts and prefabs.
    - **Fonts/**: Font assets used in the UI.
    - **Prefabs/**: Contains various UI prefabs.
      - **Composites/**: Composite UI prefabs (currently empty).
      - **Primitives/**: Primitive UI prefabs (currently empty).
      - **Screens_Templates/**: Template prefabs for various screens.
        - `ConfirmationModal.prefab`: Modal for confirming actions.
        - `FacilitiesOverview_Screen.prefab`: Prefab for the facilities overview screen.
        - `SimulationPreviewScreen Variant.prefab`: Prefab for the simulation preview screen.
      - **Widgets/**: Widget prefabs (currently empty).
        - `BaseCanvas.prefab`: Base canvas for the UI, providing a screen space overlay and themed background.

## Rebuilding Screens
To rebuild the screens for the FMG Prototype, follow these steps:
1. Drag `BaseCanvas.prefab` from `Assets/UISample/Prefabs/Widgets` into the scene.
2. Under the Canvas, add the following screen prefabs from `Assets/UISample/Prefabs/Screens_Templates`:
   - `FacilitiesOverview_Screen.prefab` (for the Season Overview Hub)
   - `SimulationPreviewScreen Variant.prefab` (for the Simulation Feedback screen)
3. Create additional prefabs for the Bracket Standings & Weekly Results and XP Reward & Milestone Tracker screens, if not already available.
4. Ensure that all screens are properly linked and that the UI flow matches the described game flow navigation map.
5. Play the scene to test the functionality and ensure that the UI elements are displayed correctly with the appropriate fonts, bars, cards, and buttons.

## Conclusion
This README provides a comprehensive overview of the FMG Prototype project, its structure, and instructions for rebuilding the UI screens. For further development, ensure to maintain the organization of assets and scripts for efficient collaboration and management.