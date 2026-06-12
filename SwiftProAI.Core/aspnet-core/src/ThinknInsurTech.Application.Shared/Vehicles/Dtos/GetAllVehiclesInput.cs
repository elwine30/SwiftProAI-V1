using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Vehicles.Dtos
{
    public class GetAllVehiclesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string MakeFilter { get; set; }

        public string ModelFilter { get; set; }

        public string SpecificationFilter { get; set; }

    }
}