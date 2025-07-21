# AstoriaUWP 0.0.13-alpha -- dev branch

![](Images/logo.png)

This is my fork based on Carl's [AstoriaUWP](https://github.com/cbialorucki/AstoriaUWP) solution. Note that if only _clone_ of original AstoriaUWP project. I am not pro in deep C# coding, so this solution is _frozen_... But today/at now (in 2025) , I still tried to do liiitttttlllleeee ProjectAstoria-inspired "flashback"... :)

## Caution /  Warning / Important Note
"Astoria for Universal Windows Platform" (AstoriaUWP) is an APK interpreter for the Universal Windows Platform. 
This program attempts to interpret an Android .APK file and runs it inside a UWP wrapper.

Important Note: This project was created as a proof-of-concept over a short amount of time. The code may not be perfect. 100500 bugs/uncompleted deals still there.
It exists for demonstration and educational purposes. Feel free to fork this project... and to do your own "engeneer journey"!


## Abstract
This is some "Project Astoria" "re-think"/ "alternative". 

### Philosophy: Why "AstoriaUWP" and Parallels with Microsoft Project Astoria
- Name Origin: The name "AstoriaUWP" is a clear homage to Microsoft's Project Astoria, which was an official Microsoft bridge project to run Android apps on Windows 10 Mobile (UWP platform). This project aims to achieve a similar goal: running Android APKs on Windows (UWP), hence the name. So, parallels:
  - Both projects attempt to bridge Android and Windows app ecosystems.
  - Both involve emulating or translating Android APIs/resources for the Windows platform.
  - Both face(d) significant technical challenges in compatibility, resource mapping, and UI rendering.
  - Project Astoria was discontinued by Microsoft, but this project continues the spirit of experimentation and cross-platform integration...

## Screenshot(s)
![Layout Render. Error](Images/sshot01.png)

## My "2 cents" 
- Some WIKI data added. 
- Quick R.E. (only some try...catch added to avoid accident app halts... and nothing more)
- DEX 7 detected (see Images/uc83.apk sample that can't de-dexed).
- Some code fixes (I've updated the extraction logic in `DroidApp.cs` so that before extracting the APK, the extraction directory   localAppRoot is fully cleaned by deleting all files and subdirectories. This prevents IOException errors caused by existing files or folders, ensuring that all APK contents, including resources.arsc , are extracted correctly. Please try installing the APK and starting the EmuPage again to confirm that the resource file is now present and the crash is resolved.)
- Adding base Android elements (FrameLayout, Button, etc.) initiated.

## Coding "workbench"
- Visual Studio 2022 (But VS 2017 compatibility remained, for Live WinPhone debugging, heh!)
- Windows SDK 19041 + 15063 installed (for W10M compatibility).
- x64 and ARM targets used

### Solution Structure and Module Overview 

1. AstoriaUWP (Main UWP Application)
- Purpose: The core application, providing the main UI, APK installation, emulation, and integration logic. Contains Applet (APK handling), Reassembly (Android resource/UI emulation), and other app-specific logic.
- Status: Many features are marked as MVP/proof-of-concept. The README and code comments indicate a lot of experimental and incomplete areas, especially in emulation, resource handling, and UI rendering. Some classes (e.g., AstoriaXmlParser , AstoriaAttrSet , AstoriaWindow ) previously threw NotImplementedException and still have stubbed or placeholder logic. 

2. AndroidUILib
- Purpose: Provides .NET/C# implementations of Android Java classes, namespaces, and UI components (e.g., android.view , android.widget ). Acts as an interop layer to mimic Android's class structure and behavior for the emulator.
- Status: Many classes are present, but a significant portion are likely stubs or partial implementations. Some files (like CloneNotSupportedException.cs ) are just basic class shells. The structure is broad but depth is limited in many areas. 

3. AndroidXml
- Purpose: Handles parsing and manipulation of Android XML resources (layouts, manifests, etc.). Includes readers, writers, and utilities for binary and text XML formats.
- Status: Appears functional for basic XML parsing, but advanced features or full compatibility with all Android XML nuances may be incomplete. Some files may be placeholders or have TODOs. 

4. DEX
- Purpose: Implements Dalvik/DEX bytecode parsing, disassembly, and related utilities. Contains classes for DEX headers, instructions, fields, methods, and disassembly logic.
- Status: Core parsing and disassembly logic is present, but advanced features (e.g., full method emulation, all opcodes, or integration with the emulator) may be incomplete or experimental.
### Incomplete/Experimental Areas
- General: The README and codebase both emphasize the MVP/experimental nature. Many modules are partial, with stubbed methods, TODOs, or comments indicating missing features.
- UI Emulation: The Reassembly/UI folder and related classes (e.g., Renderer.cs ) have placeholder logic for unsupported elements.
- Resource Handling: Some resource parsing and mapping logic is incomplete or only partially implemented.
- Exception Handling: Many places previously threw NotImplementedException or had empty catch blocks; some have been replaced with logging and fallbacks, but true implementations are still needed.

### Summary Table
Module     | Purpose  | Status/Notes 

AstoriaUWP | Main app, APK install/emulation, UI | MVP, many stubs/experimental areas 

AndroidUILib | Android class/UI emulation in .NET | Broad, many stubs/partial impl. 

AndroidXml | Android XML parsing/writing | Functional, but not fully complete 

DEX | Dalvik/DEX parsing/disassembly | Core present, advanced incomplete


## Status
- Proto. So, atnaw you can onlytest this implementation with DEX 7 files from Android 7.0+ applications. The code will now detect and log the DEX version during parsing, which should help with debugging. This in not user-friendly thing. Devs only.


## Test scenario
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

![](Images/footer.png)
