using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SoulsUnpackTools;

namespace SoulsUnpackerConsole {
    public static class DS3Setup {
        public static void SetupRawUnpack() {
            Console.Clear();

            if (!File.Exists("item_dlc2.msgbnd.dcx") || !File.Exists("menu_dlc2.msgbnd.dcx")) {
                Console.WriteLine("Couldn't find dark souls msgbnd files.");
                Console.WriteLine("Please place both item_dlc2.msgbnd.dcx and menu_dlc2.msgbnd.dcx in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item_dlc2.msgbnd.dcx and menu_dlc2.msgbnd.dcx found!");

            if (Directory.Exists("item_dlc2")) {
                Console.WriteLine("Deleting old /item_dlc2 folder...");
                Directory.Delete("item_dlc2", true);
            }
            if (Directory.Exists("menu_dlc2")) {
                Console.WriteLine("Deleting old /menu_dlc2 folder...");
                Directory.Delete("menu_dlc2", true);
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking raw text from item_dlc2.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking raw text from menu_dlc2.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DS3Tools.UnpackRawText("item_dlc2.msgbnd.dcx", "menu_dlc2.msgbnd.dcx", "item_dlc2", "menu_dlc2", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking raw text into /item_dlc2 and /menu_dlc2");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupPureUnpack() {
            Console.Clear();

            if (!File.Exists("item_dlc2.msgbnd.dcx") || !File.Exists("menu_dlc2.msgbnd.dcx")) {
                Console.WriteLine("Couldn't find dark souls msgbnd files.");
                Console.WriteLine("Please place both item_dlc2.msgbnd.dcx and menu_dlc2.msgbnd.dcx in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item_dlc2.msgbnd.dcx and menu_dlc2.msgbnd.dcx found!");

            if (Directory.Exists("item_dlc2")) {
                Console.WriteLine("Deleting old /item_dlc2 folder...");
                Directory.Delete("item_dlc2", true);
            }
            if (Directory.Exists("menu_dlc2")) {
                Console.WriteLine("Deleting old /menu_dlc2 folder...");
                Directory.Delete("menu_dlc2", true);
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking pure text from item_dlc2.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking pure text from menu_dlc2.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DS3Tools.UnpackPureText("item_dlc2.msgbnd.dcx", "menu_dlc2.msgbnd.dcx", "item_dlc2", "menu_dlc2", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking pure text into /item_dlc2 and /menu_dlc2");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupRawRepack() {
            Console.Clear();

            if (!Directory.Exists("item_dlc2") || !Directory.Exists("menu_dlc2")) {
                Console.WriteLine("Couldn't find raw text libraries.");
                Console.WriteLine("Please place both your item_dlc2 and menu_dlc2 folders in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item_dlc2 and menu_dlc2 folders found!");

            if (File.Exists("item_dlc2.msgbnd.dcx")) {
                Console.WriteLine("Deleting old item_dlc2 dcx...");
                File.Delete("item_dlc2.msgbnd.dcx");
            }
            if (File.Exists("menu_dlc2.msgbnd.dcx")) {
                Console.WriteLine("Deleting old menu_dlc2 dcx...");
                File.Delete("menu_dlc2.msgbnd.dcx");
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Repacking raw text into item_dlc2.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Repacking raw text into menu_dlc2.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DS3Tools.RepackRawText("item_dlc2", "menu_dlc2", "item_dlc2.msgbnd.dcx", "menu_dlc2.msgbnd.dcx", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking raw text into item_dlc2.msgbnd.dcx and menu_dlc2.msgbnd.dcx");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupPureRepack() {
            Console.Clear();

            if (!Directory.Exists("item_dlc2") || !Directory.Exists("menu_dlc2")) {
                Console.WriteLine("Couldn't find raw text libraries.");
                Console.WriteLine("Please place both your item_dlc2 and menu_dlc2 folders in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item_dlc2 and menu_dlc2 folders found!");

            if (File.Exists("item_dlc2.msgbnd.dcx")) {
                Console.WriteLine("Deleting old item_dlc2 dcx...");
                File.Delete("item_dlc2.msgbnd.dcx");
            }
            if (File.Exists("menu_dlc2.msgbnd.dcx")) {
                Console.WriteLine("Deleting old menu_dlc2 dcx...");
                File.Delete("menu_dlc2.msgbnd.dcx");
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Repacking pure text into item_dlc2.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Repacking pure text into menu_dlc2.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DS3Tools.RepackPureText("item_dlc2", "menu_dlc2", "item_dlc2.msgbnd.dcx", "menu_dlc2.msgbnd.dcx", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking pure text into item_dlc2.msgbnd.dcx and menu_dlc2.msgbnd.dcx");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupDS3TUnpack() {
            Console.Clear();

            if (!File.Exists("item_dlc2.msgbnd.dcx") || !File.Exists("menu_dlc2.msgbnd.dcx")) {
                Console.WriteLine("Couldn't find dark souls msgbnd files.");
                Console.WriteLine("Please place both item_dlc2.msgbnd.dcx and menu_dlc2.msgbnd.dcx in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item_dlc2.msgbnd.dcx and menu_dlc2.msgbnd.dcx found!");

            if (File.Exists("item.DS3Text")) {
                Console.WriteLine("Deleting old item.DS3Text file...");
                File.Delete("item.DS3Text");
            }
            if (File.Exists("menu.DS3Text")) {
                Console.WriteLine("Deleting old menu.DS3Text file...");
                File.Delete("menu.DS3Text");
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking DS3T from item_dlc2.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking DS3T from menu_dlc2.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DS3Tools.UnpackDS3Text("item_dlc2.msgbnd.dcx", "menu_dlc2.msgbnd.dcx", "item.DS3Text", "menu.DS3Text", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking DS3T into item.DS3Text and menu.DS3Text");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupDS3TRepack() {
            Console.Clear();

            if (!File.Exists("item.DS3Text") || !File.Exists("menu.DS3Text")) {
                Console.WriteLine("Couldn't find DS3Text files.");
                Console.WriteLine("Please place both your item.DS3Text and menu.DS3Text files in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("item.DS3Text and menu.DS3Text found!");

            if (File.Exists("item_dlc2.msgbnd.dcx")) {
                Console.WriteLine("Deleting old item_dlc2 dcx...");
                File.Delete("item_dlc2.msgbnd.dcx");
            }
            if (File.Exists("menu_dlc2.msgbnd.dcx")) {
                Console.WriteLine("Deleting old menu_dlc2 dcx...");
                File.Delete("menu_dlc2.msgbnd.dcx");
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Repacking DS3Text content into item_dlc2.msgbnd.dcx...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {
                    lb = new ConsoleLoadingBar("Repacking DS3Text content into menu_dlc2.msgbnd.dcx...", 0, maxMenuEntries);
                },
                (int menuEntries, int maxMenuEntries) => {
                    lb.Update(menuEntries);
                }
            );
            DS3Tools.RepackDS3Text("item.DS3Text", "menu.DS3Text", "item_dlc2.msgbnd.dcx", "menu_dlc2.msgbnd.dcx", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking DS3Text content into item_dlc2.msgbnd.dcx and menu_dlc2.msgbnd.dcx");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }
    }
}
