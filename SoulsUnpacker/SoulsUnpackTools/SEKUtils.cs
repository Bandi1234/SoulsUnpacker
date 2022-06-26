using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsUnpackTools {
    public static class SEKUtils {
        public static string GetAreaNameFolder(string name) {
            return CommonUtils.FindEntryIn("pureInfo/AreaNames.SEKInfo", name.Split('.')[0]);
        }

        public static string GetWeaponNameFolder(string name) {
            return CommonUtils.FindEntryIn("pureInfo/WeaponNames.SEKInfo", name.Split('.')[0]);
        }

        public static string GetWeaponTypeFolder(string name) {
            name = name.Split('.')[0];
            if (name[0] == '1' || (name[0] == '7' && name.Length >= 5)) {
                return "Prosthetic tools";
            } else {
                return "Skills";
            }
        }

        public static string GetArmorNameFolder(string name) {
            return CommonUtils.FindEntryIn("pureInfo/ArmorNames.SEKInfo", name.Split('.')[0]);
        }

        public static string GetGoodsNameFolder(string name) {
            return CommonUtils.FindEntryIn("pureInfo/GoodsNames.SEKInfo", name.Split('.')[0]);
        }

        public static string GetGoodsTypeFolder(string name) {
            name = name.Split('.')[0];
            return CommonUtils.FindEntryIn("pureInfo/GoodsTypes.SEKInfo", name.Substring(0, name.Length - 2));
        }

        public static string GetNPCNameFolder(string name) {
            return CommonUtils.FindEntryIn("pureInfo/NPCNames.SEKInfo", name.Split('.')[0]);
        }

        public static string GetNPCTypeFolder(string name) {
            name = name.Split('.')[0];
            if (name == "900000")
                return "";
            switch (name.Substring(0, 2)) {
                case "90": return "Main bosses";
                case "91": return "Minibosses";
                case "93": return "NPCs";
                case "96": return "Special NPCs";
                case "99": return "Special minibosses";
                default: return "";
            }
        }

        public static string GetDialogueNPCFolder(string name) {
            name = name.Split('.')[0];
            name = name.Substring(0, name.Length - 5);
            return CommonUtils.FindEntryIn("pureInfo/ConversationNPCNames.SEKInfo", name);
        }

        public static string GetDialogueConversationFolder(string name) {
            name = name.Split('.')[0];
            name = name.Substring(0, name.Length - 2);
            return CommonUtils.CropDialogue(CommonUtils.FindEntryIn("pureInfo/ConversationNPCDialogues.SEKInfo", name));
        }
    }
}
