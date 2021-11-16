# AstoriaUWP 0.0.0.6

Forked from: https://github.com/Ticomware/AstoriaUWP

## Abstract
"Project Astoria" remake (UWP)

Original status: Archived (Public archive)

## My actions (my 2 cents) 
1. Renamed DalvikUWPCSharp.sln onto AstoriaUWP.sln :)
2. Lite bugfix (some try...catch added to avoid accident app halts...)

## Description

Astoria for Universal Windows Platform

An APK interpreter for the Universal Windows Platform. This program attempts to interpret an Android .APK file and runs it inside a UWP wrapper.

Note: This project was created as a proof-of-concept over a short amount of time. The code may not be perfect. It exists for demonstration and educational purposes. Feel free to fork this project!


## Screenshots
![Shot 1](Images/shot1.png)

## Coding "workbench"
Visual Studio 2022 (But VS 2017 compatibility remained, for Live WinPhone debugging, heh!)
Windows SDK 16053

## Test scenario
Place _ASTORIA_/5 Test/testdpc7.apk at Pictures folder
Run AstoriaUWP, and click Install Apk Button.
After loading phase, click Install app button.
After installing phase, click App list.
Go to App Storage, find Layout folder.
Place _ASTORIA_/5 Test/activity_main.xml at Layout folder.
Try to run Emulator by tapping on TestDpc7 item
In result, some button must be generated.


## Project status

phase 1 Intro/RnD +- 0.6/100

phase 2 Design - 

phase 3 Tech. project -

phase 4 Dev. project  -

phase 5 Tests/Intro   -


## References
This project uses code from the following projects:

<a href="https://source.android.com/">Android Open Source Project</a>

<a href="https://github.com/tbaron/androidxmldotnet">androidxmldotnet</a>

<a href="https://github.com/mariokmk/dex.net">dex.net</a>

## Contribute!
There's still a TON of things missing from this proof-of-concept (MVP) and areas of improvement 

With best wishes,

  [m][e] 2021

AstoriaUWP is RnD project only. AS-IS. No support. Distributed under the MIT License.

