using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SoulsFormats;

namespace SoulsUnpackTools {
    public static class ERTools {
        public static void UnpackRawText(string itemSource, string menuSource, string itemTarget, string menuTarget, CommonUtils.TextObserver observer) {
            BND4 item = BND4.Read(itemSource);
            Directory.CreateDirectory(itemTarget);

            BND4 menu = BND4.Read(menuSource);
            Directory.CreateDirectory(menuTarget);

            int maxItemEntries = 0;
            int itemEntries = 0;
            int maxMenuEntries = 0;
            int menuEntries = 0;
            foreach (BinderFile file in item.Files) {
                maxItemEntries += FMG.Read(file.Bytes).Entries.Count;
            }
            foreach (BinderFile file in menu.Files) {
                maxMenuEntries += FMG.Read(file.Bytes).Entries.Count;
            }

            observer.onItemStart(maxItemEntries);

            foreach (BinderFile file in item.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                Directory.CreateDirectory(Path.Combine(itemTarget, file.ID + "€" + fmgName));
                FMG fmg = FMG.Read(file.Bytes);
                foreach (FMG.Entry entry in fmg.Entries) {
                    string filePath = Path.Combine(itemTarget, file.ID + "€" + fmgName, entry.ID + ".txt");
                    File.Create(filePath).Close();
                    StreamWriter sw = new StreamWriter(filePath);
                    sw.Write(entry.Text);
                    sw.Close();
                    itemEntries++;
                    observer.onItemProgress(itemEntries, maxItemEntries);
                }
            }

            observer.onMenuStart(maxMenuEntries);

            foreach (BinderFile file in menu.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                Directory.CreateDirectory(Path.Combine(menuTarget, file.ID + "€" + fmgName));
                FMG fmg = FMG.Read(file.Bytes);
                foreach (FMG.Entry entry in fmg.Entries) {
                    string filePath = Path.Combine(menuTarget, file.ID + "€" + fmgName, entry.ID + ".txt");
                    File.Create(filePath).Close();
                    StreamWriter sw = new StreamWriter(filePath);
                    sw.Write(entry.Text);
                    sw.Close();
                    menuEntries++;
                    observer.onMenuProgress(menuEntries, maxMenuEntries);
                }
            }
        }

        //TODO raw repack

        public static void UnpackPureText(string itemSource, string menuSource, string itemTarget, string menuTarget, CommonUtils.TextObserver observer) {
            BND4 item = BND4.Read(itemSource);
            Directory.CreateDirectory(itemTarget);

            int maxItemEntries = 0;
            int itemEntries = 0;
            foreach (BinderFile file in item.Files) {
                maxItemEntries += FMG.Read(file.Bytes).Entries.Count;
            }

            StreamWriter compIdWriter = new StreamWriter(Path.Combine(itemTarget, "compressionIds.ERInfo"));
            observer.onItemStart(maxItemEntries);

            foreach (BinderFile file in item.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                string engName = fmgName;
                string dirName = Path.Combine(itemTarget, engName);
                compIdWriter.WriteLine(engName + "\t" + file.ID);
                Directory.CreateDirectory(dirName);
                StreamWriter emptyWriter = new StreamWriter(Path.Combine(dirName, "empty.ERInfo"));
                StreamWriter spaceWriter = new StreamWriter(Path.Combine(dirName, "spaces.ERInfo"));
                FMG fmg = FMG.Read(file.Bytes);
                foreach (FMG.Entry entry in fmg.Entries) {
                    if (entry.Text == "" || entry.Text == null) {
                        emptyWriter.WriteLine(entry.ID);
                        itemEntries++;
                        observer.onItemProgress(itemEntries, maxItemEntries);
                        continue;
                    }
                    if (entry.Text == " ") {
                        spaceWriter.WriteLine(entry.ID);
                        itemEntries++;
                        observer.onItemProgress(itemEntries, maxItemEntries);
                        continue;
                    }
                    string name = entry.ID + ".txt";
                    string target = Path.Combine(dirName, name);
                    switch (engName) {
                        //TODO filters
                    }
                    File.Create(target).Close();
                    StreamWriter sw = new StreamWriter(target);
                    sw.Write(entry.Text);
                    sw.Close();
                    itemEntries++;
                    observer.onItemProgress(itemEntries, maxItemEntries);
                }
                emptyWriter.Close();
                spaceWriter.Close();
            }
            compIdWriter.Close();

            BND4 menu = BND4.Read(menuSource);
            Directory.CreateDirectory(menuTarget);

            int maxMenuEntries = 0;
            int menuEntries = 0;
            foreach (BinderFile file in menu.Files) {
                maxMenuEntries += FMG.Read(file.Bytes).Entries.Count;
            }

            compIdWriter = new StreamWriter(Path.Combine(menuTarget, "compressionIds.ERInfo"));
            observer.onMenuStart(maxMenuEntries);

            foreach (BinderFile file in menu.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                string engName = fmgName;
                string dirName = Path.Combine(menuTarget, engName);
                compIdWriter.WriteLine(engName + "\t" + file.ID);
                Directory.CreateDirectory(dirName);
                StreamWriter emptyWriter = new StreamWriter(Path.Combine(dirName, "empty.ERInfo"));
                StreamWriter spaceWriter = new StreamWriter(Path.Combine(dirName, "spaces.ERInfo"));
                FMG fmg = FMG.Read(file.Bytes);
                foreach (FMG.Entry entry in fmg.Entries) {
                    if (entry.Text == "" || entry.Text == null) {
                        emptyWriter.WriteLine(entry.ID);
                        menuEntries++;
                        observer.onMenuProgress(menuEntries, maxMenuEntries);
                        continue;
                    }
                    if (entry.Text == " ") {
                        spaceWriter.WriteLine(entry.ID);
                        menuEntries++;
                        observer.onMenuProgress(menuEntries, maxMenuEntries);
                        continue;
                    }
                    string name = entry.ID + ".txt";
                    string target = Path.Combine(dirName, name);
                    switch (engName) {
                        //TODO filters
                    }
                    File.Create(target).Close();
                    StreamWriter sw = new StreamWriter(target);
                    sw.Write(entry.Text);
                    sw.Close();
                    menuEntries++;
                    observer.onMenuProgress(menuEntries, maxMenuEntries);
                }
                emptyWriter.Close();
                spaceWriter.Close();
            }
            compIdWriter.Close();
        }

