using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SoulsUnpackTools;

namespace SoulsUnpackerConsole {
    public static class DS2Setup {
        public static void SetupRawUnpack() {
            Console.Clear();

            if (!Directory.Exists("english")) {
                Console.WriteLine("Couldn't find 'english' folder containing ds2 fmgs.");
                Console.WriteLine("Please place your 'english' folder in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("'english' folder found!");

            if (Directory.Exists("english_UP")) {
                Console.WriteLine("Deleting old /english_UP folder...");
                Directory.Delete("english_UP", true);
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking raw text from english...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => {},
                (int menuEntries, int maxMenuEntries) => {}
            );
            DS2Tools.UnpackRawText("english", "english_UP", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking raw text into english_UP");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupPureUnpack() {
            Console.Clear();

            if (!Directory.Exists("english")) {
                Console.WriteLine("Couldn't find 'english' folder containing ds2 fmgs.");
                Console.WriteLine("Please place your 'english' folder in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("'english' folder found!");

            if (Directory.Exists("english_UP")) {
                Console.WriteLine("Deleting old /english_UP folder...");
                Directory.Delete("english_UP", true);
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking pure text from english...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => { },
                (int menuEntries, int maxMenuEntries) => { }
            );
            DS2Tools.UnpackPureText("english", "english_UP", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking pure text into english_UP");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupRawRepack() {
            Console.Clear();

            if (!Directory.Exists("english_UP")) {
                Console.WriteLine("Couldn't find raw text library.");
                Console.WriteLine("Please place your 'english_UP' folder in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("'english_UP' folder found!");

            if (Directory.Exists("english")) {
                Console.WriteLine("Deleting old /english folder...");
                Directory.Delete("english", true);
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Repacking raw text into english...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => { },
                (int menuEntries, int maxMenuEntries) => { }
            );
            DS2Tools.RepackRawText("english_UP", "english", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking raw text into 'english'");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupPureRepack() {
            Console.Clear();

            if (!Directory.Exists("english_UP")) {
                Console.WriteLine("Couldn't find pure text library.");
                Console.WriteLine("Please place your 'english_UP' folder in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("'english_UP' folder found!");

            if (Directory.Exists("english")) {
                Console.WriteLine("Deleting old /english folder...");
                Directory.Delete("english", true);
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Repacking pure text into english...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => { },
                (int menuEntries, int maxMenuEntries) => { }
            );
            DS2Tools.RepackPureText("english_UP", "english", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking pure text into 'english'");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupDS2TUnpack() {
            Console.Clear();

            if (!Directory.Exists("english")) {
                Console.WriteLine("Couldn't find 'english' folder containing ds2 fmgs.");
                Console.WriteLine("Please place your 'english' folder in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("'english' folder found!");

            if (File.Exists("english.DS2Text")) {
                Console.WriteLine("Deleting old english.DS2Text file...");
                File.Delete("english.DS2Text");
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking DS2T from english...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => { },
                (int menuEntries, int maxMenuEntries) => { }
            );
            DS2Tools.UnpackDS2Text("english", "english.DS2Text", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking DS2T into english.DS2Text");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupDS2TRepack() {
            Console.Clear();

            if (!File.Exists("english.DS2Text")) {
                Console.WriteLine("Couldn't find DS2Text file.");
                Console.WriteLine("Please place your english.DS2Text file in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("english.DS2Text found!");

            if (Directory.Exists("english")) {
                Console.WriteLine("Deleting old /english folder...");
                Directory.Delete("english", true);
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.TextObserver observer = new CommonUtils.TextObserver(
                (int maxItemEntries) => {
                    lb = new ConsoleLoadingBar("Repacking DS2T content into english...", 0, maxItemEntries);
                },
                (int itemEntries, int maxItemEntries) => {
                    lb.Update(itemEntries);
                },
                (int maxMenuEntries) => { },
                (int menuEntries, int maxMenuEntries) => { }
            );
            DS2Tools.RepackDS2Text("english.DS2Text", "english", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking DS2Text content into /english");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupFontUnpack() {
            Console.Clear();

            if (!File.Exists("FeFont_Small.fontbnd.dcx") || !File.Exists("FeFont_Big.fontbnd.dcx")) {
                Console.WriteLine("Couldn't find FeFont files.");
                Console.WriteLine("Please place both FeFont_Small.fontbnd.dcx and FeFont_Big.fontbnd.dcx in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("FeFont_Small.fontbnd.dcx and FeFont_Big.fontbnd.dcx found!");

            if (Directory.Exists("FeFont_Small")) {
                Console.WriteLine("Deleting old /FeFont_Small...");
                Directory.Delete("FeFont_Small", true);
            }
            if (Directory.Exists("FeFont_Big")) {
                Console.WriteLine("Deleting old /FeFont_Big...");
                Directory.Delete("FeFont_Big", true);
            }
            ConsoleLoadingBar lb = null;
            CommonUtils.FontObserver2 observer = new CommonUtils.FontObserver2(
                (int maxSmallFEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking font textures from FeFont_Small.fontbnd.dcx...", 0, maxSmallFEntries);
                },
                (int smallFEntries, int maxSmallFEntries) => {
                    lb.Update(smallFEntries);
                },
                (int maxBigFEntries) => {
                    lb = new ConsoleLoadingBar("Unpacking font textures from FeFont_Big.fontbnd.dcx...", 0, maxBigFEntries);
                },
                (int bigFEntries, int maxBigFEntries) => {
                    lb.Update(bigFEntries);
                }
            );
            DS2Tools.UnpackFont("FeFont_Small.fontbnd.dcx", "FeFont_Big.fontbnd.dcx", "FeFont_Small", "FeFont_Big", observer);

            Console.Clear();
            Console.WriteLine("Finished unpacking fonts into /FeFont_Small and /FeFont_Big");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }

        public static void SetupFontRepack() {
            Console.Clear();

            if (!Directory.Exists("FeFont_Small") || !Directory.Exists("FeFont_Big")) {
                Console.WriteLine("Couldn't find FeFont libraries.");
                Console.WriteLine("Please place both your FeFont_Small and FeFont_big folders in the same folder as this tool.");
                Console.WriteLine("< Press any key to continue >");
                Console.ReadKey();
            }
            Console.WriteLine("FeFont_Small and FeFont_big folders found!");

            if (File.Exists("FeFont_Small.fontbnd.dcx")) {
                Console.WriteLine("Deleting old FeFont_Small.fontbnd.dcx...");
                File.Delete("FeFont_Small.fontbnd.dcx");
            }

            if (File.Exists("FeFont_Big.fontbnd.dcx")) {
                Console.WriteLine("Deleting old FeFont_Big.fontbnd.dcx...");
                File.Delete("FeFont_Big.fontbnd.dcx");
            }

            ConsoleLoadingBar lb = null;
            CommonUtils.FontObserver2 observer = new CommonUtils.FontObserver2(
                (int maxSmallFEntries) => {
                    lb = new ConsoleLoadingBar("Repacking font textures into DSFont24.tpf.dcx...", 0, maxSmallFEntries);
                },
                (int smallFEntries, int maxSmallFEntries) => {
                    lb.Update(smallFEntries);
                },
                (int maxBigFEntries) => {
                    lb = new ConsoleLoadingBar("Repacking font textures into TalkFont24.tpf.dcx...", 0, maxBigFEntries);
                },
                (int bigFEntries, int maxBigFEntries) => {
                    lb.Update(bigFEntries);
                }
            );
            DS2Tools.RepackFont("FeFont_Small", "FeFont_Big", "FeFont_Small.fontbnd.dcx", "FeFont_Big.fontbnd.dcx", observer);

            Console.Clear();
            Console.WriteLine("Finished repacking fonts into FeFont_Small.fontbnd.dcx and FeFont_Big.fontbnd.dcx");
            Console.WriteLine("< Press any key to continue >");
            Console.ReadKey();
        }
    }
}
