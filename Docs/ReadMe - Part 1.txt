-------------------
For the techy people who have some Reverse Engineering skills and interested in Astoria:

I have took time to run the 10240 Astoria Kernel through IDA Pro, and pulled the 'decompiled' Psudocode for each function I could load, and exported each decompiled function into seperate files.

I have not edited any of the code, as I have no idea how or where to start.
There will also be some errors in a few of the psudocode output.

Note this is NOT compliable as-is, 
Likely will need to reference with Windows 10 public headers and other available sources.

So, Currently:
ntoskrnl.exe - 10.0.10240.16384 (View readme to find link to .pdb)

Project Astoria Sources
https://drive.google.com/drive/folders/1yukSY-UOz2Wib2dd2RAUa4txCrfRySsU

Discussion Zone
https://3rdpartysource.microsoft.com/

GitHub - W10M decompiled files
https://github.com/Empyreal96/W10M_unedited-decomp

Empyreal96, [14.05.21 23:15]
F to anyone with actual Reverse Engineering knowledge.. I loaded AoWFramework.dll along with its .pdb file into IDA Pro.. over 15000 detected "Functions" (Functions, classes etc) 😭 but luckily the Public Debug symbols show us some of the Function names and not mangled names

Empyreal96, [14.05.21 23:16]
For reference I loosly think AoWFramework is Astoria(Android) on Windows Framework, it acts as the "Wrapper" to convert Android API calls into NT API calls etc

Empyreal96, [14.05.21 23:18]
I.e it seems to looks for stuff from Android IO Syncs, transfer and requests to what is called up when clicking "share" in an Android app

Empyreal96, [14.05.21 23:19]
Also heavy use on Windows RunTime which was used alot in Windows ARM iirc.. all this though is "from what I can see" so may not be 100% accurate

Empyreal96, [14.05.21 23:22]
This all gets sent through AoWFramework and other .dlls, from and back into a hidden mounted Android .wim, almost like a Headless version of Android

(End of maybe informative spam lol, some will already know this)


-------------------------------------------------------

Unknown User, [14.05.21 02:55]
[Переслано от Fadil]
Hello guys, 
Finally I‘m publishing the tools to get Project Astroria working on a few more devices.

WARNING:-
~ The Project Astoria build is not meant for daily use.
~ It is not recommended to use this build on your primary Device.
~ This is totally for fun.

NOTE:-
* The tool is still in Beta.
* Only few devices are tested.
* Only de-de, en-us, es-es, fr-fr, it-it keyboards are working for now.
* Instructions are in the readme.txt inside the tool folder.
* Project Astoria is offially supported on 1GB devices only.

10166 BUILD SUPPORTED DEVICES:-
-Htc One M8
-Samsung Ativ S
-Samsung Ativ SE
-Lumia 640
-Lumia 640
-Lumia 730
-Lumia 735
-Lumia 830
-Lumia 920
-Lumia 925
-Lumia 928
-Lumia Icon
-Lumia 930
-Lumia 1020
-Lumia 1520

10240 BUILD SUPPORTED DEVICE BY FFU:-
+ Lumia 640: https://mega.nz/file/hrxRhYIY#4rbc57p7D4jVV0BK77wvRhRQ7ezAopPm7mUeLphux2Y
+ Lumia 640xl: https://mega.nz/file/l6Rw2QbI#Jf0SFzRyAfwptTndNU2RF0nxOIgwrBAtShLsXcXLtFo
+ Lumia 930: https://mega.nz/file/l7ohjSKR#7UHGDDQ_OUaRyb1JfbhgROklyRC9rYRPTHSXRxbeIjs

10536 BUILD SUPPORTED DEVICES:-
- Htc One M8
- Samsung Ativ Odyssey
- Samsung Ativ SE
- Lumia 430
- Lumia 435
- Lumia 520
- Lumia 532
- Lumia 630
- Lumia 635
- Lumia 636
- Lumia 638
- Lumia 810
- Lumia 820
- Lumia 822
- Lumia 920
- Lumia 925
- Lumia 928
- Lumia Icon
- Lumia 930
- Lumia 1020
- Lumia 1520

TOOLS:-
× Phone Updater: http://www.mediafire.com/file/5c5kqbls7ucaj5c/04.25.6_phoneupdater_iu_refresh_astoria_refresh_4.7z/file
× Apk2Appx Packer: http://www.mediafire.com/file/vm23fvsaf443uzk/Apk2Appx_Packer_v2.0_Beta.zip/file

