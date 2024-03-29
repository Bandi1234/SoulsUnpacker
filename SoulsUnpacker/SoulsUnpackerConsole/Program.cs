﻿using System;
using SoulsUnpackTools;
using System.IO;

namespace SoulsUnpackerConsole {
    internal class Program {
        internal static string name = "Souls Unpacker";
        internal static string version = "0.1.0";
        internal static string[] description = {
            "A tool designed mainly for people looking into translating From Software titles.",
            "Currently the only supported game is DSR (Dark Souls Remastered).",
            "This tool is capable of unpacking text bounds into a library of raw text or a slightly modified library for better use.",
            "We call this modified library a pure DS text library, which contains no repetion of entries, no empty files, better categories for items, etc.",
            "Naturally repacking text into their original form is also possible from both a raw and a clean library of files.",
            "Apart from text, the tool also support font un- and repacking, which may be useful when dealing with special, language specific characters.",
            "For more information, please read readme.md",
            "The tool is based on the SoulsFormats library made by the one and only JKAnderson."
        };

        static void Main(string[] args) {
            MainLoop();
        }

        static void MainLoop() {
            bool done = false;
            while (true) {
                Console.Clear();

                Console.WriteLine(name);
                Console.WriteLine("Navigate using num keys");
                Console.WriteLine();

                string[] options = { "Unpack text", "Repack text", "Unpack font", "Repack font", "About", "Exit" };
                for (int i = 0; i < options.Length; i++) {
                    if (i < options.Length - 1) {
                        Console.WriteLine("(" + (i + 1) + ") " + options[i]);
                    } else {
                        Console.WriteLine("(0) " + options[i]);
                    }
                }

                int choice = GetKeyNum(Console.ReadKey().Key);
                if (choice == -1 || choice >= options.Length) {
                    continue;
                }

                switch (choice) {
                    case 0:
                        done = true;
                        break;
                    case 1:
                        UnpackTextMenu();
                        break;
                    case 2:
                        RepackTextMenu();
                        break;
                    case 3:
                        SetupFontUnpack();
                        break;
                    case 4:
                        SetupFontRepack();
                        break;
                    case 5:
                        About();
                        break;
                }

                if (done) {
                    break;
                }
            }
        }

        static void UnpackTextMenu() {
            while (true) {
                Console.Clear();
                string[] options = { "Unpack to raw text", "Unpack to pure text", "Unpack to DSRText format", "Back" };
                for (int i = 0; i < options.Length; i++) {
                    if (i < options.Length - 1) {
                        Console.WriteLine("(" + (i + 1) + ") " + options[i]);
                    } else {
                        Console.WriteLine("(0) " + options[i]);
                    }
                }

                int choice = GetKeyNum(Console.ReadKey().Key);
                if (choice == -1 || choice >= options.Length) {
                    continue;
                }

                switch (choice) {
                    case 0: return;
                    case 1:
                        SetupRawUnpack();
                        return;
                    case 2:
                        SetupPureUnpack();
                        return;
                    case 3:
                        SetupDSRTUnpack();
                        return;
                }
            }
        }

        static void RepackTextMenu() {
            while (true) {
                Console.Clear();
                string[] options = { "Repack from raw text", "Repack from pure text", "Repack from DSRText format", "Back" };
                for (int i = 0; i < options.Length; i++) {
                    if (i < options.Length - 1) {
                        Console.WriteLine("(" + (i + 1) + ") " + options[i]);
                    } else {
                        Console.WriteLine("(0) " + options[i]);
                    }
                }

                int choice = GetKeyNum(Console.ReadKey().Key);
                if (choice == -1 || choice >= options.Length) {
                    continue;
                }

                switch (choice) {
                    case 0: return;
                    case 1:
                        SetupRawRepack();
                        return;
                    case 2:
                        SetupPureRepack();
                        return;
                    case 3:
                        SetupDSRTRepack();
                        return;
                }
            }
        }

        static void About() {
            while (true) {
                Console.Clear();
                Console.WriteLine(name + " " + version);
                Console.WriteLine();
                foreach (string line in description) {
                    Console.WriteLine(line);
                }
                Console.WriteLine();
                Console.WriteLine("(0) Back");
                int choice = GetKeyNum(Console.ReadKey().Key);
                if (choice == 0) {
                    break;
                }
            }
        }

