using System;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Utils
{

    public class Common
    {

        public static String GetRandomGUID()
        {

            Guid randomGuid = Guid.NewGuid();
            byte[] bytes = randomGuid.ToByteArray();
            return Convert.ToBase64String(bytes);

        }

    }

}
