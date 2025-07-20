# AstoriaUWP 0.0.10 - dev branch

Forked from: https://github.com/Ticomware/AstoriaUWP

## Abstract
"Project Astoria" remake (UWP)

## Screenshots
<p align="center">
  <img src="Images/sshot01.png">
  <img src="Images/sshot02.png">
  <img src="Images/sshot03.png">
</p>

## My actions (my 2 cents) 
- Some WIKI data added. 
- Quick R.E. (only some try...catch added to avoid accident app halts... and nothing more)
- Experimental DEX 7 format added 
- Some Graphics Rrimitives Rendering improved.. i hope =)


## Description
"Astoria for Universal Windows Platform" (AstoriaUWP) is an APK interpreter for the Universal Windows Platform. 
This program attempts to interpret an Android .APK file and runs it inside a UWP wrapper.

Important Note: This project was created as a proof-of-concept over a short amount of time. The code may not be perfect. 100500 bugs/uncompleted deals still there.
It exists for demonstration and educational purposes. Feel free to fork this project... and to do your own "engeneer journey"!


## Coding "workbench"
- Visual Studio 2022 (But VS 2017 compatibility remained, for Live WinPhone debugging, heh!)
- Windows SDK 19041 + 15063 installed (for W10M compatibility).
- x64 and ARM targets used


## Test scenario
- Place Images/helloworld1.apk at Pictures folder at your PC/WinPhone

- Run AstoriaUWP, and click "Pick folder&file (experimental)". Then find and choose helloworld1.apk and try to open this apk file.
- After loading phase, click "Install app" button.
- After installing phase, click "Apps list" button.
- Go to App Storage, find Layout folder.
- Try to run Emulator by tapping on "Hello world" item at apps list

In result, the phrase "Layout not found" must be appeared on Emu screen... or, maybe, "testbox" on right bottom corner :)


## Project status
- Intro/RnD +- 13/100
- Design +- 3/100 
- Tech. project +- 21/100
- Dev. project  +- 3/100
- Tests/Intro   +- 4/100


## References
My fork based on Ticomware's (AstoriaUWP)[https://github.com/Ticomware/AstoriaUWP] solution.

AstoriaUWP also used some code from the following projects:
- https://source.android.com/                 Android Open Source Project
- https://github.com/tbaron/androidxmldotnet  Android Xml .net
- https://github.com/mariokmk/dex.net         DEX.net


## Contribute!
There's still a TON of things missing from this proof-of-concept (MVP) and areas of improvement. Help wanted. 
Feel free to PR your improvements for this experimental soft!

## References / License
- (Ticomware)[https://github.com/Ticomware/]
- Apache License

## .
With best wishes,

  [m][e] 2022--2023

AstoriaUWP is RnD project only. AS-IS. No support. Distributed under the MIT License.

