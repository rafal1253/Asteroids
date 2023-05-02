<h1>Asteroids</h1>
Asteroids for BeardedBrothers.games by Rafał Makos.

Made in Unity 2021.3.17f1.
The game must be enabled via scene MainMenu. 

<h2>Game configuration:</h2>
To adjust the number of enemies to spawn per level: Assets -> ScriptableObjects -> LevelManager
(method chosen due to the notation: "The game should have multiple levels that the level designers (**not necessarily with a technical background**) can easily create/modify.")

To change...
- player movement properties: In hierarchy - Player -> PlayerShip.cs -> MOVEMENT
- player launcher properties: In hierarchy - Player -> Launcher -> Launcher.cs

- enemy ship random spawn rate: In hierarchy - EnemySpawner -> EnemySpawner.cs -> ENEMY SHIP SPAWNING

- the player's initial number of lives - In hierarchy - GameManager -> GameManager.cs -> Start Player Lifes
- the time between finishing a level and starting a new one - In hierarchy - GameManager -> GameManager.cs -> Next Level Delay

- enemies properties: Assets -> Prefabs -> Enemy -> one of them (Direction value: A larger value means more randomness in the direction of the initial movement. If 0, the object will move towards the center.)

