# MCLPP Codeneame Mineclone Plus Plus

## What it is
Mineclone Plus Plus (MCLPP) is an engine designed to run the MineClone2 Minetest Game.
After having developed for the Mineclone 2 team, and realizing that the Minetest developers
were more than complacent to leave 5-10 year old issues unanswered, even after most of the 
developers had decided that it was a great idea, and had given their consent on the PR's 
that fixed said issues, that they would never merge the PRs. Instead, we watched them as a 
whole engage in blaming their engine's game developers "for shoddy, dumpster fire code" 
instead of making real and much needed inprovements to their engine.

So, I decided to step back from the main Mineclone 2 Development team, to work on this 
project. To give life to an engine that would help the game that I (and many others) love. 
And so, with a lot of prodding, and some pestering, from my good friend JoMW I decided to 
build this from just fanciful thoughts into a real, honest engine.

Initial versions of this project were created In Unity/C#. But, with the recent events, 
and Unity being a dumpster fire (as well as closed source, proprietary software) I have
moved the project over to the Godot engine (4.1.1 stable-mono-offical). Mineclone 2 is 
open source, this engine's code is open source, and Godot is open source. So, it all came
together as a perfect fit.

## How to use
Currently, you will need Godot 4.1.1 C# (latest C# / Mono version of Godot) to build the 
project. We will make binary versions when the project gets closer to completion. Until 
then, download the source, and open it up as a project in Godot. it'll take a minute to 
load, as it's importing all of the assets. When it's done, you can hit the Play button in
the upper righthand corner.

You can also build a fully executable version of the game for your OS by clicking 
`Project` -> `Export...` and select the predefined export package. You will need to download
the project targets from the pop up, but they are small (~120 mb) to make it. Additionally,
you will need DotNet installed, so that it can use MS Build to compile the code into a fully 
working game. 