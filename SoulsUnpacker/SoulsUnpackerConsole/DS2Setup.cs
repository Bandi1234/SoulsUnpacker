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
    }
}
