# Castles

A 2D platformer tech-demo in .NET 7 using Veldrid.  
This project's aim is to create a complete game, however simple, with the following features:

* Menu system (using imGui)
	* Main menu/options/new game/continue game/editor
	* In-game menu
	* Save game menu/Load game menu
* Data files stored in JSON
* Level editor
	* Paint or erase blocks
	* Block property editor OR select block type
	* Ability to create a "campaign"
	* Add or remove actors
	* Edit actor properties
* Physics using bepu Physics 2
* Entity Component System
	* For tiles:
		* Movable, Door, Icy, bouncy, breakable, hurt, lethal
	* For actors:
		* Enemy, item, Perhaps Door and Movable depending on how we want to do it
	* World-invisible items like spawn location, light, trigger, etc.
* Saving/loading
* Player control with keyboard or controller
* Controllable camera - respect map boundaries
* Audio
	* Sound effects
	* Midi music
* Lighting system (shaders)
* Inventory display (but not management)

I decided to start this project after a previous attempt at a 3D game engine became too ambitious, and the code quality was not up to par. It didn't employ IoC or DI and wasn't structured in a robust way employing SOLID principles strongly enough.

Task List
* [x] Setup game creation using DI and IoC
* [ ] Editor New Map UI
* [ ] Editor UI
* [ ] i18n