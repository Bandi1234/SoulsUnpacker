using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SoulsUnpackTools {
    public static class DS2Utils {
        public static string GetItemBaseType(string name) { 
            name = name.Split('.')[0];
            int l = name.Length;
            char f = name[0];
            if (l == 7 || f == '9') {
                return "Weapons";
            }
            if (f == '8') {
                return "Armor";
            }
            if (l == 8) {
                switch (f) {
                    case '1': return "Shields";
                    case '2': return "Armor";
                    case '3': return "Spells";
                    case '4': return "Rings";
                    case '5': return "Key items";
                    case '6': return "Goods";
                }
            }
            return "";
        }

        public static string GetItemSubType(string name, string bType) {
            name = name.Split('.')[0];
            char c2 = name[1];
            switch (bType) {
                case "Weapons":
                    string wType = CommonUtils.FindEntryIn("pureInfo/WeaponTypes.DS2Info", name.Substring(0, 2));
                    if (wType == "") {
                        return "Misc";
                    } else {
                        return wType;
                    }
                case "Armor": return CommonUtils.FindEntryIn("pureInfo/ArmorTypes.DS2Info", name.Substring(0, name.Length - 1));
                case "Spells":
                    switch (c2) {
                        case '1': return "Sorceries";
                        case '2': return "Miracles";
                        case '3': return "Pyromancies";
                        case '4': return "Staff hexes";
                        case '5': return "Chime hexes";
                    }
                    return "";
                case "Goods":
                    switch (c2) {
                        case '0': return "Consumables";
                        case '1': return "Upgrade materials";
                        case '2': return "Online items";
                        case '3': return "Gestures";
                        case '4': return "Boss souls";
                    }
                    return "";
            }
            return "";
        }

        public static string GetItemName(string name, string bType) {
            name = name.Split('.')[0];
            switch (bType) {
                case "Rings": return CommonUtils.FindEntryIn("pureInfo/RingNames.DS2Info", name.Substring(0, name.Length - 1));
                case "Armor": return CommonUtils.FindEntryIn("pureInfo/ArmorNames.DS2Info", name);
                case "Goods": return CommonUtils.FindEntryIn("pureInfo/GoodsNames.DS2Info", name);
                case "Key items": return CommonUtils.FindEntryIn("pureInfo/KeyItemNames.DS2Info", name);
                case "Shields:": return CommonUtils.FindEntryIn("pureInfo/ShieldNames.DS2Info", name);
                case "Spells": return CommonUtils.FindEntryIn("pureInfo/SpellNames.DS2Info", name);
                case "Weapons": return CommonUtils.FindEntryIn("pureInfo/WeaponNames.DS2Info", name);
            }
            return "";
            
        }

        public static string GetBonfireName(string name) {
            name = name.Split('.')[0];
            return CommonUtils.FindEntryIn("pureInfo/BonfireNames.DS2Info", name);
        }

        public static string GetMapName(string name) {
            name = name.Split('.')[0];
            return CommonUtils.FindEntryIn("pureInfo/MapNames.DS2Info", name);
        }

        public static string GetConversationDiaName(string name) {
            name = name.Split('.')[0];
            string convDia = name.Substring(0, name.Length - 2);
            return CommonUtils.FindEntryIn("pureInfo/ConversationNPCDialogues.DS2Info", convDia);
        }

        public static string GetConversationNpcName(string name) {
            name = name.Split('.')[0];
            string convNpc = name.Substring(0, 3);
            return CommonUtils.FindEntryIn("pureInfo/ConversationNPCNames.DS2Info", convNpc);
        }

        public static string GetBossName(string name) {
            name = name.Split('.')[0];
            return CommonUtils.FindEntryIn("pureInfo/BossNames.DS2Info", name);
        }
    }
}
