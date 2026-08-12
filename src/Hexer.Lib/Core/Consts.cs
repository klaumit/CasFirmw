using Hexer.Compat;

namespace Hexer.Core
{
    public static class Consts
    {
        public const string PvaMark = "505641504C484544563230000001000" +
                                      "0000000000000000000000000000000" +
                                      "0000000000000000000000000000000" +
                                      "0000000000000000000000000010000";

        public const string RldMark = "7F454C460102010000000000000000000" +
                                      "001002A00000001000000000000000000";

        public static byte[] PvaMarkB => NetFx.FromHexString(PvaMark);
        public static byte[] RldMarkB => NetFx.FromHexString(RldMark);
    }
}