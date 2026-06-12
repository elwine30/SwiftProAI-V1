using System.Collections.Generic;
using System.Linq;
using ThinknInsurTech.Common;
using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    internal class RemoveLookupUnneededFolder
    {

        private readonly ThinknInsurTechDbContext _context;

        public RemoveLookupUnneededFolder(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Remove()
        {
            var foldersToDelete = new List<Folder>
            {
                new Folder { MainEntity = "InsuredOwner", Field = "DRI-NRIC-F" },
                new Folder { MainEntity = "InsuredOwner", Field = "DRI-DL-F" },
                new Folder { MainEntity = "InsuredOwner", Field = "DRI-HOSPITAL" },
                new Folder { MainEntity = "InsuredOwner", Field = "DRI-DL-B" },
                new Folder { MainEntity = "InsuredOwner", Field = "DRI-NRIC-B" },
            };

            var allFolders = _context.Folders.ToList();

            var lookupsToDelete = allFolders
                .Where(f => foldersToDelete
                    .Any(d => d.MainEntity == f.MainEntity && d.Field == f.Field))
                .ToList();

            if (lookupsToDelete.Any())
            {
                _context.Folders.RemoveRange(lookupsToDelete);
                _context.SaveChanges();
            }
        }
    }
}
