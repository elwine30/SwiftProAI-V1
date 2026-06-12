using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Common.Dtos
{
    public class FolderDto : EntityDto
    {
        public string MainEntity { get; set; }

        public string Field { get; set; }

        public int MainEntityId { get; set; }

    }
}