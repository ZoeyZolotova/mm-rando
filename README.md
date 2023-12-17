Majora's Mask Randomizer is a Windows application that randomizes item locations, music, and more for Majora's Mask, presenting millions of combinations and new ways to replay the game.

A video tutorial of setting up MMR is found here: https://www.youtube.com/watch?v=4woEtyg7GyM

Currently the UI is only available on windows, it is broken in wine. The CLI version works if you install dotnet and run the CLI as "dotnet MMR.CLI.dll -input <MM.z64> [-seed *seed*]" but modifying the settings means modifying the settings.json by hand. A replacement webui is in development which will supply a UI on all platforms.

Additional information on every feature of MMR is available by reading the tooltip shown by hovering the mouse over the option, or reading the detailed manual by pressing F1, or visiting the help menu.

MMR has no unique emulator requirements, you can play a randomized rom with any method that allows you to play a MM rom. MMR roms work on N64 emulators, they can be played on N64 hardware with everdrive and similar flashcard technology, and they can be played on Wii Virtual Console, MMR will even convert the randomized rom to a wad for you.

### Known Quirks of MMR

In vanilla MM, you use [Garo's Mask] to convince the ghost to summon a tree and grant you Ikana valley access, but in vanilla MM, he will also accept you wearing a [Gibdo Mask] and grant you Ikana access. Most people don't know this because you get the Gibdo mask after getting access to the soaring owl and you never need to travel east up that cliff again if you don't want to, but MMR accepts both masks as giving you access.

There is a hidden (bomb-able) grotto on the ramp steppe path to get to goron racetrack. If you do not have goron you can still get up there: There is a scarecrow near the boulder blocking the racetrack. Call it out using the Scarecrow song, then use the Hookshot.

You will sometimes find milk in what appears to be a bottle, but it is only milk, and it will not give you a bottle. There is just no milk without a bottle model for us to use, unlike the gold dust and potions.

The Clocktown Great Fairy gives you two items in vanilla (magic as deku, GFMask as link) and in Rando she still gives two items, you have to visit her twice; once as link and once as any other race (deku/goron/zora). You can visit her twice on the same 3 day cycle, just leave her cave and transform then immediately visit her again to get the second item.

Unlike OOT, you can NOT have bombchus without a bomb bag. 

### Warnings

MMR has a feature that allows you to use Fierce Diety mask anywhere, which is fun to play, but is slightly dangerous. His body is too big and can get stuck in tunnels and doors, or wedged between ramps and walls. Play with him at your own risk. The same is true for playing as adult link.

MMR can randomize enemies on the overworld and in dungeons, which is fun to play, but is slightly dangerous. Enemizer currently ignores logic, which can lead to seeds being (rarely) unbeatable (bombs all being locked behind a beamos, enemies you can't kill or move being in your way, ect). Play with enemizer at your own risk.

Do not mash the A button during the fish feeding cutscene, you can softlock (this is a vanilla MM bug). Pulling out the Ocarina immediately after Deku Playground day 3 can softlock the game. Carrying a dog and putting it down at the same time as talking to Mamamu Yan (the lady running the dog race) can softlock the game. Defeating Keeta and attempting to warp to Mountain Village without warping elsewhere or leaving the area (into Road to Ikana) crashes the game.

There is a list of other known issues in the #known-issues channel of our discord, linked at the bottom of this page.

### FAQS 

Q: My ROM doesn't seem to work what's wrong?  
A: The ROM should be a .z64 file (big endian). If it is not .z64 it will not work. Make sure you are not trying to use a ROM with a .n64 or .v64 extension. You will have to use Tool64 here to byteswap your ROM to "Big Endian": https://www.zophar.net/utilities/n64aud/tool-n64.html Also do NOT re-randomize a already randomized rom.  

Q: I renamed my ROM to .z64 but it's not working?  
A: DO NOT RENAME YOUR ROM! It has to be BYTESWAPED using Tool64 for it to work properly!  

Q: It says I need the expansion pack when I try to boot! How do I fix this?  
A: You need to be sure you are running the game with 8MB of ram (memory) not 4MB. In P64 this can be found in Options > Settings > settings: MAJORAS MASK > Set memory to 8MB  

Q: I checked "Wii Virtual Console Channel" but I cannot find my .wad file. Why?  
A: Check in your %appdata% folder and the default wad folder with the randomizer.  

Q: I'm getting an error about the randomizer not being able to find a path!  
A: Extract everything from the zip file to an empty folder. The randomizer needs to have access to all files it comes with, the EXE alone is not enough.  

Q: I'm starting with items I shouldn't/Why don't I have Ocarina?  
A: You have to make a new save file in the randomizer, you cannot start with an old save made in another seed.  

Q: I'm having some weird graphical errors! How can I fix this?  
A: This is not a fault of the randomizer or your rom. This is a problem with whatever you are using to play the rom. Either Look up better graphical settings or look into a new method to play. If you have trouble come visit our discord #emulator-support channel we might be able to help.  

Q: My emulator has a black screen/won't load/is not working.  
A: Either try another emulator, try a different graphics plugin or download the P64 prebuilt created for playing MM in our discord #resources channel.

Q: How do I randomzie UI colors?  
A: Right click to see further options.  

Q: I saw a stream of someone playing MMR with music from other games, how do I add more music to MMR?  
A: Our discord has a #music-customization channel which has a guide on making music, and a bunch of users posting their own music you can use.  

### Install

The latest release is available on the releases page: https://github.com/ZoeyZolotova/mm-rando/releases/latest

Beta releases are available on our Discord: https://discord.gg/7jBRhhJ

### Come visit our discord
Additional help and resources can be found at our discord: https://discord.gg/7jBRhhJ

We also have a seedbot and tracker, and other tools to help you play and race other players if you're interested.

----

### Build MM Randomizer

Please use a beta release from [our Discord](https://discord.gg/7jBRhhJ). The below information is for developers only.

#### Development artifacts

Development builds are available from [GitHub Actions](https://github.com/ZoeyZolotova/mm-rando/actions).

#### PowerShell

The script will download the source for mm-rando to a folder called `mm-rando`, build the GUI and CLI, and produce the application into `mm-rando\dist\`.

You must install [Visual Studio 2019 and .NET 5.0 SDK](https://my.visualstudio.com/Downloads?q=visual%20studio%202019), and make sure the `dotnet` tool is in your path.

If you can't or don't want to install these, or are having trouble, use the Docker instructions below.

```pwsh
git clone https://github.com/ZoeyZolotova/mm-rando
cd mm-rando
.\build.ps1
```

#### Docker (Windows PowerShell)

Install Docker Desktop on Windows 10 or 11 and configure it for Windows containers (required for building the GUI):

https://learn.microsoft.com/en-us/virtualization/windowscontainers/quick-start/set-up-environment?tabs=dockerce

**The GUI cannot be built on Linux; the below command is for Docker configured to run Windows containers only.**

```pwsh
git clone https://github.com/ZoeyZolotova/mm-rando
cd mm-rando
docker run --rm -v "$(pwd):c:/mm-rando" -it mcr.microsoft.com/dotnet/sdk:5.0 pwsh -c "cd \mm-rando && .\build.ps1"
```
