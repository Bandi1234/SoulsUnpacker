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

        public static void RepackRawText(string itemFolder, string menuFolder, string itemTarget, string menuTarget, CommonUtils.TextObserver observer) {
            string[] itemFolders = Directory.GetDirectories(itemFolder);
            string[] menuFolders = Directory.GetDirectories(menuFolder);

            int maxItemEntries = 0;
            int itemEntries = 0;
            int maxMenuEntries = 0;
            int menuEntries = 0;
            foreach (string folder in itemFolders) {
                maxItemEntries += Directory.GetFiles(folder).Length;
            }
            foreach (string folder in menuFolders) {
                maxMenuEntries += Directory.GetFiles(folder).Length;
            }

            observer.onItemStart(maxItemEntries);

            BND4 item = new BND4();
            item.Version = "07D7R6";
            foreach (string folder in itemFolders) {
                FMG fmg = new FMG();
                fmg.Version = FMG.FMGVersion.DarkSouls3;
                fmg.Compression = DCX.Type.None;
                string[] files = Directory.GetFiles(folder);
                foreach (string file in files) {
                    int id = int.Parse(file.Split('\\').Last().Split('.')[0]);
                    StreamReader sr = new StreamReader(file);
                    string full = sr.ReadToEnd();
                    sr.Close();
                    FMG.Entry entry = new FMG.Entry(id, full);
                    fmg.Entries.Add(entry);
                    itemEntries++;
                    observer.onItemProgress(itemEntries, maxItemEntries);
                }
                BinderFile bFile = new BinderFile();
                string fName = folder.Split('\\').Last().Split('€').Last();
                string fId = folder.Split('\\').Last().Split('€').First();
                bFile.Name = @"N:\GR\data\INTERROOT_win64\msg\engUS\" + fName + ".fmg";
                bFile.ID = int.Parse(fId);
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                item.Files.Add(bFile);
            }
            File.Create(itemTarget).Close();
            File.WriteAllBytes(itemTarget, DCX.Compress(item.Write(), DCX.Type.DCX_KRAK));

            observer.onMenuStart(maxMenuEntries);

            BND4 menu = new BND4();
            menu.Version = "07D7R6";
            foreach (string folder in menuFolders) {
                FMG fmg = new FMG();
                fmg.Version = FMG.FMGVersion.DarkSouls3;
                fmg.Compression = DCX.Type.None;
                string[] files = Directory.GetFiles(folder);
                foreach (string file in files) {
                    int id = int.Parse(file.Split('\\').Last().Split('.')[0]);
                    StreamReader sr = new StreamReader(file);
                    string full = sr.ReadToEnd();
                    sr.Close();
                    FMG.Entry entry = new FMG.Entry(id, full);
                    fmg.Entries.Add(entry);
                    menuEntries++;
                    observer.onMenuProgress(menuEntries, maxMenuEntries);
                }
                BinderFile bFile = new BinderFile();
                string fName = folder.Split('\\').Last().Split('€').Last();
                string fId = folder.Split('\\').Last().Split('€').First();
                bFile.Name = @"N:\GR\data\INTERROOT_win64\msg\engUS\" + fName + ".fmg";
                bFile.ID = int.Parse(fId);
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                menu.Files.Add(bFile);
            }
            File.Create(menuTarget).Close();
            File.WriteAllBytes(menuTarget, DCX.Compress(menu.Write(), DCX.Type.DCX_KRAK));
        }

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

        public static void RepackPureText(string itemFolder, string menuFolder, string itemTarget, string menuTarget, CommonUtils.TextObserver observer) {
            string[] itemFolders = Directory.GetDirectories(itemFolder);
            string[] menuFolders = Directory.GetDirectories(menuFolder);

            int maxItemEntries = 0;
            int itemEntries = 0;
            int maxMenuEntries = 0;
            int menuEntries = 0;

            maxItemEntries = Directory.GetFiles(itemFolder, "*.*", SearchOption.AllDirectories).Length;
            maxMenuEntries = Directory.GetFiles(menuFolder, "*.*", SearchOption.AllDirectories).Length;

            observer.onItemStart(maxItemEntries);

            StreamReader fmgInfoReader = new StreamReader(Path.Combine(itemFolder, "compressionIds.ERInfo"));
            List<FmgInfo> itemInfos = new List<FmgInfo>();
            while (!fmgInfoReader.EndOfStream) {
                itemInfos.Add(new FmgInfo(fmgInfoReader.ReadLine()));
            }
            fmgInfoReader.Close();

            itemEntries++;
            observer.onItemProgress(itemEntries, maxItemEntries);

            BND4 item = new BND4();
            item.Version = "07D7R6";
            foreach (FmgInfo info in itemInfos) {
                string folder = Path.Combine(itemFolder, info.name);
                FMG fmg = new FMG();
                fmg.Version = FMG.FMGVersion.DarkSouls3;
                fmg.Compression = DCX.Type.None;
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "empty.ERInfo"))) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        if (line != "") {
                            FMG.Entry entry = new FMG.Entry(int.Parse(line), "");
                            fmg.Entries.Add(entry);
                        }
                    }
                }
                itemEntries++;
                observer.onItemProgress(itemEntries, maxItemEntries);
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "spaces.ERInfo"))) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        if (line != "") {
                            FMG.Entry entry = new FMG.Entry(int.Parse(line), " ");
                            fmg.Entries.Add(entry);
                        }
                    }
                }
                itemEntries++;
                observer.onItemProgress(itemEntries, maxItemEntries);
                string[] files = Directory.GetFiles(folder, "*.txt", SearchOption.AllDirectories);
                foreach (string file in files) {
                    int id = int.Parse(file.Split('\\').Last().Split('.')[0]);
                    StreamReader reader = new StreamReader(file);
                    string full = reader.ReadToEnd();
                    reader.Close();
                    FMG.Entry entry = new FMG.Entry(id, full);
                    fmg.Entries.Add(entry);
                    itemEntries++;
                    observer.onItemProgress(itemEntries, maxItemEntries);
                }
                BinderFile bFile = new BinderFile();
                bFile.Name = @"N:\GR\data\INTERROOT_win64\msg\engUS\" + info.name + ".fmg";
                bFile.ID = info.id;
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                item.Files.Add(bFile);
            }
            File.Create(itemTarget).Close();
            File.WriteAllBytes(itemTarget, DCX.Compress(item.Write(), DCX.Type.DCX_KRAK));

            observer.onMenuStart(maxMenuEntries);

            fmgInfoReader = new StreamReader(Path.Combine(menuFolder, "compressionIds.ERInfo"));
            List<FmgInfo> menuInfos = new List<FmgInfo>();
            while (!fmgInfoReader.EndOfStream) {
                menuInfos.Add(new FmgInfo(fmgInfoReader.ReadLine()));
            }
            fmgInfoReader.Close();

            BND4 menu = new BND4();
            menu.Version = "07D7R6";
            foreach (FmgInfo info in menuInfos) {
                string folder = Path.Combine(menuFolder, info.name);
                FMG fmg = new FMG();
                fmg.Version = FMG.FMGVersion.DarkSouls3;
                fmg.Compression = DCX.Type.None;
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "empty.ERInfo"))) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        if (line != "") {
                            FMG.Entry entry = new FMG.Entry(int.Parse(line), "");
                            fmg.Entries.Add(entry);
                        }
                    }
                }
                menuEntries++;
                observer.onMenuProgress(menuEntries, maxMenuEntries);
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "spaces.ERInfo"))) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        if (line != "") {
                            FMG.Entry entry = new FMG.Entry(int.Parse(line), " ");
                            fmg.Entries.Add(entry);
                        }
                    }
                }
                string[] files = Directory.GetFiles(folder, "*.txt", SearchOption.AllDirectories);
                foreach (string file in files) {
                    int id = int.Parse(file.Split('\\').Last().Split('.')[0]);
                    StreamReader reader = new StreamReader(file);
                    string full = reader.ReadToEnd();
                    reader.Close();
                    FMG.Entry entry = new FMG.Entry(id, full);
                    fmg.Entries.Add(entry);
                    menuEntries++;
                    observer.onMenuProgress(menuEntries, maxMenuEntries);
                }
                BinderFile bFile = new BinderFile();
                bFile.Name = @"N:\GR\data\INTERROOT_win64\msg\engUS\" + info.name + ".fmg";
                bFile.ID = info.id;
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                menu.Files.Add(bFile);
            }
            File.Create(menuTarget).Close();
            File.WriteAllBytes(menuTarget, DCX.Compress(menu.Write(), DCX.Type.DCX_KRAK));
        }

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

        public static void RepackERText(string itemSource, string menuSource, string itemTarget, string menuTarget, CommonUtils.TextObserver observer) {
            StreamReader itemReader = new StreamReader(itemSource);
            int maxItemEntries = 0;
            int itemEntries = 0;
            while (!itemReader.EndOfStream) {
                itemReader.ReadLine();
                maxItemEntries++;
            }

            observer.onItemStart(maxItemEntries);

            itemReader.Close();
            itemReader = new StreamReader(itemSource);
            BND4 item = new BND4();
            item.Version = "07D7R6";

            while (true) {
                string fmgLine = itemReader.ReadLine();
                itemEntries++;
                observer.onItemProgress(itemEntries, maxItemEntries);
                if (fmgLine == "€€€€€") {
                    break;
                }
                string engName = fmgLine.Split('\t')[0];
                string fmgName = engName;
                int fmgId = int.Parse(fmgLine.Split('\t')[1]);
                FMG fmg = new FMG();
                fmg.Version = FMG.FMGVersion.DarkSouls3;
                fmg.Compression = DCX.Type.None;
                while (true) {
                    string entryIdLine = itemReader.ReadLine();
                    itemEntries++;
                    observer.onItemProgress(itemEntries, maxItemEntries);
                    if (entryIdLine == "€€€") {
                        break;
                    }
                    List<string> entryContentLines = new List<string>();
                    while (true) {
                        string nextLine = itemReader.ReadLine();
                        itemEntries++;
                        observer.onItemProgress(itemEntries, maxItemEntries);
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
                    fmg.Entries.Add(entry);
                }
                string emptyLine = itemReader.ReadLine();
                itemEntries++;
                observer.onItemProgress(itemEntries, maxItemEntries);
                if (emptyLine != "") {
                    string[] empties = emptyLine.Split('\t');
                    foreach (string empty in empties) {
                        FMG.Entry entry = new FMG.Entry(int.Parse(empty), "");
                        fmg.Entries.Add(entry);
                    }
                }
                string spacesLine = itemReader.ReadLine();
                itemEntries++;
                observer.onItemProgress(itemEntries, maxItemEntries);
                if (spacesLine != "") {
                    string[] spaces = spacesLine.Split('\t');
                    foreach (string space in spaces) {
                        FMG.Entry entry = new FMG.Entry(int.Parse(space), " ");
                        fmg.Entries.Add(entry);
                    }
                }
                BinderFile bFile = new BinderFile();
                bFile.Name = @"N:\GR\data\INTERROOT_win64\msg\engUS\" + fmgName + ".fmg";
                bFile.ID = fmgId;
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                item.Files.Add(bFile);
            }
            File.Create(itemTarget).Close();
            File.WriteAllBytes(itemTarget, DCX.Compress(item.Write(), DCX.Type.DCX_KRAK));
            itemReader.Close();

            StreamReader menuReader = new StreamReader(menuSource);
            int maxMenuEntries = 0;
            int menuEntries = 0;
            while (!menuReader.EndOfStream) {
                menuReader.ReadLine();
                maxMenuEntries++;
            }

            observer.onMenuStart(maxMenuEntries);

            menuReader.Close();
            menuReader = new StreamReader(menuSource);
            BND4 menu = new BND4();
            menu.Version = "07D7R6";

            while (true) {
                string fmgLine = menuReader.ReadLine();
                menuEntries++;
                observer.onMenuProgress(menuEntries, maxMenuEntries);
                if (fmgLine == "€€€€€") {
                    break;
                }
                string engName = fmgLine.Split('\t')[0];
                string fmgName = engName;
                int fmgId = int.Parse(fmgLine.Split('\t')[1]);
                FMG fmg = new FMG();
                fmg.Version = FMG.FMGVersion.DarkSouls3;
                fmg.Compression = DCX.Type.None;
                while (true) {
                    string entryIdLine = menuReader.ReadLine();
                    menuEntries++;
                    observer.onMenuProgress(menuEntries, maxMenuEntries);
                    if (entryIdLine == "€€€") {
                        break;
                    }
                    List<string> entryContentLines = new List<string>();
                    while (true) {
                        string nextLine = menuReader.ReadLine();
                        menuEntries++;
                        observer.onMenuProgress(menuEntries, maxMenuEntries);
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
                    fmg.Entries.Add(entry);
                }
                string emptyLine = menuReader.ReadLine();
                menuEntries++;
                observer.onMenuProgress(menuEntries, maxMenuEntries);
                if (emptyLine != "") {
                    string[] empties = emptyLine.Split('\t');
                    foreach (string empty in empties) {
                        FMG.Entry entry = new FMG.Entry(int.Parse(empty), "");
                        fmg.Entries.Add(entry);
                    }
                }
                string spacesLine = menuReader.ReadLine();
                menuEntries++;
                observer.onMenuProgress(menuEntries, maxMenuEntries);
                if (spacesLine != "") {
                    string[] spaces = spacesLine.Split('\t');
                    foreach (string space in spaces) {
                        FMG.Entry entry = new FMG.Entry(int.Parse(space), " ");
                        fmg.Entries.Add(entry);
                    }
                }
                BinderFile bFile = new BinderFile();
                bFile.Name = @"N:\GR\data\INTERROOT_win64\msg\engUS\" + fmgName + ".fmg";
                bFile.ID = fmgId;
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                menu.Files.Add(bFile);
            }
            File.Create(menuTarget).Close();
            File.WriteAllBytes(menuTarget, DCX.Compress(menu.Write(), DCX.Type.DCX_KRAK));
            menuReader.Close();
        }

        private class FmgInfo {
            public string name;
            public int id;

            public FmgInfo(string line) {
                this.name = line.Split('\t')[0];
                this.id = int.Parse(line.Split('\t')[1]);
            }
        }
    }
}
