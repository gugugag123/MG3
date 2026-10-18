# MG3

## Devlog

### 1. Default member-variable values
The Timer class includes uninitialized public fields named defaultFloat, defaultInt, defaultBool, and defaultString for the Inspector experiment. The default values exposed through Unity serialization are 0 for float, 0 for int, an unchecked box for bool (false), and a blank text field for string. In C#, an uninitialized string field defaults to null; Unity's serialized Inspector displays a blank field and may serialize it as an empty string. A blank Inspector field therefore does not prove that the C# default is an empty string. Editor checks of these serialized fields returned 0, 0, false, and a blank string field.

### 2. How Update is called
Unity calls Update automatically once per rendered frame on enabled MonoBehaviour components whose GameObjects are active. My code does not call Update manually. Timer.Update subtracts Time.deltaTime, the elapsed time since the previous frame, so the countdown measures seconds instead of counting frames. The remaining time is clamped at zero.

### 3. How the timer is used
TimerText.Start runs once and calls _gameTimer.BeginTimer(_maxGameTime), starting the 30-second game countdown. Each frame, Timer.Update reduces the remaining time, while TimerText.Update reads GetTimeLeft and displays it with one decimal place. TimerText.Update also calls IsFinished. When it returns true, the player is disabled and the GameOver text is shown.

KickWeapon.ActivateKick starts its own Timer with _lifetimeSeconds (0.2 seconds in the scene). KickWeapon.Update checks IsFinished every frame and deactivates the kick object when time expires. Space uses GetKeyDown so holding the key does not restart the kick timer every frame. Each kicked item passes its configured value to Player.IncreasePoints, which adds that value and refreshes the Points display. Bagels and wine bottles give 1 point, sushi and chicken give 2 points, and steak gives 3 points. The value is serialized on each Kickable component; all eight scene items have been configured.

## Controls
- Left/right arrows or A/D: move
- Up arrow or W: jump
- Space: kick once per press
- Refresh the web page to restart

## Open-Source assets
- [Cat fighter spritesheet](https://opengameart.org/content/cat-fighter-sprite-sheet)
- [Food sprites](https://rcpstd.itch.io/pixel-food-pack)
