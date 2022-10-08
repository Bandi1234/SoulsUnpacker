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
        private int percentage1 = 0;
        private int percentage2 = 0;
        private int percentage3 = 0;

        public ConsoleLoadingBar(string title, int currentProgress, int totalProgress) { 
            this.title = title;
            this.currentProgress = currentProgress;
            this.totalProgress = totalProgress;
            this.consoleWidth = Console.WindowWidth - 2;
            this.maxSigns = consoleWidth - 13;

            FirstDraw();
        }

        public void FirstDraw() {
            Console.Clear();
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine(title);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            string firstDraw = " [";
            for (int i = 0; i < maxSigns; i++) {
                firstDraw += ".";
            }
            firstDraw += "] { 00.0% } ";
            Console.Write(firstDraw);
        }

        public void Update(int newProgress) {
            int nextSigns = (int)Math.Round((float)newProgress / (float)totalProgress * maxSigns);
            if (nextSigns > currentSigns) {
                if (currentSigns > 0) {
                    Console.SetCursorPosition(1 + currentSigns, Console.CursorTop);
                    Console.Write("=");
                }
                Console.SetCursorPosition(2 + currentSigns, Console.CursorTop);
                for (int i = 0; i < nextSigns - currentSigns - 1; i++) {
                    Console.Write("=");
                }
                Console.Write(">");
            }

            currentSigns = nextSigns;

            float newNum = (float)Math.Round((float)newProgress / (float)totalProgress, 3) * 100;
            if (newNum > 99.9f) {
                newNum = 99.9f;
            }
            int newPercentage1 = int.Parse(newNum.ToString("00.0")[0] + "");
            int newPercentage2 = int.Parse(newNum.ToString("00.0")[1] + "");
            int newPercentage3 = int.Parse(newNum.ToString("00.0")[3] + "");
            if (newPercentage1 != percentage1) {
                Console.SetCursorPosition(6 + maxSigns, Console.CursorTop);
                Console.Write(newPercentage1);
                percentage1 = newPercentage1;
            }
            if (newPercentage2 != percentage2) {
                Console.SetCursorPosition(7 + maxSigns, Console.CursorTop);
                Console.Write(newPercentage2);
                percentage2 = newPercentage2;
            }
            if (newPercentage3 != percentage3) {
                Console.SetCursorPosition(9 + maxSigns, Console.CursorTop);
                Console.Write(newPercentage3);
                percentage3 = newPercentage3;
            }
            

            currentProgress = newProgress;
        } 
    }
}
