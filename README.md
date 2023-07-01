# Match3

My own implementation of a match three puzzle game from Udemy course: https://www.udemy.com/course/make-a-puzzle-match-game-in-unity/

I was following the course step by step, but implemented features in my way and add some new functionality.

## Key Features

### Game State Machine
State machine is used to define core application states (Bootstrap, LoadProgress, MainMenu, GameLoop). It allows you to switch states from any place when needed. 
Also states have Enter and Exit methods where you can control the lifecycle of all services in that state.
[GameStateMachine.cs](../master/Assets/Scripts/Infrastructure/StateMachine/GameStateMachine.cs)

### Application Entry Point
Separate [Bootstrap state](../master/Assets/Scripts/Infrastructure/StateMachine/BootstrapState.cs), Bootstrap scene with Bootstrapper GameObject and [script](../master/Assets/Scripts/Infrastructure/Bootstrapper.cs). All that combined define the single entry point in application. That approach helps you to control all services initialization and avoid problems related to Unity Script Execution Order.

### Composition Root
Defined places where all services initialization is placed. That approach is useful for dependencies control.
One place is [GlobalServices.cs](../master/Assets/Scripts/Infrastructure/GlobalServices.cs) for global services that active and used application lifecycle
Second place is [GameLoopState.cs](../master/Assets/Scripts/Infrastructure/StateMachine/GameLoopState.cs) where all game loop related services are initializing.

### Dependency Injection
