using System;
using SoulsUnpackTools;
using System.IO;

namespace SoulsUnpackerConsole {
    internal class Program {
        internal static string name = "Souls Unpacker";
        internal static string version = "0.2.0";

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
            ConsoleMenu ds2Menu = new ConsoleMenu(new string[] { "Dark souls 2 SOTFS" });
            ConsoleMenu ds2UnpackMenu = new ConsoleMenu(new string[] { "Unpack DS2 text" });
            ConsoleMenu ds2RepackMenu = new ConsoleMenu(new string[] { "Repack DS2 text" });
            ConsoleMenu ds3Menu = new ConsoleMenu(new string[] {"Dark Souls 3"});
            ConsoleMenu ds3UnpackMenu = new ConsoleMenu(new string[] {"Unpack DS3 text"});
            ConsoleMenu ds3RepackMenu = new ConsoleMenu(new string[] {"Repack DS3 text"});
            ConsoleMenu sekMenu = new ConsoleMenu(new string[] {"Sekiro"});
            ConsoleMenu sekUnpackMenu = new ConsoleMenu(new string[] {"Unpack Sekiro text"});
            ConsoleMenu sekRepackMenu = new ConsoleMenu(new string[] {"Repack Sekiro text"});

            gameMenu.options.Add(new ConsoleOption("Dark Souls Remastered", () => dsrMenu.Show(), false));
            gameMenu.options.Add(new ConsoleOption("Dark souls 2 SOTFS", () => ds2Menu.Show(), false));
            gameMenu.options.Add(new ConsoleOption("Dark Souls 3", () => ds3Menu.Show(), false));
            gameMenu.options.Add(new ConsoleOption("Sekiro", () => sekMenu.Show(), false));
            gameMenu.options.Add(new ConsoleOption("About", () => aboutMenu.Show(), false));
            gameMenu.options.Add(new ConsoleOption("Exit", () => { }, true));

            aboutMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            //TODO DeS!!!

            dsrMenu.options.Add(new ConsoleOption("Unpack text", () => dsrUnpackMenu.Show(), false));
            dsrMenu.options.Add(new ConsoleOption("Repack text", () => dsrRepackMenu.Show(), false));
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

            ds2Menu.options.Add(new ConsoleOption("Unpack text", () => ds2UnpackMenu.Show(), false));
            ds2Menu.options.Add(new ConsoleOption("Repack text", () => ds2RepackMenu.Show(), false));
            ds2Menu.options.Add(new ConsoleOption("Unpack font", () => DS2Setup.SetupFontUnpack(), false));
            ds2Menu.options.Add(new ConsoleOption("Repack font", () => DS2Setup.SetupFontRepack(), false));
            ds2Menu.options.Add(new ConsoleOption("Back", () => { }, true));

            ds2UnpackMenu.options.Add(new ConsoleOption("Unpack to raw text", () => DS2Setup.SetupRawUnpack(), true));
            ds2UnpackMenu.options.Add(new ConsoleOption("Unpack to pure text", () => DS2Setup.SetupPureUnpack(), true));
            ds2UnpackMenu.options.Add(new ConsoleOption("Unpack to DS2Text format", () => DS2Setup.SetupDS2TUnpack(), true));
            ds2UnpackMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            ds2RepackMenu.options.Add(new ConsoleOption("Repack from raw text", () => DS2Setup.SetupRawRepack(), true));
            ds2RepackMenu.options.Add(new ConsoleOption("Repack from pure text", () => DS2Setup.SetupPureRepack(), true));
            ds2RepackMenu.options.Add(new ConsoleOption("Repack from D2RText format", () => DS2Setup.SetupDS2TRepack(), true));
            ds2RepackMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            ds3Menu.options.Add(new ConsoleOption("Unpack text", () => ds3UnpackMenu.Show(), false));
            ds3Menu.options.Add(new ConsoleOption("Repack text", () => ds3RepackMenu.Show(), false));
            ds3Menu.options.Add(new ConsoleOption("Back", () => { }, true));

            ds3UnpackMenu.options.Add(new ConsoleOption("Unpack to raw text", () => DS3Setup.SetupRawUnpack(), true));
            ds3UnpackMenu.options.Add(new ConsoleOption("Unpack to pure text", () => DS3Setup.SetupPureUnpack(), true));
            ds3UnpackMenu.options.Add(new ConsoleOption("Unpack to DSRText format", () => DS3Setup.SetupDS3TUnpack(), true));
            ds3UnpackMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            ds3RepackMenu.options.Add(new ConsoleOption("Repack from raw text", () => DS3Setup.SetupRawRepack(), true));
            ds3RepackMenu.options.Add(new ConsoleOption("Repack from pure text", () => DS3Setup.SetupPureRepack(), true));
            ds3RepackMenu.options.Add(new ConsoleOption("Repack from DSRText format", () => DS3Setup.SetupDS3TRepack(), true));
            ds3RepackMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            sekMenu.options.Add(new ConsoleOption("Unpack text", () => sekUnpackMenu.Show(), false));
            sekMenu.options.Add(new ConsoleOption("Repack text", () => sekRepackMenu.Show(), false));
            sekMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            sekUnpackMenu.options.Add(new ConsoleOption("Unpack to raw text", () => SEKSetup.SetupRawUnpack(), true));
            sekUnpackMenu.options.Add(new ConsoleOption("Unpack to pure text", () => SEKSetup.SetupPureUnpack(), true));
            sekUnpackMenu.options.Add(new ConsoleOption("Unpack to SEKText format", () => SEKSetup.SetupSEKTUnpack(), true));
            sekUnpackMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            sekRepackMenu.options.Add(new ConsoleOption("Repack from raw text", () => SEKSetup.SetupRawRepack(), true));
            sekRepackMenu.options.Add(new ConsoleOption("Repack from pure text", () => SEKSetup.SetupPureRepack(), true));
            sekRepackMenu.options.Add(new ConsoleOption("Repack from SEKTExt format", () => SEKSetup.SetupSEKTRepack(), true));
            sekRepackMenu.options.Add(new ConsoleOption("Back", () => { }, true));

            //TODO Elden Ring!!!

            gameMenu.Show();
        }
    }
}
