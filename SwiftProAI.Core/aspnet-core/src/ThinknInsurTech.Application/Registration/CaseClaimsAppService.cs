using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ThinknInsurTech.Authorization;
using ThinknInsurTech.Case;
using ThinknInsurTech.Companies;
using ThinknInsurTech.Companies.Dto;
using ThinknInsurTech.Configuration;
using ThinknInsurTech.Registration.Dtos;
using ThinknInsurTech.Reports.Dtos;
using ThinknInsurTech.Runtime;

namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseClaims)]
    public class CaseClaimsAppService : ThinknInsurTechAppServiceBase, ICaseClaimsAppService
    {
        private readonly IRepository<CaseClaim> _caseClaimRepository;
        private readonly IRepository<Status, int> _lookup_statusRepository;
        private readonly IRepository<CaseSearchFee, int> _caseSearchFeeRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IMapper _mapper;
        private readonly IRepository<MainRegistration, int> _mainRegistrationRepository;
        private readonly IRepository<InsuranceCompany, int> _insuranceCompanyRepository;
        private readonly ICompanyManager _companyManager;

        public CaseClaimsAppService(IMapper mapper, IRepository<CaseClaim> caseClaimRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository, IRepository<Status, int> lookup_statusRepository, IRepository<CaseSearchFee, int> caseSearchFeeRepository, IAppConfigurationAccessor appConfigurationAccessor, IRepository<InsuranceCompany, int> insuranceCompanyRepository, ICompanyManager companyManager)
        {
            _caseClaimRepository = caseClaimRepository;
            _lookup_statusRepository = lookup_statusRepository;
            _caseSearchFeeRepository = caseSearchFeeRepository;
            _appConfiguration = appConfigurationAccessor.Configuration;
            _mainRegistrationRepository = lookup_mainRegistrationRepository;
            _mapper = mapper;
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _companyManager = companyManager;

        }


        [AbpAuthorize(AppPermissions.Pages_CaseClaims_Edit)]
        public virtual async Task<GetCaseClaimForEditOutput> GetCaseClaimForEdit(EntityDto input)
        {
            var caseClaim = _caseClaimRepository.GetAll().Where(w => w.RegisterId.Equals(input.Id)).FirstOrDefault();

            var output = new GetCaseClaimForEditOutput { CaseClaim = ObjectMapper.Map<CreateOrEditCaseClaimDto>(caseClaim) };
            var mileageUnitPrice = this.GetClaimRateByRegisterId(input.Id).Result;
            output.MileageUnitPrice = mileageUnitPrice;

            if (caseClaim != null)
            {
                caseClaim.MileageUnitPrice = mileageUnitPrice;

                if (caseClaim.StatusId != null)
                {
                    var _lookupStatus = await _lookup_statusRepository.FirstOrDefaultAsync((int)output.CaseClaim.StatusId);
                    output.StatusDescription = _lookupStatus?.Description?.ToString();
                }
            }

            return output;
        }

        public async Task<PagedResultDto<CreateOrEditCaseClaimDto>> GetAll(CaseClaimMainRegistrationInput input)
        {

            var filteredMainRegistration = _mainRegistrationRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.VehicleNumber), mr => mr.VehicleNo.Contains(input.VehicleNumber))
                .WhereIf(input.Month != DateTime.MinValue, mr => mr.CreationTime.Month == input.Month.Month)
                .WhereIf(input.Year != DateTime.MinValue, mr => mr.CreationTime.Year == input.Year.Year);

            var query = _caseClaimRepository.GetAll()
                 .Join(filteredMainRegistration,
                       caseClaim => caseClaim.RegisterId,
                       mainRegister => mainRegister.Id,
                      (caseClaim, mainRegister) => new CreateOrEditCaseClaimDto
                      {
                          Id = caseClaim.Id,
                          CaseNo = mainRegister.CaseNo,
                          Total = caseClaim.Total,
                          Location = caseClaim.Location,
                          FileCharges = caseClaim.FileCharges,
                          FileChargesRemark = caseClaim.FileChargesRemark,
                          SD = caseClaim.SD,
                          SearchFee = caseClaim.SearchFee,
                          Hotel = caseClaim.Hotel,
                          Fraud = caseClaim.Fraud,
                          FraudAmount = caseClaim.FraudAmount,
                          Remarks = caseClaim.Remarks,
                          Police = caseClaim.Police,
                          PoliceRemark = caseClaim.PoliceRemark,
                          AirFare = caseClaim.AirFare,
                          AirFareRemark = caseClaim.AirFareRemark,
                          CharteredTransport = caseClaim.CharteredTransport,
                          CharteredTransportRemark = caseClaim.CharteredTransportRemark,
                          Toll = caseClaim.Toll,
                          TollRemark = caseClaim.TollRemark,
                          MileageKM = caseClaim.MileageKM,
                          MileageUnitPrice = caseClaim.MileageUnitPrice,
                          MileageTotal = caseClaim.MileageTotal,
                          RegisterId = caseClaim.RegisterId,
                          StatusId = caseClaim.StatusId
                      });


            var pagedAndFilteredCaseExpenses = query
                .OrderBy(input.Sorting ?? "registerid asc")
                .PageBy(input);
            var caseClaimCount = await pagedAndFilteredCaseExpenses.CountAsync();

            var caseClaimDetails = await pagedAndFilteredCaseExpenses.ToListAsync();

            return new PagedResultDto<CreateOrEditCaseClaimDto>(caseClaimCount, caseClaimDetails);
        }

        public virtual async Task CreateOrEdit(CreateOrEditCaseClaimDto input)
        {
            var searchFeesQuery = _caseSearchFeeRepository.GetAll()
            .Where(w => w.RegisterId.Equals(input.RegisterId))
            .Select(w => w.Amount);

            var searchFeesList = await searchFeesQuery.ToListAsync();

            if (searchFeesList.Any())
            {
                input.SearchFee = searchFeesList.Sum();
            }

            input.MileageTotal = input.MileageKM * input.MileageUnitPrice;

            input.Total = input.FileCharges + input.MileageTotal + input.Police
                + input.AirFare + input.CharteredTransport + input.Toll + input.Hotel
                + input.SD + input.SearchFee + input.FraudAmount;

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CaseClaims_Create)]
        protected virtual async Task Create(CreateOrEditCaseClaimDto input)
        {
            var caseClaim = ObjectMapper.Map<CaseClaim>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                caseClaim.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                caseClaim.TenantId = (int)AbpSession.TenantId;
            }

            await _caseClaimRepository.InsertAsync(caseClaim);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseClaims_Edit)]
        protected virtual async Task Update(CreateOrEditCaseClaimDto input)
        {
            var caseClaim = await _caseClaimRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, caseClaim);

        }

        public async Task<bool> GetCaseClaimsByRegisterId(int registerId)
        {
            var caseClaims = await _caseClaimRepository.CountAsync(x => x.RegisterId.Equals(registerId));
            if (caseClaims > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<decimal> GetClaimRateByRegisterId(int registerId)
        {
            var companyMileageUnitPrice = _mainRegistrationRepository.GetAll()
                .Where(x=>x.Id.Equals(registerId))
                .Join(_insuranceCompanyRepository.GetAll(),
                o=>o.CompanyId,
                o1=>o1.Id,
                (o,o1)=> o1.ClaimRate)
                .FirstOrDefault();
            return companyMileageUnitPrice;
        }


    }
}