APPS COLLECTIONS:-
• Matheus Repo: https://mega.nz/#F!om4kkKCa!Nv7RCZOhCXlCfzRnKpfCXg

THANKS:-
# Thanks to @Tourniquet88 for his tool to make our dream as reality. 
# Thanks to HD2Owner for providing the update cabs.
# Thanks to Matheus silva and others who participated to testing and feedbacking.

Empyreal96, [14.05.21 23:15]
F to anyone with actual Reverse Engineering knowledge.. I loaded AoWFramework.dll along with its .pdb file into IDA Pro.. over 15000 detected "Functions" (Functions, classes etc) 😭 but luckily the Public Debug symbols show us some of the Function names and not mangled names

Empyreal96, [14.05.21 23:16]
For reference I loosly think AoWFramework is Astoria(Android) on Windows Framework, it acts as the "Wrapper" to convert Android API calls into NT API calls etc

Empyreal96, [14.05.21 23:18]
I.e it seems to looks for stuff from Android IO Syncs, transfer and requests to what is called up when clicking "share" in an Android app

Empyreal96, [14.05.21 23:19]
Also heavy use on Windows RunTime which was used alot in Windows ARM iirc.. all this though is "from what I can see" so may not be 100% accurate

Empyreal96, [14.05.21 23:22]
This all gets sent through AoWFramework and other .dlls, from and back into a hidden mounted Android .wim, almost like a Headless version of Android

(End of maybe informative spam lol, some will already know this)

---------------

Unknown User, [14.05.21 02:55]
[Переслано от Fadil]
Hello guys, 
Finally I‘m publishing the tools to get Project Astroria working on a few more devices.

WARNING:-
~ The Project Astoria build is not meant for daily use.
~ It is not recommended to use this build on your primary Device.
~ This is totally for fun.

NOTE:-
* The tool is still in Beta.
* Only few devices are tested.
* Only de-de, en-us, es-es, fr-fr, it-it keyboards are working for now.
* Instructions are in the readme.txt inside the tool folder.
* Project Astoria is offially supported on 1GB devices only.

10166 BUILD SUPPORTED DEVICES:-
-Htc One M8
-Samsung Ativ S
-Samsung Ativ SE
-Lumia 640
-Lumia 640
-Lumia 730
-Lumia 735
-Lumia 830
-Lumia 920
-Lumia 925
-Lumia 928
-Lumia Icon
-Lumia 930
-Lumia 1020
-Lumia 1520

10240 BUILD SUPPORTED DEVICE BY FFU:-
+ Lumia 640: https://mega.nz/file/hrxRhYIY#4rbc57p7D4jVV0BK77wvRhRQ7ezAopPm7mUeLphux2Y
+ Lumia 640xl: https://mega.nz/file/l6Rw2QbI#Jf0SFzRyAfwptTndNU2RF0nxOIgwrBAtShLsXcXLtFo
+ Lumia 930: https://mega.nz/file/l7ohjSKR#7UHGDDQ_OUaRyb1JfbhgROklyRC9rYRPTHSXRxbeIjs

10536 BUILD SUPPORTED DEVICES:-
- Htc One M8
- Samsung Ativ Odyssey
- Samsung Ativ SE
- Lumia 430
- Lumia 435
- Lumia 520
- Lumia 532
- Lumia 630
- Lumia 635
- Lumia 636
- Lumia 638
- Lumia 810
- Lumia 820
- Lumia 822
- Lumia 920
- Lumia 925
- Lumia 928
- Lumia Icon
- Lumia 930
- Lumia 1020
- Lumia 1520

TOOLS:-
× Phone Updater: http://www.mediafire.com/file/5c5kqbls7ucaj5c/04.25.6_phoneupdater_iu_refresh_astoria_refresh_4.7z/file
× Apk2Appx Packer: http://www.mediafire.com/file/vm23fvsaf443uzk/Apk2Appx_Packer_v2.0_Beta.zip/file

APPS COLLECTIONS:-
• Matheus Repo: https://mega.nz/#F!om4kkKCa!Nv7RCZOhCXlCfzRnKpfCXg

THANKS:-
# Thanks to @Tourniquet88 for his tool to make our dream as reality. 
# Thanks to HD2Owner for providing the update cabs.
# Thanks to Matheus silva and others who participated to testing and feedbacking.

Empyreal96, [14.05.21 23:15]
F to anyone with actual Reverse Engineering knowledge.. I loaded AoWFramework.dll along with its .pdb file into IDA Pro.. over 15000 detected "Functions" (Functions, classes etc) 😭 but luckily the Public Debug symbols show us some of the Function names and not mangled names

