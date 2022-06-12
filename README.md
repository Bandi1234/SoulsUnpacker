# SoulsUnpacker

A tool for unpacking, repacking and formating dark souls related files, mainly useful for fan-translations.

Current version: 0.1.0

> Builds on top of SoulsFormats by JKAnderson: https://github.com/JKAnderson/SoulsFormats

> SoulsUnpacker currently supports: 
> - DSR text 
> - DSR font

## DSR Text
- Raw text: text unpacked into 1 file per fmg entry and 1 folder per fmg. Results in long un/repack times, and
a hard to use structure.
- Pure text: modified raw text for easier editing. No repeating fmgs, no separate empty files and files stored in folders
based on their names and categories. (For example: item/weapon_names_/swords/broadsword)
- DSRText: a single file for all the unpacked text. This format provides the fastest un/repack times and is also the
easiest for another program or script to process.

> **IMPORTANT**   
> Always check what the format of your text is before attempting to repack it.   
> SoulsUnpacker is currently NOT capable of recognising different formats (for example raw vs pure text)   

## DSR font
- Both DSFont24.ccm.dcx and TalkFont24.ccm.dcx unpack into a folder of .dds files. This files can't be edited
in most image editing software, paint.net worked best for me.
- Whenever you make a change in a .dds file, use the following settings when saving over the it: 
    - BC3 (linear DXT5)
    - Mip Maps on
    - Mip Maps: Nearest neighbour
**if after saving over the file the filesize changed at all, then your settings weren't correctly set to the above values.**   
- TalkFont is used when npc dialog subtitles are displayed, and DSFont is used anywhere else. DSFont seems to be ignored
by the game, so changing that file currently doesn't affect actual gameplay. The cause of this is currently unkown. 