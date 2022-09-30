using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SoulsFormats;

namespace SoulsUnpackTools {
    public static class DS3Tools {
        public static void UnpackRawText(string itemSource, string menuSource, string itemTarget, string menuTarget, CommonUtils.TextObserver observer) {
            BND4 item_dlc2 = BND4.Read(itemSource);
            Directory.CreateDirectory(itemTarget);

            BND4 menu_dlc2 = BND4.Read(menuSource);
            Directory.CreateDirectory(menuTarget);

            int maxItemEntries = 0;
            int itemEntries = 0;
            int maxMenuEntries = 0;
            int menuEntries = 0;
            foreach (BinderFile file in item_dlc2.Files) {
                maxItemEntries += FMG.Read(file.Bytes).Entries.Count;
            }
            foreach (BinderFile file in menu_dlc2.Files) {
                maxMenuEntries += FMG.Read(file.Bytes).Entries.Count;
            }

            observer.onItemStart(maxItemEntries);

            foreach (BinderFile file in item_dlc2.Files) {
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

            foreach (BinderFile file in menu_dlc2.Files) {
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

            BND4 item_dlc2 = new BND4();
            item_dlc2.Version = "07D7R6";
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
                bFile.Name = @"N:\FDP\data\INTERROOT_win64\msg\engUS\64bit\" + fName + ".fmg";
                bFile.ID = int.Parse(fId);
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                item_dlc2.Files.Add(bFile);
            }
            File.Create(itemTarget).Close();
            File.WriteAllBytes(itemTarget, DCX.Compress(item_dlc2.Write(), DCX.Type.DCX_DFLT_10000_44_9));

            observer.onMenuStart(maxMenuEntries);

            BND4 menu_dlc2 = new BND4();
            menu_dlc2.Version = "07D7R6";
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
                bFile.Name = @"N:\FDP\data\INTERROOT_win64\msg\engUS\64bit\" + fName + ".fmg";
                bFile.ID = int.Parse(fId);
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                menu_dlc2.Files.Add(bFile);
            }
            File.Create(menuTarget).Close();
            File.WriteAllBytes(menuTarget, DCX.Compress(menu_dlc2.Write(), DCX.Type.DCX_DFLT_10000_44_9));
        }

