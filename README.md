# Match3

My own implementation of a match three puzzle game from Udemy course: https://www.udemy.com/course/make-a-puzzle-match-game-in-unity/

I was following the course step by step, but implemented features in my way and added some new functionality.

## Key Features

### Game State Machine
State machine is used to define core application states (Bootstrap, LoadProgress, MainMenu, GameLoop). It allows you to switch states from any place when needed. 
Also states have Enter() and Exit() methods where you can control the lifecycle of all services in that state.
[GameStateMachine.cs](../master/Assets/Scripts/Infrastructure/StateMachine/GameStateMachine.cs)

### Application Entry Point
Separate Bootstrap state, Bootstrap scene with Bootstrapper GameObject and script. All that combined define the single entry point in application. That approach helps you to control services initialization and to avoid problems related to Unity Script Execution Order.
[BootstrapState.cs](../master/Assets/Scripts/Infrastructure/StateMachine/BootstrapState.cs)
[Bootstrapper.cs](../master/Assets/Scripts/Infrastructure/Bootstrapper.cs)

### Composition Root
Defined places where all services initialization is happening. That approach is useful for dependencies control and clear overview of functionality.
One place is GlobalServices for global services that active during application lifetime.
Second place is GameLoopState where all game loop related services are initializing.
[GlobalServices.cs](../master/Assets/Scripts/Infrastructure/GlobalServices.cs)
[GameLoopState.cs](../master/Assets/Scripts/Infrastructure/StateMachine/GameLoopState.cs)

### Dependency Injection
All services and entities gets their dependencies in constructor or Init() method (for MonoBehaviour objects).
Services are passed through interfaces, that allows to change concrete implementations and use DI container with easy refactoring.
