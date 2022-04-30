using System;
using System.Collections.Generic;
using System.IO;
using SoulsFormats;
using System.Linq;

namespace SoulsUnpackTools {
    public static class DSRTools {
        public static void UnpackRawText(string itemSource, string menuSource, string itemTarget, string menuTarget, TextObserver observer) {
            Directory.CreateDirectory(itemTarget);
            Directory.CreateDirectory(menuTarget);

            BND3 itemBnd = BND3.Read(itemSource);
            BND3 menuBnd = BND3.Read(menuSource);

            int maxItemEntries = 0;
            int itemEntries = 0;
            int maxMenuEntries = 0;
            int menuEntries = 0;
            List<FmgHandler> itemFmgs = new List<FmgHandler>();
            List<FmgHandler> menuFmgs = new List<FmgHandler>();
            foreach (BinderFile file in itemBnd.Files) {
                FmgHandler handler = new FmgHandler(FMG.Read(file.Bytes), file.Name, file.ID);
                maxItemEntries += handler.fmgData.Entries.Count;
                itemFmgs.Add(handler);
            }
            foreach (BinderFile file in menuBnd.Files) {
                FmgHandler handler = new FmgHandler(FMG.Read(file.Bytes), file.Name, file.ID);
                maxMenuEntries += handler.fmgData.Entries.Count;
                menuFmgs.Add(handler);
            }

            observer.onItemStart(maxItemEntries);

            foreach (FmgHandler handler in itemFmgs) {
                string dirName = handler.id + "€" + handler.name.Split('\\').Last().Split('.')[0];
                Directory.CreateDirectory(itemTarget + "/" + dirName);
                foreach (FMG.Entry entry in handler.fmgData.Entries) {
                    StreamWriter sw = new StreamWriter(itemTarget + "/" + dirName + "/" + entry.ID + ".txt");
                    sw.Write(entry.Text);
                    sw.Close();
                    itemEntries++;
                    observer.onItemProgress(itemEntries, maxItemEntries);
                }
            }

            observer.onMenuStart(maxMenuEntries);

            foreach (FmgHandler handler in menuFmgs) {
                string dirName = handler.id + "€" + handler.name.Split('\\').Last().Split('.')[0];
                Directory.CreateDirectory(menuTarget + "/" + dirName);
                foreach (FMG.Entry entry in handler.fmgData.Entries) {
                    StreamWriter sw = new StreamWriter(menuTarget + "/" + dirName + "/" + entry.ID + ".txt");
                    sw.Write(entry.Text);
                    sw.Close();
                    menuEntries++;
                    observer.onMenuProgress(menuEntries, maxMenuEntries);
                }
            }
        }

        private class FmgHandler {
            public FMG fmgData;
            public string name;
            public int id;

            public FmgHandler(FMG fmgData, string name, int id) {
                this.id = id;
                this.name = name;
                this.fmgData = fmgData;
            }
        }

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
    }
}
