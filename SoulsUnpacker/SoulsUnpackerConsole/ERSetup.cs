﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SoulsUnpackTools;

namespace SoulsUnpackerConsole {
    public static class ERSetup {
        public static void SetupRawUnpack() {
            Console.Clear();

            if (!File.Exists("item.msgbnd.dcx") || !File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Couldn't find elden ring msgbnd files.");
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
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
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
            ERTools.UnpackRawText("item.msgbnd.dcx", "menu.msgbnd.dcx", "item", "menu", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking raw text into /item and /menu");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupPureUnpack() {
            Console.Clear();

            if (!File.Exists("item.msgbnd.dcx") || !File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Couldn't find elden ring msgbnd files.");
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
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
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
            ERTools.UnpackPureText("item.msgbnd.dcx", "menu.msgbnd.dcx", "item", "menu", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking pure text into /item and /menu");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupRawRepack() { 
            //TODO this
        }

        public static void SetupPureRepack() { 
            //Todo this
        }

        public static void SetupERTUnpack() {
            Console.Clear();

            if (!File.Exists("item.msgbnd.dcx") || !File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Couldn't find elden ring msgbnd files.");
                Console.WriteLine("Please place both item.msgbnd.dcx and menu.msgbnd.dcx in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item.msgbnd.dcx and menu.msgbnd.dcx found!");

            if (File.Exists("item.ERText")) {
                Console.WriteLine("Deleting old item.ERText file...");
                File.Delete("item.ERText");
            }
            if (File.Exists("menu.ERText")) {
                Console.WriteLine("Deleting old menu.ERText file...");
                File.Delete("menu.ERText");
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking ERT from item.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking ERT from menu.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            ERTools.UnpackERText("item.msgbnd.dcx", "menu.msgbnd.dcx", "item.ERText", "menu.ERText", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking ERT into item.ERText and menu.ERText");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }
    }
}
