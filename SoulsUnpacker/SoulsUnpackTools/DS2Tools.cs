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

        public static void UnpackPureText(string sourceFolder, string targetFolder, CommonUtils.TextObserver observer) {
            string[] baseFmgs = Directory.GetFiles(sourceFolder, "*.fmg", SearchOption.TopDirectoryOnly);
            string[] talkFmgs = Directory.GetFiles(Path.Combine(sourceFolder, "talk"), "*.fmg", SearchOption.TopDirectoryOnly);
            string[] bmFmgs = Directory.GetFiles(Path.Combine(sourceFolder, "bloodmes"), "*.fmg", SearchOption.TopDirectoryOnly);
            Directory.CreateDirectory(targetFolder);
            Directory.CreateDirectory(Path.Combine(targetFolder, "base"));
            Directory.CreateDirectory(Path.Combine(targetFolder, "talk"));
            Directory.CreateDirectory(Path.Combine(targetFolder, "bloodmes"));
            List<DS2FMGHandler> handlers = new List<DS2FMGHandler>();

            int maxEntries = 0;
            int entries = 0;

            foreach (string file in baseFmgs) {
                FMG fmg = FMG.Read(file);
                maxEntries += fmg.Entries.Count;
                string name = file.Split('\\').Last().Split('.')[0];
                string target = Path.Combine(targetFolder, "base", name);
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
                StreamWriter emptyWriter = new StreamWriter(Path.Combine(handler.textDir, "empty.DS2Info"));
                StreamWriter spaceWriter = new StreamWriter(Path.Combine(handler.textDir, "spaces.DS2Info"));
                string path = "";
                foreach (FMG.Entry entry in handler.fmgData.Entries) {
                    if (entry.Text == "" || entry.Text == null) {
                        emptyWriter.WriteLine(entry.ID);
                        entries++;
                        observer.onItemProgress(entries, maxEntries);
                        continue;
                    }
                    if (entry.Text == " ") {
                        spaceWriter.WriteLine(entry.ID);
                        entries++;
                        observer.onItemProgress(entries, maxEntries);
                        continue;
                    }
                    string name = entry.ID + ".txt";
                    string target = Path.Combine(handler.textDir, name);
                    string text = entry.Text;
                    string fmgName = handler.textDir.Split('\\').Last();
                    switch (fmgName) {
                        case "itemname":
                        case "simpleexplanation":
                        case "detailedexplanation":
                            string iType = DS2Utils.GetItemBaseType(name);
                            string iSType = DS2Utils.GetItemSubType(name, iType);
                            string iName = DS2Utils.GetItemName(name, iType);
                            path = Path.Combine(handler.textDir, iType, iSType, iName);
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            target = Path.Combine(path, name);
                            break;
                        case "bonfirename":
                            string bName = DS2Utils.GetBonfireName(name);
                            path = Path.Combine(handler.textDir, bName);
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            target = Path.Combine(path, name);
                            break;
                        case "mapname":
                            string mName = DS2Utils.GetMapName(name);
                            path = Path.Combine(handler.textDir, mName);
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            target = Path.Combine(path, name);
                            break;
                        case "m10_02_00_00":
                        case "m10_04_00_00":
                        case "m10_10_00_00":
                        case "m10_14_00_00":
                        case "m10_15_00_00":
                        case "m10_16_00_00":
                        case "m10_17_00_00":
                        case "m10_18_00_00":
                        case "m10_19_00_00":
                        case "m10_23_00_00":
                        case "m10_25_00_00":
                        case "m10_27_00_00":
                        case "m10_29_00_00":
                        case "m10_31_00_00":
                        case "m10_32_00_00":
                        case "m10_33_00_00":
                        case "m10_34_00_00":
                        case "m20_10_00_00":
                        case "m20_11_00_00":
                        case "m20_21_00_00":
                        case "m20_24_00_00":
                        case "m50_35_00_00":
                        case "m50_36_00_00":
                        case "m50_37_00_00":
                        case "m50_38_00_00":
                            if (handler.textDir.Split('\\')[handler.textDir.Split('\\').Length - 2] == "talk") {
                                string npcName = DS2Utils.GetConversationNpcName(name);
                                string diaName = DS2Utils.GetConversationDiaName(name);
                                path = Path.Combine(handler.textDir, npcName, diaName);
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                target = Path.Combine(path, name);
                            }
                            break;
                        case "charaname":
                            if (handler.textDir.Split('\\')[handler.textDir.Split('\\').Length - 2] == "talk") {
                                string bossName = DS2Utils.GetBossName(name);
                                path = Path.Combine(handler.textDir, bossName);
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                target = Path.Combine(path, name);
                            }
                            break;
                    }
                    StreamWriter sw = new StreamWriter(target);
                    sw.Write(text);
                    sw.Close();
                    entries++;
                    observer.onItemProgress(entries, maxEntries);
                }
                emptyWriter.Close();
                spaceWriter.Close();
            }
        }

        public static void RepackPureText(string sourceFolder, string targetFolder, CommonUtils.TextObserver observer) {
            string[] baseDirectories = Directory.GetDirectories(Path.Combine(sourceFolder, "base"), "*", SearchOption.TopDirectoryOnly);
            string[] talkDirectores = Directory.GetDirectories(Path.Combine(sourceFolder, "talk"), "*", SearchOption.TopDirectoryOnly);
            string[] bmDirectores = Directory.GetDirectories(Path.Combine(sourceFolder, "bloodmes"), "*", SearchOption.TopDirectoryOnly);
            Directory.CreateDirectory(targetFolder);
            Directory.CreateDirectory(Path.Combine(targetFolder, "talk"));
            Directory.CreateDirectory(Path.Combine(targetFolder, "bloodmes"));
            List<DS2FMGHandler> handlers = new List<DS2FMGHandler>();

            int maxEntries = 0;
            int entries = 0;

            foreach (string dir in baseDirectories) {
                maxEntries += Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Length;
                string name = dir.Split('\\').Last();
                string fmgFile = Path.Combine(targetFolder, name + ".fmg");
                handlers.Add(new DS2FMGHandler(fmgFile, new FMG(), dir));
            }
            foreach (string dir in talkDirectores) {
                maxEntries += Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Length;
                string name = dir.Split('\\').Last();
                string fmgFile = Path.Combine(targetFolder, "talk", name + ".fmg");
                handlers.Add(new DS2FMGHandler(fmgFile, new FMG(), dir));
            }
            foreach (string dir in bmDirectores) {
                maxEntries += Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Length;
                string name = dir.Split('\\').Last();
                string fmgFile = Path.Combine(targetFolder, "bloodmes", name + ".fmg");
                handlers.Add(new DS2FMGHandler(fmgFile, new FMG(), dir));
            }

            observer.onItemStart(maxEntries);

            foreach (DS2FMGHandler handler in handlers) {
                handler.fmgData.Version = FMG.FMGVersion.DarkSouls1;
                handler.fmgData.Compression = DCX.Type.None;
                using (StreamReader reader = new StreamReader(Path.Combine(handler.textDir, "empty.DS2Info"))) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        if (line != "") {
                            FMG.Entry entry = new FMG.Entry(int.Parse(line), "");
                            handler.fmgData.Entries.Add(entry);
                        }
                    }
                }
                entries++;
                observer.onItemProgress(entries, maxEntries);
                using (StreamReader reader = new StreamReader(Path.Combine(handler.textDir, "spaces.DS2Info"))) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        if (line != "") {
                            FMG.Entry entry = new FMG.Entry(int.Parse(line), " ");
                            handler.fmgData.Entries.Add(entry);
                        }
                    }
                }
                entries++;
                observer.onItemProgress(entries, maxEntries);
                foreach (string file in Directory.GetFiles(handler.textDir, "*.txt", SearchOption.AllDirectories)) {
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

        public static void UnpackFont(string smallSource, string bigSource, string smallTarget, string bigTarget, CommonUtils.FontObserver2 observer) {
            BND4 smallFont = BND4.Read(smallSource);
            Directory.CreateDirectory(smallTarget);

            int smallEntries = 0;
            int maxSmallEntries = smallFont.Files.Count;

            observer.onSmallFontStart(maxSmallEntries);

            foreach (BinderFile file in smallFont.Files) {
                if (file.Name.EndsWith(".ccm")) {
                    string name = Path.Combine(smallTarget, file.ID + "€" + file.Name);
                    File.Create(name).Close();
                    File.WriteAllBytes(name, file.Bytes);
                } else {
                    string folderName = Path.Combine(smallTarget, file.ID + "€" + file.Name.Split('.')[0]);
                    Directory.CreateDirectory(folderName);
                    TPF tpf = TPF.Read(file.Bytes);
                    foreach (TPF.Texture texture in tpf.Textures) {
                        string path = Path.Combine(folderName, texture.Name + ".dds");
                        File.Create(path).Close();
                        File.WriteAllBytes(path, texture.Bytes);
                    }
                }
                smallEntries++;
                observer.onSmallFontProgress(smallEntries, maxSmallEntries);
            }

            BND4 bigFont = BND4.Read(bigSource);
            Directory.CreateDirectory(bigTarget);

            int bigEntries = 0;
            int maxBigEntries = bigFont.Files.Count;

            observer.onBigFontStart(maxBigEntries);

            foreach (BinderFile file in bigFont.Files) {
                if (file.Name.EndsWith(".ccm")) {
                    string name = Path.Combine(bigTarget, file.ID + "€" + file.Name);
                    File.Create(name).Close();
                    File.WriteAllBytes(name, file.Bytes);
                } else {
                    string folderName = Path.Combine(bigTarget, file.ID + "€" + file.Name.Split('.')[0]);
                    Directory.CreateDirectory(folderName);
                    TPF tpf = TPF.Read(file.Bytes);
                    foreach (TPF.Texture texture in tpf.Textures) {
                        string path = Path.Combine(folderName, texture.Name + ".dds");
                        File.Create(path).Close();
                        File.WriteAllBytes(path, texture.Bytes);
                    }
                }
                bigEntries++;
                observer.onBigFontProgress(bigEntries, maxBigEntries);
            }
        }

        public static void RepackFont(string smallSource, string bigSource, string smallTarget, string bigTarget, CommonUtils.FontObserver2 observer) {
            BND4 smallFont = new BND4();
            smallFont.Version = "14M18O9";

            int smallEntries = 0;
            int maxSmallEntries = Directory.GetDirectories(smallSource).Length + 1;

            observer.onSmallFontStart(maxSmallEntries);

            string smallCCMFile = Directory.GetFiles(smallSource, "*.ccm").First();
            BinderFile smallCCM = new BinderFile();
            smallCCM.Name = smallCCMFile.Split('\\').Last().Split('€')[1];
            smallCCM.ID = int.Parse(smallCCMFile.Split('\\').Last().Split('€')[0]);
            smallCCM.CompressionType = DCX.Type.Zlib;
            smallCCM.Bytes = File.ReadAllBytes(smallCCMFile);
            smallFont.Files.Add(smallCCM);
            smallEntries++;
            observer.onSmallFontProgress(smallEntries, maxSmallEntries);

            foreach (string directory in Directory.GetDirectories(smallSource)) {
                TPF tpf = new TPF();
                tpf.Encoding = 2;
                tpf.Flag2 = 3;
                tpf.Platform = TPF.TPFPlatform.PC;
                foreach (string file in Directory.GetFiles(directory)) {
                    byte format = 5;
                    if (int.Parse(directory.Split('\\').Last().Split('€')[0]) != 1) {
                        format = 3;
                    }
                    TPF.Texture texture = new TPF.Texture(Path.GetFileName(file).Split('.')[0], format, 0, File.ReadAllBytes(file));
                    tpf.Textures.Add(texture);
                }
                BinderFile bFile = new BinderFile();
                bFile.Name = directory.Split('\\').Last().Split('€')[1] + ".tpf";
                bFile.ID = int.Parse(directory.Split('\\').Last().Split('€')[0]);
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = tpf.Write();
                smallFont.Files.Add(bFile);
                smallEntries++;
                observer.onSmallFontProgress(smallEntries, maxSmallEntries);
            }
            File.Create(smallTarget).Close();
            File.WriteAllBytes(smallTarget, DCX.Compress(smallFont.Write(), DCX.Type.DCX_DFLT_10000_24_9));

            BND4 bigFont = new BND4();
            bigFont.Version = "14M18O9";

            int bigEntries = 0;
            int maxBigEntries = Directory.GetDirectories(bigSource).Length + 1;

            observer.onBigFontStart(maxBigEntries);

            string bigCCMFile = Directory.GetFiles(bigSource, "*.ccm").First();
            BinderFile bigCCM = new BinderFile();
            bigCCM.Name = bigCCMFile.Split('\\').Last().Split('€')[1];
            bigCCM.ID = int.Parse(bigCCMFile.Split('\\').Last().Split('€')[0]);
            bigCCM.CompressionType = DCX.Type.Zlib;
            bigCCM.Bytes = File.ReadAllBytes(bigCCMFile);
            bigFont.Files.Add(bigCCM);
            bigEntries++;
            observer.onBigFontProgress(bigEntries, maxBigEntries);

            foreach (string directory in Directory.GetDirectories(bigSource)) {
                TPF tpf = new TPF();
                tpf.Encoding = 2;
                tpf.Flag2 = 3;
                tpf.Platform = TPF.TPFPlatform.PC;
                foreach (string file in Directory.GetFiles(directory)) {
                    byte format = 5;
                    TPF.Texture texture = new TPF.Texture(Path.GetFileName(file).Split('.')[0], format, 0, File.ReadAllBytes(file));
                    tpf.Textures.Add(texture);
                }
                BinderFile bFile = new BinderFile();
                bFile.Name = directory.Split('\\').Last().Split('€')[1] + ".tpf";
                bFile.ID = int.Parse(directory.Split('\\').Last().Split('€')[0]);
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = tpf.Write();
                bigFont.Files.Add(bFile);
                bigEntries++;
                observer.onBigFontProgress(bigEntries, maxBigEntries);
            }
            File.Create(bigTarget).Close();
            File.WriteAllBytes(bigTarget, DCX.Compress(bigFont.Write(), DCX.Type.DCX_DFLT_10000_24_9));
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
