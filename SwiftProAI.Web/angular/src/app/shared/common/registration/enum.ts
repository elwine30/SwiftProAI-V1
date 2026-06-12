
export enum EnumRegistrationStatus {
    UnderInvestigation = 1,
    Adjusters = 2,
    PendingInvoices = 3,
    CompletedInvoices = 4,
    Cancelled = 5
}

export enum Enum3rdPartyRegistrationStatus {
    UnderInvestigation = 1,
    Completed = 4,
    Cancelled = 5
}

export enum EnumGroupType {
    Admin = 1,
    Adjuster = 2,
    Editor = 3,
    Finance = 4,
    Others = 5
}

export enum ExpensessStatusGroupType {
    PendingForApproval = 'EXP001',
    PendingForPayment = 'EXP002',
    PaymentDone = 'EXP003',
    Submitted = 'EXP004',
    Rejected = 'EXP005',
    SubmitWithoutPayment = 'EXP006'
}

export enum ExpensessClaimType {
    Expenses = 'ExClaimAppr001',
    Claims = 'ExClaimAppr002'
}

export enum ThirdPartyCaseViewApprovalStatus {
    Approved = 'Approved',
    Rejected = 'Rejected',
    PendingApproval = 'Pending Approval'
}

export enum CasePoliceReportSummaryType {
    Generated = 'Generated',
    Edited = 'Edited'
}