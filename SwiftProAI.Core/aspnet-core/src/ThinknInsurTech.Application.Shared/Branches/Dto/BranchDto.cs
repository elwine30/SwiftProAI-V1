using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Branches.Dto
{
    public class BranchDto : EntityDto
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

    }
}
