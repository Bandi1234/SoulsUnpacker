using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SoulsFormats;

namespace SoulsUnpackTools {
    public static class SEKTools {
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
                Console.WriteLine(fmg.Version);
                Console.ReadKey();
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
                bFile.Name = @"N:\NTC\data\Target\INTERROOT_win64\msg\engUS\" + fName + ".fmg";
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
                bFile.Name = @"N:\NTC\data\Target\INTERROOT_win64\msg\engUS\" + fName + ".fmg";
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

            StreamWriter compIdWriter = new StreamWriter(Path.Combine(itemTarget, "compressionIds.SEKInfo"));
            observer.onItemStart(maxItemEntries);

            foreach (BinderFile file in item.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                string engName = DS3Utils.GetItemEnglishFmg(fmgName);
                string dirName = Path.Combine(itemTarget, engName);
                compIdWriter.WriteLine(engName + "\t" + file.ID);
                Directory.CreateDirectory(dirName);
                StreamWriter emptyWriter = new StreamWriter(Path.Combine(dirName, "empty.SEKInfo"));
                StreamWriter spaceWriter = new StreamWriter(Path.Combine(dirName, "spaces.SEKInfo"));
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
                        case "Area names":
                            string areaName = SEKUtils.GetAreaNameFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, areaName)))
                                Directory.CreateDirectory(Path.Combine(dirName, areaName));
                            target = Path.Combine(dirName, areaName, name);
                            break;
                        case "Weapon long descriptions":
                        case "Weapon names":
                        case "Weapon short descriptions":
                        case "Maneuver types":
                            string weaponName = SEKUtils.GetWeaponNameFolder(name);
                            string weaponType = SEKUtils.GetWeaponTypeFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, weaponType, weaponName)))
                                Directory.CreateDirectory(Path.Combine(dirName, weaponType, weaponName));
                            target = Path.Combine(dirName, weaponType, weaponName, name);
                            break;
                        case "Armor long descriptions":
                        case "Armor names":
                        case "Armor short descriptions":
                            string armorName = SEKUtils.GetArmorNameFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, armorName)))
                                Directory.CreateDirectory(Path.Combine(dirName, armorName));
                            target = Path.Combine(dirName, armorName, name);
                            break;
                        case "Goods long descriptions":
                        case "Goods names":
                        case "Goods short descriptions":
                            string goodsName = SEKUtils.GetGoodsNameFolder(name);
                            string goodsType = SEKUtils.GetGoodsTypeFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, goodsType, goodsName)))
                                Directory.CreateDirectory(Path.Combine(dirName, goodsType, goodsName));
                            target = Path.Combine(dirName, goodsType, goodsName, name);
                            break;
                        case "NPC names":
                            string npcName = SEKUtils.GetNPCNameFolder(name);
                            string npcType = SEKUtils.GetNPCTypeFolder(name);
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

            BND4 menu = BND4.Read(menuSource);
            Directory.CreateDirectory(menuTarget);

            int maxMenuEntries = 0;
            int menuEntries = 0;
            foreach (BinderFile file in menu.Files) {
                maxMenuEntries += FMG.Read(file.Bytes).Entries.Count;
            }

            compIdWriter = new StreamWriter(Path.Combine(menuTarget, "compressionIds.SEKInfo"));
            observer.onMenuStart(maxMenuEntries);

            foreach (BinderFile file in menu.Files) {
                string fmgName = file.Name.Split('\\').Last().Split('.')[0];
                string engName = DS3Utils.GetMenuEnglishFmg(fmgName);
                string dirName = Path.Combine(menuTarget, engName);
                compIdWriter.WriteLine(engName + "\t" + file.ID);
                Directory.CreateDirectory(dirName);
                StreamWriter emptyWriter = new StreamWriter(Path.Combine(dirName, "empty.SEKInfo"));
                StreamWriter spaceWriter = new StreamWriter(Path.Combine(dirName, "spaces.SEKInfo"));
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
                            string dialName = SEKUtils.GetDialogueConversationFolder(name);
                            string dialNpc = SEKUtils.GetDialogueNPCFolder(name);
                            if (!Directory.Exists(Path.Combine(dirName, dialNpc, dialName)))
                                Directory.CreateDirectory(Path.Combine(dirName, dialNpc, dialName));
                            target = Path.Combine(dirName, dialNpc, dialName, name);
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
            //TODO
        }
    }
}
