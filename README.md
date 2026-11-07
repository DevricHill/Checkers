# Checkers (C# / Windows Forms)

<img width="640" height="451" alt="checkers-gameplay" src="https://github.com/user-attachments/assets/ae100d36-54b9-4918-9293-ef3afb980b69" />

A two-player desktop checkers game built in C# and .NET 10 with Windows Forms.

## Why I built it
I built it to learn Windows Forms by making a complete, playable game: drawing the board in code, handling player clicks, and keeping the display in sync with the game state.

## Features
- Two-player game on an 8×8 board with a random first player
- Move validation for steps and captures
- King promotion; kings move forward and backward
- Win detection when a player loses all pieces
- A blocked player's turn is skipped; the game is a draw if neither player can move
- In-game rules screen

## Tech stack
C# · .NET 10 · Windows Forms · object-oriented design · delegates · Dictionary collections

## Design
- Game rules live in `Model/Checkers.cs`, separate from the forms.
- One `IsValidMove` method handles both players by using each piece's forward direction; kings check both directions.
- One `CanMove` method replaces two near-identical per-color methods (about 250 duplicated lines removed).

## How to run
1. Clone the repo and open `MiniProject - Checkers.slnx` in Visual Studio 2026 on Windows.
2. Press F5.

## What I'd add next
- Multi-jump captures
- Unit tests for the rules engine (xUnit)
- A single-player mode against a simple AI

**Download:** [V1.0.0 (Windows)](https://github.com/DevricHill/checkers-winforms/releases/latest)
