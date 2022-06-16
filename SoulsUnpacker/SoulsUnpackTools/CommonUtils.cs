using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SoulsUnpackTools {
    public static class CommonUtils {
        public class TextObserver {
            public Action<int> onItemStart;
            public Action<int, int> onItemProgress;
            public Action<int> onMenuStart;
            public Action<int, int> onMenuProgress;

            public TextObserver(
                Action<int> onItemStart,
                Action<int, int> onItemProgress,
                Action<int> onMenuStart,
                Action<int, int> onMenuProgress) {
                this.onItemStart = onItemStart;
                this.onItemProgress = onItemProgress;
                this.onMenuStart = onMenuStart;
                this.onMenuProgress = onMenuProgress;
            }
        }

        public class FontObserver {
            public Action<int> onDSFontStart;
            public Action<int, int> onDSFontProgress;
            public Action<int> onTalkFontStart;
            public Action<int, int> onTalkFontProgress;

            public FontObserver(
                Action<int> onDSFontStart,
                Action<int, int> onDSFontProgress,
                Action<int> onTalkFontStart,
                Action<int, int> onTalkFontProgress) {
                this.onDSFontStart = onDSFontStart;
                this.onDSFontProgress = onDSFontProgress;
                this.onTalkFontStart = onTalkFontStart;
                this.onTalkFontProgress = onTalkFontProgress;
            }
        }

        public static string FindEntryIn(string infoFile, string entryName) {
            using (StreamReader reader = new StreamReader(infoFile)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string id = line.Split('\t')[0];
                    string name = line.Split('\t')[1];
                    if (id == entryName) {
                        return name
                            .Replace('/', '_')
                            .Replace('\\', '_')
                            .Replace(":", "")
                            .Replace(".", "")
                            .Replace("?", "")
                            .Replace("!", "")
                            .Replace("\"", "")
                            .Replace("<", "")
                            .Replace(">", "")
                            .Replace("|", "");
                    }
                }
            }
            return "";
        }
    }
}
