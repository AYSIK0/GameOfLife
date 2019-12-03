# GameOfLife
## General Information

This a Game of life variation variation that model involves creating a simple 2D console-based predator-prey simulation between greenflies (prey) and ladybirds (predator). These insects live in a world composed of a grid of cells. The grid is enclosed so an insect is not allowed to move off the edges of the world (nor incidentally, can either insect fly). Only one insect can occupy a cell at a time. Each insect performs some action everytime step.

**Greenflies behave according to the following rules:**

1. Move:   
Every time step, randomly try to move up, down, left or right. If the neighbouring cell in the selected direction is occupied or would move the greenfly off the grid, then the greenfly stays in the current cell.

2. Breed:  
If a greenfly survives for three time steps, then at the end of the time step (that is, after moving) the greenfly will breed. This is simulated by creating a new greenfly in an adjacent (up, down, left or right) cell that is empty. If there are no  empty cells available, then no breeding occurs.Once an offspring is produced a greenfly cannot produce an offspring until three more time steps have elapsed.

**Ldybirds behave according to the following rules:**

1. Move:   
Every time step, if there is an adjacent greenfly (up, down, left or right), then the ladybird will move to that cell and eat the greenfly. Otherwise, the ladybird moves according to the same rules as the greenfly. Note that a ladybird cannot eat  other ladybirds.

2. Breed:  
If a ladybird survives for eight time steps, then at the end of the time step it will spawn off another ladybird in the same manner as the greenfly.

3. Starve:  
If a ladybird has not eaten a greenfly within the last three time steps, then at the end of the third time step it will  starve and die. The ladybird should then be removed from the grid of cells.

*N.B All Ladybirds must move before Greenflies each turn.*

**Settings:**

There are 2 settings that can be changed during the simulation:

1. *Simulation Speed*: It decides how many turns will pass after the user click enter. Simulation must be larger than 0.
2. *Display Mode*: Can be 0 or 1, if it's **0** then every grid will be drawn. if it's **1** only the final grid will be drawn.

## How to play the Game

When you start you will be asked to choose between 2 start options:

* Default Mode (0): This mode will create a 20x20 grid, 100 greenflies and 5 ladybirds, also it will set the simulation speed to 1 and display mode to 0.

* Custom Mode (1): In this mode you can choose the size of the grid, number of greenflies, ladybirds, the simulation speed and the display mode.

*N.B in custom mode you need to make sure that number of cells is equal or larger than the number of creatures (Greenflies + Ladybirds)*

* Press enter to go to the next step (turn).

* During the simulation you can press 'S' to change simulation speed and display mode.

* Press 'N' to start a new world.

* To exit the game press Escape key (esc).

* When you finish a game a Two files will be created:
1. Information.txt: It contains some inforamtion about the simulation.
2. Table.txt      : A table will be created that has the number of Greenflies and Ladybirds for each turn.      

*N.B Only one pair of files (information, table) is created for each game. when you start a new world the information about the previous and current world will be in the same file. (each world has it's own ID)*
