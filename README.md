<h1>
  <img src="https://i.ibb.co/syfQDzq/MMR-LOGO.png" width=55%>
</h1>

This is a randomizer for *The Legend of Zelda: Majora's Mask* for the *Nintendo 64* video game console.

## Table of Contents
1. [Installation](#--installation-)
     - [Majora's Mask ROM Requirements](#--majoras-mask-rom-requirements-)
     - [Changing a ROM's Byte Order](#--changing-a-roms-byte-order-)
     - [Installing .NET Dependencies](#--installing-net-dependencies-)
     - [Downloading the Randomizer](#--downloading-the-randomizer-)
2. [General Description](#--general-description-)
3. [Logical Quirks](#--logical-quirks-)
     - [Ikana Canyon Access](#ikana-canyon-access)
     - [Twin Islands Secret Grotto](#twin-islands-secret-grotto)
     - [Clock Town Great Fairy](#clock-town-great-fairy)
4. [Known Issues](#--known-issues-)
     - [Fierce Deity's Mask Anywhere](#fierce-deitys-mask-anywhere)
     - [Enemizer](#enemizer)
     - [Termina Field](#termina-field)
     - [Clock Town](#clock-town)
     - [Doggy Racetrack](#doggy-racetrack)
     - [Snowhead](#snowhead)
     - [Marine Research Lab](#marine-research-lab)
     - [Ikana Graveyard](#ikana-graveyard)
     - [Ikana Canyon](#ikana-canyon)
     - [Inverted Stone Tower Temple](#inverted-stone-tower-temple)
5. [Frequently Asked Questions](#--frequently-asked-questions-)
6. [Majora's Mask Randomizer Discord Server](#--majoras-mask-randomizer-discord-server-)

<h2>
  Installation <a href="#table-of-contents" title="Return to Table of Contents">🔝</a>
</h2>

A video tutorial for setting up Majora's Mask Randomizer can be found in <a href="https://www.youtube.com/watch?v=4woEtyg7GyM" target="_blank" title="How to Download and Play Majora's Mask Randomizer | 2022-2024">this YouTube video</a> — the video is a few years old, but it should help you through the process should you prefer watching video instructions to reading written instructions.

<h3>
  Majora's Mask ROM Requirements <a href="#table-of-contents" title="Return to Table of Contents">🔝</a>
</h3>

In order to randomize a ROM file with the randomizer, your ROM file must be a clean and unmodified Majora's Mask NTSC-U version 1.0 ROM file in the big endian byte order (`.z64` file extension) — any version of NTSC-J region or PAL region ROM files will not work. Your ROM file must also not be from the Gamecube release of Majora's Mask.

#### MD5 Checksum Verification
To make sure your ROM file is unmodified and is the correct game version, there is an MD5 checksum available below to cross-reference for a compressed Majora's Mask NTSC-U version 1.0 ROM file:

> [!TIP]
> To check if a ROM file is correct is to go to <a href="https://emn178.github.io/online-tools/md5_checksum.html" target="_blank" title="MD5 Checksum Tool">this site</a> and open the ROM file to check the MD5 checksum. If your ROM file does not return the same MD5 checksum as the one available below, then your ROM file is not a compressed and unmodified Majora's Mask NTSC-U version 1.0 ROM file.
>
> **MD5 Checksum:**
> ```
> 2A0A8ACB61538235BC1094D297FB6556
> ```

<h3>
  Changing a ROM's Byte Order <a href="#table-of-contents" title="Return to Table of Contents">🔝</a>
</h3>

If your ROM file has the file extension of either `.n64` (Little Endian) or `.v64` (Bytesswapped), the byte order will need to be changed to `.z64` (Big Endian) using <a href="https://www.zophar.net/download_file/2854" target="_blank" title="Click to Download Tool64">Tool64</a>.

> [!CAUTION]
> Changing a ROM file's file extension from `.n64` or `.v64` to `z64` *will not* change the ROM file's byte order — you must use a tool capable of changing data byte ordering to change a ROM file's byte order.

To change your ROM file's byte ordering using Tool64 follow the steps below:
> 1. Open Tool64.
> 2. At the top of the window, click "File > Open...".
> 3. In the file explorer window that opens, select the directory containing the ROM file to be modified.
> 4. Click "OK".
> 5. In the main window, Select the ROM file you wish to alter the byte order of.
> 6. Right-click and select "Big Endian" from the context menu.

<h3>
  Installing .NET Dependencies <a href="#table-of-contents" title="Return to Table of Contents">🔝</a>
</h3>

In order to run the Majora's Mask Randomizer program on your PC, you will need to install a dependency known as the ".NET Desktop Runtime".

Below are direct links to the .NET version 8.0 desktop runtime, for both the 64 bit (x64) and 32 bit (x86) installers:

- 64 Bit Windows Systems: <a href="https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.15-windows-x64-installer" target="_blank">.NET version 8.0 Desktop Runtime (Windows x64) Installer</a>
- 32 Bit Windows Systems: <a href="https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.15-windows-x86-installer" target="_blank">.NET version 8.0 Desktop Runtime (Windows x86) Installer</a>
- 64 Bit macOS Systems: <a href="https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-8.0.15-macos-x64-installer" target="_blank">.NET Version 8.0 Desktop Runtime (macOS x64) Installer</a>
- Other PC systems: <a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0" target="_blank">.NET Version 8.0 Installation Page</a>

> [!NOTE]
> When clicking either of the links above, the download for the .NET version 8.0 Desktop Runtime should begin automatically. If — for some reason — the download does not begin when the page finishes loading, there is a link in the middle of the page with the text, "click here to download manually", that will initiate the download manually.

Once you have downloaded the .NET version 8.0 Desktop Runtime, open the installer and click the "Install" button. If you already have the .NET version 8.0 Desktop Runtime, then an error will be displayed. Click "close" to exit the installer process if this occurs.

#### Checking Your .NET Desktop Runtime Version
If you want to ensure you have the correct version of the .NET Desktop Runtime, you can refer to the information below to check every version of the runtime that you have installed on Windows 10 or Windows 11.

#### Windows 11
To view the installed apps on your Windows 11 PC, follow the steps below:
> 1. Select "Start > Settings > Apps > <a href="https://tinyurl.com/y99z55pr" title="Click to open Installed Apps in Windows">Installed Apps</a>"
> 2. On the "Installed Apps" page, enter the text "runtime" in the search bar.
> 3. The list of apps will repopulate with apps containing the text "runtime" in their name.
> 4. In the repopulated list, find an entry of the "Micosoft Windows Desktop Runtime" with the following version number: "8.0.15".

#### Windows 10
To view the installed apps on your Windows 10 PC, follow the steps below:
> 1. Select "Start > Settings > <a href="https://tinyurl.com/y99z55pr" title="Click to open Apps &amp; Features in Windows">Apps</a>"
> 2. On the "Apps & Features" page, enter the text "runtime" in the search bar.
> 3. The list of apps will repopulate with apps containing the text "runtime" in their name.
> 4. In the repopulated list, find an entry of the "Micosoft Windows Desktop Runtime" with the following version number: "8.0.15".

<h3>
  Downloading the Randomizer <a href="#table-of-contents" title="Return to Table of Contents">🔝</a>
</h3>

Please ensure you have installed the .NET version 8.0 Desktop Runtime using the instructions above before downloading the randomizer. Once your PC is prepared, you can follow the steps below.

#### Downloading From GitHub
To download the latest release of the Majora's Mask Randomizer, follow the steps below:
> 1. Click "Releases" in the sidebar on the right-side of the page.
> 2. On the releases page of the Majora's Mask Randomizer GitHub, find the latest release and scroll to the "Assets" section of the release.
> 3. In the "Assets" section of the latest release, click the zipped file "MM-Randomizer-v1.16.0.12.zip" to download Majora's Mask Randomizer.
> 4. Find where you downloaded the zipped file, then extract the files into a folder of your choosing.
> 5. To open the randomizer GUI, open "MM Randomizer.exe". To run the CLI, open "MMR-CLI.exe" to generate a seed or use the instructions below.

> [!IMPORTANT]
> In order to use the randomizer on macOS and Linux installations, you will need to use the CLI (Command Line Interface) for Majora's Mask Randomizer with .NET installed. The randomizer does not have cross-platform GUI support currently as Wine does not work with Majora's Mask Randomizer. There is also no currently available web app that is able to generate seeds.
>
> To use the CLI, open a terminal window and use the following command:
> ```
> dotnet MMR.CLI.dll -input <ROM> [-seed *seed number*]
> ```
> In order to change the settings when using the CLI, you will need to modify the `settings.json` file manually.

<h2>
  General Description <a href="#table-of-contents" title="Return to Table of Contents">🔝</a>
</h2>

Majora's Mask Randomizer — often abbreviated as "MMR" — is a Windows program that takes *The legend of Zelda: Majora's Mask* and places items in the game at new random locations. This allows for a more dynamic playing experience. A seed generated with logic will ensure that the game is completeable without the use of glitches or softlocks.

Standard randomization settings will ensure a glitchless path through the seed exists — the randomizer will never prevent the use of glitches, but there is no guarantee that glitches will behave the exact way they do in the original vanilla game.

To enhance the randomizer experience, Majora's Mask Randomizer also offers an array of different cosmetic options to the player — such as custom music and custom tunic colors. There are also different options for player comfort and ensuring a more bite-sized experience — like an option for removing critical wiggle, and shortening or removing unecessary cutscenes from the game.

A full list of changes between one randomizer version to the next can be found on every randomizer version's releases page on Github.

> [!NOTE]
> Additional information for every feature of Majora's Mask Randomizer in the GUI via tooltips that appear when hovering the mouse over an option, or by reading the manual which can be opened by pressing `F1` or can be opened via the help menu at the top of the randomizer window: `Help > Manual`

<h3>
  Logical Quirks <a href="#table-of-contents" title="Return to Table of Contents">🔝</a>
</h3>

#### Ikana Canyon Access
> [!TIP]
> In the vanilla game, the `Garo's Mask` is used to convince the Poe Collector to summon a tree that allows you to enter Ikana Canyon. However, in the vanilla game, the Poe Collector will also accept the `Gibdo Mask` despite the mask only being obtainable after you free Pamela's Father from his curse with the Song of Healing in Ikana Canyon. Because of this, it is behavior that most players will not know, but the randomizer accounts for the player having either mask for access to Ikana Canyon.

#### Twin Islands Secret Grotto
> [!TIP]
> In the Twin Islands of the Snowhead region, there is a secret grotto by the tree on the ramp path to get into Goron Racetrack. If you do not have Goron, you can still access the secret grotto with Hookshot and Pierre the Scarecrow. To do this, call Pierre using the Scarecrow's Song on first island in the corner closest to the Goron Racetrack, then hookshot up to Pierre.

#### Clock Town Great Fairy
> [!TIP]
> The Clock Town Great Fairy gives Link two rewards — magic as Deku Link, and the Great Fairy Mask as Hylian Link. In Majora's Mask Randomizer, the Great Fairy will still reward you with two items if you visit her twice — once as Hylian Link, and once as Deku Link, Goron Link, or Zora Link. Both rewards can be obtained in the same 3-day cycle, just enter the Great Fairy's Fountain twice — the second reward can be obtained immediately after the first by exiting and re-entering the Great Fairy's Fountain.

<h3>
  Known Issues <a href="#table-of-contents" title="Return to Table of Contents">🔝</a>
</h3>

Below is a small list of warnings for settings and known issues in Majora's Mask, please take heed of them. Other known issues not found here, can be found in the `#known-issues` channel of the Majora's Mask Randomizer Discord server linked at the bottom of the page.

#### Fierce Deity's Mask Anywhere
> [!WARNING]
> Majora's Mask Randomizer has a feature that allows you to equip the Fierce Deity's Mask anywhere. While this is a fun option to play with, it also comes with risks — play with this option enabled at your own risk:
> 1. Fierce Deity Link's body is too tall and may get stuck in tunnels, get stuck doors, or may get wedged between ramps and walls.
> 2. When receiving a sword as Fierce Deity Link, he will repeatedly begin swinging his sword, taking off Fierce Deity's Mask fixes the issue. 

#### Enemizer
> [!WARNING]
> Majora's Mask Randomizer is capable of randomizing enemies in the overworld and dungeons. Enemizer currently ignores logic and can place enemies where their presence will crash the game or prevent you from obtaining a required item, which can lead to seeds being unbeatable — play with this option enabled at your own risk.

#### Termina Field
> [!WARNING]
> In Termina Field, performing various actions after playing a song for the Guay circling Clock Town can crash or softlock the game. Below are actions that will or may cause this:
> - **Reproducible:** Releasing bottled bugs into soft soil.
> - ***Potential:*** Getting revived by a bottled fairy.
> - ***Potential:*** Playing the song on the song wall in northern Termina Field.
> 
> There are other potential actions that may cause this bug to occur, so it is best to reload the area after performing any of the above actions or any action you may fear will cause this bug.

#### Clock Town
> [!WARNING]
> In Clock Town, there are various soflocks, crashes, or bugs that can occur if you are not careful:
> 1. In South Clock Town, after giving the Clock Town Business Scrub the Moon's Tear, and the Southern Swamp Business Scrub the Land Title Deed — which relocates the Southern Swamp Business Scrub to South Clock Town — beating Town Archery and going directly to South Clock Town will crash the game.
> 2. In the Stock Pot Inn, collecting the reward from Anju and Kafei — the Couple's Mask in vanilla — can be interrupted by putting on a transformation mask or using a bottle.
> 3. In the Deku Scrub Playground, there is a soflock that can occur if you use the ocarina immediately after completing the Deku Scrub Playground minigame on the final day.
> 4. In the Milk Bar, there is a softlock that can occur if you talk to Gorman — the male patron at the Milk Bar — just before 5 AM.
> 5. In the Curiosity Shop, viewing the 100/500 rupee item after buying it can crash the game. To prevent this, exit and re-enter the Curiosity Shop.

#### Doggy Racetrack
> [!WARNING]
> In the Doggy Racetrack, there are various softlocks, crashes, or bugs that can occur if you are not careful:
> 1. When carrying a dog, if the dog is dropped at the same time as you talk to Mamamu Yan — the woman running the dog race — you can softlock the game.
> 2. Mashing through the text that kicks you out of Doggy Racetrack past 8 PM after losing the Dog Race 500 Minigame will softlock the game.
> 3. Very rarely during the Dog Race 500 minigame, a dog can be considered to have finished the race at the halfway point. This is a vanilla bug.

#### Snowhead
> [!WARNING]
> In Snowhead, there are various softlocks, crashes, or bugs that can occur if you are not careful:
> 1. In Goron Shrine, backflipping or sidehopping into the Goron Elder's Son's — the crying Goron baby — item trigger will softlock the game.
> 2. In Goron Shrine, carrying an item — such as a clay jar — into the Goron Elder's Son's item trigger can prevent you from receiving the reward permanently.

#### Marine Research Lab
> [!WARNING]
> In the Marine Reseearch Lab, you can, and will, softlock the game by mashing the `A` button during the cutscene of either of the two red fish eating another fish (bottled or otherwise) — this is a vanilla Majora's Mask bug.

#### Ikana Graveyard
> [!WARNING]
> In Ikana Graveyard, there is a game crash that can occur when exiting from the Ikana Graveyard secret grotto, Dampé's hut, or the cutscene after defeating Captain Keeta. This crash will occur if you soar to the Mountain Village owl statue — only if the aforementioned conditions are met — if you don't soar to a different owl statue, entering one of the three graves in Ikana Graveyard, or exiting Road to Ikana to Termina Field or Ikana Canyon first. Entering Road to Ikana Canyon from Ikana Graveyard, or entering Ikana Graveyard from Road to Ikana Canyon without going elsewhere before you soar to the Mountain Village owl statue *will not* avoid this crash.

#### Ikana Canyon
> [!WARNING]
> In Ikana Canyon, there are various softlocks, crashes, or bugs that can occur if you are not careful:
> 1. In the Music-Box House, having Hookshot in your hand while playing Song of Healing for Pamela's Father will crash the game when you would normally receive the reward — the Gibdo Mask in Vanilla.
> 2. In the Ancient Castle of Ikana, burning one of the curtains in the Igos du Ikana boss room after defeating the King's Lackeys and before starting the fight with Igos du Ikana himself can crash the game.

#### Inverted Stone Tower Temple
> [!WARNING]
> In Inverted Stone Tower Temple — after defeating Twinmold — standing where the blue warp will appear as Giant Link will softlock the game if you step away far enough for the blue warp to spawn at the same time you take off Giant's Mask. Putting the Giant's Mask on and taking it back off fixes the issue.

<h2>
  Frequently Asked Questions <a href="#table-of-contents" title="Return to Table of Contents">🔝</a>
</h2>

Below is a list of frequently asked questions, please see if your question is available below before asking in the Majora's Mask Randomizer Discord server.

### Why Can't I Use Bombchus Without Bomb Bag?
In *The Legend of Zelda: Ocarina of Time*, it is possible to use Bombchus without having a Bomb Bag. However, in *The Legend of Zelda: Majora's Mask* this behavior has been changed and Bombchus *require* a Bomb Bag in order to be used.

### I Received a Bottle, but It Didn’t Appear in My Inventory?
You did not receive a bottle, you received the item that goes into an empty bottle that you don't currently have.

### Why Did My Boss Keys, Small Keys, and Quest Items Disappear When I Played Song of Time?
This is intended behavior. In the vanilla game, all consumable items — this includes both types of keys, all bottled contents, arrows, explosives, Deku Sticks, Deku Nuts, and rupees in Link's wallet — and quest items — this includes Stray Fairies, Letter to Kafei, Anju's Pendant, Letter to Mama, every title deed, and Gold Dust — are removed from your inventory or have their amount reduced to 0 when you play Song of Time.

### I Gave a Business Scrub My Moon’s Tear and/or Title Deed, but It Didn’t Disappear from My Inventory?
This is intended behavior. In the vanilla game, Business Scrubs do not remove items from your inventory because they get replaced with another item, with the exception of the Business Scrub in Ikana Canyon who does remove the Ocean Title Deed from your inventory. If the option "Quest Item Extra Storage" in the "Comfort" tab is enabled, then the Business Scrubs *will* remove their associated item from your inventory.

### Why Am I Not Getting the Right Item from a Cow?
For the pair two cows in the secret grottoes and the cows in the Romani Ranch barn at night, the cow's zones overlap so you will trigger one of the cows to give you its item instead of the other. If the "Close Cows" option in the "Comfort" tab is enabled, then the cows will no longer have the zone overlap issue.

### Why Doesn't My ROM Work with Majora's Mask Randomizer?
The ROM file you use to create a seed needs to be a clean and unmodified Majora's Mask NTSC-U version 1.0 ROM file in the big endian byte order (`.z64`). If your ROM file is modified in any way and/or not in the correct byte order, then it *will not* work with the randomizer.

> [!IMPORTANT]
> If you have a ROM file with the wrong byte order — little endian byte order (`.n64`) or byteswapped byte order (`.v64`) — you will have to use <a href="https://www.zophar.net/download_file/2854" target="_blank" title="Click to Download Tool64">Tool64</a> to change the byte order to big endian.

> [!CAUTION]
> Changing a ROM file's file extension from `.n64` or `.v64` to `z64` *will not* change the ROM file's byte order — you must use a tool capable of changing data byte ordering to change a ROM file's byte order.

### It Says I Need the Expansion Pak When I Try to Boot, How Do I Fix This?
You need to be sure you are running the game with 8MB of RAM (memory) not 4MB.

> [!NOTE]
> In Project64 this is located in the options menu:
> > Options \> Settings \> settings: MAJORAS MASK \> Set memory to 8MB

### I Checked “Wii Virtual Console Channel”, but Why Can I Not Find the Output File?
Check in your `%appdata%` folder and the default "output" folder in the randomizer's folder.

>[!NOTE]
> To check your `%appdata%` folder, press `Win + R`, then type `%appdata%` into the "Run" window that opens.

### I’m Getting an Error about the Randomizer Not Being Able to Find a Path, How Do I Fix This?
It is likely that you did not extract everything in the randomizer release's zip archive, or that your PC was unable to extract everything into the folder you specified. The randomizer requires every file in the randomizer release's zip archive to run — the executable file (`.exe`) alone is not enough.

>[!TIP]
> It is recommended that you do not extract the randomizer's files into a folder that requires privilege — e.g. a folder that creates a UAC (User Account Control) pop-up that asks for an administrator's permission when moving files to or from the folder.

### Why Am I Starting with Items I Shouldn’t Be?
You have most likely started the game on a previous save file. Starting with a previous save file instead of a new save file will cause you to have everything you collected in the previous save file.

### I’m Having Some Weird Graphical Errors, How Can I Fix Them?
This is not an issue with the randomizer, it is a problem with whatever you are using to play the ROM file created by the randomizer. You can search the internet for better graphical settings, or a different method to play. If you want or need help, someone in the Majora's Mask Randomizer Discord server may be able to help you in the `#support` channel.

### Why Does My Emulator Have a Black Screen, Won’t Load, or Just Isn’t Working?
This is an issue with either your emulator itself, or the graphics plugin the emulator is currently using. Try switching emulators, or switching the graphics plugin the emulator is using.

### How Do I Randomize the Game’s UI Colors?
In the randomizer's GUI interface, click the "Cosmetics" tab at the top of the window, then change the options in the "HUD" section of the cosmetics page.

### How Do I Randomize the Game's Music?
In the randomizer's GUI interface, click the "Cosmetics" tab at the top of the window, then select "Random" in the "Music / Sound" section of the cosmetics page.

The randomizer comes with a small variety of custom music in the randomizer installation's "music" folder, but you can find more custom music available in the Majora's Mask Randomizer Discord server in the `#music-releases` channel, on the <a href="https://github.com/MajorasMusicBoxHouse/Majoras-MusicBox-House" target="_blank">Majora's Music-Box House GitHub repository</a>, or other places on the internet where users post custom music for Majora's Mask Randomizer.

> [!TIP]
> To add more music to Majora's Mask Randomizer, just place the music files you download into your randomizer installation's "music" folder.

<h2>
  Majora's Mask Randomizer Discord Server <a href="#table-of-contents" title="Return to Table of Contents">🔝</a>
</h2>

There is a community server on Discord for <a href="https://discord.gg/7jBRhhJ" target="_blank" title="Cick to join the Majora's Mask Randomizer Discord Sever">Majora's Mask Randomizer</a> where you can find additional help and resources, and are able to connect with others who enjoy Majora's Mask Randomizer.

There is also a seedbot that can generate patches automatically for you from a variety of different settings — you still need the randomizer to apply the patch files — as well as trackers and other tools to help you in your playthrough with Majora's Mask Randomizer. And — should you be interested — races are also held by other players semi-frequently for those wish to compete against ne another to see how fast they can complete a randomizer seed.
