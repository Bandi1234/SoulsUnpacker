using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SoulsFormats;

namespace SoulsUnpackTools {
    public static class DS2Tools {
        public static void UnpackRawText(string sourceFolder, string targetFolder, CommonUtils.TextObserver observer) {
            string[] baseFmgs = Directory.GetFiles(sourceFolder, "*.fmg", SearchOption.TopDirectoryOnly);
            string[] talkFmgs = Directory.GetFiles(Path.Combine(sourceFolder, "talk"), "*.fmg", SearchOption.TopDirectoryOnly);
            string[] bmFmgs = Directory.GetFiles(Path.Combine(sourceFolder, "bloodmes"), "*.fmg", SearchOption.TopDirectoryOnly);
            Directory.CreateDirectory(targetFolder);
            Directory.CreateDirectory(Path.Combine(targetFolder, "talk"));
            Directory.CreateDirectory(Path.Combine(targetFolder, "bloodmes"));
            List<DS2FMGHandler> handlers = new List<DS2FMGHandler>();

            int maxEntries = 0;
            int entries = 0;

            foreach (string file in baseFmgs) {
                FMG fmg = FMG.Read(file);
                maxEntries += fmg.Entries.Count;
                string name = file.Split('\\').Last().Split('.')[0];
                string target = Path.Combine(targetFolder, name);
                handlers.Add(new DS2FMGHandler(file, fmg, target));
            }
            foreach (string file in talkFmgs) {
                FMG fmg = FMG.Read(file);
                maxEntries += fmg.Entries.Count;
                string name = file.Split('\\').Last().Split('.')[0];
                string target = Path.Combine(targetFolder, "talk", name);
                handlers.Add(new DS2FMGHandler(file, fmg, target));
            }
            foreach (string file in bmFmgs) {
                FMG fmg = FMG.Read(file);
                maxEntries += fmg.Entries.Count;
                string name = file.Split('\\').Last().Split('.')[0];
                string target = Path.Combine(targetFolder, "bloodmes", name);
                handlers.Add(new DS2FMGHandler(file, fmg, target));
            }

            observer.onItemStart(maxEntries);

            foreach (DS2FMGHandler handler in handlers) { 
                Directory.CreateDirectory(handler.textDir);
                foreach (FMG.Entry entry in handler.fmgData.Entries) {
                    string target = Path.Combine(handler.textDir, entry.ID + ".txt");
                    string text = entry.Text;
                    StreamWriter sw = new StreamWriter(target);
                    sw.Write(text);
                    sw.Close();
                    entries++;
                    observer.onItemProgress(entries, maxEntries);
                }
            }
        }

        public static void RepackRawText(string sourceFolder, string targetFolder, CommonUtils.TextObserver observer) { 
            string[] baseDirectories = Directory.GetDirectories(sourceFolder, "*", SearchOption.TopDirectoryOnly);
            string[] talkDirectores = Directory.GetDirectories(Path.Combine(sourceFolder, "talk"), "*", SearchOption.TopDirectoryOnly);
            string[] bmDirectores = Directory.GetDirectories(Path.Combine(sourceFolder, "bloodmes"), "*", SearchOption.TopDirectoryOnly);
            Directory.CreateDirectory(targetFolder);
            Directory.CreateDirectory(Path.Combine(targetFolder, "talk"));
            Directory.CreateDirectory(Path.Combine(targetFolder, "bloodmes"));
            List<DS2FMGHandler> handlers = new List<DS2FMGHandler>();

            int maxEntries = 0;
            int entries = 0;

            foreach (string dir in baseDirectories) {
                maxEntries += Directory.GetFiles(dir, "*.txt", SearchOption.TopDirectoryOnly).Length;
                string name = dir.Split('\\').Last();
                string fmgFile = Path.Combine(targetFolder, name + ".fmg");
                handlers.Add(new DS2FMGHandler(fmgFile, new FMG(), dir));
            }
            foreach (string dir in talkDirectores) {
                maxEntries += Directory.GetFiles(dir, "*.txt", SearchOption.TopDirectoryOnly).Length;
                string name = dir.Split('\\').Last();
                string fmgFile = Path.Combine(targetFolder, "talk", name + ".fmg");
                handlers.Add(new DS2FMGHandler(fmgFile, new FMG(), dir));
            }
            foreach (string dir in bmDirectores) {
                maxEntries += Directory.GetFiles(dir, "*.txt", SearchOption.TopDirectoryOnly).Length;
                string name = dir.Split('\\').Last();
                string fmgFile = Path.Combine(targetFolder, "bloodmes", name + ".fmg");
                handlers.Add(new DS2FMGHandler(fmgFile, new FMG(), dir));
            }

            observer.onItemStart(maxEntries);

            foreach (DS2FMGHandler handler in handlers) {
                handler.fmgData.Version = FMG.FMGVersion.DarkSouls1;
                handler.fmgData.Compression = DCX.Type.None;
                foreach (string file in Directory.GetFiles(handler.textDir, "*.txt", SearchOption.TopDirectoryOnly)) { 
                    StreamReader sr = new StreamReader(file);
                    string text = sr.ReadToEnd();
                    sr.Close();
                    int id = int.Parse(file.Split('\\').Last().Split('.')[0]);
                    FMG.Entry entry = new FMG.Entry(id, text);
                    handler.fmgData.Entries.Add(entry);
                    entries++;
                    observer.onItemProgress(entries, maxEntries);
                }
                File.Create(handler.fmgFile).Close();
                File.WriteAllBytes(handler.fmgFile, handler.fmgData.Write());
            }
        }