        static void SetupRawUnpack() {
            Console.Clear();

            if (!File.Exists("item.msgbnd.dcx") || !File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Couldn't find dark souls msgbnd files.");
                Console.WriteLine("Please place both item.msgbnd.dcx and menu.msgbnd.dcx in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item.msgbnd.dcx and menu.msgbnd.dcx found!");

            if (Directory.Exists("item")) {
                Console.WriteLine("Deleting old /item folder...");
                Directory.Delete("item", true);
            }
            if (Directory.Exists("menu")) {
                Console.WriteLine("Deleting old /menu folder...");
                Directory.Delete("menu", true);
            }

            ConsoleLoadingBar lb = null;
            DSRTools.TextObserver observer = new DSRTools.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking raw text from item.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking raw text from menu.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DSRTools.UnpackRawText("item.msgbnd.dcx", "menu.msgbnd.dcx", "item", "menu", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking raw text into /item and /menu");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        static void SetupPureUnpack() {
            Console.Clear();

            if (!File.Exists("item.msgbnd.dcx") || !File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Couldn't find dark souls msgbnd files.");
                Console.WriteLine("Please place both item.msgbnd.dcx and menu.msgbnd.dcx in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item.msgbnd.dcx and menu.msgbnd.dcx found!");

            if (Directory.Exists("item")) {
                Console.WriteLine("Deleting old /item folder...");
                Directory.Delete("item", true);
            }
            if (Directory.Exists("menu")) {
                Console.WriteLine("Deleting old /menu folder...");
                Directory.Delete("menu", true);
            }

            ConsoleLoadingBar lb = null;
            DSRTools.TextObserver observer = new DSRTools.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking pure text from item.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking pure text from menu.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DSRTools.UnpackPureText("item.msgbnd.dcx", "menu.msgbnd.dcx", "item", "menu", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking pure text into /item and /menu");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        static void SetupRawRepack() {
            Console.Clear();

            if (!Directory.Exists("item") || !Directory.Exists("menu")) {
                Console.WriteLine("Couldn't find raw text libraries.");
                Console.WriteLine("Please place both your item and menu folders in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item and menu folders found!");

            if (File.Exists("item.msgbnd.dcx")) {
                Console.WriteLine("Deleting old item dcx...");
                File.Delete("item.msgbnd.dcx");
            }
            if (File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Deleting old menu dcx...");
                File.Delete("menu.msgbnd.dcx");
            }

            ConsoleLoadingBar lb = null;
            DSRTools.TextObserver observer = new DSRTools.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Repacking raw text into item.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Repacking raw text into menu.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DSRTools.RepackRawText("item", "menu", "item.msgbnd.dcx", "menu.msgbnd.dcx", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking raw text into item.msgbnd.dcx and menu.msgbnd.dcx");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        static void SetupPureRepack() {
            Console.Clear();

            if (!Directory.Exists("item") || !Directory.Exists("menu")) {
                Console.WriteLine("Couldn't find pure text libraries.");
                Console.WriteLine("Please place both your item and menu folders in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item and menu folders found!");

            if (File.Exists("item.msgbnd.dcx")) {
                Console.WriteLine("Deleting old item dcx...");
                File.Delete("item.msgbnd.dcx");
            }
            if (File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Deleting old menu dcx...");
                File.Delete("menu.msgbnd.dcx");
            }

            ConsoleLoadingBar lb = null;
            DSRTools.TextObserver observer = new DSRTools.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Repacking pure text into item.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Repacking pure text into menu.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DSRTools.RepackPureText("item", "menu", "item.msgbnd.dcx", "menu.msgbnd.dcx", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking pure text into item.msgbnd.dcx and menu.msgbnd.dcx");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        static void SetupDSRTUnpack() {
            Console.Clear();

            if (!File.Exists("item.msgbnd.dcx") || !File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Couldn't find dark souls msgbnd files.");
                Console.WriteLine("Please place both item.msgbnd.dcx and menu.msgbnd.dcx in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item.msgbnd.dcx and menu.msgbnd.dcx found!");

            if (File.Exists("item.DSRText")) {
                Console.WriteLine("Deleting old item.DSRText file...");
                File.Delete("item.DSRText");
            }
            if (File.Exists("menu.DSRText")) {
                Console.WriteLine("Deleting old menu.DSRText file...");
                File.Delete("menu.DSRText");
            }

            ConsoleLoadingBar lb = null;
            DSRTools.TextObserver observer = new DSRTools.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking DSRT from item.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking DSRT from menu.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DSRTools.UnpackDSRText("item.msgbnd.dcx", "menu.msgbnd.dcx", "item.DSRText", "menu.DSRText", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking DSRT into item.DSRText and menu.DSRText");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        static void SetupDSRTRepack() {
            Console.Clear();

            if (!File.Exists("item.DSRText") || !File.Exists("menu.DSRText")) {
                Console.WriteLine("Couldn't find DSRText files.");
                Console.WriteLine("Please place both your item.DSRText and menu.DSRText files in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item.DSRText and menu.DSRText found!");

            if (File.Exists("item.msgbnd.dcx")) {
                Console.WriteLine("Deleting old item dcx...");
                File.Delete("item.msgbnd.dcx");
            }
            if (File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Deleting old menu dcx...");
                File.Delete("menu.msgbnd.dcx");
            }

            ConsoleLoadingBar lb = null;
            DSRTools.TextObserver observer = new DSRTools.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Repacking DSRText content into item.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Repacking DSRText content into menu.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DSRTools.RepackDSRText("item.DSRText", "menu.DSRText", "item.msgbnd.dcx", "menu.msgbnd.dcx", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking DSRText content into item.msgbnd.dcx and menu.msgbnd.dcx");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        static void SetupFontUnpack() {
            Console.Clear();

            if (!File.Exists("DSFont24.tpf.dcx") || !File.Exists("TalkFont24.tpf.dcx")) {
                Console.WriteLine("Couldn't find DS font files.");
                Console.WriteLine("Please place DSFont24.tpf.dcx and Talkfont24.tpf.dcx in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("DSFont24.tpf.dcx and Talkfont24.tpf.dcx found!");

            if (Directory.Exists("DSFont24_TPF")) {
                Console.WriteLine("Deleting old /DSFont24_TPF...");
                Directory.Delete("DSFont24_TPF", true);
            }

            if (Directory.Exists("TalkFont24_TPF")) {
                Console.WriteLine("Deleting old /TalkFont24_TPF...");
                Directory.Delete("TalkFont24_TPF", true);
            }

            ConsoleLoadingBar lb = null;
            DSRTools.FontObserver observer = new DSRTools.FontObserver(
                (int maxDsFEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking font textures from DSFont24.tpf.dcx...", 0, maxDsFEntries);
                },
                (int dsFEntries, int maxDsFEntries) => {
                    lb.Update(dsFEntries);
                },
                (int maxTalkFEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking font textures from TalkFont24.tpf.dcx...", 0, maxTalkFEntries);
                },
                (int talkFEntries, int maxTalkFEntries) => {
                    lb.Update(talkFEntries);
                }
            );
            DSRTools.UnpackFont("DSFont24.tpf.dcx", "Talkfont24.tpf.dcx", "DSFont24_TPF", "TalkFont24_TPF", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking fonts into /DSFont24_TPF and /TalkFont24_TPF");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        static void SetupFontRepack() {
            Console.Clear();

            if (!Directory.Exists("DSFont24_TPF") || !Directory.Exists("TalkFont24_TPF")) {
                Console.WriteLine("Couldn't find DS font libraries.");
                Console.WriteLine("Please place both your DSFont24_TPF and TalkFont24_TPF folders in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("DSFont24_TPF and Talkfont24_TPF folders found!");

            if (File.Exists("DSFont24.tpf.dcx")) {
                Console.WriteLine("Deleting old DSFont24.tpf.dcx...");
                File.Delete("DSFont24.tpf.dcx");
            }

            if (File.Exists("TalkFont24.tpf.dcx")) {
                Console.WriteLine("Deleting old TalkFont24.tpf.dcx...");
                File.Delete("TalkFont24.tpf.dcx");
            }

            ConsoleLoadingBar lb = null;
            DSRTools.FontObserver observer = new DSRTools.FontObserver(
                (int maxDsFEntries) => {
                    lb = new ConsoleLoadingBar("Repacking font textures into DSFont24.tpf.dcx...", 0, maxDsFEntries);
                },
                (int dsFEntries, int maxDsFEntries) => {
                    lb.Update(dsFEntries);
                },
                (int maxTalkFEntries) => {
                    lb = new ConsoleLoadingBar("Repacking font textures into TalkFont24.tpf.dcx...", 0, maxTalkFEntries);
                },
                (int talkFEntries, int maxTalkFEntries) => {
                    lb.Update(talkFEntries);
                }
            );
            DSRTools.RepackFont("DSFont24_TPF", "TalkFont24_TPF", "DSFont24.tpf.dcx", "TalkFont24.tpf.dcx", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking fonts into DSFont24.ccm.dcx and TalkFont24.ccm.dcx");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        static int GetKeyNum(ConsoleKey key) {
            switch (key) {
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    return 0;
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return 1;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    return 2;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    return 3;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    return 4;
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    return 5;
                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    return 6;
                case ConsoleKey.D7:
                case ConsoleKey.NumPad7:
                    return 7;
                case ConsoleKey.D8:
                case ConsoleKey.NumPad8:
                    return 8;
                case ConsoleKey.D9:
                case ConsoleKey.NumPad9:
                    return 9;
                default:
                    return -1;
            }
        }
    }
}
