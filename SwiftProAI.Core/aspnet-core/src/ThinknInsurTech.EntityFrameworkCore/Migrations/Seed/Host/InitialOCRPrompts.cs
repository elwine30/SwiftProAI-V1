using System.Linq;
using ThinknInsurTech.EntityFrameworkCore;
using ThinknInsurTech.OCR;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialOCRPrompts
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialOCRPrompts(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        //public string Code { get; set; }

        //public string Description { get; set; }

        //public string Closeflag { get; set; }

        //public string Type { get; set; }

        public void Create()
        {
            var prp = _context.Prompts.FirstOrDefault(p => p.PromptName == "PoliceReport");
            if (prp == null)
            {
                _context.Prompts.Add(
                    new Prompt
                    {
                        PromptName = "PoliceReport",
                        PromptRequest = "You are an admin clerk. You would be provided with a scanned police report which is written in Malay. Extract the following info from the report into a JSON object using camel case format without any extra string before and after the object { }. The incident date time is usually located in the summary.\r\n\r\nPolice station\r\nOfficer name\r\nOfficer service no\r\nReport no\r\nReport datetime\r\nIncident datetime (Retrieve from the report summary)\r\nReporter name\r\nOrganization name\r\nIncident location\r\nSummary of incident reported\r\nNature of incident reported\r\nVehicle registration numbers\r\nVehicle models\r\nVehicle colours\r\nIdentityNo\r\nArmedForceId\r\nPassportNo\r\n. Please extract the IdentityNo, ArmedForceId, PassportNo from the driver details. For any data fields that are not available, set as \"NA\". Return vehicle registration numbers, vehicle models, vehicle colours in an array of strings. Return Report datetime and Incident datetime in this datetime format 'dd/MM/yyyy HH:mm'. Return the summary of the incident reported from a third-party narrative perspective, with car plate details included for each vehicle involved, translated to English."
                    });
            }

            var icfp = _context.Prompts.FirstOrDefault(p => p.PromptName == "ICFront");
            if (icfp == null)
            {
                _context.Prompts.Add(
                   new Prompt
                   {
                       PromptName = "ICFront",
                       PromptRequest = "You are an admin clerk tasked with extracting details from a scanned image of an Identity Card (IC). Before extracting the details, check the orientation of the IC image and correct any upside-down, mirrored, or rotated orientations.\\r\\n\r\n\r\nThe accurate image can be described that the KAD PENGENALAN MALAYSIA IDENTITY CARD box is at upper left, Malaysia flag on upper right, Identity photo will be below the malaysia flag and the citizenship type is below the identity photo. \r\n\r\nOnce it is fixed to the right orientation use the accurate and corrected image to extract the following info from the IC into a JSON object using camel case format without any extra string before and after the object { }. If the image is not the Front IC document as described you may just set the isAccurateDocument as false and other fields Set to null .\\r\\n\\r\\nIdentity number\\r\\nIdentityType (IC/Passport/CompanyID/ArmedForceID/Old IC)\\r\\nFull name\\r\\nAddress line\\r\\nPostcode\\r\\nCity\\r\\nState\\r\\nCountry\\r\\nNationality (MALAYSIAN?)\\r\\nGender (MALE/FEMALE)\\r\\nisAccurateDocument\\r\\n. For Country, can you determine the country name at the left top badge or by the flag at the top right corner?\\r\\nIdentity Number value will be below the KAD PENGENALAN MALAYSIA IDENTITY CARD title box and above a rectangular chip look alike\\r\\nFull name will be below of the rectangular chip look alike and is usually 1-2 lines and has a lighter text color compared to the address\\r\\nAddress will be below of the name which is darker in text color compared to the full name\\r\\nFor any data fields that are not available, set as \"NA\"."
                   });
            }
            else
            {
                icfp.PromptRequest = "You are an admin clerk tasked with extracting details from a scanned image of an Identity Card (IC). Before extracting the details, check the orientation of the IC image and correct any upside-down, mirrored, or rotated orientations.\\r\\n\r\n\r\nThe accurate image can be described that the KAD PENGENALAN MALAYSIA IDENTITY CARD box is at upper left, Malaysia flag on upper right, Identity photo will be below the malaysia flag and the citizenship type is below the identity photo. \r\n\r\nOnce it is fixed to the right orientation use the accurate and corrected image to extract the following info from the IC into a JSON object using camel case format without any extra string before and after the object { }. If the image is not the Front IC document as described you may just set the isAccurateDocument as false and other fields Set to null .\\r\\n\\r\\nIdentity number\\r\\nIdentityType (IC/Passport/CompanyID/ArmedForceID/Old IC)\\r\\nFull name\\r\\nAddress line\\r\\nPostcode\\r\\nCity\\r\\nState\\r\\nCountry\\r\\nNationality (MALAYSIAN?)\\r\\nGender (MALE/FEMALE)\\r\\nisAccurateDocument\\r\\n. For Country, can you determine the country name at the left top badge or by the flag at the top right corner?\\r\\nIdentity Number value will be below the KAD PENGENALAN MALAYSIA IDENTITY CARD title box and above a rectangular chip look alike\\r\\nFull name will be below of the rectangular chip look alike and is usually 1-2 lines and has a lighter text color compared to the address\\r\\nAddress will be below of the name which is darker in text color compared to the full name\\r\\nFor any data fields that are not available, set as \"NA\".";
            }

            var lfp = _context.Prompts.FirstOrDefault(p => p.PromptName == "LicenseFront");
            if (lfp == null)
            {
                _context.Prompts.Add(
                   new Prompt
                   {
                       PromptName = "LicenseFront",
                       PromptRequest = "As an admin clerk, you will be provided with a scanned image of the front side of a Malaysian driving license. \\r\\nBefore extracting any details, check the orientation of the driving license image and correct any upside-down, mirrored, or rotated orientations. \\r\\n\r\n\r\nThe accurate image in the correct orientation will have the Perlembagaan Malaysia logo at the upper left, the title LESEN MEMANDU beside it to the right, followed by the Malaysian flag. Below the Perlembagaan Malaysia logo, there should be the persons identity photo. \\r\\n\r\n\r\nOnce the image is properly oriented, extract the following information and return it in a JSON object using camel case format, without any additional text before or after the object { } If the image is not the driving license document as described you may just set the isAccurateDocument as false and other fields Set to null . \\nLicense classes\\nLicense date from\\nLicense date to\\nIdentity number\\nisAccurateDocument.\r\n\r\nIf any data fields are missing, set their value to NA. For the LicenseDateFrom and LicenseDateTo field, please format in dd/mm/yyyy.Please return License classes in an array of string.",
                   });
            }
            else
            {
                lfp.PromptRequest = "As an admin clerk, you will be provided with a scanned image of the front side of a Malaysian driving license. \\r\\nBefore extracting any details, check the orientation of the driving license image and correct any upside-down, mirrored, or rotated orientations. \\r\\n\r\n\r\nThe accurate image in the correct orientation will have the Perlembagaan Malaysia logo at the upper left, the title LESEN MEMANDU beside it to the right, followed by the Malaysian flag. Below the Perlembagaan Malaysia logo, there should be the persons identity photo. \\r\\n\r\n\r\nOnce the image is properly oriented, extract the following information and return it in a JSON object using camel case format, without any additional text before or after the object { } If the image is not the driving license document as described you may just set the isAccurateDocument as false and other fields Set to null . \\nLicense classes\\nLicense date from\\nLicense date to\\nIdentity number\\nisAccurateDocument.\r\n\r\nIf any data fields are missing, set their value to NA. For the LicenseDateFrom and LicenseDateTo field, please format in dd/mm/yyyy.Please return License classes in an array of string.";
            }

            var lbp = _context.Prompts.FirstOrDefault(p => p.PromptName == "LicenseBack");
            if (lbp == null)
            {
                _context.Prompts.Add(
                   new Prompt
                   {
                       PromptName = "LicenseBack",
                       PromptRequest = "You are an admin clerk. You would be provided with a scanned malaysian driving license at the back side of the card. Before extracting the details, check the orientation of the driving license image and correct any upside-down, mirrored, or rotated orientations.\\r\\n The accurate image can be described that the lower right corner should have the Ketua Pengarah Pengakutan Jalan wording , then will have a hibiscus flower look a like icon above the wording. \r\n\r\nOnce it is fixed to the right orientation use the accurate and corrected image to extract the following info from the driving license into a JSON object using camel case format without any extra string before and after the object { }. If the image is not the back driving license document as described you may just set the isAccurateDocument as false and other fields Set to null. Please extract the License No\\nisAccurateDocument from the top right of the card into a JSON object using camel case format without any extra string before and after the object { }. For any data fields that is not available, set as \"NA\"."
                   });
            }
            else
            {
                lbp.PromptRequest = "You are an admin clerk. You would be provided with a scanned malaysian driving license at the back side of the card. Before extracting the details, check the orientation of the driving license image and correct any upside-down, mirrored, or rotated orientations.\\r\\n The accurate image can be described that the lower right corner should have the Ketua Pengarah Pengakutan Jalan wording , then will have a hibiscus flower look a like icon above the wording. \r\n\r\nOnce it is fixed to the right orientation use the accurate and corrected image to extract the following info from the driving license into a JSON object using camel case format without any extra string before and after the object { }. If the image is not the back driving license document as described you may just set the isAccurateDocument as false and other fields Set to null. Please extract the License No\\nisAccurateDocument from the top right of the card into a JSON object using camel case format without any extra string before and after the object { }. For any data fields that is not available, set as \"NA\".";
            }

            var hdp = _context.Prompts.FirstOrDefault(p => p.PromptName == "HospitalDetail");
            if (hdp == null)
            {
                _context.Prompts.Add(
                   new Prompt
                   {
                       PromptName = "HospitalDetail",
                       PromptRequest = ""
                   });
            }

            var edp = _context.Prompts.FirstOrDefault(p => p.PromptName == "EmploymentDetail");
            if (edp == null)
            {
                _context.Prompts.Add(
                   new Prompt
                   {
                       PromptName = "EmploymentDetail",
                       PromptRequest = "You are an admin clerk. You would be provided with an employment letter. Please extract the following info from the report into a JSON object using camel case format without any extra string before and after the object { }.\\n' + 'a. Employer Name\\n' + 'b. Employer Contact\\n' + 'c. Employer Address\\n' + 'd. Monthly Income\\n'+ 'For any data fields that is not available, set as \"NA\". '."
                   });
            }

            var cgp = _context.Prompts.FirstOrDefault(p => p.PromptName == "CarGrant");
            if (cgp == null)
            {
                _context.Prompts.Add(
                   new Prompt
                   {
                       PromptName = "CarGrant",
                       PromptRequest = "You are an admin clerk. You would be provided with a scanned Malaysian car grant image in Bahasa Malaysia. Extract the following info from the car grant into a JSON object using camel case format without any extra string before and after the object { }.\\r\\n\\r\\nMake\\r\\nModel\\r\\nSpecs\\r\\nYear\\r\\nJPJRegisterNo\\r\\nJPJRegisterDate\\r\\n\\r\\n.For any data fields that are not available, set as \"NA\"."
                   });
            }

            var grs = _context.Prompts.FirstOrDefault(p => p.PromptName == "GeneratePoliceReportSummary");
            if (grs == null)
            {
                _context.Prompts.Add(
                    new Prompt
                    {
                        PromptName = "GeneratePoliceReportSummary",
                        PromptRequest = "You are an admin clerk. You will be provided with several police report statements with the report category. With the statements, write an overall summary of the incident(reportSummary), and find any inconsistencies or discrepancies between each statement and format the inconsistencies to be display the title and below, the description (discrepancies) in an array format. Return your findings into a JSON object using camel case format without any extra string before and after the object, and any dateTime information should be using 'dd/MM/yyyy' format. The summary should be written in a third party narrative perspective.\r\nThe statements from the police report are : \n\n"
                    });
            }

        }
    }
}
