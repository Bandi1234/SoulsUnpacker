﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SoulsUnpackTools;

namespace SoulsUnpackerConsole {
    public static class SEKSetup {
        public static void SetupRawUnpack() {
            Console.Clear();

            if (!File.Exists("item.msgbnd.dcx") || !File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Couldn't find sekiro msgbnd files.");
                Console.WriteLine("Please palce both item.msgbnd.dcx and menu.msgbnd.dcx in the same folder as this tool.");
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
            SEKTools.UnpackRawText("item.msgbnd.dcx", "menu.msgbnd.dcx", "item", "menu", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking raw text into /item and /menu");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupPureUnpack() {
            Console.Clear();

            if (!File.Exists("item.msgbnd.dcx") || !File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Couldn't find sekiro msgbnd files.");
                Console.WriteLine("Please palce both item.msgbnd.dcx and menu.msgbnd.dcx in the same folder as this tool.");
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
            SEKTools.UnpackPureText("item.msgbnd.dcx", "menu.msgbnd.dcx", "item", "menu", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking pure text into /item and /menu");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupRawRepack() {
            Console.Clear();

            if (!Directory.Exists("item") || !Directory.Exists("menu")) {
                Console.WriteLine("Couldn't find raw text libraries.");
                Console.WriteLine("Please place both your item and menu folders in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item and menufolders found!");

            if (File.Exists("item.msgbnd.dcx")) {
                Console.WriteLine("Deleting old item dcx...");
                File.Delete("item.msgbnd.dcx");
            }
            if (File.Exists("menu.msgbnd.dcx")) {
                Console.WriteLine("Deleting old menu dcx...");
                File.Delete("menu.msgbnd.dcx");
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
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
            SEKTools.RepackRawText("item", "menu", "item.msgbnd.dcx", "menu.msgbnd.dcx", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking raw text into item.msgbnd.dcx and menu.msgbnd.dcx");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }
    }
}