        public static void UnpackDS2Text(string sourceFolder, string targetFile, CommonUtils.TextObserver observer) {
            string[] baseFmgs = Directory.GetFiles(sourceFolder, "*.fmg", SearchOption.TopDirectoryOnly);
            string[] talkFmgs = Directory.GetFiles(Path.Combine(sourceFolder, "talk"), "*.fmg", SearchOption.TopDirectoryOnly);
            string[] bmFmgs = Directory.GetFiles(Path.Combine(sourceFolder, "bloodmes"), "*.fmg", SearchOption.TopDirectoryOnly);
            StreamWriter sw = new StreamWriter(targetFile);
            List<PureDS2FMGHandler> handlers = new List<PureDS2FMGHandler>();

            int maxEntries = 0;
            int entries = 0;

            foreach (string file in baseFmgs) {
                FMG fmg = FMG.Read(file);
                maxEntries += fmg.Entries.Count;
                string name = file.Split('\\').Last().Split('.')[0];
                handlers.Add(new PureDS2FMGHandler(file, fmg, "", name));
            }
            foreach (string file in talkFmgs) {
                FMG fmg = FMG.Read(file);
                maxEntries += fmg.Entries.Count;
                string name = file.Split('\\').Last().Split('.')[0];
                handlers.Add(new PureDS2FMGHandler(file, fmg, "talk", name));
            }
            foreach (string file in bmFmgs) {
                FMG fmg = FMG.Read(file);
                maxEntries += fmg.Entries.Count;
                string name = file.Split('\\').Last().Split('.')[0];
                handlers.Add(new PureDS2FMGHandler(file, fmg, "bloodmes", name));
            }

            observer.onItemStart(maxEntries);

            foreach (PureDS2FMGHandler handler in handlers) {
                if (handler.fmgDir == "") {
                    sw.WriteLine(handler.fmgName);
                } else {
                    sw.WriteLine(handler.fmgDir + "\t" + handler.fmgName);
                }
                List<int> emptyIds = new List<int>();
                List<int> spaceIds = new List<int>();

                foreach (FMG.Entry entry in handler.fmgData.Entries) {
                    if (entry.Text == "" || entry.Text == null) {
                        emptyIds.Add(entry.ID);
                        entries++;
                        observer.onItemProgress(entries, maxEntries);
                        continue;
                    }
                    if (entry.Text == " ") {
                        spaceIds.Add(entry.ID);
                        entries++;
                        observer.onItemProgress(entries, maxEntries);
                        continue;
                    }
                    sw.WriteLine(entry.ID);
                    sw.WriteLine(entry.Text);
                    sw.WriteLine("€");
                    entries++;
                    observer.onItemProgress(entries, maxEntries);
                }
                sw.WriteLine("€€€");
                string emptyLine = "";
                for (int i = 0; i < emptyIds.Count; i++) {
                    emptyLine += emptyIds[i];
                    if (i < emptyIds.Count - 1) {
                        emptyLine += "\t";
                    }
                }
                sw.WriteLine(emptyLine);
                string spaceLine = "";
                for (int i = 0; i < spaceIds.Count; i++) {
                    spaceLine += spaceIds[i];
                    if (i < spaceIds.Count - 1) {
                        spaceLine += "\t";
                    }
                }
                sw.WriteLine(spaceLine);
            }
            sw.Write("€€€€€");
            sw.Close();
        }

