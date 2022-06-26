using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsUnpackerConsole {
    public class ConsoleMenu {
        private string[] text = { };
        public List<ConsoleOption> options = new List<ConsoleOption>();

        public ConsoleMenu(string[] text) { 
            this.text = text;
        }

        public void Show() {
            while (true) {
                Console.Clear();
                
                if (text.Length > 0) {
                    foreach (string line in text) {
                        Console.WriteLine(line);
                    }
                    Console.WriteLine();
                }

                for (int i = 0; i < options.Count; i++) {
                    if (i < options.Count - 1) {
                        Console.WriteLine("(" + (i + 1) + ") " + options[i].name);
                    } else {
                        Console.WriteLine("(0) " + options[i].name);
                    }
                }

                int choice = GetKeyNum(Console.ReadKey().Key);
                if (choice == -1 || choice >= options.Count) {
                    continue;
                }

                if (choice == 0) {
                    options.Last().action();
                    if (options.Last().isBack) {
                        break;
                    }
                } else {
                    options[choice - 1].action();
                    if (options[choice - 1].isBack) {
                        break;
                    }
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
