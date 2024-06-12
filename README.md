# Pixel Match

My first simple game that made it to release on one web platform.
You can play Pixel Match on itch.io (https://serj13.itch.io/pixel-match)

## Key Features

### Game State Machine
State machine is used to define core application states (LoadSaveData, MainMenu, GameLoop). It allows you to switch states from any place when needed. 
States have Enter() and Exit() methods where you can control transitions between states and define core logic.
[GameStateMachine.cs](../master/Assets/Scripts/Infrastructure/StateMachine/GameStateMachine.cs)

### Application Entry Point
Separate Bootstrap scene with Bootstrapper GameObject and script. All that combined define the single entry point in application. That approach helps you to control services initialization and to avoid problems related to Unity Script Execution Order.
[Bootstrapper.cs](../master/Assets/Scripts/Infrastructure/Bootstrapper.cs)

### Composition Root
Defined places where all services initialization is happening. That approach is useful for dependencies control and clear overview of functionality.
One place is ServiceLocator for global services that active during application lifetime.
Second place is EndlessGameLoopState where all game loop related services are initializing.
[ServiceLocator.cs](../master/Assets/Scripts/Infrastructure/ServiceLocator.cs)
[EndlessGameLoopState.cs](../master/Assets/Scripts/Infrastructure/StateMachine/EndlessGameLoopState.cs)

### Dependency Injection
All services and entities gets their dependencies in constructor or Init() method (for MonoBehaviour objects).
Services are passed through interfaces, that allows to change concrete implementations, apply tests and use DI container in future without lots of refactoring.
Using services by interfaces we can create separate services for debugging in editor or Windows builds and WebGL builds and inject them (as an example see usages of temporary hardcoded bool "isYaGamesEnvironment" in Bootstrapper).
[Bootstrapper.cs](../master/Assets/Scripts/Infrastructure/Bootstrapper.cs)


### JavaScript plug-in for external SDK
Custom plugin is used to call JavaScript functions and recieve callbacks via C# code.
[YaGames.jslib](../master/Assets/Plugins/YaGames.jslib)
[YaGamesMonoService.cs](../master/Assets/Scripts/Services/Mono/YaGamesMonoService.cs)
