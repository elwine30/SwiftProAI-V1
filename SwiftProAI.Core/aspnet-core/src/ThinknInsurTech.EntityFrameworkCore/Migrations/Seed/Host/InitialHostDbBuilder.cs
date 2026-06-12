using ThinknInsurTech.EntityFrameworkCore;

namespace ThinknInsurTech.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly ThinknInsurTechDbContext _context;

        public InitialHostDbBuilder(ThinknInsurTechDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new InitialCaseTypeCreator(_context).Create();
            new InitialBranchCreator(_context).Create();
            new InitialStatusCreator(_context).Create();
            new InitialLocationCreator(_context).Create();
            new InitialDeclarationQuestionCreator(_context).Create();
            new InitialLookupExpensesStatus(_context).Create();
            new InitialLookupExpensesType(_context).Create();
            new InitialLookupMaritalStatus(_context).Create();
            new InitialLookupRelationship(_context).Create();
            new InitialLookupGender(_context).Create();
            new InitialLookupIDType(_context).Create();
            new InitialLookupInvolvedPartyType(_context).Create();
            new InitialLookupDriverStatus(_context).Create();
            new InitialLookupViewOfRoad(_context).Create();
            new InitialLookupViewThirdPartyRequestStatus(_context).Create();
            new InitialLookupRoadType(_context).Create();
            new InitialLookupVisibility(_context).Create();
            new InitialLookupCenterDemarcation(_context).Create();
            new InitialLookupVisbilityReasons(_context).Create();
            new InitialLookupSurroundingArea(_context).Create();
            new InitialLookupRoadCondition(_context).Create();
            new InitialLookupWeatherCondition(_context).Create();
            new InitialLookupModeOfAssignment(_context).Create();
            new RemoveLookupUnneededExpensesType(_context).Remove();
            new InitialLookupPaymentMode(_context).Create();
            new InitialLookupPoliceReportType(_context).Create();
            new InitialFolderSeeder(_context).Create();
            new InitialOCRPrompts(_context).Create();
            new InitialExpensesClaimsApproval(_context).Create();
            new RemoveLookupUnneededFolder(_context).Remove();
            new InitialVehicleDetails(_context).Create();
            new UpdateLookupExpensesStatus(_context).Update();
            new InitialScopeAssignmentSeeder(_context).Create();
            _context.SaveChanges();
        }
    }
}
