using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoulsUnpackTools {
    public static class ERUtils {

        public static string GetWeaponTypeFolder(string name) {
            name = name.Split('.')[0];
            string type = "";
            if (name.Length == 7) {
                type = name.Substring(0, 1);
            } else if (name.Length == 8) {
                type = name.Substring(0, 2);
            }
            return CommonUtils.FindEntryIn("pureInfo/WeaponTypes.ERInfo", type);

        }

        public static string GetWeaponNameFolder(string name) {
            name = name.Split('.')[0];
            name = name.Substring(0, name.Length - 4);
            return CommonUtils.FindEntryIn("pureInfo/WeaponNames.ERInfo", name);
        }

        public static string GetArmorSetFolder(string name) {
            name = name.Split('.')[0];
            name = name.Substring(0, name.Length - 4);
            return CommonUtils.FindEntryIn("pureInfo/ArmorSets.ERInfo", name);
        }

        public static string GetArmorNameFolder(string name) {
            name = name.Split('.')[0];
            return CommonUtils.FindEntryIn("pureInfo/ArmorNames.ERInfo", name);
        }

        public static string GetAccessoryNameFolder(string name) {
            name = name.Split('.')[0];
            if (name.Length == 4) {
                name = name.Substring(0, 3);
                return CommonUtils.FindEntryIn("pureInfo/AccessoryNames.ERInfo", name);
            } else if (name == "100") {
                return "Petition for Help";
            } else if (name == "101") {
                return "Broken Finger Stalker Contract";
            }
            return "";
        }

        public static string GetPlaceNameFolder(string name) {
            name = name.Split('.')[0];
            return CommonUtils.FindEntryIn("pureInfo/PlaceNames.ERInfo", name);
        }

        public static string GetNPCTypeFolder(string name) {
            name = name.Split('.')[0];
            if (name.Length < 6) {
                return "Regular";
            } else if (name.First() == '9') {
                return "Bosses";
            } else {
                return "Unseen";
            }
        }

        public static string GetNPCNameFolder(string name) {
            name = name.Split('.')[0];
            return CommonUtils.FindEntryIn("pureInfo/NpcNames.ERInfo", name);
        }

        //TODO goods types

        public static string GetGoodsNameFolder(string name) {
            name = name.Split('.')[0];
            switch (name.Length) {
                case 3:
                case 4:
                case 5:
                    return CommonUtils.FindEntryIn("pureInfo/GoodsNames.ERInfo", name);
                case 6:
                    name = name.Substring(0, 3);
                    return CommonUtils.FindEntryIn("pureInfo/GoodsNames.ERInfo", "A" + name);
                default: 
                    return "";
            }
        }

        public static string GetArtTypeFolder(string name) {
            name = name.Split('.')[0];
            if (name.Length == 4) {
                return "Unique";
            } else {
                return "Regular";
            }
        }

        public static string GetArtNameFolder(string name) {
            name = name.Split('.')[0];
            return CommonUtils.FindEntryIn("pureInfo/ArtNames.ERInfo", name);
        }

        public static string GetGemNameFolder(string name) {
            name = name.Split('.')[0];
            if (name.Length < 3) {
                return "";
            }
            name = name.Substring(0, 3);
            return CommonUtils.FindEntryIn("pureInfo/ArtNames.ERInfo", name);
        }
    }
}
