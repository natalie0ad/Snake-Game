# Snake Game
This game was developed for CS 3500: Software Practice 1
- This game uses client/server architecture.
- In this assignment, we implemented the client side of the game.

## About the game
- The goal of the game is to control the snake by consuming powerups to increase its length by a fixed amount.
- The snake dies if it hits one of the walls present in the game's world, if it hits another snake present in the game's world, or if it hits itself.
- After dying, the snake respawns after a certain amount of time to a random spot in the world.

## Developers
- Rishabh Saini
- Natalie DeSimone

## Team
Delegate_Duo

## How to play
This is a client/server game, so the first step of the game is to connect to the server.
- To connect to the server, enter the server address as "localhost" and connect on port 11000. These fields will be auto-filled in.
- Entering a username is a required field. The username cannot be empty and cannot exceed 16 characters.

To move around
- Press 'w' to move up
- Press 'a' to move left
- Press 's' to move down
- Press 'd' to move right

To consume a powerup
- Move the snake in the direction of the powerup and have the snake's head make contact with the powerup

## Additional features
- We added two modes in the game, namely Forest Mode and Space Mode
  - In Forest Mode, the world's graphics try to replicate a forest. The background of the playable space is made to look like grass, the non-playable space is made     to look like a forest, and the powerups are apples. The snake's head is the image of a cartoon snake.
  - In Space Mode, the world's graphics try to replicate outer space. The background of the playable space is made to look like the interior of a spaceship, the        non-playable space is made to look like the universe, and the powerups are stars. The snake's head is the image of an alien's head.
- When the snake dies, we added an explosion animation.
- When an error occurs such as a failure to connect, the user is notified through a pop-up message
- We added a counter displaying the number of players present in the game.

## Design Decisions to fix problems
- We had a hard time figuring out a solution to center the perspective of the game on the user's game. The main reason for this was that we were trying to figure out how to know what snake is the user's snake when translating the canvas to a snake head. We eventually realized that this gap would be bridged by using the player's unique ID, which corresponds to the snake's unique ID.
- Drawing the explosion was also tough to figure out. We got the explosion to render for the user's snake without too much effort, but for it to show up for every snake in the game was a challenge. The reason why it was hard was because we were initially keeping track of what frame of the explosion to draw through global variables, so that allowed us to draw the explosion for only one snake. Our first thought to get around this was to add the global variables into the object representing the snake. However, the problem with this was that when deserializing the snake objects from the server, we were creating a new snake for every snake received from the server and changing the pre-existing snakes in the dictionary to the newly created snakes. This was updating the instance variables of the Snake object every single time, so the state of the global variables made for the rendering of the explosion was not saved, which made the frame draw only the first frame of the explosion when the snake was dying. Hence to fix the issue, we made a Dictionary in the class responsible for the game's GUI, and in the Dictionary, we mapped an integer (the snake's unique ID) to a Tuple of two types(int to keep track of what frame of the explosion to draw, bool for whether the explosion was drawn or not). This helped save the state of the explosion for every snake present in the game and thus solved the problem.
- We were also having problems with the state of the game after connecting. When connected, the user sends their name to the server, to which the server responds to the client with the user's unique ID, the size of the world, and the Walls of the world encoded as JSON objects. But after connecting, there were times when the user was able to enter the connection process again, and this caused errors because once the client is connected, the game continually sends JSON objects, but since the user is trying to connect again, the controller tries to look for a unique ID, and it ends up trying to parse a JSON object string as an integer, causing an exception. We fixed this by simply disabling the user input fields once the client connects, and re-enabling them only after the client disconnects.
- We handle the client disconnecting by simply stopping the drawing of the game and restarting to an "empty" canvas (the empty canvas is the game's non-playable background).


