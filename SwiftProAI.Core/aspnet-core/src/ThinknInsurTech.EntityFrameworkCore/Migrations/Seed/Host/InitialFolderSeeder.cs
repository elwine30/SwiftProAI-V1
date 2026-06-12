using System.Collections.Generic;
using System.Linq;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialFolderSeeder
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialFolderSeeder(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            var folders = new List<Folder>
            {
                new Folder {  MainEntity = "AssignmentSheet", Field = "ASSG-SHEET-DOCUMENT" },
                new Folder {  MainEntity = "IncidentDetails", Field = "INC-DET" },

                new Folder {  MainEntity = "InsuredDriver", Field = "DRI-HOSPITAL" },
                new Folder {  MainEntity = "InsuredDriver", Field = "DRI-DL-B" },
                new Folder {  MainEntity = "InsuredDriver", Field = "DRI-NRIC-B" },
                new Folder {  MainEntity = "InsuredDriver", Field = "DRI-DL-F" },
                new Folder {  MainEntity = "InsuredDriver", Field = "DRI-NRIC-F" },
                new Folder {  MainEntity = "InsuredDriver", Field = "DRI-EMP" },
                new Folder {  MainEntity = "InsuredDriver", Field = "DRI-CAR-GRANT" },
                new Folder {  MainEntity = "InsuredOwner", Field = "INS-DL-F" },
                new Folder {  MainEntity = "InsuredOwner", Field = "INS-NRIC-F" },
                new Folder {  MainEntity = "InsuredOwner", Field = "INS-EMP" },
                new Folder {  MainEntity = "InsuredOwner", Field = "INS-HOSPITAL" },
                new Folder {  MainEntity = "InsuredOwner", Field = "INS-DL-B" },
                new Folder {  MainEntity = "InsuredOwner", Field = "INS-NRIC-B" },
                new Folder {  MainEntity = "InsuredOwner", Field = "INS-CAR-GRANT" },
                new Folder {  MainEntity = "Insurer", Field = "INSURER-DET" },
                new Folder {  MainEntity = "Lawyer", Field = "LAW-REP" },
                new Folder {  MainEntity = "PoliceReport", Field = "POL-REP-INS" },
                new Folder {  MainEntity = "PoliceReport", Field = "POL-REP-DRI" },
                new Folder {  MainEntity = "Workshop", Field = "WORKSHOP-REP" },
                new Folder {  MainEntity = "Others", Field = "OTHER-REP" },
                new Folder {  MainEntity = "PoliceReport", Field = "CLM-POL-REP" },
                new Folder {  MainEntity = "PoliceReport", Field = "TRP-POL-REP" }
            };

            foreach (var folder in folders)
            {
                var existingFolder = _context.Folders
                    .FirstOrDefault(f => f.MainEntity == folder.MainEntity && f.Field == folder.Field);

                if (existingFolder == null)
                {
                    _context.Folders.Add(folder);
                }
            }
        }
    }
}
