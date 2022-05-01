using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SoulsUnpackTools {
    public static class DSRUtils {
        public static string GetWeaponTypeFolder(string fileName) {
            fileName = fileName.Split('.')[0];
            string withoutLevel = fileName.Substring(0, fileName.Length - 3);
            string mainType = "";
            string subType = "";
            if (withoutLevel.Length == 4) {
                mainType = withoutLevel.Substring(0, 2);
                switch (withoutLevel[2]) {
                    case '3':
                    case '5':
                    case '6':
                    case '7':
                    case '9':
                        subType = "" + withoutLevel[2];
                        break;
                }
            } else {
                switch (withoutLevel[1]) {
                    case '3':
                    case '5':
                    case '6':
                    case '7':
                    case '9':
                        subType = "" + withoutLevel[1];
                        break;
                }
                mainType = withoutLevel.Substring(0, 1);
            }
            using (StreamReader reader = new StreamReader("pureInfo/WeaponTypes.DSRInfo")) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string type = line.Split('\t')[0];
                    string name = line.Split('\t')[1];
                    if (type == mainType || type == mainType + subType) {
                        return name;
                    }
                }
            }

            return "";
        }

        public static string GetWeaponNameFolder(string fileName) {
            fileName = fileName.Split('.')[0];
            string withoutLevel = fileName.Substring(0, fileName.Length - 3);

            return FindEntryIn("pureInfo/WeaponNames.DSRInfo", withoutLevel);
        }

        public static string GetPlaceTypeFolder(string fileName) {
            string type = fileName.Substring(0, 1);
            if (type == "1") {
                return "Location";
            } else if (type == "2") {
                return "Warp_Point";
            }
            return "";
        }

        public static string GetPlaceNameFolder(string fileName) {
            fileName = fileName.Split('.')[0];

            return FindEntryIn("pureInfo/PlaceNames.DSRInfo", fileName);
        }

        public static string GetAccessoryNameFolder(string fileName) {
            fileName = fileName.Split('.')[0];

            return FindEntryIn("pureInfo/AccessoryNames.DSRInfo", fileName);
        }

        public static string GetNpcTypeFolder(string fileName) {
            fileName = fileName.Split('.')[0];
            if (fileName.Length == 4) {
                return "Boss_Name";
            } else {
                return "Summonable_Phantom";
            }
        }

        public static string GetNpcNameFolder(string fileName) {
            fileName = fileName.Split('.')[0];

            return FindEntryIn("pureInfo/NpcNames.DSRInfo", fileName);
        }

        public static string GetMagicTypeFolder(string fileName) {
            string typeChar = fileName.Substring(0, 1);

            return FindEntryIn("pureInfo/MagicTypes.DSRInfo", typeChar);
        }

        public static string GetMagicNameFolder(string fileName) {
            fileName = fileName.Split('.')[0];

            return FindEntryIn("pureInfo/MagicNames.DSRInfo", fileName);
        }

        public static string GetArmorTypeFolder(string fileName) {
            if (!fileName.EndsWith(".txt")) {
                return "";
            }
            fileName = fileName.Split('.')[0];

            if (fileName.Length == 4) {
                return "Hairs";
            }

            string withoutLevelPiece = fileName.Substring(0, fileName.Length - 4);

            if (int.Parse(withoutLevelPiece) < 80) {
                return "Armor_Sets";
            } else {
                return "Other";
            }
        }

        public static string GetArmorNameFolder(string fileName) {
            if (!fileName.EndsWith(".txt")) {
                return "";
            }
            fileName = fileName.Split('.')[0];

            string searchId = "";
            if (fileName.Length == 4) {
                searchId = "h" + fileName.Substring(0, 2);
            } else {
                searchId = fileName.Substring(0, fileName.Length - 3);
            }

            return FindEntryIn("pureInfo/ArmorNames.DSRInfo", searchId);
        }

        public static string GetItemTypeFolder(string fileName) {
            fileName = fileName.Split('.')[0];

            string typeId = "";
            if (fileName.Length == 3) {
                typeId = fileName.Substring(0, 1);
            } else {
                typeId = fileName.Substring(0, 2);
            }

            return FindEntryIn("pureInfo/ItemTypes.DSRInfo", typeId);

        }

        public static string GetItemNameFolder(string fileName) {
            fileName = fileName.Split('.')[0];

            return FindEntryIn("pureInfo/ItemNames.DSRInfo", fileName);
        }

        public static string GetMovSubFolder(string fileName) {
            fileName = fileName.Split('.')[0];

            if (fileName.Length == 3) {
                return "Intro";
            } else if (fileName[0] == '1') {
                return "Yes_Indeed";
            } else if (fileName[0] == '2') {
                return "Leave_The_Asylum";
            } else {
                return "";
            }
        }

        public static string GetConversationNPC(string fileName) {
            if (!fileName.EndsWith(".txt"))
                return "";

            fileName = fileName.Split('.')[0];
            if (fileName.Length == 4) {
                return "Extra";
            }
            string npcId = fileName.Substring(0, 2);

            return FindEntryIn("pureInfo/ConversationNpcNames.DSRInfo", npcId);
        }

        public static string GetConversationDialogue(string fileName) {
            if (!fileName.EndsWith(".txt"))
                return "";

            fileName = fileName.Split('.')[0];
            string diaId = fileName.Substring(0, fileName.Length - 2);

            return FindEntryIn("pureInfo/ConversationNpcDialogues.DSRInfo", diaId);
        }

        public static string FindEntryIn(string infoFile, string entryName) {
            using (StreamReader reader = new StreamReader(infoFile)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string id = line.Split('\t')[0];
                    string name = line.Split('\t')[1];
                    if (id == entryName) {
                        return name
                            .Replace(' ', '_')
                            .Replace('/', '_')
                            .Replace('\\', '_')
                            .Replace(":", "")
                            .Replace(".", "")
                            .Replace(",", "")
                            .Replace("?", "")
                            .Replace("!", "");
                    }
                }
            }
            return "";
        }
    }
}
