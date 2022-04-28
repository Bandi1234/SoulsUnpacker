using System;
using SoulsUnpackTools;

namespace SoulsUnpackerConsole {
    internal class Program {

        internal static string name = "Souls Unpacker";
        internal static string version = "0.0.1";
        internal static string author = "Zelenák András";
        internal static string[] description = {
            "A tool designed mainly for people looking into translating DS-Remastered.",
            "This tool is capable of unpacking text bounds into a library of raw text or a slightly modified library for better use.",
            "We call this modified library a pure DS text library, which contains no repetion of entries, no empty files, better categories for items, etc.",
            "Naturally repacking text into their original form is also possible from both a raw and a clean library of files.",
            "Apart from text, the tool also support font un- and repacking, which may be useful when dealing with special, language specific characters.",
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
                Console.WriteLine("Made by: " + author);
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
                        UnpackFontMenu();
                        break;
                    case 4:
                        RepackFontMenu();
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

        }

        static void RepackTextMenu() { 
            
        }

        static void UnpackFontMenu() { 
            
        }

        static void RepackFontMenu() { 
            
        }

        static void About() {
            while (true) {
                Console.Clear();
                Console.WriteLine(name + " " + version);
                Console.WriteLine("Made by: " + author);
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