        public static void UnpackPureText(string itemSource, string menuSource, string itemTarget, string menuTarget, CommonUtils.TextObserver observer) {
            BND4 item_dlc2 = BND4.Read(itemSource);
            Directory.CreateDirectory(itemTarget);

            int maxItemEntries = 0;
            int itemEntries = 0;
            foreach (BinderFile file in item_dlc2.Files) {
                maxItemEntries += FMG.Read(file.Bytes).Entries.Count;
            }

            StreamWriter compIdWriter = new StreamWriter(Path.Combine(itemTarget, "compressionIds.DS3Info"));
            observer.onItemStart(maxItemEntries);

            foreach (BinderFile file in item_dlc2.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                string engName = DS3Utils.GetItemEnglishFmg(fmgName);
                string dirName = Path.Combine(itemTarget, engName);
                compIdWriter.WriteLine(engName + "\t" + file.ID);
                Directory.CreateDirectory(dirName);
                StreamWriter emptyWriter = new StreamWriter(Path.Combine(dirName, "empty.DS3Info"));
                StreamWriter spaceWriter = new StreamWriter(Path.Combine(dirName, "spaces.DS3Info"));
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
                        case "Weapon long descriptions_dlc1":
                        case "Weapon long descriptions":
                        case "Weapon long descriptions_dlc2":
                        case "Weapon names":
                        case "Weapon names_dlc1":
                        case "Weapon names_dlc2":
                        case "Weapon short descriptions":
                            string wName = DS3Utils.GetWeaponNameFolder(name);
                            string wType = DS3Utils.GetWeaponTypeFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, wType, wName)))
                                Directory.CreateDirectory(Path.Combine(dirName, wType, wName));
                            target = Path.Combine(dirName, wType, wName, name);
                            break;
                        case "Accessory long descriptions":
                        case "Accessory long descriptions_dlc1":
                        case "Accessory long descriptions_dlc2":
                        case "Accessory names":
                        case "Accessory names_dlc1":
                        case "Accessory names_dlc2":
                        case "Accessory short descriptions":
                        case "Accessory short descriptions_dlc1":
                        case "Accessory short descriptions_dlc2":
                            string accName = DS3Utils.GetAccessoryNameFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, accName)))
                                Directory.CreateDirectory(Path.Combine(dirName, accName));
                            target = Path.Combine(dirName, accName, name);
                            break;
                        case "Area names":
                        case "Area names_dlc1":
                        case "Area names_dlc2":
                            string areaName = DS3Utils.GetAreaNameFolder(name);
                            string areaType = DS3Utils.GetAreaTypeFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, areaType, areaName)))
                                Directory.CreateDirectory(Path.Combine(dirName, areaType, areaName));
                            target = Path.Combine(dirName, areaType, areaName, name);
                            break;
                        case "Magic long descriptions":
                        case "Magic long descriptions_dlc1":
                        case "Magic long descriptions_dlc2":
                        case "Magic names":
                        case "Magic names_dlc1":
                        case "Magic names_dlc2":
                        case "Magic short descriptions":
                        case "Magic short descriptions_dlc1":
                        case "Magic short descriptions_dlc2":
                            string magicName = DS3Utils.GetMagicNameFolder(name);
                            string magicType1 = DS3Utils.GetMagicType1Folder(name);
                            string magicType2 = DS3Utils.GetMagicType2Folder(name);
                            if (!Directory.Exists(Path.Combine(dirName, magicType1, magicType2, magicName)))
                                Directory.CreateDirectory(Path.Combine(dirName, magicType1, magicType2, magicName));
                            target = Path.Combine(dirName, magicType1, magicType2, magicName, name);
                            break;
                        case "Goods long descriptions":
                        case "Goods long descriptions_dlc1":
                        case "Goods long descriptions_dlc2":
                        case "Goods names":
                        case "Goods names_dlc1":
                        case "Goods names_dlc2":
                        case "Goods short descriptions":
                        case "Goods short descriptions_dlc1":
                        case "Goods short descriptions_dlc2":
                            string goodsName = DS3Utils.GetGoodsNameFolder(name);
                            string goodsType = DS3Utils.GetGoodsTypeFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, goodsType, goodsName)))
                                Directory.CreateDirectory(Path.Combine(dirName, goodsType, goodsName));
                            target = Path.Combine(dirName, goodsType, goodsName, name);
                            break;
                        case "Armor long descriptions":
                        case "Armor long descriptions_dlc1":
                        case "Armor long descriptions_dlc2":
                        case "Armor names":
                        case "Armor names_dlc1":
                        case "Armor names_dlc2":
                        case "Armor short descriptions":
                            string armorName = DS3Utils.GetArmorNameFolder(name);
                            string armorType = DS3Utils.GetArmorTypeFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, armorType, armorName)))
                                Directory.CreateDirectory(Path.Combine(dirName, armorType, armorName));
                            target = Path.Combine(dirName, armorType, armorName, name);
                            break;
                        case "NPC names":
                        case "NPC names_dlc1":
                        case "NPC names_dlc2":
                            string npcName = DS3Utils.GetNpcNameFolder(name);
                            string npcType = DS3Utils.GetNpcTypeFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, npcType, npcName)))
                                Directory.CreateDirectory(Path.Combine(dirName, npcType, npcName));
                            target = Path.Combine(dirName, npcType, npcName, name);
                            break;
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

            BND4 menu_dlc2 = BND4.Read(menuSource);
            Directory.CreateDirectory(menuTarget);

            int maxMenuEntries = 0;
            int menuEntries = 0;
            foreach (BinderFile file in menu_dlc2.Files) {
                maxMenuEntries += FMG.Read(file.Bytes).Entries.Count;
            }

            compIdWriter = new StreamWriter(Path.Combine(menuTarget, "compressionIds.DS3Info"));
            observer.onMenuStart(maxMenuEntries);

            foreach (BinderFile file in menu_dlc2.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                string engName = DS3Utils.GetMenuEnglishFmg(fmgName);
                string dirName = Path.Combine(menuTarget, engName);
                compIdWriter.WriteLine(engName + "\t" + file.ID);
                Directory.CreateDirectory(dirName);
                StreamWriter emptyWriter = new StreamWriter(Path.Combine(dirName, "empty.DS3Info"));
                StreamWriter spaceWriter = new StreamWriter(Path.Combine(dirName, "spaces.DS3Info"));
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
                        case "NPC dialogue":
                            string dialName = DS3Utils.GetDialogueConversationFolder(name);
                            string dialType = DS3Utils.GetDialogueTypeFolder(name);
                            string dialNpc = DS3Utils.GetDialogueNpcFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, dialType, dialNpc, dialName)))
                                Directory.CreateDirectory(Path.Combine(dirName, dialType, dialNpc, dialName));
                            target = Path.Combine(dirName, dialType, dialNpc, dialName, name);
                            break;
                        case "NPC dialogue_dlc1":
                            string dial1Name = DS3Utils.GetDialogueConversationFolder1(name);
                            string dial1Type = DS3Utils.GetDialogueTypeFolder1(name);
                            string dial1Npc = DS3Utils.GetDialogueNpcFolder1(name);
                            if (!Directory.Exists(Path.Combine(dirName, dial1Type, dial1Npc, dial1Name)))
                                Directory.CreateDirectory(Path.Combine(dirName, dial1Type, dial1Npc, dial1Name));
                            target = Path.Combine(dirName, dial1Type, dial1Npc, dial1Name, name);
                            break;
                        case "NPC dialogue_dlc2":
                            string dial2Name = DS3Utils.GetDialogueConversationFolder2(name);
                            string dial2Type = DS3Utils.GetDialogueTypeFolder2(name);
                            string dial2Npc = DS3Utils.GetDialogueNpcFolder2(name);
                            if (!Directory.Exists(Path.Combine(dirName, dial2Type, dial2Npc, dial2Name)))
                                Directory.CreateDirectory(Path.Combine(dirName, dial2Type, dial2Npc, dial2Name));
                            target = Path.Combine(dirName, dial2Type, dial2Npc, dial2Name, name);
                            break;
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

            StreamReader fmgInfoReader = new StreamReader(Path.Combine(itemFolder, "compressionIds.DS3Info"));
            List<FmgInfo> itemInfos = new List<FmgInfo>();
            while (!fmgInfoReader.EndOfStream) {
                itemInfos.Add(new FmgInfo(fmgInfoReader.ReadLine()));
            }
            fmgInfoReader.Close();

            itemEntries++;
            observer.onItemProgress(itemEntries, maxItemEntries);

            BND4 item_dlc2 = new BND4();
            item_dlc2.Version = "07D7R6";
            foreach (FmgInfo info in itemInfos) {
                string folder = Path.Combine(itemFolder, info.name);
                FMG fmg = new FMG();
                fmg.Version = FMG.FMGVersion.DarkSouls3;
                fmg.Compression = DCX.Type.None;
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "empty.DS3Info"))) {
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
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "spaces.DS3Info"))) {
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
                bFile.Name = @"N:\FDP\data\INTERROOT_win64\msg\engUS\64bit\" + DS3Utils.GetItemJapaneseFmg(info.name) + ".fmg";
                bFile.ID = info.id;
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                item_dlc2.Files.Add(bFile);
            }
            File.Create(itemTarget).Close();
            File.WriteAllBytes(itemTarget, DCX.Compress(item_dlc2.Write(), DCX.Type.DCX_DFLT_10000_44_9));

            observer.onMenuStart(maxMenuEntries);

            fmgInfoReader = new StreamReader(Path.Combine(menuFolder, "compressionIds.DS3Info"));
            List<FmgInfo> menuInfos = new List<FmgInfo>();
            while (!fmgInfoReader.EndOfStream) {
                menuInfos.Add(new FmgInfo(fmgInfoReader.ReadLine()));
            }
            fmgInfoReader.Close();

            BND4 menu_dlc2 = new BND4();
            menu_dlc2.Version = "07D7R6";
            foreach (FmgInfo info in menuInfos) {
                string folder = Path.Combine(menuFolder, info.name);
                FMG fmg = new FMG();
                fmg.Version = FMG.FMGVersion.DarkSouls3;
                fmg.Compression = DCX.Type.None;
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "empty.DS3Info"))) {
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
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "spaces.DS3Info"))) {
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
                bFile.Name = @"N:\FDP\data\INTERROOT_win64\msg\engUS\64bit\" + DS3Utils.GetMenuJapaneseFmg(info.name) + ".fmg";
                bFile.ID = info.id;
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                menu_dlc2.Files.Add(bFile);
            }
            File.Create(menuTarget).Close();
            File.WriteAllBytes(menuTarget, DCX.Compress(menu_dlc2.Write(), DCX.Type.DCX_DFLT_10000_44_9));
        }

        public static void UnpackDS3Text(string itemSource, string menuSource, string itemTarget, string menuTarget, CommonUtils.TextObserver observer) {
            StreamWriter itemWriter = new StreamWriter(itemTarget);
            BND4 item_dlc2 = BND4.Read(itemSource);

            int maxItemEntries = 0;
            int itemEntries = 0;
            foreach (BinderFile file in item_dlc2.Files) {
                maxItemEntries += FMG.Read(file.Bytes).Entries.Count;
            }

            observer.onItemStart(maxItemEntries);

            foreach (BinderFile file in item_dlc2.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                string engName = DS3Utils.GetItemEnglishFmg(fmgName);
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
            BND4 menu_dlc2 = BND4.Read(menuSource);

            int maxMenuEntries = 0;
            int menuEntries = 0;
            foreach (BinderFile file in menu_dlc2.Files) {
                maxMenuEntries += FMG.Read(file.Bytes).Entries.Count;
            }

            observer.onMenuStart(maxMenuEntries);

            foreach (BinderFile file in menu_dlc2.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                string engName = DS3Utils.GetMenuEnglishFmg(fmgName);
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

        public static void RepackDS3Text(string itemSource, string menuSource, string itemTarget, string menuTarget, CommonUtils.TextObserver observer) {
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
            BND4 item_dlc2 = new BND4();
            item_dlc2.Version = "07D7R6";

            while (true) {
                string fmgLine = itemReader.ReadLine();
                itemEntries++;
                observer.onItemProgress(itemEntries, maxItemEntries);
                if (fmgLine == "€€€€€") {
                    break;
                }
                string engName = fmgLine.Split('\t')[0];
                string fmgName = DS3Utils.GetItemJapaneseFmg(engName);
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
                bFile.Name = @"N:\FDP\data\INTERROOT_win64\msg\engUS\64bit\" + fmgName + ".fmg";
                bFile.ID = fmgId;
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                item_dlc2.Files.Add(bFile);
            }
            File.Create(itemTarget).Close();
            File.WriteAllBytes(itemTarget, DCX.Compress(item_dlc2.Write(), DCX.Type.DCX_DFLT_10000_44_9));
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
            BND4 menu_dlc2 = new BND4();
            menu_dlc2.Version = "07D7R6";

            while (true) {
                string fmgLine = menuReader.ReadLine();
                menuEntries++;
                observer.onMenuProgress(menuEntries, maxMenuEntries);
                if (fmgLine == "€€€€€") {
                    break;
                }
                string engName = fmgLine.Split('\t')[0];
                string fmgName = DS3Utils.GetMenuJapaneseFmg(engName);
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
                bFile.Name = @"N:\FDP\data\INTERROOT_win64\msg\engUS\64bit\" + fmgName + ".fmg";
                bFile.ID = fmgId;
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                menu_dlc2.Files.Add(bFile);
            }
            File.Create(menuTarget).Close();
            File.WriteAllBytes(menuTarget, DCX.Compress(menu_dlc2.Write(), DCX.Type.DCX_DFLT_10000_44_9));
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
