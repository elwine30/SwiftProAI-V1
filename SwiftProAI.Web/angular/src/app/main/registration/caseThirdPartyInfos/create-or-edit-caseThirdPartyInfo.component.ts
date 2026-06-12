import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    OnInit,
    ElementRef,
    viewChild,
    ViewChildren,
    QueryList,
} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    CaseThirdPartyInfosServiceProxy,
    CreateOrEditCaseThirdPartyInfoDto,
    CreateOrEditInsuredPersonDto,
    MainRegistrationServiceProxy,
    CaseClaimsServiceProxy,
    CommonDropdownDto,
    CommonDropdownServiceProxy,
    GetLookupByGroupDto,
    LookupsServiceProxy,
    CaseThirdPartyInfoDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Subscription } from '@node_modules/rxjs';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { EnumRegistrationStatus } from '@app/shared/common/registration/enum';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { AppConsts } from '@shared/AppConsts';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { DatePipe } from '@angular/common';
import { CaseThirdPartyUploadComponent } from './caseThirdPartyUpload.component';

@Component({
    templateUrl: './create-or-edit-caseThirdPartyInfo.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditCaseThirdPartyInfoComponent extends AppComponentBase implements OnInit, DirtyFormGuard {
    active = false;
    saving = false;
    isHidden = AppConsts.isComponentDisabled;
    showSubmitToAdjusterButton = false;
    showSubmitForInvoicingButton = false;

    caseInsuredPerson: CreateOrEditInsuredPersonDto = new CreateOrEditInsuredPersonDto();
    caseThirdPartyInfo: CreateOrEditCaseThirdPartyInfoDto = new CreateOrEditCaseThirdPartyInfoDto();
    @ViewChild('caseThirdPartyInfoForm') caseThirdPartyInfoForm: NgForm;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    receivedFileToken: string;
    @ViewChild('fileUploadModal', { static: true }) fileUploadModal: ModalDirective;
    // @ViewChildren(CaseThirdPartyUploadComponent) fileUploadComponents: QueryList<CaseThirdPartyUploadComponent>;

    @ViewChild('licenseBackUpload') licenseBackUpload: CaseThirdPartyUploadComponent;
    @ViewChild('licenseFrontUpload') licenseFrontUpload: CaseThirdPartyUploadComponent;
    @ViewChild('nricBackUpload') nricBackUpload: CaseThirdPartyUploadComponent;
    @ViewChild('nricFrontUpload') nricFrontUpload: CaseThirdPartyUploadComponent;
    @ViewChild('employmentDetailUpload') employmentDetailUpload: CaseThirdPartyUploadComponent;
    @ViewChild('detailUpload') detailUpload: CaseThirdPartyUploadComponent;
    @ViewChild('noiUpload') noiUpload: CaseThirdPartyUploadComponent;
    @ViewChild('hospitalUpload') hospitalUpload: CaseThirdPartyUploadComponent;

    mainRegistrationVehicleNo = '';
    hospitalName = '';
    hospitalAddress = '';
    hospitalName2 = '';
    hospitalAddress2 = '';
    hospitalName3 = '';
    hospitalAddress3 = '';

    registerId = 0;
    locationName = '';
    locationName2 = '';

    allHospitals: CommonDropdownDto[];
    allCountryLocations: CommonDropdownDto[];
    allStateLocations: CommonDropdownDto[];
    thirdPartyVehicles: CommonDropdownDto[];
    MaritalStatus: GetLookupByGroupDto[];
    Relationship: GetLookupByGroupDto[];
    Sex: GetLookupByGroupDto[];
    IDType: GetLookupByGroupDto[];
    ThirdPartyType: GetLookupByGroupDto[];

    private subSelectedRowId: Subscription;

    licenseDateFromString: any;
    licenseDateToString: any;
    licenseNRIC: string;
    isLicenseBackFileUploaded: boolean;
    isLicenseFrontFileUploaded: boolean;
    isNRICBackFileUploaded: boolean;
    isNRICFrontFileUploaded: boolean;
    isEmploymentDetailFileUploaded: boolean;
    isDetailFileUploaded: boolean;
    isNOIFileUploaded: boolean;
    isHospitalFileUploaded: boolean;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseThirdPartyInfosServiceProxy: CaseThirdPartyInfosServiceProxy,
        private _router: Router,
        private _mainRegistrationService: MainRegistrationServiceProxy,
        private _dateTimeService: DateTimeService,
        public navigationService: NavigationService,
        private _caseClaimsService: CaseClaimsServiceProxy,
        private _CommonDropdownService: CommonDropdownServiceProxy,
        private _lookupService: LookupsServiceProxy,
        private datePipe: DatePipe,
    ) {
        super(injector);
    }

    ngAfterViewInit(): void {
        if (AppConsts.isComponentDisabled) {
            setTimeout(() => {
                if (this.caseThirdPartyInfoForm && this.caseThirdPartyInfoForm.controls) {
                    this.disableForm();
                }
            });
        }
    }

    disableForm(): void {
        if (this.caseThirdPartyInfoForm && this.caseThirdPartyInfoForm.controls) {
            Object.keys(this.caseThirdPartyInfoForm.controls).forEach((controlName) => {
                const control = this.caseThirdPartyInfoForm.controls[controlName];
                control.disable();
            });
        } else {
            console.log('Form or controls not available');
        }
    }

    getLookupData(): void {
        //Enter the list of lookupGroups needed in here to retrieve from backend.
        const lookupGroups = ['MaritalStatus', 'Relationship', 'Gender', 'IDType', 'InvolvedPartyType'];
        //Map the results back to each dropdown
        this._lookupService.getAllLookupsByGroups(lookupGroups).subscribe(
            (allLookups: { [key: string]: GetLookupByGroupDto[] }) => {
                this.MaritalStatus = allLookups['MaritalStatus'];
                this.Relationship = allLookups['Relationship'];
                this.Sex = allLookups['Gender'];
                this.IDType = allLookups['IDType'];
                this.ThirdPartyType = allLookups['InvolvedPartyType'];
            },
            (error) => {
                console.error('Error fetching lookup data:', error);
                // Handle error as needed
            },
        );
    }

    // ngOnDestroy(): void {
    //     this.dataService.selectItem(null);
    //     this.subSelectedRowId.unsubscribe();
    // }

    handleDLBackFileToken(item: any): void {
        try {
            this.caseThirdPartyInfo.dlDetBackToken = item.fileToken;
            this.caseInsuredPerson.licenseNo = item.content.licenseNo;
            console.log('Received file token from child:', item);
            this.caseThirdPartyInfoForm.form.markAsDirty();
            this.isLicenseBackFileUploaded = true;
            this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
        } catch (error) {
            console.error(error.message);
            this.message.error(this.l('FailedToExtractInformationFromImage'));
        }
    }

    handleDLFrontFileToken(item: any): void {
        try {
            this.caseThirdPartyInfo.dlDetFrontToken = item.fileToken;

            this.caseInsuredPerson.licenseClasses = item.content.licenseClasses.join(',');
            const licenseFromDateTime = DateTime.fromFormat(item.content.licenseDateFrom, 'dd/MM/yyyy');
            const licenseToDateTime = DateTime.fromFormat(item.content.licenseDateTo, 'dd/MM/yyyy');

            this.licenseDateFromString = licenseFromDateTime.isValid ? licenseFromDateTime.toJSDate() : null;
            this.licenseDateToString = licenseToDateTime.isValid ? licenseToDateTime.toJSDate() : null;

            this.licenseNRIC = item.content.identityNumber;
            this.caseThirdPartyInfoForm.form.markAsDirty();

            console.log('Received file token from child:', item);
            this.isLicenseFrontFileUploaded = true;

            this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
        } catch (error) {
            console.error(error.message);
            this.message.error(this.l('FailedToExtractInformationFromImage'));
        }
    }

    handleNRICBackFileToken(item: any): void {
        this.caseThirdPartyInfo.dlNricBackToken = item.fileToken;
        this.caseThirdPartyInfoForm.form.markAsDirty();
        this.isNRICBackFileUploaded = true;

        console.log('Received file token from child:', item);
    }

    handleNRICDetFileToken(item: any): void {
        try {
            this.caseInsuredPerson.name = item.content.fullName;
            this.caseInsuredPerson.identicationType = item.content.identityType;
            this.caseInsuredPerson.identicationNo = item.content.identityNumber;
            this.caseInsuredPerson.city = item.content.city;
            this.caseInsuredPerson.nationality = item.content.nationality;
            this.caseThirdPartyInfo.sex = item.content.gender;
            this.caseInsuredPerson.address = item.content.addressLine;
            this.caseInsuredPerson.postcode = item.content.postcode;

            var selectedCountry = this.allCountryLocations.find(
                (x) => x.displayName.toLowerCase() == item.content.country.trim().toLowerCase(),
            );
            this.caseInsuredPerson.countryLocationId = selectedCountry ? selectedCountry.id : null;
            this._CommonDropdownService
                .getAllLocationByStateForTableDropdown(selectedCountry ? selectedCountry.id : 1)
                .subscribe((result) => {
                    this.allStateLocations = result;
                    var selectedState = result.find(
                        (x) => x.displayName.toLowerCase() == item.content.state.trim().toLowerCase(),
                    );
                    this.caseInsuredPerson.stateLocationId = selectedState ? selectedState.id : null;
                });
            this.caseThirdPartyInfoForm.form.markAsDirty();

            this.caseThirdPartyInfo.nricDetFrontToken = item.fileToken;
            console.log('Received file token from child:', item);
            this.isNRICFrontFileUploaded = true;

            this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
        } catch (error) {
            console.error(error.message);
            this.message.error(this.l('FailedToExtractInformationFromImage'));
        }
    }

    handleEmployerFileToken(item: any): void {
        try {
            this.caseThirdPartyInfo.empToken = item.fileToken;
            this.caseThirdPartyInfo.employerPrior = item.content.employerName;
            this.caseInsuredPerson.employerAddress = item.content.employerAddress;
            this.caseInsuredPerson.employerContact = item.content.employerContact;
            //Need to parse values from string to int in case the value has RM infront
            const parsedIncome = Number(item.content.monthlyIncome.replace(/[^0-9.-]+/g, ''));
            this.caseInsuredPerson.monthlyIncome = parsedIncome;
            this.caseThirdPartyInfoForm.form.markAsDirty();
            this.isEmploymentDetailFileUploaded = true;

            this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
        } catch (error) {
            console.error(error.message);
            this.message.error(this.l('FailedToExtractInformationFromImage'));
        }
    }

    handleDetailFileToken(item: any): void {
        this.caseThirdPartyInfo.detToken = item.fileToken;
        this.caseThirdPartyInfoForm.form.markAsDirty();
        this.isDetailFileUploaded = true;

        console.log('Received file token from child:', item);
    }

    handleNOIFileToken(item: any): void {
        this.caseThirdPartyInfo.noiToken = item.fileToken;
        this.caseThirdPartyInfoForm.form.markAsDirty();
        this.isNOIFileUploaded = true;

        console.log('Received file token from child:', item);
    }

    handleHospitalFileToken(item: any): void {
        this.caseThirdPartyInfo.hospToken = item.fileToken;
        this.caseThirdPartyInfoForm.form.markAsDirty();
        this.isHospitalFileUploaded = true;

        console.log('Received file token from child:', item);
    }

    showModal() {
        this.fileUploadModal.show();
    }
    close() {
        this.fileUploadModal.hide();
    }

    ngOnInit(): void {
        this._CommonDropdownService.getAllHospitalForTableDropdown().subscribe((result) => {
            this.allHospitals = result;
        });
        this._CommonDropdownService.getAllLocationByCountryForTableDropdown(0).subscribe((result) => {
            this.allCountryLocations = result;
        });

        // this.subSelectedRowId = this.dataService.selectedItem$.subscribe(item => {
        //     this.show(item);
        // });

        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        this.navigationService.registerId = this.registerId.toString();
        this.getLookupData();

        this._CommonDropdownService.getThirdPartyVehicleDetails(this.registerId).subscribe((result) => {
            this.thirdPartyVehicles = result;
        });

        this._mainRegistrationService.getMainRegistrationDetailsByRegisterId(this.registerId).subscribe((result) => {
            // console.log(result);
            switch (result.statusId) {
                case EnumRegistrationStatus.UnderInvestigation:
                    this.showSubmitToAdjusterButton = true;
                    this.showSubmitForInvoicingButton = false;
                    break;
                case EnumRegistrationStatus.Adjusters:
                    this.showSubmitToAdjusterButton = false;
                    this.showSubmitForInvoicingButton = true;
                    break;
                default:
                    this.showSubmitToAdjusterButton = false;
                    this.showSubmitForInvoicingButton = false;
                    break;
            }
            this.markFormAsPristine(this.caseThirdPartyInfoForm);
        });
    }

    getCaseThirdPartyInfos(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        var registerId = this._activatedRoute.snapshot.queryParams['id'];

        this._caseThirdPartyInfosServiceProxy
            .getAll(
                registerId,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event),
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    deleteCaseThirdPartyInfo(caseThirdPartyInfo: CaseThirdPartyInfoDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._caseThirdPartyInfosServiceProxy.delete(caseThirdPartyInfo.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    show(caseThirdPartyInfoId?: number): void {
        if (!caseThirdPartyInfoId) {
            this.caseThirdPartyInfo = new CreateOrEditCaseThirdPartyInfoDto();
            this.caseInsuredPerson = new CreateOrEditInsuredPersonDto();
            this.caseThirdPartyInfo.id = caseThirdPartyInfoId;
            this.mainRegistrationVehicleNo = '';
            this.hospitalName = '';
            this.hospitalName2 = '';
            this.hospitalName3 = '';
            this.active = true;
        } else {
            this._caseThirdPartyInfosServiceProxy
                .getCaseThirdPartyInfoForEdit(caseThirdPartyInfoId)
                .subscribe((result) => {
                    this.caseThirdPartyInfo = result.caseThirdPartyInfo;
                    this.caseInsuredPerson = result.caseInsuredPerson;

                    this.licenseDateFromString = this.caseInsuredPerson.licenseDateFrom
                        ? this.caseInsuredPerson.licenseDateFrom.toFormat('dd/MM/yyyy')
                        : null;
                    this.licenseDateToString = this.caseInsuredPerson.licenseDateTo
                        ? this.caseInsuredPerson.licenseDateTo.toFormat('dd/MM/yyyy')
                        : null;
                    this.mainRegistrationVehicleNo = result.mainRegistrationVehicleNo;
                    this.hospitalName = result.hospitalName;
                    this.hospitalAddress = result.hospitalAddress;
                    this.hospitalName2 = result.hospitalName2;
                    this.hospitalAddress2 = result.hospitalAddress2;
                    this.hospitalName3 = result.hospitalName3;
                    this.hospitalAddress3 = result.hospitalAddress3;
                    this.active = true;
                    this.onCountrySelect(this.caseInsuredPerson.countryLocationId);
                    if (this.caseThirdPartyInfo.detFileName) this.isDetailFileUploaded = true;
                    if (this.caseThirdPartyInfo.empFileName) this.isEmploymentDetailFileUploaded = true;
                    if (this.caseThirdPartyInfo.nricDetFrontFileName) this.isNRICFrontFileUploaded = true;
                    if (this.caseThirdPartyInfo.dlNricBackFileName) this.isNRICBackFileUploaded = true;
                    if (this.caseThirdPartyInfo.hospFileName) this.isHospitalFileUploaded = true;
                    if (this.caseThirdPartyInfo.noiFileName) this.isNOIFileUploaded = true;
                    if (this.caseThirdPartyInfo.dlDetBackFileName) this.isLicenseBackFileUploaded = true;
                    if (this.caseThirdPartyInfo.dlDetFrontFileName) this.isLicenseFrontFileUploaded = true;
                });
        }
    }

    updateHospitalAddress(hospitalId: number, hospitalNum: number): void {
        const setAddress = (address: string) => {
            switch (hospitalNum) {
                case 1:
                    this.hospitalAddress = address;
                    break;
                case 2:
                    this.hospitalAddress2 = address;
                    break;
                case 3:
                    this.hospitalAddress3 = address;
                    break;
                default:
                    break;
            }
        };

        if (hospitalId && hospitalNum) {
            this._CommonDropdownService.getHospitalByIdForTableDropdown(hospitalId).subscribe((result) => {
                setAddress(result?.displayName ? result.address : '');
            });
        } else {
            setAddress('');
        }
    }

    saveAndNew(): void {
        // NRIC and License checking
        if (this.caseInsuredPerson.identicationNo != null && this.licenseNRIC != null) {
            const NRICIdentityNo = this.caseInsuredPerson.identicationNo.replace(/-/g, '');
            const LicenseIdentityNo = this.licenseNRIC.replace(/-/g, '');
            if (NRICIdentityNo != LicenseIdentityNo) {
                this.message.error(this.l('ICDoesNotMatch'));
                return;
            }
        }

        this.saving = true;
        this.caseInsuredPerson.isThirdParty = true;
        this.caseInsuredPerson.registerId = this.registerId;
        this.caseThirdPartyInfo.registerId = this.registerId;
        this.caseThirdPartyInfo.caseInsuredPerson = this.caseInsuredPerson;

        this.caseInsuredPerson.licenseDateFrom =
            this.licenseDateFromString != null ? DateTime.fromJSDate(new Date(this.licenseDateFromString)) : null;
        this.caseInsuredPerson.licenseDateTo =
            this.licenseDateToString != null ? DateTime.fromJSDate(new Date(this.licenseDateToString)) : null;

        this.caseThirdPartyInfo.disablementPeriodFrom =
            this.caseThirdPartyInfo.disablementPeriodFrom != null
                ? DateTime.fromJSDate(new Date(this.caseThirdPartyInfo.disablementPeriodFrom.toLocaleString()))
                : this.caseThirdPartyInfo.disablementPeriodFrom;
        this.caseThirdPartyInfo.disablementPeriodTo =
            this.caseThirdPartyInfo.disablementPeriodTo != null
                ? DateTime.fromJSDate(new Date(this.caseThirdPartyInfo.disablementPeriodTo.toLocaleString()))
                : this.caseThirdPartyInfo.disablementPeriodTo;

        this._caseThirdPartyInfosServiceProxy
            .createOrEdit(this.caseThirdPartyInfo)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe((result) => {
                this.saving = false;
                if (result) {
                    this.swalAlert
                        .fire({
                            title: 'Identity number is inconsistent with the Third Party Report in Step 6. Kindly reupload and update the Third Party Police Report in Step 6 to regenerate the statement summary.',
                            icon: 'warning',
                            showCancelButton: false,
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'Ok',
                        })
                        .then(() => {
                            this.notify.info(this.l('SavedSuccessfully'));
                        });
                    this.resetUploader();
                } else {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.resetUploader();
                }

                this.show();
                this.resetUploader();
                this.getCaseThirdPartyInfos();
                this.caseThirdPartyInfo = new CreateOrEditCaseThirdPartyInfoDto();
                this.caseInsuredPerson = new CreateOrEditInsuredPersonDto();
                this.caseThirdPartyInfoForm.resetForm(new CreateOrEditInsuredPersonDto());
                this.markFormAsPristine(this.caseThirdPartyInfoForm);

                this.caseThirdPartyInfoForm.control.markAsPristine();
            });
    }

    submitToAdjuster(): void {
        //Update the status from Under Investigation to Adjuster
        this.markFormAsPristine(this.caseThirdPartyInfoForm);
        this._mainRegistrationService
            .updateStatus(this._activatedRoute.snapshot.queryParams['id'])
            .subscribe((result) => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.resetUploader();
            });
        this._router.navigate(['/app/main/dashboard']);
    }

    submitForInvoicing(): void {
        this.markFormAsPristine(this.caseThirdPartyInfoForm);
        this._mainRegistrationService
            .getMainRegistrationDetailsByRegisterId(this.registerId)
            .subscribe((registrationDetails) => {
                if (registrationDetails != null) {
                    this._caseClaimsService.getCaseClaimsByRegisterId(this.registerId).subscribe((claimDetails) => {
                        // If registration status is not Adjuster no need check
                        if (!claimDetails && registrationDetails.statusId == EnumRegistrationStatus.Adjusters) {
                            console.log(claimDetails);
                            this.message.confirm(
                                this.l('Redirect to Add Claims page?'),
                                this.l('Claims Not Submitted. Please Add/Submit Claims To Proceed Further'),
                                (isConfirmed) => {
                                    if (isConfirmed) {
                                        this._router.navigate(['/app/main/registration/caseClaims/createOrEdit'], {
                                            queryParams: {
                                                stepId: 5,
                                                id: this._activatedRoute.snapshot.queryParams['id'],
                                            },
                                        });
                                    }
                                },
                            );
                            console.log('Registration status is in Adjuster but Has no Claims', claimDetails);
                        } else {
                            //Update case status from Adjuster to Pending Invoice
                            this._mainRegistrationService
                                .updateStatus(this._activatedRoute.snapshot.queryParams['id'])
                                .subscribe((result) => {
                                    this.notify.info(this.l('SavedSuccessfully'));
                                    this.resetUploader();
                                });
                            console.log('Status changed from Adjuster to Pending Invoice');
                            this._router.navigate(['/app/main/dashboard']);
                        }
                    });
                } else {
                    this.notify.error(this.l('Could not find registration details.'));
                }
            });
    }

    resetUploader() {
        this.isNOIFileUploaded = false;
        this.isDetailFileUploaded = false;
        this.isHospitalFileUploaded = false;
        this.isNRICBackFileUploaded = false;
        this.isNRICFrontFileUploaded = false;
        this.isLicenseBackFileUploaded = false;
        this.isLicenseFrontFileUploaded = false;
        this.isEmploymentDetailFileUploaded = false;

        this.licenseBackUpload.clearQueue();
        this.licenseFrontUpload.clearQueue();
        this.nricBackUpload.clearQueue();
        this.nricFrontUpload.clearQueue();
        this.employmentDetailUpload.clearQueue();
        this.detailUpload.clearQueue();
        this.noiUpload.clearQueue();
    }

    onCountrySelect(parentLocationId): void {
        if (parentLocationId) {
            this._CommonDropdownService.getAllLocationByStateForTableDropdown(parentLocationId).subscribe((result) => {
                this.allStateLocations = result;
            });
        }
    }

    canDeactivate(): Promise<boolean> | boolean {
        this.hideMainSpinner();

        if (this.caseThirdPartyInfoForm.dirty) {
            var returnVal = this.swalAlert.fire({
                title: 'Are you sure you want to leave?',
                text: 'Your changes will be lost.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, proceed!',
            });

            return returnVal.then((result) => result.isConfirmed);
        } else {
            return true;
        }
    }

    parseDate(dateString: string): Date {
        const parts = dateString.split('/');
        const day = parseInt(parts[0], 10);
        const month = parseInt(parts[1], 10) - 1; // Months are zero-based in JavaScript Date
        const year = parseInt(parts[2], 10);
        return new Date(year, month, day);
    }
}
