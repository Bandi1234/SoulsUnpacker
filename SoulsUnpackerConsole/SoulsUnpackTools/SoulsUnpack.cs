using System;
using SoulsFormats;

namespace SoulsUnpackTools {
    public static class SoulsUnpack {
        public static void SayHello() {
            Console.WriteLine("Hello!");
            SaySouls();
        }

        internal static void SaySouls() {
            BND3 bnd3 = new BND3();
            Console.WriteLine(bnd3.Version);
        }
    }
}
