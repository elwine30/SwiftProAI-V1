using Abp.Application.Services.Dto;
using System;

namespace ThinknInsurTech.Registration.Dtos
{
    public class GetAllInsuredPersonsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? IsOwnerFilter { get; set; }

        public int? IsDriverFilter { get; set; }

        public int? IsThirdPartyFilter { get; set; }

        public string RelationshipFilter { get; set; }

        public string NameFilter { get; set; }

        public string IdenticationTypeFilter { get; set; }

        public string IdenticationNoFilter { get; set; }

        public string ContactFilter { get; set; }

        public string NationalityFilter { get; set; }

        public string MakeFilter { get; set; }

        public string ModelFilter { get; set; }

        public string SpecsFilter { get; set; }

        public short? MaxYearFilter { get; set; }
        public short? MinYearFilter { get; set; }

        public double? MaxValuationFilter { get; set; }
        public double? MinValuationFilter { get; set; }

        public decimal? MaxValuationEquiryFilter { get; set; }
        public decimal? MinValuationEquiryFilter { get; set; }

        public string PolicyNoFilter { get; set; }

        public string CoverageFilter { get; set; }

        public string PostcodeFilter { get; set; }

        public string CityFilter { get; set; }

        public string AddressFilter { get; set; }

        public string JpjRegisterNoFilter { get; set; }

        public DateTime? MaxJpjRegisterDateFilter { get; set; }
        public DateTime? MinJpjRegisterDateFilter { get; set; }

        public string OccupationFilter { get; set; }

        public string EmployerNameFilter { get; set; }

        public string EmployerContactFilter { get; set; }

        public string EmployerAddressFilter { get; set; }

        public double? MaxMonthlyIncomeFilter { get; set; }
        public double? MinMonthlyIncomeFilter { get; set; }

        public string LicenseClassesFilter { get; set; }

        public string LicenseNoFilter { get; set; }

        public string DrivingExperienceFilter { get; set; }

        public DateTime? MaxLicenseDateFromFilter { get; set; }
        public DateTime? MinLicenseDateFromFilter { get; set; }

        public DateTime? MaxLicenseDateToFilter { get; set; }
        public DateTime? MinLicenseDateToFilter { get; set; }

        public string MainRegistrationVehicleNoFilter { get; set; }

        public string HospitalNameFilter { get; set; }

        public string LocationNameFilter { get; set; }

        public string LocationName2Filter { get; set; }

    }
}