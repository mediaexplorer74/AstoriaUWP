So something that may or may not be interesting to techy people here:

I have fetched and uploaded to Google drive 7GB of files, these are from: https://3rdpartysource.microsoft.com/ and are labelled "Project Astoria"

What this seems to be is some kind of Android source used in development with the Astoria Android image, and as it is originally Open Source Code, MS have to supply links and Info on the Open Sourced code they use in Microsoft Products..

Not before any excitement brews, I strongly feel this still lacks Device Configs for the "hyperv hardware" that Astoria is targeted for, as these are likely Microsoft made.. but we could try extract from image/recreate the configs..

Now I have never built ASOP or even know what the source tree for ASOP looks like, but to any here who has Android experience, these could help in some way! Even if just to understand what they used to build.

(Archives are unedited, from the MS website, thanks to @Tourniquet88 for pointing me towards the original files)

https://drive.google.com/drive/folders/1yukSY-UOz2Wib2dd2RAUa4txCrfRySsU?usp=sharing

Microsoft (https://3rdpartysource.microsoft.com/)
Third Party Disclosures
How-to: filter third party notices and source code from Microsoft products by typing "Astoria"

Empyreal96, [18.05.21 02:05]
Could someone who currently has a device that can run Astoria, possibly send me the C:\Windows\System32\aow\aow.wim file from a Fresh install(I.e reflash FFU and pass oobe) please? 

I want to check something about it against one from the FFUs to see if a few things are placed after install or not

Empyreal96, [18.05.21 02:06]
Or at the very least an already installed system, open the .wim and tell me if anything exists in: AoW.wim\proc\


Reason is that if files get linked on init, there isn't a visible kernel config, which my guess would imply it is still very "bash on windows" style with the Android/linux kernel being baked into a special system file, which would suck eggs

Empyreal96, [18.05.21 02:09]
Also anyone interested in info, some of the Astoria android image references ro.hardware = hyperv which is interesting

--------------

zhin_, [27.04.21 14:58]
[ Файл : MyBoy! (GBA).xap ]
Guys you can fix the Myboy xap to work in wp10 build 1511, the application installs correctly but when entering the menu it closes :( this is the best gameboy advance emulator for windows phone, please repair it 🥺🙏

Andrew, [27.04.21 16:35]
[В ответ на Anonymous]
Pinned message

Anonymous, [27.04.21 19:24]
[В ответ на Fadil]
I'm using Lumia 550 with a 1gb RAM, can it work??

JULIΛN (Pinguin2001), [27.04.21 19:46]
[В ответ на Anonymous]
read the message and you will see its not supported

VLΛD, [27.04.21 19:56]
[В ответ на Anonymous]
Lumia 550 is not supported

Naughty Choudhary, [28.04.21 14:59]
Is Astoria working in Nokia Lumia 929 icon... like whatsapp Facebook Instagram ??

Netrotion, [28.04.21 15:09]
[В ответ на Naughty Choudhary]
Read pinned message

Naughty Choudhary, [28.04.21 15:09]
It mean support 👍

Netrotion, [28.04.21 15:10]
Think it doesn't for lumia 929

Maimoon Jamil, [28.04.21 17:31]
how to boot to uefi mode on lumia 640

Jamal Meddani, [28.04.21 17:57]
/rules@MissRose_bot

Rose, [28.04.21 17:57]
[В ответ на Jamal Meddani]
The rules for ProjectA are:

The rules are simple:
1.Only use ENGLISH to chat
2.Do not ask OFF-TOPIC questions (WhatsApp, Facebook, windows 10 arm32,...)

Jamal Meddani, [28.04.21 17:58]
Hp elite x3 need apps

Maimoon Jamil, [28.04.21 18:46]
how to install project astoria witout PhoneUpdater

Maimoon Jamil, [28.04.21 19:24]
[ Файл : Annotation 2021-04-28 212205.png ]
can anyone help me plz

Fadil, [28.04.21 19:41]
[В ответ на Maimoon Jamil]
Read the instructions

Maimoon Jamil, [28.04.21 19:41]
[В ответ на Fadil]
where it is

Fadil, [28.04.21 19:42]
[В ответ на Maimoon Jamil]
Readme.txt in the phone updater

Maimoon Jamil, [28.04.21 20:55]
how to know project astoria installed or not

Maimoon Jamil, [28.04.21 21:29]
how to install project astoria without going  back to windows 8 or 8.1

د.مصطفى, [28.04.21 21:54]
[ Фотография ]
Hi please i want the link of 
547
603
Update cos i just have 
107
1066
297
Using universal updater offline 2021 pc app

Maimoon Jamil, [28.04.21 22:56]
convert apk to appx

Maimoon Jamil, [28.04.21 23:24]
[ Файл : {F8D54AA7-4657-431D-BBCA-1EA81CC68CE2}.png ]
PLZ help me

¥Д§ƩℛℋДτДᛗ¶ℬJ, [28.04.21 23:47]
/rules@MissRose_bot

Rose, [28.04.21 23:47]
[В ответ на ¥Д§ƩℛℋДτДᛗ¶ℬJ]
The rules for ProjectA are:

The rules are simple:
1.Only use ENGLISH to chat
2.Do not ask OFF-TOPIC questions (WhatsApp, Facebook, windows 10 arm32,...)

¥Д§ƩℛℋДτДᛗ¶ℬJ, [28.04.21 23:47]
/rules@MissRose_bot

Rose, [28.04.21 23:47]
[В ответ на ¥Д§ƩℛℋДτДᛗ¶ℬJ]
The rules for ProjectA are:

The rules are simple:
1.Only use ENGLISH to chat
2.Do not ask OFF-TOPIC questions (WhatsApp, Facebook, windows 10 arm32,...)

Candela, [29.04.21 00:10]
is there any interop tools for 10240?

Fadil, [29.04.21 00:17]
[В ответ на Candela]
IT preview

Candela, [29.04.21 00:19]
ok thanks

د.مصطفى, [29.04.21 00:41]
Why win 10 with project a developer wont be able to used after reboot or second day it just make crash

Andrew, [29.04.21 00:46]
It could be a windows bug

د.مصطفى, [29.04.21 00:47]
[В ответ на Andrew]
How can fix it ?

Andrew, [29.04.21 00:48]
If you are using build 10166 I have read before that you have to disable developer mode before restart

Candela, [29.04.21 00:57]
[В ответ на د.مصطفى]
it is common bug with astoria. you should disable devmode with interop tools or factory reset your phone

Candela, [29.04.21 01:00]
[В ответ на Fadil]
can you give appx of it, i cant login to store or download with adguard

Fadil, [29.04.21 01:00]
[В ответ на Fadil]
Here it is

Candela, [29.04.21 01:04]
ok thanks, can i install them with enterprise installer or should i use another method?

د.مصطفى, [29.04.21 01:08]
[В ответ на Andrew]
Yes last time when i make update to 
297 i was able the developer 
But it just stuck on logo when it update is that the problem ?

د.مصطفى, [29.04.21 01:09]
[В ответ на Candela]
If i dissable develop mode 
I cant work with apk to appx

Candela, [29.04.21 01:10]
you should disable it to access to developer mode and device portal again

Candela, [29.04.21 01:11]
after disabling it with registry you can enable it back and use it usually again

د.مصطفى, [29.04.21 01:12]
[В ответ на Candela]
Just do this to fix it ever
Or every day make it

Candela, [29.04.21 01:13]
[В ответ на د.مصطفى]
need to do this only when developer mode page crashing

د.مصطفى, [29.04.21 01:13]
I was install every interiptool from 1.1 beta to 2.0
Not work or show on app

Candela, [29.04.21 01:14]
and for preventing crashing, keep your device portal off when you arent going to install anything

د.مصطفى, [29.04.21 01:15]
[В ответ на Candela]
Oh that good thx

Fadil, [29.04.21 01:16]
[В ответ на د.مصطفى]
Keep dev unlocked and keep device discovery disabled after the use

د.مصطفى, [29.04.21 01:18]
[В ответ на Fadil]
Ok i will do this

د.مصطفى, [29.04.21 01:18]
[В ответ на د.مصطفى]
And this any fix ?

Fadil, [29.04.21 01:19]
[В ответ на د.مصطفى]
Use 2.0 preview

Fadil, [29.04.21 01:20]
Install dependencies first

د.مصطفى, [29.04.21 01:20]
[В ответ на Fadil]
Where can i find it please ?

د.مصطفى, [29.04.21 01:20]
[В ответ на Fadil]
Ok i will do this

Fadil, [29.04.21 01:20]
[В ответ на Fadil]
^

د.مصطفى, [29.04.21 01:22]
[В ответ на Fadil]
Thx i will do it

د.مصطفى, [29.04.21 01:23]
But left the forth update 547 
When will fix this

Candela, [29.04.21 14:11]
is there any workaround for memory leak?

Andrew, [29.04.21 14:27]
What do you mean with memory leak?

Candela, [29.04.21 14:35]
hadn't astoria framework got memory leaks or am i wrong?

Andrew, [29.04.21 15:21]
Idk

Maimoon Jamil, [29.04.21 16:11]
[ Файл : Annotation 2021-04-29 181011.png ]
help me plz

zhin_, [29.04.21 18:14]
guys android apps run slow in lumia 640 :(

Lasitha Samarasinghe, [29.04.21 19:09]
[В ответ на zhin_]
Astoria is an unfinished project so expected as usual

Maimoon Jamil, [29.04.21 19:09]
can i install project astoria on lumia 640 lte ATT US verion

zhin_, [29.04.21 19:14]
[В ответ на Lasitha Samarasinghe]
ok guy :(

Newb Stuff, [02.05.21 05:49]
/rules@MissRose_bot

Rose, [02.05.21 05:49]
[В ответ на Newb Stuff]
The rules for ProjectA are:

The rules are simple:
1.Only use ENGLISH to chat
2.Do not ask OFF-TOPIC questions (WhatsApp, Facebook, windows 10 arm32,...)

thakor arvind, [02.05.21 09:34]
/rules@MissRose_bot

Rose, [02.05.21 09:34]
[В ответ на thakor arvind]
The rules for ProjectA are:

The rules are simple:
1.Only use ENGLISH to chat
2.Do not ask OFF-TOPIC questions (WhatsApp, Facebook, windows 10 arm32,...)

Sandeep Soul, [02.05.21 18:45]
Can I install Apk in 640l dual sim 3g??

Andrew, [02.05.21 19:02]
[В ответ на Sandeep Soul]
Install astoria build first

Sandeep Soul, [02.05.21 19:05]
Can uh help me

A8 Player, [02.05.21 19:06]
[В ответ на Sandeep Soul]
Read pinned message

Sandeep Soul, [02.05.21 19:08]
My build is 15063

Andrew, [02.05.21 19:11]
[В ответ на Sandeep Soul]
Read pinned message

A8 Player, [02.05.21 19:11]
[В ответ на Sandeep Soul]
Model?

Sandeep Soul, [02.05.21 19:12]
Rm-1067

A8 Player, [02.05.21 19:15]
It supports to every ver of w10m project Astoria in pinned message

Sandeep Soul, [02.05.21 19:18]
Ok bro

Sandeep Soul, [02.05.21 19:18]
I will try

A8 Player, [02.05.21 19:19]
[В ответ на Sandeep Soul]
Good luck

Fadil, [03.05.21 22:09]
[ Альбом ]
Hello everyone,
   It’s been a long time there was nothing about ASTORIA. And we all know that only few devices are working with latest ASTORIA build. also it’s kinda buggy OS.
   So now I have to inform that we have just got a progress. Yeah! we have going to get even more updates with all devices support. Which is 10549 and 10570 build. It is perfect stable and uwp apps from TH2 build will also supported in it. Also we got added 10536 build for devices having 720x1280 resolution, like lumia 640 and XL. So STAY TUNED.

JULIΛN (Pinguin2001), [03.05.21 22:09]
[В ответ на Fadil]
Wow 🥳

xTR, [03.05.21 22:09]
[В ответ на Fadil]
yey

xTR, [03.05.21 22:09]
@PEPSIMANTR

Alex, [03.05.21 22:09]
🥳🥳🥳

VLΛD, [03.05.21 22:10]
[В ответ на Fadil]
Nice 🥳

Candela, [03.05.21 22:10]
[В ответ на Fadil]
🥳

Andrew, [03.05.21 22:10]
[В ответ на Fadil]
Good job

J3rry, [03.05.21 22:10]
Will it work on lumia 950xl?

Empyreal96, [18.05.21 00:47]
[В ответ на Arañita Ando_pero_no_pico]
After 10586 Astoria binaries are blocked by the OS so without heavy patching by people who know how, we will always be somewhat limited if we want to use Astoria

Empyreal96, [18.05.21 00:48]
One thing I would like to see, is if the minimal Android base has a kernel, and if so, a kernel config

Arañita Ando_pero_no_pico, [18.05.21 00:48]
Every new compilation will also have different keys

Arañita Ando_pero_no_pico, [18.05.21 00:48]
If we somehow could run a chroot-like windows mobile subsystem inside a newer version...

Empyreal96, [18.05.21 00:52]
In a sense yeah, MS could have marketed WP much better, but without Astoria we wouldn't have WSL

Empyreal96, [18.05.21 02:05]
Could someone who currently has a device that can run Astoria, possibly send me the C:\Windows\System32\aow\aow.wim file from a Fresh install(I.e reflash FFU and pass oobe) please? 

I want to check something about it against one from the FFUs to see if a few things are placed after install or not

Empyreal96, [18.05.21 02:06]
Or at the very least an already installed system, open the .wim and tell me if anything exists in: AoW.wim\proc\


Reason is that if files get linked on init, there isn't a visible kernel config, which my guess would imply it is still very "bash on windows" style with the Android/linux kernel being baked into a special system file, which would suck eggs

Empyreal96, [18.05.21 02:09]
Also anyone interested in info, some of the Astoria android image references ro.hardware = hyperv which is interesting