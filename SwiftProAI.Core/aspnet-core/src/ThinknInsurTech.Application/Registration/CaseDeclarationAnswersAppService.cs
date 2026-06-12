using ThinknInsurTech.Registration;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ThinknInsurTech.Registration.Dtos;
using Abp.Application.Services.Dto;
using ThinknInsurTech.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ThinknInsurTech.Runtime;
using Abp.Domain.Uow;
using Abp.Authorization.Users;


namespace ThinknInsurTech.Registration
{
    [AbpAuthorize(AppPermissions.Pages_CaseDeclarationAnswers)]
    public class CaseDeclarationAnswersAppService : ThinknInsurTechAppServiceBase, ICaseDeclarationAnswersAppService
    {
        private readonly IRepository<CaseDeclarationAnswer> _caseDeclarationAnswerRepository;
        private readonly IRepository<MainRegistration, int> _lookup_mainRegistrationRepository;
        private readonly IRepository<DeclarationQuestion, int> _declarationQuestionRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CaseDeclarationAnswersAppService(IRepository<CaseDeclarationAnswer> caseDeclarationAnswerRepository, IRepository<MainRegistration, int> lookup_mainRegistrationRepository, IRepository<DeclarationQuestion, int> declarationQuestionRepository, IRepository<UserOrganizationUnit,long> userOrganizationUnitRepository ,IUnitOfWorkManager unitOfWorkManager)
        {
            _caseDeclarationAnswerRepository = caseDeclarationAnswerRepository;
            _lookup_mainRegistrationRepository = lookup_mainRegistrationRepository;
            _declarationQuestionRepository = declarationQuestionRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [AbpAuthorize(AppPermissions.Pages_CaseDeclarationAnswers_Edit)]
        public virtual async Task<GetCaseDeclarationAnswerForEditOutput> GetCaseDeclarationAnswerForEdit(EntityDto input)
        {
            var currentOUId = AbpSession.GetCurrentOUId();
            var declarationQuestions = _declarationQuestionRepository.GetAll().Where(d => d.OrganizationUnitId == currentOUId);
            var caseDeclarationAnswers = _caseDeclarationAnswerRepository.GetAll().Where(w => w.RegisterId == input.Id);

            var sortedDeclarationQuestions = declarationQuestions.OrderBy("id asc");

            var queriedDeclarationQuestions = from o in sortedDeclarationQuestions
                                              join w in caseDeclarationAnswers on o.Id equals w.QuestionId into gj
                                              from subcda in gj.DefaultIfEmpty()
                                              select new
                                              {
                                                  o.Question,
                                                  o.OptionType,
                                                  o.OptionValues,
                                                  o.Id,
                                                  Answer = subcda == null ? "" : subcda.Answer,
                                                  AnswerId = subcda == null ? (int?)null : subcda.Id
                                              };

            var questionCount = await sortedDeclarationQuestions.CountAsync();
            var dbList = await queriedDeclarationQuestions.ToListAsync();
            var declarationQuestionsAnswersList = new List<DeclarationQuestionAnswerDto>();

            foreach (var o in dbList)
            {
                var res = new DeclarationQuestionAnswerDto
                {
                    Question = o.Question,
                    OptionType = o.OptionType,
                    OptionValues = o.OptionValues,
                    QuestionId = o.Id,
                    Answer = o.Answer, 
                    AnswerId = o.AnswerId 
                };

                declarationQuestionsAnswersList.Add(res);
            }

            return new GetCaseDeclarationAnswerForEditOutput()
            {
                DeclarationQuestionAnswerList = declarationQuestionsAnswersList,
                QuestionCount = questionCount,
                AnswerCount = declarationQuestionsAnswersList.Count(dq => dq.Answer != "" )
            };
        }

        public virtual async Task<GetCaseDeclarationAnswerForEditOutput> GetCaseDeclarationAnswerForView(EntityDto input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(ThinknInsurTechDataFilters.HaveOrganizationUnit))
            {
                //Get AdjusterMemberId from mainRegistration, find the OU_Id of that adjuster
                var adjusterMemberOrganizationUnitId = (from m in _lookup_mainRegistrationRepository.GetAll()
                                                        join uou in _userOrganizationUnitRepository.GetAll()
                                                        on m.AdjusterMemberId equals uou.UserId
                                                        where m.Id == input.Id
                                                        select uou.OrganizationUnitId)
                                       .FirstOrDefault();

                var declarationQuestions = _declarationQuestionRepository.GetAll().Where(x=>x.OrganizationUnitId == adjusterMemberOrganizationUnitId);
                var caseDeclarationAnswers = _caseDeclarationAnswerRepository.GetAll().Where(w => w.RegisterId == input.Id);

                var sortedDeclarationQuestions = declarationQuestions.OrderBy("id asc");

                var queriedDeclarationQuestions = from o in sortedDeclarationQuestions
                                                  join w in caseDeclarationAnswers on o.Id equals w.QuestionId into gj
                                                  from subcda in gj.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      o.Question,
                                                      o.OptionType,
                                                      o.OptionValues,
                                                      o.Id,
                                                      Answer = subcda == null ? "" : subcda.Answer,
                                                      AnswerId = subcda == null ? (int?)null : subcda.Id
                                                  };

                var questionCount = await sortedDeclarationQuestions.CountAsync();
                var dbList = await queriedDeclarationQuestions.ToListAsync();
                var declarationQuestionsAnswersList = new List<DeclarationQuestionAnswerDto>();

                foreach (var o in dbList)
                {
                    var res = new DeclarationQuestionAnswerDto
                    {
                        Question = o.Question,
                        OptionType = o.OptionType,
                        OptionValues = o.OptionValues,
                        QuestionId = o.Id,
                        Answer = o.Answer,
                        AnswerId = o.AnswerId
                    };

                    declarationQuestionsAnswersList.Add(res);
                }

                return new GetCaseDeclarationAnswerForEditOutput()
                {
                    DeclarationQuestionAnswerList = declarationQuestionsAnswersList,
                    QuestionCount = questionCount,
                    AnswerCount = declarationQuestionsAnswersList.Count(dq => dq.Answer != "")
                };
            }
        }



        public virtual async Task CreateOrEdit(List<CreateOrEditCaseDeclarationAnswerDto> inputList)
        {
            foreach (var input in inputList)
            {
                if (input.Id == null)
                {
                    if (string.IsNullOrEmpty(input.Answer))
                    {
                        continue; 
                    }
                    await Create(input);
                }
                else
                {
                    await Update(input);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CaseDeclarationAnswers_Create)]
        protected virtual async Task Create(CreateOrEditCaseDeclarationAnswerDto input)
        {
            var caseDeclarationAnswer = ObjectMapper.Map<CaseDeclarationAnswer>(input);

            // Check if the user belongs to any organization unit
            if (AbpSession.GetCurrentOUId() != null)
            {
                caseDeclarationAnswer.OrganizationUnitId = AbpSession.GetCurrentOUId().Value;

            }

            if (AbpSession.TenantId != null)
            {
                caseDeclarationAnswer.TenantId = (int)AbpSession.TenantId;
            }

            await _caseDeclarationAnswerRepository.InsertAsync(caseDeclarationAnswer);

        }

        [AbpAuthorize(AppPermissions.Pages_CaseDeclarationAnswers_Edit)]
        protected virtual async Task Update(CreateOrEditCaseDeclarationAnswerDto input)
        {
            var caseDeclarationAnswer = await _caseDeclarationAnswerRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, caseDeclarationAnswer);

        }


    }
}