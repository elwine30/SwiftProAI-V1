
namespace ThinknInsurTech.Registration
{
    public enum EnumRegistrationStatus
    {
        UnderInvestigation = 1,
        Adjusters = 2,
        PendingInvoices = 3,
        CompletedInvoices = 4,
        Cancelled = 5
    }

    public enum EnumExpensesStatus
    {
        EXP001_PENDING_FOR_APPROVAL = 1,
        EXP002_PENDING_FOR_PAYMENT = 2,
        EXP003_PAYMENT_DONE = 3,
        EXP004_SUBMITTED = 4,
        EXP005_REJECTED = 5,
        EXP006_SUBMIT_WITHOUT_PAYMENT = 6
    }

}