        //TODO pure repack

        public static void UnpackERText(string itemSource, string menuSource, string itemTarget, string menuTarget, CommonUtils.TextObserver observer) {
            StreamWriter itemWriter = new StreamWriter(itemTarget);
            BND4 item = BND4.Read(itemSource);

            int maxItemEntries = 0;
            int itemEntries = 0;
            foreach (BinderFile file in item.Files) {
                maxItemEntries += FMG.Read(file.Bytes).Entries.Count;
            }

            observer.onItemStart(maxItemEntries);

            foreach (BinderFile file in item.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                string engName = fmgName;
                itemWriter.WriteLine(engName + "\t" + file.ID);
                List<int> emptyIds = new List<int>();
                List<int> spaceIds = new List<int>();
                FMG fmg = FMG.Read(file.Bytes);

                foreach (FMG.Entry entry in fmg.Entries) {
                    if (entry.Text == "" || entry.Text == null) {
                        emptyIds.Add(entry.ID);
                        itemEntries++;
                        observer.onItemProgress(itemEntries, maxItemEntries);
                        continue;
                    }
                    if (entry.Text == " ") {
                        spaceIds.Add(entry.ID);
                        itemEntries++;
                        observer.onItemProgress(itemEntries, maxItemEntries);
                        continue;
                    }
                    itemWriter.WriteLine(entry.ID);
                    itemWriter.WriteLine(entry.Text);
                    itemWriter.WriteLine("€");
                    itemEntries++;
                    observer.onItemProgress(itemEntries, maxItemEntries);
                }
                itemWriter.WriteLine("€€€");
                string emptyLine = "";
                for (int i = 0; i < emptyIds.Count; i++) {
                    emptyLine += emptyIds[i];
                    if (i < emptyIds.Count - 1) {
                        emptyLine += "\t";
                    }
                }
                itemWriter.WriteLine(emptyLine);
                string spaceLine = "";
                for (int i = 0; i < spaceIds.Count; i++) {
                    spaceLine += spaceIds[i];
                    if (i < spaceIds.Count - 1) {
                        spaceLine += "\t";
                    }
                }
                itemWriter.WriteLine(spaceLine);
            }
            itemWriter.Write("€€€€€");
            itemWriter.Close();

            StreamWriter menuWriter = new StreamWriter(menuTarget);
            BND4 menu = BND4.Read(menuSource);

            int maxMenuEntries = 0;
            int menuEntries = 0;
            foreach (BinderFile file in menu.Files) {
                maxMenuEntries += FMG.Read(file.Bytes).Entries.Count;
            }

            observer.onMenuStart(maxMenuEntries);

            foreach (BinderFile file in menu.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                string engName = fmgName;
                menuWriter.WriteLine(engName + "\t" + file.ID);
                List<int> emptyIds = new List<int>();
                List<int> spaceIds = new List<int>();
                FMG fmg = FMG.Read(file.Bytes);

                foreach (FMG.Entry entry in fmg.Entries) {
                    if (entry.Text == "" || entry.Text == null) {
                        emptyIds.Add(entry.ID);
                        menuEntries++;
                        observer.onMenuProgress(menuEntries, maxMenuEntries);
                        continue;
                    }
                    if (entry.Text == " ") {
                        spaceIds.Add(entry.ID);
                        menuEntries++;
                        observer.onMenuProgress(menuEntries, maxMenuEntries);
                        continue;
                    }
                    menuWriter.WriteLine(entry.ID);
                    menuWriter.WriteLine(entry.Text);
                    menuWriter.WriteLine("€");
                    menuEntries++;
                    observer.onMenuProgress(menuEntries, maxMenuEntries);
                }
                menuWriter.WriteLine("€€€");
                string emptyLine = "";
                for (int i = 0; i < emptyIds.Count; i++) {
                    emptyLine += emptyIds[i];
                    if (i < emptyIds.Count - 1) {
                        emptyLine += "\t";
                    }
                }
                menuWriter.WriteLine(emptyLine);
                string spaceLine = "";
                for (int i = 0; i < spaceIds.Count; i++) {
                    spaceLine += spaceIds[i];
                    if (i < spaceIds.Count - 1) {
                        spaceLine += "\t";
                    }
                }
                menuWriter.WriteLine(spaceLine);
            }
            menuWriter.Write("€€€€€");
            menuWriter.Close();
        }

        //TODO DSXT repack
    }
}
