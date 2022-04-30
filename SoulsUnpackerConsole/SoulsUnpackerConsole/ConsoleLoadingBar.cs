using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsUnpackerConsole {
    public class ConsoleLoadingBar {
        private string title;
        private int currentProgress = 0;
        private int totalProgress = 0;
        private int currentSigns = 0;
        private int maxSigns = 0;

        private int consoleWidth = 0;
        public ConsoleLoadingBar(string title, int currentProgress, int totalProgress) { 
            this.title = title;
            this.currentProgress = currentProgress;
            this.totalProgress = totalProgress;
            this.consoleWidth = Console.WindowWidth - 2;
            this.maxSigns = consoleWidth - 4;

            FirstDraw();
        }

        public void FirstDraw() {
            Console.Clear();
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine(title);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write(" <");
            Console.SetCursorPosition(consoleWidth - 2, Console.CursorTop);
            Console.Write("> ");
        }

        public void Update(int newProgress) {
            int nextSigns = (int)Math.Round((float)newProgress / (float)totalProgress * maxSigns);
            if (nextSigns > currentSigns) {
                Console.SetCursorPosition(2 + currentSigns, Console.CursorTop);
                for (int i = 0; i < nextSigns - currentSigns; i++) {
                    Console.Write("=");
                }
            }

            currentSigns = nextSigns;
            currentProgress = newProgress;
        } 
    }
}
