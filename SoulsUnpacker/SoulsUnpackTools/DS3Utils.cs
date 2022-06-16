using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SoulsUnpackTools {
    public static class DS3Utils {
        public static string[] possibleFmgEndings = {
            "", "_dlc1", "_dlc2",
            "_win64", "_ps4", "_xboxone",
            "_win64_dlc1", "_win64_dlc2",
            "_ps4_dlc1", "_ps4_dlc2",
            "_xboxone_dlc1", "_xboxone_dlc2"
        };

        public static string GetItemEnglishFmg(string japName) {
            StreamReader sr = new StreamReader("PureInfo/ItemFMGs.DS3Info");
            while (!sr.EndOfStream) {
                string line = sr.ReadLine();
                string jap = line.Split('\t')[0];
                string eng = line.Split('\t')[1];
                foreach (string ending in possibleFmgEndings) {
                    if (jap + ending == japName) {
                        sr.Close();
                        return eng + ending;
                    }
                }
            }

            sr.Close();
            return japName;
        }

        public static string GetItemJapaneseFmg(string engName) {
            StreamReader sr = new StreamReader("PureInfo/ItemFMGs.DS3Info");
            while (!sr.EndOfStream) {
                string line = sr.ReadLine();
                string jap = line.Split('\t')[0];
                string eng = line.Split('\t')[1];
                foreach (string ending in possibleFmgEndings) {
                    if (eng + ending == engName) {
                        sr.Close();
                        return jap + ending;
                    }
                }
            }

            sr.Close();
            return engName;
        }

        public static string GetMenuEnglishFmg(string japName) {
            StreamReader sr = new StreamReader("PureInfo/MenuFMGs.DS3Info");
            while (!sr.EndOfStream) {
                string line = sr.ReadLine();
                string jap = line.Split('\t')[0];
                string eng = line.Split('\t')[1];
                foreach (string ending in possibleFmgEndings) {
                    if (jap + ending == japName) {
                        sr.Close();
                        return eng + ending;
                    }
                }
            }

            sr.Close();
            return japName;
        }

        public static string GetMenuJapaneseFmg(string engName) {
            StreamReader sr = new StreamReader("PureInfo/MenuFMGs.DS3Info");
            while (!sr.EndOfStream) {
                string line = sr.ReadLine();
                string jap = line.Split('\t')[0];
                string eng = line.Split('\t')[1];
                foreach (string ending in possibleFmgEndings) {
                    if (eng + ending == engName) {
                        sr.Close();
                        return jap + ending;
                    }
                }
            }

            sr.Close();
            return engName;
        }

        public static string GetWeaponNameFolder(string name) {
            name = name.Split('.')[0];
            string withoutLevel = name.Substring(0, name.Length - 4);
            return CommonUtils.FindEntryIn("pureInfo/WeaponNames.DS3Info", withoutLevel);
        }

        public static string GetWeaponTypeFolder(string name) {
            name = name.Split('.')[0];
            string withoutLevel = name.Substring(0, name.Length - 4);
            if (withoutLevel.Length == 3) {
                string typeId = withoutLevel.Substring(0, 1);
                return CommonUtils.FindEntryIn("pureInfo/WeaponTypes.DS3Info", typeId);
            } else if (withoutLevel.Length == 4) {
                string typeId = withoutLevel.Substring(0, 2);
                return CommonUtils.FindEntryIn("pureInfo/WeaponTypes.DS3Info", typeId);
            } else {
                return "";
            }
        }

        public static string GetAccessoryNameFolder(string name) {
            name = name.Split('.')[0];
            if (name.Length == 3) {
                return CommonUtils.FindEntryIn("pureInfo/AccessoryNames.DS3Info", name);
            } else if (name.Length == 5) {
                name = name.Substring(0, 4);
                return CommonUtils.FindEntryIn("pureInfo/AccessoryNames.DS3Info", name);
            } else {
                return "";
            }
        }

        public static string GetAreaNameFolder(string name) {
            return CommonUtils.FindEntryIn("pureInfo/AreaNames.DS3Info", name.Split('.')[0]);
        }

        public static string GetAreaTypeFolder(string name) {
            name = name.Split('.')[0];
            if (name.First() == '1' || name.First() == '2') {
                return "Old";
            } else {
                return "DS3";
            }
        }

        public static string GetMagicNameFolder(string name) {
            return CommonUtils.FindEntryIn("pureInfo/MagicNames.DS3Info", name.Split('.')[0]);
        }

        public static string GetMagicType1Folder(string name) {
            name = name.Split('.')[0];
            if (name.Length == 4) {
                return "Old";
            } else {
                return "DS3";
            }
        }

        public static string GetMagicType2Folder(string name) {
            name = name.Split('.')[0];
            if (name.Length == 4) {
                switch (name[0]) {
                    case '3': return "Sorceries";
                    case '4': return "Pyromancies";
                    case '5': return "Miracles";
                    default: return "";
                }
            } else if (name.Length == 7) {
                switch (name[0]) {
                    case '1': return "Sorceries";
                    case '2': return "Pyromancies";
                    case '3': return "Miracles";
                    default: return "";
                }
            } else {
                return "";
            }
        }

        public static string GetNpcNameFolder(string name) {
            name = name.Split('.')[0];
            if (name.Length == 4 || name.Length == 5) {
                return CommonUtils.FindEntryIn("pureInfo/NPCNames.DS3Info", name.Substring(0, name.Length - 2));
            } else {
                return CommonUtils.FindEntryIn("pureInfo/NPCNames.DS3Info", name);
            }
        }

        public static string GetNpcTypeFolder(string name) {
            name = name.Split('.')[0];
            switch (name.Length) {
                case 4:
                case 5:
                    return "Regular NPCs";
                case 6:
                    switch (name[0]) {
                        case '6':
                        case '7':
                            return "Invaders";
                        case '1':
                        case '8':
                            return "Covenant NPCs";
                        case '9':
                            return "Bosses";
                        default: return "";
                    }
                default: return "";
            }
        }

        public static string GetDialogueTypeFolder(string name) {
            name = name.Split('.')[0];
            if (name.Length == 7 || name.Length == 8) {
                name = name.Substring(0, name.Length - 5);
                if (name == "240" || name == "220") {
                    return "Known Boss Dialogue";
                } else {
                    return "Known NPCs";
                }
            } else {
                return "Unmarked dialogue from bosses and cutscenes, and dummy text";
            }
        }

        public static string GetDialogueNpcFolder(string name) {
            name = name.Split('.')[0];
            if (name.Length == 7 || name.Length == 8) {
                name = name.Substring(0, name.Length - 5);
                if (name == "270")
                    return "Caged Hollow";
                if (name == "290")
                    return "Nestling";
                if (name == "240")
                    return "Oceiros, the Consumed King";
                return CommonUtils.FindEntryIn("pureInfo/NPCNames.Ds3Info", name);
            } else {
                return "";
            }
        }

        public static string GetDialogueConversationFolder(string name) {
            name = name.Split('.')[0];
            name = name.Substring(0, name.Length - 2);
            return CommonUtils.FindEntryIn("pureInfo/DialogueNames.DS3Info", name);
        }

        public static string GetDialogueTypeFolder1(string name) {
            return "NPCs and bosses";
        }

        public static string GetDialogueNpcFolder1(string name) {
            name = name.Split('.')[0];
            name = name.Substring(0, name.Length - 5);
            return CommonUtils.FindEntryIn("pureInfo/NPCNames1.Ds3Info", name);
        }

        public static string GetDialogueConversationFolder1(string name) {
            name = name.Split('.')[0];
            name = name.Substring(0, name.Length - 2);
            return CommonUtils.FindEntryIn("pureInfo/DialogueNames1.Ds3Info", name);
        }

        public static string GetDialogueTypeFolder2(string name) {
            return "NPCs and bosses";
        }

        public static string GetDialogueNpcFolder2(string name) {
            name = name.Split('.')[0];
            name = name.Substring(0, name.Length - 5);
            return CommonUtils.FindEntryIn("pureInfo/NPCNames2.Ds3Info", name);
        }

        public static string GetDialogueConversationFolder2(string name) {
            name = name.Split('.')[0];
            name = name.Substring(0, name.Length - 2);
            return CommonUtils.FindEntryIn("pureInfo/DialogueNames2.Ds3Info", name);
        }

        public static string GetGoodsNameFolder(string name) {
            if (name.Split('.')[0].Length == 7) {
                return GetMagicNameFolder(name);
            } else {
                return CommonUtils.FindEntryIn("pureInfo/GoodsNames.DS3Info", name.Split('.')[0]);
            }
        }

        public static string GetGoodsTypeFolder(string name) {
            name = name.Split('.')[0];
            if (name.Length == 7)
                return GetMagicType2Folder(name + ".txt");
            if (name.Length == 3)
                return "Consumables";
            if (name.Length == 4) {
                switch (name[0]) {
                    case '1': return "Upgrade Materials";
                    case '2': return "Key Items";
                    case '9': return "Gestures";
                    default: return "";
                }
            } else {
                return "";
            }
        }

        public static string GetArmorNameFolder(string name) {
            name = name.Split('.')[0];
            if (name.Length == 4) {
                return CommonUtils.FindEntryIn("pureInfo/ArmorNames.DS3Info", "h" + name);
            } else {
                return CommonUtils.FindEntryIn("pureInfo/ArmorNames.DS3Info", name.Substring(0, name.Length - 3));
            }
        }

        public static string GetArmorTypeFolder(string name) {
            name = name.Split('.')[0];
            if (name.Length == 4) {
                return "Hairs";
            } else {
                return "Sets and Pieces";
            }
        }
    }
}
