using System;
using System.Collections.Generic;
using System.IO;
using SoulsFormats;
using System.Linq;

namespace SoulsUnpackTools {
    public static class DSRTools {
        public static void UnpackRawText(string itemSource, string menuSource, string itemTarget, string menuTarget, Action<string, int, int> onProgress) {
            Directory.CreateDirectory(itemTarget);
            Directory.CreateDirectory(menuTarget);

            BND3 itemBnd = BND3.Read(itemSource);
            BND3 menuBnd = BND3.Read(menuSource);

            int maxEntries = 0;
            int currentEntries = 0;
            List<FmgHandler> itemFmgs = new List<FmgHandler>();
            List<FmgHandler> menuFmgs = new List<FmgHandler>();
            foreach (BinderFile file in itemBnd.Files) {
                FmgHandler handler = new FmgHandler(FMG.Read(file.Bytes), file.Name, file.ID);
                maxEntries += handler.fmgData.Entries.Count;
                itemFmgs.Add(handler);
            }
            foreach (BinderFile file in menuBnd.Files) {
                FmgHandler handler = new FmgHandler(FMG.Read(file.Bytes), file.Name, file.ID);
                maxEntries += handler.fmgData.Entries.Count;
                menuFmgs.Add(handler);
            }

            onProgress("Unpacking item...", currentEntries, maxEntries);

            foreach (FmgHandler handler in itemFmgs) {
                string dirName = handler.id + "€" + handler.name.Split('\\').Last().Split('.')[0];
                Directory.CreateDirectory(itemTarget + "/" + dirName);
                foreach (FMG.Entry entry in handler.fmgData.Entries) {
                    StreamWriter sw = new StreamWriter(itemTarget + "/" + dirName + "/" + entry.ID + ".txt");
                    sw.Write(entry.Text);
                    sw.Close();
                    currentEntries++;
                    onProgress("Unpacking item...", currentEntries, maxEntries);
                }
            }

            foreach (FmgHandler handler in menuFmgs) {
                string dirName = handler.id + "€" + handler.name.Split('\\').Last().Split('.')[0];
                Directory.CreateDirectory(menuTarget + "/" + dirName);
                foreach (FMG.Entry entry in handler.fmgData.Entries) {
                    StreamWriter sw = new StreamWriter(menuTarget + "/" + dirName + "/" + entry.ID + ".txt");
                    sw.Write(entry.Text);
                    sw.Close();
                    currentEntries++;
                    onProgress("Unpacking menu...", currentEntries, maxEntries);
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
    }
}
