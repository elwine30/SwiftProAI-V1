using Abp.Application.Services.Dto;

namespace ThinknInsurTech.Common.Dtos
{
    public class GetAllFoldersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string MainEntityFilter { get; set; }

        public string FieldFilter { get; set; }

        public int? MainEntityIdFilter { get; set; }

    }
}