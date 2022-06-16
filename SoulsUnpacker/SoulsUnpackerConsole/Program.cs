﻿using System;
using SoulsUnpackTools;
using System.IO;

namespace SoulsUnpackerConsole {
    internal class Program {
        internal static string name = "Souls Unpacker";
        internal static string version = "0.1.1";

        static void Main(string[] args) {
            ConsoleMenu gameMenu = new ConsoleMenu(new string[] {name});
            ConsoleMenu aboutMenu = new ConsoleMenu(new string[] {
                name + " " + version,
                "",
                "A tool designed mainly for people looking into translating From Software titles.",
                "Currently the only supported game is DSR (Dark Souls Remastered).",
                "This tool is capable of unpacking text bounds into a library of raw text or a slightly modified library for better use.",
                "We call this modified library a pure DS text library, which contains no repetion of entries, no empty files, better categories for items, etc.",
                "Naturally repacking text into their original form is also possible from both a raw and a clean library of files.",
                "Apart from text, the tool also support font un- and repacking, which may be useful when dealing with special, language specific characters.",
                "For more information, please read readme.md",
                "The tool is based on the SoulsFormats library made by the one and only JKAnderson."
            });
            ConsoleMenu dsrMenu = new ConsoleMenu(new string[] {"Dark Souls Remastered"});
            ConsoleMenu dsrUnpackMenu = new ConsoleMenu(new string[] {"Unpack DSR text"});
            ConsoleMenu dsrRepackMenu = new ConsoleMenu(new string[] {"Repack DSR text"});
            ConsoleMenu ds3Menu = new ConsoleMenu(new string[] {"Dark Souls 3"});
            ConsoleMenu ds3UnpackMenu = new ConsoleMenu(new string[] {"Unpack DS3 text"});
            ConsoleMenu ds3RepackMenu = new ConsoleMenu(new string[] {"Repack DS3 text"});

            gameMenu.options.Add(new ConsoleOption("Dark Souls Remastered", () => dsrMenu.show(), false));
            gameMenu.options.Add(new ConsoleOption("Dark Souls 3", () => ds3Menu.show(), false));
            gameMenu.options.Add(new ConsoleOption("About", () => aboutMenu.show(), false));
            gameMenu.options.Add(new ConsoleOption("Exit", () => { }, true));

            aboutMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            //TODO DeS!!!

            dsrMenu.options.Add(new ConsoleOption("Unpack text", () => dsrUnpackMenu.show(), false));
            dsrMenu.options.Add(new ConsoleOption("Repack text", () => dsrRepackMenu.show(), false));
            dsrMenu.options.Add(new ConsoleOption("Unpack font", () => DSRSetup.SetupFontUnpack(), false));
            dsrMenu.options.Add(new ConsoleOption("Repack font", () => DSRSetup.SetupFontRepack(), false));
            dsrMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            dsrUnpackMenu.options.Add(new ConsoleOption("Unpack to raw text", () => DSRSetup.SetupRawUnpack(), true));
            dsrUnpackMenu.options.Add(new ConsoleOption("Unpack to pure text", () => DSRSetup.SetupPureUnpack(), true));
            dsrUnpackMenu.options.Add(new ConsoleOption("Unpack to DSRText format", () => DSRSetup.SetupDSRTUnpack(), true));
            dsrUnpackMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            dsrRepackMenu.options.Add(new ConsoleOption("Repack from raw text", () => DSRSetup.SetupRawRepack(), true));
            dsrRepackMenu.options.Add(new ConsoleOption("Repack from pure text", () => DSRSetup.SetupPureRepack(), true));
            dsrRepackMenu.options.Add(new ConsoleOption("Repack from DSRText format", () => DSRSetup.SetupDSRTRepack(), true));
            dsrRepackMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            //TODO DS2!!!

            ds3Menu.options.Add(new ConsoleOption("Unpack text", () => ds3UnpackMenu.show(), false));
            ds3Menu.options.Add(new ConsoleOption("Repack text", () => ds3RepackMenu.show(), false));
            ds3Menu.options.Add(new ConsoleOption("Back", () => { }, true));

            //TODO unpack to raw
            //TODO unpack to pure
            //TODO unpack to DSXText
            ds3UnpackMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            //TODO repack from raw
            //TODO repack from pure
            //TODO repack from DSXText
            ds3RepackMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            //TODO Sekiro!!!

            //TODO Elden Ring!!!

            gameMenu.show();
        }
    }
}
