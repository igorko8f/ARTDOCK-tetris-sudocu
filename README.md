# Tetris Sudoku Puzzle

A test assignment for a **Unity Developer** position.

A small puzzle game combining mechanics from **Tetris** and **Sudoku / Block Puzzle**. The player receives a set of three figures and places them on the game board. Once all three figures have been used, a new set is generated. Whenever a complete row or column is formed, it is cleared and the player receives points.

## Technologies

* **Unity 6000.3.16f1**
* **C#**
* **Zenject** — Dependency Injection
* **R3** — Reactive programming
* **DOTween** — Animations and transitions
* **Unity New Input System** — Keyboard input
* **System.IO** — Saving and loading player data

## Configuration

The main project settings are located in:

```text
Resources/
└── Configuration/
```

The configuration contains ScriptableObjects responsible for game board settings, scoring, sounds and available figures.

### Creating a New Figure

To create a new figure, create a `FigureConfiguration` inside:

```text
Resources/
└── Configuration/
    └── Figures/
        └── FigureConfiguration
```

You can create `FigureConfiguration` by pressing `Right Mouse Button -> Create -> Gameplay -> Figure -> Empty`

The figure matrix can then be configured directly in the Unity Inspector.

The maximum supported figure size is **5x5**.

The available figures are determined by the configurations located in the `Figures` folder.

## Implemented Features

All requirements from the assignment have been implemented:

* Configurable game board size through ScriptableObject.
* Configurable score awarded for each cleared cell.
* Saving and displaying the **Top 3** scores. Could be found on lose UI window.
* Responsive UI for different screen resolutions and aspect ratios.
* ScriptableObject-based figure configurations.
* Inspector tool for creating figures up to **5x5**.
* Figure rotation before placement.
* `R` hotkey for rotating the current figure.
* `ESC` hotkey for pausing the game.
* Clearing completed rows and columns.
* Figure placement validation.
* Sound effects and background music.
* Background music volume reduction while the game is paused.
* Gameplay animations using DOTween.
* Player data saving and loading using `System.IO`.
* Unity New Input System for keyboard input.
