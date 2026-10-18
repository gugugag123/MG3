# MG3

## Devlog

### 1. Default Values for Member Variables

I added four public member variables—`float`, `int`, `bool`, and `string`—to the `Timer` class without assigning any initial values. Unity’s serialization check reveals the following: `float` and `int` default to 0, `bool` defaults to `false`, and `string` appears as blank.

There is a subtle distinction here: in C#, the default value for a `string` field is actually `null`. The fact that it appears blank in the Inspector does not mean its default value in C# is an empty string.

### 2. How `Update()` Works

We do not need to call `Update()` ourselves; Unity invokes it automatically every frame, provided that the component is enabled and the `GameObject` to which it is attached is active.

In the `Timer` class, `Update()` subtracts `Time.deltaTime`—the time elapsed between two consecutive frames—during every frame. This ensures the countdown tracks actual elapsed time in seconds, remaining consistent regardless of the computer's frame rate. Once the time reaches 0, it stops decreasing (it does not continue into negative values).

### 3. How the Game Uses the Timer

When the game begins, the `Start()` method in `TimerText` calls `BeginTimer()`, initiating a 30-second countdown. In every subsequent frame, `Timer.Update()` handles the time decrement, while `TimerText.Update()` reads and displays the remaining time. Simultaneously, `IsFinished()` checks whether the time has run out. Once time expires, player controls are disabled, and a "Game Over" screen is displayed.

The kicking mechanic also utilizes its own timer. Whenever a kick is initiated, `KickWeapon` starts a 0.2-second timer, which is then monitored frame-by-frame via `Update()`. Once the time elapses, the kick object is deactivated, preventing the player from remaining in an "attacking" state indefinitely after a single kick. Since `GetKeyDown` is used for the spacebar, holding down the key will not cause the timer to reset repeatedly.

I have also assigned different point values to various food items: 1 point for a bagel or a bottle, 2 points for sushi or chicken, and 3 points for a steak. When an item is struck, it passes its point value to the player’s scoring method, which then updates the score displayed on the screen.

## Controls
- Left/right arrows or A/D: move
- Up arrow or W: jump
- Space: kick once per press
- Refresh the web page to restart

## Open-Source assets
- [Cat fighter spritesheet](https://opengameart.org/content/cat-fighter-sprite-sheet)
- [Food sprites](https://rcpstd.itch.io/pixel-food-pack)