        public static void RepackDS2Text(string sourceFile, string targetFolder, CommonUtils.TextObserver observer) {
            Directory.CreateDirectory(targetFolder);
            Directory.CreateDirectory(Path.Combine(targetFolder, "talk"));
            Directory.CreateDirectory(Path.Combine(targetFolder, "bloodmes"));
            StreamReader sr = new StreamReader(sourceFile);
            int maxEntries = 0;
            int entries = 0;
            while (!sr.EndOfStream) {
                sr.ReadLine();
                maxEntries++;
            }

            observer.onItemStart(maxEntries);

            sr.Close();
            sr = new StreamReader(sourceFile);

            while (true) {
                string fmgLine = sr.ReadLine();
                entries++;
                observer.onItemProgress(entries, maxEntries);
                if (fmgLine == "€€€€€") {
                    break;
                }
                string[] nameParts = fmgLine.Split('\t');
                string fmgName = "";
                string fmgDir = "";
                string fmgFile = "";
                if (nameParts.Length == 1) {
                    fmgName = nameParts[0];
                    fmgFile = Path.Combine(targetFolder, fmgName + ".fmg");
                } else {
                    fmgDir = nameParts[0];
                    fmgName = nameParts[1];
                    fmgFile = Path.Combine(targetFolder, fmgDir, fmgName + ".fmg");
                }
                PureDS2FMGHandler handler = new PureDS2FMGHandler(fmgFile, new FMG(), fmgDir, fmgName);
                handler.fmgData.Version = FMG.FMGVersion.DarkSouls1;
                handler.fmgData.Compression = DCX.Type.None;
                while (true) {
                    string entryIdLine = sr.ReadLine();
                    entries++;
                    observer.onItemProgress(entries, maxEntries);
                    if (entryIdLine == "€€€") {
                        break;
                    }
                    List<string> entryContentLines = new List<string>();
                    while (true) {
                        string nextLine = sr.ReadLine();
                        entries++;
                        observer.onItemProgress(entries, maxEntries);
                        if (nextLine == "€") {
                            break;
                        }
                        entryContentLines.Add(nextLine);
                    }
                    string entryContent = "";
                    for (int i = 0; i < entryContentLines.Count; i++) {
                        entryContent += entryContentLines[i];
                        if (i < entryContentLines.Count - 1) {
                            entryContent += "\n";
                        }
                    }
                    FMG.Entry entry = new FMG.Entry(int.Parse(entryIdLine), entryContent);
                    handler.fmgData.Entries.Add(entry);
                }
                string emptyLine = sr.ReadLine();
                entries++;
                observer.onItemProgress(entries, maxEntries);
                if (emptyLine != "") {
                    string[] empties = emptyLine.Split('\t');
                    foreach (string empty in empties) {
                        FMG.Entry entry = new FMG.Entry(int.Parse(empty), "");
                        handler.fmgData.Entries.Add(entry);
                    }
                }
                string spacesLine = sr.ReadLine();
                entries++;
                observer.onItemProgress(entries, maxEntries);
                if (spacesLine != "") {
                    string[] spaces = spacesLine.Split('\t');
                    foreach (string space in spaces) {
                        FMG.Entry entry = new FMG.Entry(int.Parse(space), " ");
                        handler.fmgData.Entries.Add(entry);
                    }
                }
                File.Create(handler.fmgFile).Close();
                File.WriteAllBytes(handler.fmgFile, handler.fmgData.Write());
            }
            sr.Close();
        }

        private class PureDS2FMGHandler {
            public string fmgFile;
            public FMG fmgData;
            public string fmgDir;
            public string fmgName;

            public PureDS2FMGHandler(string fmgFile, FMG fmgData, string fmgDir, string fmgName) { 
                this.fmgFile = fmgFile;
                this.fmgData = fmgData;
                this.fmgDir = fmgDir;
                this.fmgName = fmgName;
            }

        }

        private class DS2FMGHandler {
            public string fmgFile;
            public FMG fmgData;
            public string textDir;

            public DS2FMGHandler(string fmgFile, FMG fmgData, string textDir) {
                this.fmgFile = fmgFile;
                this.fmgData = fmgData;
                this.textDir = textDir;
            }
        }
    }
}