Empyreal96, [14.05.21 23:16]
For reference I loosly think AoWFramework is Astoria(Android) on Windows Framework, it acts as the "Wrapper" to convert Android API calls into NT API calls etc

Empyreal96, [14.05.21 23:18]
I.e it seems to looks for stuff from Android IO Syncs, transfer and requests to what is called up when clicking "share" in an Android app

Empyreal96, [14.05.21 23:19]
Also heavy use on Windows RunTime which was used alot in Windows ARM iirc.. all this though is "from what I can see" so may not be 100% accurate

Empyreal96, [14.05.21 23:22]
This all gets sent through AoWFramework and other .dlls, from and back into a hidden mounted Android .wim, almost like a Headless version of Android

(End of maybe informative spam lol, some will already know this)

Fadil, [24.05.21 14:26]
INTRODUCTION:
 Project Astoria is a abandoned project of Microsoft back in 2015s. It’s an Android runtime environment for Windows 10 Mobile, which would allow Android to run in an emulated environment with minimal changes, and have access to Microsoft platform APIs. It help developers to port their android apps without having much effort. Which means it let the users to install android packages directly to the windows 10 Mobile through this bridge. But unfortunately Microsoft has abandoned the project and blacklisted on production builds. So it was unable to bring Astoria back to the Windows 10 Mobile running 10586 or higher builds.

 But recently we had got two insider builds 10549 and 10570 update packages that support Astoria. And we can push the update packages by little tricky. Surprisingly, it supports all Windows Phones having an unlocked bootloader. Most importantly, this is Astoria’s all-time consistency and best performance build. Also the Android subsystem has been improved in this build. But the Android version is still KitKat (4.4.4). Keep in mind that this is an insider build, so bugs are expected. But we have fixed almost all the bugs with a patch, and we provide the patch with the updater.

Fadil, [24.05.21 14:26]
NOTES:
 * The Lumia devices which is X50s are unsupported. But Lumia 550 can be install by flashing the leaked Astoria FFU.

 * You must have to update to 10549 build first and then update to 10570 build.

 * After the build 10570 update, you must have to apply ‘Patch 2.bat’ before doing anything. And you have to wait about 10 minutes to install some apps in the background.

 * Project Astoria is offially supported on 1GB devices only.

* Only able to install APK that is Targeted to API 19 or lower.

* Most of the Google serviced apps are unsupported.

Fadil, [24.05.21 14:26]
KNOWN BUGS:
 - After the build 10549 update, there is a possibility of getting stuck in the black screen. In such cases, reboot or wait until the phone boots.

 - Can’t able to set a LockScreen Pin. Due to Settings app is bugged, The signin option in Settings app will crashes. But you can set a Pin by signing into MS account and it will ask you for setup a Pin.

 - If you have did a Hard Reset for some reason, you must have to set date to 14 oct 2015 in the OOBE setup. Else you will loss some system apps, and the only solution to get back the apps is... perform a Hard Reset again and set the date as mentioned above.

Fadil, [24.05.21 14:26]
DOWNLOADS:
 • Phone Updater (https://www.mediafire.com/file/jzryci5vwi9v7xr/04.25.6_phoneupdater_iu_refresh_astoria_refresh_7.zip/file) helps to download and push insider builds into Windows Phone devices.

 • Astoria Patch (https://www.mediafire.com/file/va15592og212n0k/Astoria_Patch.zip/file) helps to enable testsigning, fix some major bugs and much more etc...

 • Apk2Appx Packer (https://www.mediafire.com/file/tz76d83u2898e0x/Apk2Appx_Packer_v2.1_Beta.zip/file) helps to install APK and convert installed APK to APPX using a PC.

 • Ak2Appx Converter (https://www.mediafire.com/file/fpwwsyvjge0mpyp/Apk2Appx_Converter_v2.3.5.zip/file) helps to convert APK to APPX directly from PC.

 • Astoria Package Installer (https://www.mediafire.com/file/sqreqwiao5anpft/AstoriaPackageInstaller_0.3.0.0.zip/file) helps to install android converted appx along with it's data.

 • ProjectA Repo (https://mega.nz/folder/QVFQnBSA#2Rj8OvBoF1Qd5tYte7cbQA) is a mega cloud based repository, contains android converted apps & games appx for Astoria builds.

Fadil, [24.05.21 14:26]
TUTORIALS:
 + How to get PROJECT ASTORIA on Windows Phones (https://youtu.be/vP-z8jVXVBQ)

 + How To Convert & Install Android APK To Windows 10 Mobile APPX (https://youtu.be/YhqctrrFDpo)