# AstoriaUWP 0.0.11-alpha -- master branch

This is my fork based on Carl's [AstoriaUWP](https://github.com/cbialorucki/AstoriaUWP) solution. Note that if only _clone_ of original AstoriaUWP project. I am not pro in deep C# coding, so this solution is _frozen_... But today/at now (in 2025) , I still tried to do liiitttttlllleeee ProjectAstoria-inspired "flashback"... :)

## Caution /  Warning / Important Note
"Astoria for Universal Windows Platform" (AstoriaUWP) is an APK interpreter for the Universal Windows Platform. 
This program attempts to interpret an Android .APK file and runs it inside a UWP wrapper.

Important Note: This project was created as a proof-of-concept over a short amount of time. The code may not be perfect. 100500 bugs/uncompleted deals still there.
It exists for demonstration and educational purposes. Feel free to fork this project... and to do your own "engeneer journey"!


## Screenshots
<p align="center">
  <img src="Images/sshot01.png">
  <img src="Images/sshot02.png">
  <img src="Images/sshot03.png">
</p>

## My actions (my 2 cents) 
- Some WIKI data added. 
- Quick R.E. (only some try...catch added to avoid accident app halts... and nothing more)
- DEX 7 detected (see Images/uc83.apk sample that can't de-dexed).
- Some code fixes (I've updated the extraction logic in `DroidApp.cs` so that before extracting the APK, the extraction directory   localAppRoot is fully cleaned by deleting all files and subdirectories. This prevents IOException errors caused by existing files or folders, ensuring that all APK contents, including resources.arsc , are extracted correctly. Please try installing the APK and starting the EmuPage again to confirm that the resource file is now present and the crash is resolved.)

## Coding "workbench"
- Visual Studio 2022 (But VS 2017 compatibility remained, for Live WinPhone debugging, heh!)
- Windows SDK 19041 + 15063 installed (for W10M compatibility).
- x64 and ARM targets used

## What's new?

### DEX 7 Detection and Parsing Support Added
### Experimental & dirty "layout" output via AndroidUIInteropLib :) 
 
## Testing
You can test this implementation with DEX 7 files from Android 7.0+ applications. The code will now detect and log the DEX version during parsing, which should help with debugging.

Enjoy your Android simulator for old Nokia phones with Windows 10 Mobile OS!

### Test scenario
- Move helloworld1.apk, testdpc7 or uc83.apk to Pictures folder at your PC/WinPhone
- Run AstoriaUWP, and click "Pick folder&file (experimental)". Then find and choose some apk and try to open this apk file.
- After loading phase, click "Install app" button.
- After installing phase, click "Apps list" button.
- Go to App Storage, find Layout folder.
- Try to run Emulator by tapping on "Hello world" item. You must see blue-colored empty view ("layout imitation" at now, not real layout).
- Try to run Emulator by tapping on "testdpc7" item. You must see some simple graphic primitive ("Easter Eggs" item parsing not ready).
- Try to run Emulator by tapping on "uc83" item. You must see blue-colored empty view ("Ungoogled Chromium" bad imitaion, hehe!)))) 

## Project status
- Intro/RnD +- 14/100
- Design +- 3/100 
- Tech. project +- 22/100
- Dev. project  +- 4/100
- Tests/Intro   +- 5/100


## Explore more resources for your own dev journey

AstoriaUWP also used some code from the following projects:
- https://source.android.com/                 Android Open Source Project
- https://github.com/tbaron/androidxmldotnet  Android Xml .net
- https://github.com/mariokmk/dex.net         DEX.net


## Contribute!
There's still a TON of things missing from this proof-of-concept (MVP) and areas of improvement. Help wanted. 
Feel free to PR your improvements for this experimental soft!

## References
- https://github.com/cbialorucki/AstoriaUWP Original AstoriaUWP project 
- https://github.com/cbialorucki Carl J. Bialorucki, C# developer, author/creator of AstoriaUWP

## .. 

- AstoriaUWP is RnD project only. AS-IS. No support. Distributed under the MIT License.

## .
- [M][E] Jul,21 2025


