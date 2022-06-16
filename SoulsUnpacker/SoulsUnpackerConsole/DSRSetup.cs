using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SoulsUnpackTools;

namespace SoulsUnpackerConsole {
    public static class DSRSetup {

        public static void SetupRawUnpack() {
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
            DSRTools.UnpackRawText("item.msgbnd.dcx", "menu.msgbnd.dcx", "item", "menu", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking raw text into /item and /menu");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupPureUnpack() {
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
            DSRTools.UnpackPureText("item.msgbnd.dcx", "menu.msgbnd.dcx", "item", "menu", observer);

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
            DSRTools.RepackRawText("item", "menu", "item.msgbnd.dcx", "menu.msgbnd.dcx", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking raw text into item.msgbnd.dcx and menu.msgbnd.dcx");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupPureRepack() {
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
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
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

        public static void SetupDSRTUnpack() {
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
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
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

        public static void SetupDSRTRepack() {
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
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
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

        public static void SetupFontUnpack() {
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
            CommonUtils.FontObserver observer = new CommonUtils.FontObserver(
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

        public static void SetupFontRepack() {
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
            CommonUtils.FontObserver observer = new CommonUtils.FontObserver(
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

    }
}
