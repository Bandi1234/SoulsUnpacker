﻿using System;
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

        public static void RepackRawText(string itemFolder, string menuFolder, string itemTarget, string menuTarget, TextObserver observer) { 
            
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

            BND3 itemBnd = new BND3();
            itemBnd.Version = "07D7R6";
            foreach (string folder in itemFolders) {
                FMG fmg = new FMG();
                fmg.Version = FMG.FMGVersion.DarkSouls1;
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
                bFile.Name = @"N:\FRPG\data\Msg\DATA_ENGLISH\" + fName + ".fmg";
                bFile.ID = int.Parse(fId);
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                itemBnd.Files.Add(bFile);
            }
            File.Create(itemTarget).Close();
            File.WriteAllBytes(itemTarget, DCX.Compress(itemBnd.Write(), DCX.Type.DCX_DFLT_10000_24_9));

            observer.onMenuStart(maxMenuEntries);

            BND3 menuBnd = new BND3();
            menuBnd.Version = "07D7R6";
            foreach (string folder in menuFolders) {
                FMG fmg = new FMG();
                fmg.Version = FMG.FMGVersion.DarkSouls1;
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
                bFile.Name = @"N:\FRPG\data\Msg\DATA_ENGLISH\" + fName + ".fmg";
                bFile.ID = int.Parse(fId);
                bFile.CompressionType = DCX.Type.Zlib;
                bFile.Bytes = fmg.Write();
                menuBnd.Files.Add(bFile);
            }
            File.Create(menuTarget).Close();
            File.WriteAllBytes(menuTarget, DCX.Compress(menuBnd.Write(), DCX.Type.DCX_DFLT_10000_24_9));


        }

        public static void UnpackPureText(string itemSource, string menuSource, string itemTarget, string menuTarget, TextObserver observer) {
            Directory.CreateDirectory(itemTarget);

            BND3 itemBnd = BND3.Read(itemSource);

            int maxItemEntries = 0;
            int itemEntries = 0;
            List<PureFmgHandler> itemFmgs = new List<PureFmgHandler>();

            foreach (BinderFile file in itemBnd.Files) {
                if (itemFmgs.Exists((PureFmgHandler fmgH) => fmgH.name == file.Name)) {
                    itemFmgs.Find((PureFmgHandler fmgH) => fmgH.name == file.Name).id2 = file.ID;
                } else {
                    PureFmgHandler handler = new PureFmgHandler(FMG.Read(file.Bytes), file.Name, file.ID);
                    maxItemEntries += handler.fmgData.Entries.Count;
                    itemFmgs.Add(handler);
                }
            }

            StreamWriter compIdWriter = new StreamWriter(itemTarget + "/compressionIds.DSRInfo");
            foreach (PureFmgHandler handler in itemFmgs) {
                string hName = handler.name.Split('\\').Last().Split('.')[0];
                compIdWriter.WriteLine(hName + "\t" + handler.id1 + "\t" + handler.id2);
                Directory.CreateDirectory(Path.Combine(itemTarget, hName));
            }
            compIdWriter.Close();

            observer.onItemStart(maxItemEntries);

            foreach (PureFmgHandler handler in itemFmgs) {
                string hName = handler.name.Split('\\').Last().Split('.')[0];
                string targetBaseDir = Path.Combine(itemTarget, hName);
                StreamWriter emptyWriter = new StreamWriter(Path.Combine(targetBaseDir, "empty.DSRInfo"));
                StreamWriter spaceWriter = new StreamWriter(Path.Combine(targetBaseDir, "spaces.DSRInfo"));
                foreach (FMG.Entry entry in handler.fmgData.Entries) {
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
                    string target = Path.Combine(targetBaseDir, name);
                    switch (hName) {
                        case "Weapon_description_":
                        case "Weapon_long_desc_":
                        case "Weapon_name_":
                            string wType = DSRUtils.GetWeaponTypeFolder(name);
                            string wName = DSRUtils.GetWeaponNameFolder(name);
                            if (!Directory.Exists(Path.Combine(targetBaseDir, wType, wName)))
                                Directory.CreateDirectory(Path.Combine(targetBaseDir, wType, wName));
                            target = Path.Combine(targetBaseDir, wType, wName, name);
                            break;
                        case "Place_name_":
                            string pType = DSRUtils.GetPlaceTypeFolder(name);
                            string pName = DSRUtils.GetPlaceNameFolder(name);
                            if (!Directory.Exists(Path.Combine(targetBaseDir, pType, pName)))
                                Directory.CreateDirectory(Path.Combine(targetBaseDir, pType, pName));
                            target = Path.Combine(targetBaseDir, pType, pName, name);
                            break;
                        case "Accessory_description_":
                        case "Accessory_long_desc_":
                        case "Accessory_name_":
                            string aName = DSRUtils.GetAccessoryNameFolder(name);
                            if (!Directory.Exists(Path.Combine(targetBaseDir, aName)))
                                Directory.CreateDirectory(Path.Combine(targetBaseDir, aName));
                            target = Path.Combine(targetBaseDir, aName, name);
                            break;
                        case "NPC_name_":
                            string nType = DSRUtils.GetNpcTypeFolder(name);
                            string nName = DSRUtils.GetNpcNameFolder(name);
                            if (!Directory.Exists(Path.Combine(targetBaseDir, nType, nName)))
                                Directory.CreateDirectory(Path.Combine(targetBaseDir, nType, nName));
                            target = Path.Combine(targetBaseDir, nType, nName, name);
                            break;
                        case "Magic_description_":
                        case "Magic_long_desc_":
                        case "Magic_name_":
                            string mType = DSRUtils.GetMagicTypeFolder(name);
                            string mName = DSRUtils.GetMagicNameFolder(name);
                            if (!Directory.Exists(Path.Combine(targetBaseDir, mType, mName)))
                                Directory.CreateDirectory(Path.Combine(targetBaseDir, mType, mName));
                            target = Path.Combine(targetBaseDir, mType, mName, name);
                            break;
                        case "Armor_description_":
                        case "Armor_long_desc_":
                        case "Armor_name_":
                            string arType = DSRUtils.GetArmorTypeFolder(name);
                            string arName = DSRUtils.GetArmorNameFolder(name);
                            if (!Directory.Exists(Path.Combine(targetBaseDir, arType, arName)))
                                Directory.CreateDirectory(Path.Combine(targetBaseDir, arType, arName));
                            target = Path.Combine(targetBaseDir, arType, arName, name);
                            break;
                        case "Item_description_":
                        case "Item_long_desc_":
                        case "Item_name_":
                            string iType = DSRUtils.GetItemTypeFolder(name);
                            string iName = DSRUtils.GetItemNameFolder(name);
                            if (!Directory.Exists(Path.Combine(targetBaseDir, iType, iName)))
                                Directory.CreateDirectory(Path.Combine(targetBaseDir, iType, iName));
                            target = Path.Combine(targetBaseDir, iType, iName, name);
                            break;
                    }
                    StreamWriter sw = new StreamWriter(target);
                    sw.Write(entry.Text);
                    sw.Close();
                    itemEntries++;
                    observer.onItemProgress(itemEntries, maxItemEntries);
                }
                emptyWriter.Close();
                spaceWriter.Close();
            }

            Directory.CreateDirectory(menuTarget);

            BND3 menuBnd = BND3.Read(menuSource);

            int maxMenuEntries = 0;
            int menuEntries = 0;
            List<PureFmgHandler> menuFmgs = new List<PureFmgHandler>();

            foreach (BinderFile file in menuBnd.Files) {
                if (menuFmgs.Exists((PureFmgHandler fmgH) => fmgH.name == file.Name)) {
                    menuFmgs.Find((PureFmgHandler fmgH) => fmgH.name == file.Name).id2 = file.ID;
                } else {
                    PureFmgHandler handler = new PureFmgHandler(FMG.Read(file.Bytes), file.Name, file.ID);
                    maxMenuEntries += handler.fmgData.Entries.Count;
                    menuFmgs.Add(handler);
                }
            }

            compIdWriter = new StreamWriter(menuTarget + "/compressionIds.DSRInfo");
            foreach (PureFmgHandler handler in menuFmgs) {
                string hName = handler.name.Split('\\').Last().Split('.')[0];
                compIdWriter.WriteLine(hName + "\t" + handler.id1 + "\t" + handler.id2);
                Directory.CreateDirectory(Path.Combine(menuTarget, hName));
            }
            compIdWriter.Close();

            observer.onMenuStart(maxMenuEntries);

            foreach (PureFmgHandler handler in menuFmgs) {
                string hName = handler.name.Split('\\').Last().Split('.')[0];
                string targetBaseDir = Path.Combine(menuTarget, hName);
                StreamWriter emptyWriter = new StreamWriter(Path.Combine(targetBaseDir, "empty.DSRInfo"));
                StreamWriter spaceWriter = new StreamWriter(Path.Combine(targetBaseDir, "spaces.DSRInfo"));
                foreach (FMG.Entry entry in handler.fmgData.Entries) {
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
                    string target = Path.Combine(targetBaseDir, name);
                    switch (hName) {
                        case "Movie_subtitles_":
                            string mSub = DSRUtils.GetMovSubFolder(name);
                            if (!Directory.Exists(Path.Combine(targetBaseDir, mSub)))
                                Directory.CreateDirectory(Path.Combine(targetBaseDir, mSub));
                            target = Path.Combine(targetBaseDir, mSub, name);
                            break;
                        case "Conversation_":
                            string cNpc = DSRUtils.GetConversationNPC(name);
                            string cDia = DSRUtils.GetConversationDialogue(name);
                            if (!Directory.Exists(Path.Combine(targetBaseDir, cNpc, cDia)))
                                Directory.CreateDirectory(Path.Combine(targetBaseDir, cNpc, cDia));
                            target = Path.Combine(targetBaseDir, cNpc, cDia, name);
                            break;
                    }
                    StreamWriter sw = new StreamWriter(target);
                    sw.Write(entry.Text);
                    sw.Close();
                    menuEntries++;
                    observer.onMenuProgress(menuEntries, maxMenuEntries);
                }
                emptyWriter.Close();
                spaceWriter.Close();
            }
        }

        public static void RepackPureText(string itemFolder, string menuFolder, string itemTarget, string menuTarget, TextObserver observer) {
            string[] itemFolders = Directory.GetDirectories(itemFolder);
            string[] menuFolders = Directory.GetDirectories(menuFolder);

            int maxItemEntries = 0;
            int itemEntries = 0;
            int maxMenuEntries = 0;
            int menuEntries = 0;

            maxItemEntries = Directory.GetFiles(itemFolder, "*.*", SearchOption.AllDirectories).Length;
            maxMenuEntries = Directory.GetFiles(menuFolder, "*.*", SearchOption.AllDirectories).Length;

            observer.onItemStart(maxItemEntries);

            StreamReader fmgInfoReader = new StreamReader(Path.Combine(itemFolder, "compressionIds.DSRInfo"));
            List<FmgInfo> itemInfos = new List<FmgInfo>();
            while (!fmgInfoReader.EndOfStream) {
                itemInfos.Add(new FmgInfo(fmgInfoReader.ReadLine()));
            }

            itemEntries++;
            observer.onItemProgress(itemEntries, maxItemEntries);

            BND3 itemBnd = new BND3();
            itemBnd.Version = "07D7R6";
            foreach (FmgInfo info in itemInfos) {
                string folder = Path.Combine(itemFolder, info.name);
                FMG fmg1 = new FMG();
                FMG fmg2 = new FMG();
                fmg1.Version = FMG.FMGVersion.DarkSouls1;
                fmg2.Version = FMG.FMGVersion.DarkSouls1;
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "empty.DSRInfo"))) {
                    while (!reader.EndOfStream) { 
                        string line = reader.ReadLine();
                        if (line != "") {
                            FMG.Entry entry1 = new FMG.Entry(int.Parse(line), "");
                            fmg1.Entries.Add(entry1);
                            if (info.id2 != -1) {
                                FMG.Entry entry2 = new FMG.Entry(int.Parse(line), "");
                                fmg2.Entries.Add(entry2);
                            }
                        }
                    }
                }
                itemEntries++;
                observer.onItemProgress(itemEntries, maxItemEntries);
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "spaces.DSRInfo"))) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        if (line != "") {
                            FMG.Entry entry1 = new FMG.Entry(int.Parse(line), " ");
                            fmg1.Entries.Add(entry1);
                            if (info.id2 != -1) {
                                FMG.Entry entry2 = new FMG.Entry(int.Parse(line), " ");
                                fmg2.Entries.Add(entry2);
                            }
                        }
                    }
                }
                string[] files = Directory.GetFiles(folder, "*.txt", SearchOption.AllDirectories);
                foreach (string file in files) {
                    int id = int.Parse(file.Split('\\').Last().Split('.')[0]);
                    StreamReader reader = new StreamReader(file);
                    string full = reader.ReadToEnd();
                    reader.Close();
                    FMG.Entry entry1 = new FMG.Entry(id, full);
                    fmg1.Entries.Add(entry1);
                    if (info.id2 != -1) {
                        FMG.Entry entry2 = new FMG.Entry(id, full);
                        fmg2.Entries.Add(entry2);
                    }
                    itemEntries++;
                    observer.onItemProgress(itemEntries, maxItemEntries);
                }
                BinderFile bFile1 = new BinderFile();
                BinderFile bFile2 = new BinderFile();
                bFile1.Name = @"N:\FRPG\data\Msg\DATA_ENGLISH\" + info.name + ".fmg";
                bFile1.ID = info.id1;
                bFile1.CompressionType = DCX.Type.Zlib;
                bFile1.Bytes = fmg1.Write();
                itemBnd.Files.Add(bFile1);
                if (info.id2 != -1) {
                    bFile2.Name = @"N:\FRPG\data\Msg\DATA_ENGLISH\" + info.name + ".fmg";
                    bFile2.ID = info.id2;
                    bFile2.CompressionType = DCX.Type.Zlib;
                    bFile2.Bytes = fmg2.Write();
                    itemBnd.Files.Add(bFile2);
                }
            }
            File.Create(itemTarget).Close();
            File.WriteAllBytes(itemTarget, DCX.Compress(itemBnd.Write(), DCX.Type.DCX_DFLT_10000_24_9));

            observer.onMenuStart(maxMenuEntries);

            fmgInfoReader = new StreamReader(Path.Combine(menuFolder, "compressionIds.DSRInfo"));
            List<FmgInfo> menuInfos = new List<FmgInfo>();
            while (!fmgInfoReader.EndOfStream) {
                menuInfos.Add(new FmgInfo(fmgInfoReader.ReadLine()));
            }

            menuEntries++;
            observer.onMenuProgress(menuEntries, maxMenuEntries);

            BND3 menuBnd = new BND3();
            menuBnd.Version = "07D7R6";
            foreach (FmgInfo info in menuInfos) {
                string folder = Path.Combine(menuFolder, info.name);
                FMG fmg1 = new FMG();
                FMG fmg2 = new FMG();
                fmg1.Version = FMG.FMGVersion.DarkSouls1;
                fmg2.Version = FMG.FMGVersion.DarkSouls1;
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "empty.DSRInfo"))) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        if (line != "") {
                            FMG.Entry entry1 = new FMG.Entry(int.Parse(line), "");
                            fmg1.Entries.Add(entry1);
                            if (info.id2 != -1) {
                                FMG.Entry entry2 = new FMG.Entry(int.Parse(line), "");
                                fmg2.Entries.Add(entry2);
                            }
                        }
                    }
                }
                menuEntries++;
                observer.onMenuProgress(menuEntries, maxMenuEntries);
                using (StreamReader reader = new StreamReader(Path.Combine(folder, "spaces.DSRInfo"))) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        if (line != "") {
                            FMG.Entry entry1 = new FMG.Entry(int.Parse(line), " ");
                            fmg1.Entries.Add(entry1);
                            if (info.id2 != -1) {
                                FMG.Entry entry2 = new FMG.Entry(int.Parse(line), " ");
                                fmg2.Entries.Add(entry2);
                            }
                        }
                    }
                }
                string[] files = Directory.GetFiles(folder, "*.txt", SearchOption.AllDirectories);
                foreach (string file in files) {
                    int id = int.Parse(file.Split('\\').Last().Split('.')[0]);
                    StreamReader reader = new StreamReader(file);
                    string full = reader.ReadToEnd();
                    reader.Close();
                    FMG.Entry entry1 = new FMG.Entry(id, full);
                    fmg1.Entries.Add(entry1);
                    if (info.id2 != -1) {
                        FMG.Entry entry2 = new FMG.Entry(id, full);
                        fmg2.Entries.Add(entry2);
                    }
                    menuEntries++;
                    observer.onMenuProgress(menuEntries, maxMenuEntries);
                }
                BinderFile bFile1 = new BinderFile();
                BinderFile bFile2 = new BinderFile();
                bFile1.Name = @"N:\FRPG\data\Msg\DATA_ENGLISH\" + info.name + ".fmg";
                bFile1.ID = info.id1;
                bFile1.CompressionType = DCX.Type.Zlib;
                bFile1.Bytes = fmg1.Write();
                menuBnd.Files.Add(bFile1);
                if (info.id2 != -1) {
                    bFile2.Name = @"N:\FRPG\data\Msg\DATA_ENGLISH\" + info.name + ".fmg";
                    bFile2.ID = info.id2;
                    bFile2.CompressionType = DCX.Type.Zlib;
                    bFile2.Bytes = fmg2.Write();
                    menuBnd.Files.Add(bFile2);
                }
            }
            File.Create(menuTarget).Close();
            File.WriteAllBytes(menuTarget, DCX.Compress(menuBnd.Write(), DCX.Type.DCX_DFLT_10000_24_9));
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

        private class PureFmgHandler {
            public FMG fmgData;
            public string name;
            public int id1;
            public int id2 = -1;

            public PureFmgHandler(FMG fmgData, string name, int id1) {
                this.id1 = id1;
                this.name = name;
                this.fmgData = fmgData;
            }
        }

        private class FmgInfo {
            public string name;
            public int id1;
            public int id2;

            public FmgInfo(string line) {
                this.name = line.Split('\t')[0];
                this.id1 = int.Parse(line.Split('\t')[1]);
                this.id2 = int.Parse(line.Split('\t')[2]);
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