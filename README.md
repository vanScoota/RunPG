# RunPG
Jump 'n' Run meets RPG!

## ACHTUNG!
### Skills
Level 1: keine
Level 2: Sprung
Level 3: Sprung, Doppelsprung
Level 4: Sprung, Doppelsprung, Kriechen
Level 5: Sprung, Doppelsprung, Kriechen
Level 6: hoher Sprung, Doppelsprung, Kriechen

### Sprung-Reichweite
Sprung: 1 hoch, 3 Lücken weit
Doppelsprung: 2 hoch, 6 Lücken weit
hoher Doppelsprung: 4 hoch, 7 Lücken weit

## TODOs
### Max
- Tileset und Backgrounds für Level 3 und 4 erstellen.
- Level 3 und 4 erstellen.
- Spielanleitung korrekturlesen.

### Markus, Niklas, Shelly
- Monster inkl. Physik bugfrei implementieren (z. B. in Level 5 bei X=90 oder in Level 6 bei X=20).
- Keine Monster in den Schlussteil eines Levels (bei der Fahne).
- XML-Dokumentation in eure Skripte einfügen (vgl. PlayerController).
- Menü – insbesondere die Wahl der Level – vervollständigen.
- Evtl. eine Logik, dass Level erst freigeschalten werden müssen.

## Changelog
All notable changes to this project will be documented in this file.

### [0.1.0] - 2018-11-23
#### Added
- Basic grass tileset, palette and map.
- Basic player sprite and idle animation.
- Player object, including PlayerController, which controls horizontal movement, jumps, double jumps and high jumps.

### [0.1.1] - 2018-12-20
#### Added
- Created jump and run sprites.
- Added run animation.

### [0.1.2] - 2019-01-09
#### Added
- Sprites for ducking and crouching.
- Ducking animation.

### [0.1.3] - 2019-01-11
#### Added
- Crouching animation.
- Simple jump animation.
#### Changed
- Crouching sprites.

### [0.1.4] - 2019-01-13
#### Added
- Dying sprites.
- Dying animation triggered by pressing button "D".

### [0.1.5] - 2019-01-14
#### Added
- Public properties to activate/deactivate skills.
- When crouching collider changes so the head doesn't disappear within the wall.

### [0.1.6] - 2019-01-15
#### Added
- Script that let's main camera follow the player.
- Boundaries for the camera so it stops when level is over.
- Improved sample scene level.

### [0.1.7] - 2019-01-17
#### Added
- Camera script again.
- Changed crouching sprites.

### [0.1.8] - 2019-01-22
#### Added
- Put Respawn and checkpoints into PlayerController.
- Camera Controller.

### [0.1.9] - 2019-01-23
#### Added
- Controls with WASD keys.
#### Changed
- Improved rouching and ducking.

## [0.2.0] - 2019-02-07
#### Added
- New tilesets for Grassland and Lavahell.
- Finish flags which load the next scene on collision.
- Prefabs for all cross scene objects.
- Backgrounds that move with the camera.
- Levels 1, 2, 5 and 6 are finished.

#### Fixed
- Crouching doesn't glitch anymore.