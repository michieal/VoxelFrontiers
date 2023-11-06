# MCLPP Codeneame Mineclone Plus Plus
==========================

<!-- TOC -->
* [# MCLPP Codeneame Mineclone Plus Plus](#-mclpp-codeneame-mineclone-plus-plus)
  * [What it is](#what-it-is)
  * [How to use](#how-to-use)
  * [Contributing](#contributing)
  * [License](#license)
  * [Code Of Conduct](#code-of-conduct)
<!-- TOC -->

What it is
-------
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
moved the project over to the Godot engine 4.1.1 stable-mono-official (.NET Version). 
Mineclone 2 is open source, this engine's code is open source, and Godot is open source. 
So, it all came together as a perfect fit.

How to use
-------
Currently, you will need LuaAPI Godot 4.1.2 C# (latest .NET version of Godot) to build the 
project. We will make binary versions when the project gets closer to completion. Until 
then, download the source, and open it up as a project in Godot. It'll take a minute to 
load, as it's importing all of the assets. When it's done, you can hit the `Play` button in
the upper right-hand corner.

With the switch to using LuaAPI and using the editor with it built in, the current version of the
editor being used is this [Editor Download](https://github.com/WeaselGames/godot_luaAPI/releases/tag/v2.1-beta8).
I have been working with Weasel Games to get the DotNet side of their editor working as intended
and I have been creating examples for their LuaAPI. As it is a different editor build, it requires the use
of updated Export Templates. Please read the documentation there, as I fully explain how to install the editor,
and templates to use it. Those instructions apply here. 

You can also build a fully executable version of the game for your OS by clicking 
`Project` -> `Export...` and select the predefined export package. You will need to download
the project targets from the pop up, but they are small (~120 mb) to make it. Additionally,
you will need DotNet installed (DotNet 6) and configured, so that it can use MS Build to compile 
the code into a fully working game. There have been some issues for people installing DotNet, and 
if you're one of those affected by that, use the command `dotnet --list-sdks` to make sure that 
dotnet can see that it has the sdk(s) listed. Ideally, install both version 6 and 7.

Contributing
-------
At the current moment, we happily accept contributions, and they are needed. We also need people to test the engine on various systems. 
Please use the Issue Tracker to report the outcomes, so that if needed, we can look into it.
Please see [CONTRIBUTING.md](CONTRIBUTING.md) for more information.

License
-------
Please see [LICENSE.md](LICENSE.md) for information on the License of this software.

Code Of Conduct
-------
Please see [CODEOFCONDUCT.md](CODEOFCONDUCT.md) for more information.