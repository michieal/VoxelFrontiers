# Contributing to MineClonePlusPlus (MCLPP)



## Introduction
So you want to contribute to MineClone++? 
Wow, thank you! :-) 

MineClone++ is maintained by Michieal. If you have any
problems or questions, contact us on Discord/Matrix (See Links section below).

You can help with MineClone++'s development in many different ways,
whether you're a programmer or not.

## MineClone++'s development target is to...
- Create a stable, peformant, moddable, free/libre game engine based on Minecraft, 
  usable in singleplayer, and multiplayer environments, including as a game server.
- Polishing existing and adding new features is always welcome.

## Links
This section will be filled out as we go along, as this project is still in its 
infancy stage. We recognize that this project will be a massive undertaking and 
we are currently working on the infrastructure needed to handle such an undertaking.

* [Buy me a Taco!](https://ko-fi.com/apophissoftware) (Funding).

## Using git
MineClone++ is developed using the version control system
[git](https://git-scm.com/). If you want to contribute code to the
project, it is **highly recommended** that you learn the git basics.
For non-programmers and people who do not plan to contribute code to
MineClone++, git is not required. However, git is a tool that will be
referenced frequently because of its usefulness. As such, it is valuable
in learning how git works and its terminology. It can also help you
keeping your game updated, and easily test pull requests.

For Non-Command-Line people, most modern IDEs (Visual Studio, Rider, etc.) have 
Git-Version Control built in. We recommend that at the very least, you familiarize 
yourself with this, especially if you are contributing code.

The Mineclone2 Wiki has some great guides on getting started with Git, and are 
highly recommended:
https://git.minetest.land/MineClone2/MineClone2/wiki/

## I'm a programmer, how can I help?
### Game Engine.
Mineclone++ is primarily written in C# using the Godot Game Engine. We use a 
variation of the main Godot Engine that is maintained by Weasel Games. This version is
the Godot Game Engine with LuaAPI built in. You will need this version of the engine, to
build the game from the Source Code, and to make any modifications. You can download it 
from their github [Here.](https://github.com/WeaselGames/godot_luaAPI)

### Integrated Development Environment (IDE)
We recommend using JetBrain's Rider for development, as that is what we use. You can, however, 
use Visual Studio, VSCode, etc., for programming. VS and Rider both have plugins for Godot, and 
we find this to be extremely helpful.

### I don't know C Sharp (C#), but I do know Lua, can I still help?
**YES!!!** Our implementation of this engine that we are building uses Lua to help "glue" the code 
together. Your skillset will fit in nicely here, and in other areas. First off, you can help use 
construct the architecture of the Lua Glue (our name for it). Also, you can create examples or lua code, 
or write mods to use with our engine. Another, and very helpful way to contribute, is to help MineClone2
polish their product, as those changes will be reflected here. (Our game engine uses MineClone2 as its 
flagship game.) And finally, as someone with programming experience, testing out PRs will make you a hero!

## How you can help as a non-programmer
As someone who does not know how to write programs in Lua or does not know how to use 
the MCLPP API, you can still help us out a lot. For example, by opening an issue in 
the [Issue tracker](https://codeberg.org/MCLPP/MCLPP/issues), you can report a bug or 
request a feature.

### Rules about both bugs and feature requests
* Stay polite towards the developers and anyone else involved in the
  discussion.
* Choose a descriptive title (e.g. not just "crash", "bug" or "question").
* Always check the currently opened issues before creating a new one.
  Try not to report bugs that have already been reported or request features
  that already have been requested. This can often be ambiguous though.
  If in doubt open an issue!
* If you know about Mineclone2's inner workings, please think about
  whether the bug / the feature that you are reporting / requesting is
  actually an issue with Mineclone2 itself, and if it is, head to the
  [MineClone2 issue tracker](https://git.minetest.land/MineClone2/MineClone2/issues)
  instead.
* You need an account with the specific repository to actually make an Issue. For Mineclone2, we have 
  included a link to their current repository host's account creation. They use MeseHub,
  a Gitea style site. that link is: [Mesehub](https://git.minetest.land/user/sign_up).
* For Codeberg, go [here](https://codeberg.org/) and click on 'Register'. For help, 
  [Codeberg's Getting Started page](https://docs.codeberg.org/getting-started/first-steps/). 


### Reporting bugs
* A bug is an unintended behavior or, in the worst case, a crash.
  However, it is not a bug if you believe something is missing in the
  game. In this case, please read "Requesting features".
* Take a screenshot of the main game screen, as this has the Version codes for the software and 
  the Mineclone2 game being used.
* If you report a crash, always include the error message. This can be found in the 
  file `debug.txt` located in the Users directory, in the folder 
  `.Apophis Software/.mineclonepp/debug.txt`. For Windows users, this is usually `C:\Users\yourname\...`, 
  for Linux and MacOS users, it's usually `/home/yourname/...`.
* Tell us how to reproduce the problem: What you were doing to trigger
  the bug, e.g. before the crash happened or what causes the faulty
  behavior.
* When Reporting bugs to MineClone2, use the Version code following the `:`, such as `0.85.0-Snapshot` to let them
  know what version of their game that you are running. Do this only to report found errors with the gameplay code.
  If you are unsure if you should make a Bug Report to them, make a Bug Report with us, and we will tell you that it 
  is a MineClone2 issue if it is related to their code.  

### Requesting features
* Ensure the requested feature fulfills our development targets and
  goals.
* Begging or excessive attention seeking does not help us in the
  slightest, and may very well disrupt MineClone++ development. It's better
  to put that energy into helping or researching the feature in question.
  After all, we're just volunteers working on our spare time.
* Ensure the requested feature has not been implemented in MineClone++
  latest or development versions. (If in doubt, always update to the latest 
  game engine version, and use the in-game `Update Game` button to make sure 
  that you are using the latest version. 

### Testing code
If you want to help us with speeding up MineClone++ development and
making the game more stable, a great way to do that is by testing out
new features from contributors. For most new things that get into the
game, a pull request is created. A pull request is essentially a
programmer saying "Look, I modified the game, please apply my changes
to the upstream version of the game". However, every programmer makes
mistakes sometimes, some of which are hard to spot. You can help by
downloading this modified version of the game and trying it out - then
tell us if the code works as expected without any issues. Ideally, you
would report issues with pull requests similar to when you were
reporting bugs that are the mainline (See Reporting bugs section). You
can find currently open pull requests here:
<https://codeberg.org/MCLPP/MCLPP/pulls>. Note that pull
requests that start with a `WIP:` are not done yet and therefore could
still undergo substantial change. Testing these is still helpful however
because that is the reason developers put them up as WIP so other people
can have a look at the PR.

### Contributing assets
Due to license problems, this project cannot use any of Minecraft's assets,
therefore we are always looking for asset contributions. 

To contribute assets, it can be useful to learn git basics and read
the section for Programmers of this document, however this is not required.
It's also a good idea to join the Discord server (to be created.)

#### Textures
For textures we prefer original art, or [voxelgoodenough](https://git.minetest.land/MineClone2/voxelgoodenough) 
contributions. All submitted artwork must be either CC0 or CC-BY-SA 4.0 international 
for us to consider using it. The intent of this project is to use original, royaltee free artwork. 
Assets that are "ripped" from Minecraft, or that are exact reproductions (even by hand) will not be accepted. 
Our goal is not to rip off Minecraft, but to create a usable engine that makes playing Libre Minecraft clones 
easier and an overall better gameplay experience.

If you want to make such contributions, join our Discord server, or Make a Pull Request here so that we can see 
the assets that you wish to contribute.

#### Sounds
Currently, we have two sets of sound usage locations. The one that we are interested is for the Engine itself. 
If you wish to create sounds for inside of the game, and not the engine itself, please see the Mineclone2 repository 
for Contributing to them. All sounds used by this game engine must be either CC0 or CC-BY-SA 4.0 international, 
just like textures. And, like textures, they must be royalty free, and allow the ability to remix, change, or reduce 
them to fit our needs.  

Please also let us know how and where you envision your sounds to be used, and maybe work with one of our 
developers in this regard. We look forward to seeing what you contribute and bring to this game!

In game sounds are the second usage location. Unless we have specifically modded the Mineclone2 game, we will 
generally refer you to their repository and game to submit sounds directly to them.  

#### 3D Models
Most of our models have been made by our contributors, and by Apophis Software. The way that we have integrated the 
models, however, is a bit complex. It is a mixture of in-engine models, and in-game models. The in-game models are 
under the control of the Mineclone2 development team.

However, there is a lot of common ground between the two. We accept models in the .Blend (Blender) format, and in 
GLTF format. The Godot game engine uses glTF format natively, and that is the format that we create/export our 
models to. Unlike Mineclone2, we do NOT accept models that have been exported in the B3D format. The reasons for 
this is that the format is no longer supported and is, well, ancient by most standards. Nor, have we implemented any 
way to open, read, use, etc., this format, nor will we. FBX models (Filmbox, by Autodesk) need to be converted into 
glTF format, for inclusion. There is a really nice FBX2GLTF converter on the web.

##### Animations
Animations for models also fall under this category. There is always a need for cleaner animations, and animated 
models for new entities. Please bake your animations into the model for importing, and let us know the keyframe 
ranges and how that matches up with the model's various animations. 

##### Mineclone2's models:
Most of the 3D Models in MineClone2 come from
[22i's repository](https://github.com/22i/minecraft-voxel-blender-models).
Similar to the textures, we need people that can make 3D Models with
Blender on demand. Many of the models have to be patched, some new
animations have to be added etc.

#### Crediting
Asset contributions will be credited in their mods and their own respective
sections in CREDITS.md. If you have committed the results yourself, you will
also be credited in the Contributors section.

### Contributing Translations
We use our own solution for doing translations. We have created basic tooling to help 
create the required translation files, and that tool is [i18nscan](https://codeberg.org/MCLPP/i18nscan).

#### Contributing to the Game Engine Translations
To create update the existing template, please place the exectuable of i18nscan, and any necessary files 
needed to run it in the main project directory. (Be Sure to EXCLUDE these files from Git.) Run the executable 
from the command line. It will scan the source code for any strings that use our Translation code. Make sure 
to remove any duplicates from the Template that it creates. If you are a programmer and want to make this tool
better, great! Please submit a Pull Request (PR) to it's repository for us to review it!

Copy the file that i18nscan creates to the project directory `/locale/` and make sure that it is named 
`template.txt` as that is what people will be used to create translations from. Place your translations into a **copy** 
of this file, after the `=`'s sign in the file. Then name it `locale.XX.tr` where `XX` is the 2 character language code 
of the translations language. For example, the spanish translations are named `locale.es.tr`, with `es` being the code 
for spanish.

#### Contributing to missing in-game translations
Please see Mineclone2's Contributing.md in their repository for contributing translations to them. Contributing to them
is the de facto way to contribute to in-game translations.

We do not handle PRs for translating their in game code. However, for our custom modules and add ins, we do accept 
translations for those. Please familiarize yourself with the code for that, to make a translation contribution. Thank you!

#### Translation Crediting
Translation contributions will be credited in the CREDITS.md, under translations.
Feel free to add in a CREDITS.XX.TR.md file, if you want to make specific credits 
for the translations.
If you have committed the results yourself, you will also be credited in the 
Contributors section.

### Let us know your opinion
It is always encouraged to actively contribute to issue discussions on
Codeberg, let us know what you think about a topic and help us make
decisions. Also, note that a lot of discussion takes place on the
Discord server, so it's definitely worth checking it out. 

### Funding
We do accept donations. If you would like to help us out with a donation, great!!!
Please let us know what the donation should be put towards (infrastructure, 
continued development, etc.) and we will give you instructions on how to do that. 
And, let me be the first to say thank you for supporting this work!

Currently, the best way to support this work is:
[Buy me a Taco!](https://ko-fi.com/apophissoftware)

### Crediting
Funders are credited in their own section in `CREDITS.md` and will receive a special role 
"Funder" on our discord (unless they have made their donation Incognito) once that is set up.

## How you can help as a programmer
(Almost) all the MineClone++ development is done using pull requests.

### Recommended workflow
* Fork the repository (in case you have not already)
* Do your change in a new branch
* Create a pull request to get your changes merged into master
* It is important that conflicts are resolved prior to merging the pull
  request.
* We update our branches via rebasing. Please avoid merging master into
  your branch unless it's the only way you can resolve a conflict. We can
  rebase branches from the GUI if the user has not merged master into the
  branch. (If in doubt, please ask us first!!!)
* After the pull request has been merged, you can delete the branch if the
  merger hasn't done this already.

### On Tabs, Spaces, Curly Braces, etc., Naming Conventions...
Use Tabs. It may seem silly, but this is often a heated topic in most 
programming circles. Additionally, our Curly Braces style is that the Brace 
starts on the same line as, say, the function or `if` statement. Also, please do
not reformat our code to a different format before submitting a PR.

Example: `if (true) {` or `private void SomeFunction() {`

For Naming, we use Pascal Case (`SomeFunctionOrAnother()`). Please see existing 
code for examples. We follow this convention (mostly) with variable names.

#### Comments and their usage.
If you are contributing to the code base for the engine, please comment your code, 
where necessary. I say "where necessary" because not all code needs to be commented. 
But, it's always a great idea to comment what something does. Remember, you might not 
be around, or even in the same headspace, in 6 months from now, and the code has to 
be maintained. This means that commenting what something does, even if it is just an 
overview, is necessary and extremely helpful. 

#### Whitespace in PRs
When testing a Pull Request (PR), we like to test each line that is changed to make 
sure that nothing breaks. If your PR changes the whitespace on every line, you will 
be asked to fix that, before we merge the PR into the main code base. This makes 
testing code easier for everyone. Thank you.  

### Discuss first
If you feel like a problem needs to fixed or you want to make a new
feature, you could start writing the code right away and notifying us
when you're done, but it never hurts to discuss things first. If there
is no issue on the topic, open one. If there is an issue, tell us that
you'd like to take care of it, to avoid duplicate work.

### Don't hesitate to ask for help
We appreciate any contributing effort to MineClone++. If you are a
relatively new programmer, you can reach us on Discord for questions about 
git, Lua, MCLPP API, or anything related to MineClone++. We can help you 
avoid writing code that would be deemed inadequate, or help you become 
familiar with MineClone++ better, or assist your use of development tools.

### Maintain your own code, even if already got merged
Sometimes, your code may cause crashes or bugs - we try to avoid such
scenarios by testing every time before merging it, but if your merged
work causes problems, we ask you fix the issues as soon as possible.

### Changing Gameplay
Pull Requests that change gameplay have to be properly researched and
need to state their sources. These PRs also need the maintainer's approval
before they are merged.
You can use these sources:

* Testing things inside of Minecraft (Attach screenshots / video footage
  of the results)
* Looking at [Minestom](https://github.com/Minestom/Minestom) code. An open source Minecraft Server implementation
* [Official Minecraft Wiki](https://minecraft.fandom.com/wiki/Minecraft_Wiki)
  (Include a link to the specific page you used)

### Guidelines

#### Git Guidelines
* Pushing to master is disabled - don't even try it.
* Every change is tracked as a PR.
* All but the tiniest changes require at least one approval from a Developer.
* To update branches we use rebase not merge (so we don't end up with
  excessive git bureaucracy commits in master).
* We use merge to add the commits from a PR/branch to master.
* Submodules should only be used if a) upstream is highly reliable and
  b) it is 100% certain that no mclpp specific changes to the code will be
  needed (this has never been the case before, hence mclpp is submodule free so far).
* Commit messages should be descriptive.
* Try to group your submissions best as you can:
* Try to keep your PRs small: In some cases things reasonably be can't
  split up but in general multiple small PRs are better than a big one.
* Similarly multiple small commits are better than a giant one. (use git commit -p)

#### Lua Code Guidelines
* Lua: Each mod must provide `mod.conf`.
* Lua: Mod names are snake case, and newly added mods start with `mcl_`, e.g.
  `mcl_core`, `mcl_farming`, `mcl_monster_eggs`. Keep in mind Minetest
  does not support capital letters in mod names.
* To export functions, store them inside a global table named like the
  mod, e.g.

```lua
mcl_example = {}

function mcl_example.do_something()
	-- ...
end

```

* Public functions should not use self references but rather just access
  the table directly, e.g.

```lua
-- bad
function mcl_example:do_something()
end

-- good
function mcl_example.do_something()
end
```

* Use modern Minetest API, e.g. no usage of `minetest.env`
* Tabs should be used for indent, spaces for alignment, e.g.

```lua

-- use tabs for indent

for i = 1, 10 do
	if i % 3 == 0 then
		print(i)
	end
end

-- use tabs for indent and spaces to align things

some_table = {
	{"a string",                   5},
	{"a very much longer string", 10},
}
```

* Use double quotes for strings, e.g. `"asdf"` rather than `'asdf'`
* Use snake_case rather than CamelCase, e.g. `my_function` rather than
  `MyFunction`
* Don't declare functions as an assignment, e.g.

```lua
-- bad
local some_local_func = function()
	-- ...
end

my_mod.some_func = function()
	-- ...
end

-- good
local function some_local_func()
	-- ...
end

function my_mod.some_func()
	-- ...
end
```

### Developer status
Active and trusted contributors are often granted write access to the
MineClone2 repository as a contributor. Those that have demonstrated the right
technical skills and behaviours may be granted developer access. These are the
most trusted contributors who will contribute to ensure coding standards and
processes are followed.

#### Developer responsibilities
- If you have developer/contributor privileges you can just open a new branch
  in the mcl2 repository (which is preferred). From that you create a pull request.
  This way other people can review your changes and make sure they work
  before they get merged.
- If you do not (yet) have developer privs you do your work on a branch
  on your private repository e.g. using the "fork" function on mesehub.
- Any developer is welcome to review, test and approve PRs. A maintainer may prefer
  to merge the PR especially if it is in a similar area to what has been worked on
  and could result in merge conflicts for a larger older branch, or needs
  art/licencing reviewing. A PR needs at least one approval (by someone else other
  than the author).
- The maintainers are usually relatively quick to react to new submissions.

### Maintainer status
Maintainers carry the main responsibility for the project.

#### Maintainer responsibilities
- Making sure issues are addressed and pull requests are reviewed and
  merged.
- Making releases.
- Making project decisions based on community feedback.
- Granting/revoking developer access.
- Enforcing the code of conduct (See [CODEOFCONDUCT.md](CODEOFCONDUCT.md)).
- Moderating official community spaces (See Links section)
- Resolving conflicts and problems within the community. Maintainer Decisions are final. 
  You, or others, may discuss decisions with the Maintainer regarding actions, but ultimately, 
  the Maintainer(s) are the final say. 

#### Current maintainers
* Michieal - Responsible for pretty much everything.

### Licensing
By asking us to include your changes in this game, you agree that they
fall under the terms of the GPLv3, which basically means they will
become part of a free/libre software. You may also license your code under 
the MIT license, and have it included here, if it is a self contained module 
and is used as an add on. All code contributions to the main project will be GPL3. 

### Crediting
Contributors, Developers and Maintainers will be credited in
`CREDITS.md`. If you make your first time contribution, please add
yourself to this file. There are also Discord roles for Contributors,
Developers and Maintainers.