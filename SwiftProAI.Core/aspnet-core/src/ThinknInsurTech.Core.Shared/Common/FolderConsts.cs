using System.Collections.Generic;

namespace ThinknInsurTech.Common
{
    public class FolderConsts
    {
        public const string ThirdPartyInfoMainEntity = "ThirdPartyInfo";
        public static List<string> ThirdPartyFields = new List<string>
        {
            "THP-EMP",
            "THP-DET",
            "THP-DL-DET-B",
            "THP-DL-DET-F",
            "THP-DL-NRIC-B",
            "THP-NRIC-DET-F",
            "THP-NOI-DET",
            "THP-HOSPITAL"
        };
        public enum ThirdPartyInfoEnum
        {
            THP_EMP = 0,
            THP_DET = 1,
            THP_DL_DET_B = 2,
            THP_DL_DET_F = 3,
            THP_DL_NRIC_B = 4,
            THP_NRICAL_DET_F = 5,
            THP_NOI_DET = 6,
            THP_HOSPITAL = 7
        }

        public const string ThirdPartyVehicleMainEntity = "ThirdPartyVehicle";
        public static List<string> ThirdPartyVehicleFields = new List<string>
        {
            "TPV-CAR-GRANT",
            "TPV-DET"

        };
    }
}
