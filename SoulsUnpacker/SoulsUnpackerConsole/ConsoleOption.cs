using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsUnpackerConsole {
    public class ConsoleOption {
        public string name = "";
        public Action action = () => {};
        public bool isBack = false;

        public ConsoleOption(string name, Action action, bool isBack) { 
            this.name = name;
            this.action = action;
            this.isBack = isBack;
        }
    }
}
