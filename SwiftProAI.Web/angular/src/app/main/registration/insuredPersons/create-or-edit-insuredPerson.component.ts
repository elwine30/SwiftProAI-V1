import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef, viewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    InsuredPersonsServiceProxy,
    CreateOrEditInsuredPersonDto,
    InsuredPersonMainRegistrationLookupTableDto,
    CommonDropdownDto,
    CommonDropdownServiceProxy,
    LookupsServiceProxy,
    GetLookupByGroupDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Observable } from '@node_modules/rxjs';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FileItem, FileUploader, FileUploaderOptions } from '@node_modules/ng2-file-upload';
import { IAjaxResponse, TokenService } from '@node_modules/abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';

import { HttpClient } from '@angular/common/http';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import Swal from 'sweetalert2';
import { FormBuilder, FormGroup, NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { result } from 'lodash-es';

@Component({
    templateUrl: './create-or-edit-insuredPerson.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditInsuredPersonComponent extends AppComponentBase implements OnInit, DirtyFormGuard {
    active = false;
    saving = false;
    isHidden = AppConsts.isComponentDisabled;
    swalAlert = Swal;

    insuredPerson: CreateOrEditInsuredPersonDto = new CreateOrEditInsuredPersonDto();

    registerId = '';
    hospitalName = '';
    hospitalAddress = '';
    locationName = '';
    locationName2 = '';
    locationName3 = '';
    locationName4 = '';
    //licenseDateTo!: DateTime;

    allMainRegistrations: InsuredPersonMainRegistrationLookupTableDto[];
    allHospitals: CommonDropdownDto[];
    allCountryLocations: CommonDropdownDto[];
    allStateLocations: CommonDropdownDto[];
    IDType: GetLookupByGroupDto[];

    driverICFrontFileUploader: FileUploader;
    driverICFrontFileToken: string;
    driverICFrontFileName: string;
    driverICFrontFileAcceptedTypes: string = '';
    @ViewChild('InsuredPerson_driverICFrontLabel') insuredPerson_driverICFrontLabel: ElementRef;
    driverICFrontFileUploaded: boolean;

    driverICBackFileUploader: FileUploader;
    driverICBackFileToken: string;
    driverICBackFileName: string;
    driverICBackFileAcceptedTypes: string = '';
    @ViewChild('InsuredPerson_driverICBackLabel') insuredPerson_driverICBackLabel: ElementRef;
    driverICBackFileUploaded: boolean;

    driverLicenseFrontFileUploader: FileUploader;
    driverLicenseFrontFileToken: string;
    driverLicenseFrontFileName: string;
    driverLicenseFrontFileAcceptedTypes: string = '';
    driverLicenseFrontUploaded: boolean;

    @ViewChild('InsuredPerson_driverLicenseFrontLabel') insuredPerson_driverLicenseFrontLabel: ElementRef;

    driverLicenseBackFileUploader: FileUploader;
    driverLicenseBackFileToken: string;
    driverLicenseBackFileName: string;
    driverLicenseBackFileAcceptedTypes: string = '';
    @ViewChild('InsuredPerson_driverLicenseBackLabel') insuredPerson_driverLicenseBackLabel: ElementRef;
    driverLicenseBackFileUploaded: boolean;

    driverEmploymentDetailFileUploader: FileUploader;
    driverEmploymentDetailFileToken: string;
    driverEmploymentDetailFileName: string;
    driverEmploymentDetailFileAcceptedTypes: string = '';
    @ViewChild('InsuredPerson_driverEmploymentDetailLabel') insuredPerson_driverEmploymentDetailLabel: ElementRef;
    driverEmploymentDetailFileuploaded: boolean;

    driverHospitalDetailFileUploader: FileUploader;
    driverHospitalDetailFileToken: string;
    driverHospitalDetailFileName: string;
    driverHospitalDetailFileAcceptedTypes: string = '';
    @ViewChild('InsuredPerson_driverHospitalDetailLabel') insuredPerson_driverHospitalDetailLabel: ElementRef;
    driverHospitalDetailFileUploaded: boolean;

    driverCarGrantFileUploader: FileUploader;
    driverCarGrantFileToken: string;
    driverCarGrantFileName: string;
    driverCarGrantFileAcceptedTypes: string = '';
    @ViewChild('InsuredPerson_driverCarGrantLabel') insuredPerson_driverCarGrantLabel: ElementRef;
    uploadUrl: string;
    mainRegistrationVehicleNo = '';
    driverCarGrantFileUploaded: boolean;

    @ViewChild('caseInsuredPersonForm') caseInsuredPersonForm: NgForm;
    jpjRegisterDateString: any = null;
    licenseFromDateString: any = null;
    licenseToDateString: any = null;
    licenseNRIC: string = null;
    @ViewChild('fileUploadModal', { static: true }) fileUploadModal: ModalDirective;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _insuredPersonsServiceProxy: InsuredPersonsServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService,
        private _tokenService: TokenService,
        private _http: HttpClient,
        public navigationService: NavigationService,
        private _CommonDropdownService: CommonDropdownServiceProxy,
        private _lookupService: LookupsServiceProxy,
        private fb: FormBuilder,
    ) {
        super(injector);
    }
    ngAfterViewInit(): void {
        if (AppConsts.isComponentDisabled) {
            setTimeout(() => {
                if (this.caseInsuredPersonForm && this.caseInsuredPersonForm.controls) {
                    this.disableForm();
                }
            });
        }
    }

    disableForm(): void {
        if (this.caseInsuredPersonForm && this.caseInsuredPersonForm.controls) {
            Object.keys(this.caseInsuredPersonForm.controls).forEach((controlName) => {
                const control = this.caseInsuredPersonForm.controls[controlName];
                control.disable();
            });
        } else {
            console.log('Form or controls not available');
        }
    }

    ngOnInit(): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        this.show(this._activatedRoute.snapshot.queryParams['id']);
        this.navigationService.registerId = this.registerId;
        this._lookupService.getByGroup('IDType').subscribe((result) => {
            this.IDType = result;
        });

        //Centralised PDF Get File Type
        this._http.get(AppConsts.remoteServiceBaseUrl + '/fileOcr/GetFileAllowedTypes').subscribe((data: any) => {
            if (!data || !data.result) {
                return;
            }

            let list = data.result as string[];
            if (list.length == 0) {
                return;
            }

            for (let i = 0; i < list.length; i++) {
                this.driverICFrontFileAcceptedTypes += '.' + list[i] + ',';
                this.driverICBackFileAcceptedTypes += '.' + list[i] + ',';
                this.driverLicenseFrontFileAcceptedTypes += '.' + list[i] + ',';
                this.driverLicenseBackFileAcceptedTypes += '.' + list[i] + ',';
                this.driverCarGrantFileAcceptedTypes += '.' + list[i] + ',';
                this.driverHospitalDetailFileAcceptedTypes += '.' + list[i] + ',';
                this.driverEmploymentDetailFileAcceptedTypes += '.' + list[i] + ',';
            }
        });
    }

    show(registerId?: number): void {
        this._insuredPersonsServiceProxy.getInsuredPersonForEdit(registerId, true).subscribe((result) => {
            this.insuredPerson = result.insuredPerson;

            if (!this.insuredPerson) {
                this.insuredPerson = new CreateOrEditInsuredPersonDto();
                this.hospitalName = '';
                this.locationName = '';
                this.locationName2 = '';

                this.driverICFrontFileName = null;
                this.driverICBackFileName = null;
                this.driverLicenseFrontFileName = null;
                this.driverLicenseBackFileName = null;
                this.driverEmploymentDetailFileName = null;
                this.driverHospitalDetailFileName = null;
                this.driverCarGrantFileName = null;

                this.insuredPerson.valuation = null;
                this.insuredPerson.year = null;
                this.insuredPerson.valuationEquiry = null;
            }
            this.insuredPerson.registerId = registerId;
            this.hospitalName = result.hospitalName;
            this.hospitalAddress = result.hospitalAddress;
            this.locationName = result.locationName;
            this.locationName2 = result.locationName2;
            this.mainRegistrationVehicleNo = result.mainRegistrationVehicleNo;
            this.driverICFrontFileName = result.driverICFrontFileName;
            this.driverICBackFileName = result.driverICBackFileName;
            this.driverLicenseFrontFileName = result.driverLicenseFrontFileName;
            this.driverLicenseBackFileName = result.driverLicenseBackFileName;
            this.driverEmploymentDetailFileName = result.driverEmploymentDetailFileName;
            this.driverHospitalDetailFileName = result.driverHospitalDetailFileName;
            this.driverCarGrantFileName = result.driverCarGrantFileName;

            //To string the datetime date for input fields
            this.jpjRegisterDateString = this.insuredPerson.jpjRegisterDate
                ? this.insuredPerson.jpjRegisterDate.toFormat('dd/MM/yyyy')
                : null;
            this.licenseFromDateString = this.insuredPerson.licenseDateFrom
                ? this.insuredPerson.licenseDateFrom.toFormat('dd/MM/yyyy')
                : null;
            this.licenseToDateString = this.insuredPerson.licenseDateTo
                ? this.insuredPerson.licenseDateTo.toFormat('dd/MM/yyyy')
                : null;
            this.insuredPerson.make = result.insuredPerson.make;
            this.insuredPerson.model = result.insuredPerson.model;
            this.insuredPerson.specs = result.insuredPerson.specs;
            this.active = true;
            if (this.insuredPerson.countryLocationId != null) {
                this.onCountrySelect(this.insuredPerson.countryLocationId);
            }
        });

        this._insuredPersonsServiceProxy.getAllMainRegistrationForTableDropdown().subscribe((result) => {
            this.allMainRegistrations = result;
        });
        this._CommonDropdownService.getAllHospitalForTableDropdown().subscribe((result) => {
            this.allHospitals = result;
        });
        this._CommonDropdownService.getAllLocationByCountryForTableDropdown(0).subscribe((result) => {
            this.allCountryLocations = result;
        });

        this.driverICFrontFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=ICFront`,
            ({ content, fileToken }) => {
                try {
                    this.insuredPerson.driverICFrontToken = fileToken;
                    this.insuredPerson.identicationNo = content.identityNumber;
                    this.insuredPerson.identicationType = content.identityType;
                    this.insuredPerson.name = content.fullName;
                    this.insuredPerson.address = content.addressLine;
                    this.insuredPerson.postcode = content.postcode;
                    this.insuredPerson.city = content.city;
                    this.insuredPerson.nationality = content.nationality;

                    var selectedCountry = this.allCountryLocations.find(
                        (x) => x.displayName.toLowerCase() == content.country.trim().toLowerCase(),
                    );
                    this.insuredPerson.countryLocationId = selectedCountry ? selectedCountry.id : null;
                    this._CommonDropdownService
                        .getAllLocationByStateForTableDropdown(selectedCountry ? selectedCountry.id : 1)
                        .subscribe((result) => {
                            this.allStateLocations = result;
                            var selectedState = result.find(
                                (x) => x.displayName.toLowerCase() == content.state.trim().toLowerCase(),
                            );
                            this.insuredPerson.stateLocationId = selectedState ? selectedState.id : null;
                        });

                    this.driverICFrontFileUploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                } catch (error) {
                    console.error(error.message);
                    this.message.error(this.l('FailedToExtractInformationFromImage'));
                }
            },
        );

        this.driverICBackFileUploader = this.initializeUploader(
            //!Not migrated yet
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=None`,
            (fileToken) => (this.insuredPerson.driverICBackToken = fileToken),
        );

        this.driverLicenseFrontFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=LicenseFront`,
            ({ content, fileToken }) => {
                try {
                    this.licenseNRIC = content.identityNumber;
                    const licenseFromDateTime = DateTime.fromFormat(content.licenseDateFrom, 'dd/MM/yyyy');
                    const licenseToDateTime = DateTime.fromFormat(content.licenseDateTo, 'dd/MM/yyyy');

                    this.licenseFromDateString = licenseFromDateTime.isValid ? licenseFromDateTime.toJSDate() : null;
                    this.licenseToDateString = licenseToDateTime.isValid ? licenseToDateTime.toJSDate() : null;

                    this.insuredPerson.licenseClasses =
                        content.licenseClasses != null ? content.licenseClasses.join(', ') : null;
                    this.insuredPerson.driverLicenseFrontToken = fileToken;
                    this.driverLicenseFrontUploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                } catch (error) {
                    console.error(error.message);
                    this.message.error(this.l('FailedToExtractInformationFromImage'));
                }
            },
        );

        this.driverLicenseBackFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=LicenseBack`,
            ({ content, fileToken }) => {
                try {
                    this.insuredPerson.licenseNo = content.licenseNo;
                    this.insuredPerson.driverLicenseBackToken = fileToken;
                    this.driverLicenseBackFileUploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                } catch (error) {
                    console.error(error.message);
                    this.message.error(this.l('FailedToExtractInformationFromImage'));
                }
            },
        );

        this.driverEmploymentDetailFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=None`,
            ({ content, fileToken }) => {
                console.log('This is the content for employment', content);
                try {
                    this.insuredPerson.employerName = content.employerName;
                    this.insuredPerson.employerContact = content.employerContact;
                    this.insuredPerson.employerAddress = content.employerAddress;
                    //Need to parse values from string to int
                    const parsedIncome = Number(content.monthlyIncome.replace(/[^0-9.-]+/g, ''));
                    this.insuredPerson.monthlyIncome = parsedIncome;
                    this.insuredPerson.driverEmploymentDetailToken = fileToken;
                    this.driverEmploymentDetailFileuploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                } catch (error) {
                    console.error(error.message);
                    this.message.error(this.l('FailedToExtractInformationFromImage'));
                }
            },
        );

        this.driverHospitalDetailFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=None`,
            (fileToken) => (this.insuredPerson.driverHospitalDetailToken = fileToken),
        );

        this.driverCarGrantFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + `/fileOcr/UploadFile?caseId=${registerId}&docTypeValue=CarGrant`,
            ({ content, fileToken }) => {
                try {
                    this.insuredPerson.make = content.make;
                    this.insuredPerson.model = content.model;
                    this.insuredPerson.specs = content.specs;
                    this.insuredPerson.year = content.year;
                    const jpjRegisterDate = DateTime.fromFormat(content.jpjRegisterDate, 'dd/MM/yyyy');
                    this.jpjRegisterDateString = jpjRegisterDate.isValid ? jpjRegisterDate.toJSDate() : null;

                    this.insuredPerson.jpjRegisterNo = content.jpjRegisterNo;
                    this.insuredPerson.driverCarGrantToken = fileToken;
                    this.driverCarGrantFileUploaded = true;
                    this.message.success(this.l('SuccessfullyExtractedInformationFromImage'));
                } catch (error) {
                    console.error(error.message);
                    this.message.error(this.l('FailedToExtractInformationFromImage'));
                }
            },
        );
    }

    initializeUploader(url: string, onSuccess: (result: any) => void): FileUploader {
        let uploader = new FileUploader({ url: url });
        var token = 'Bearer ' + this._tokenService.getToken();

        let _uploaderOptions: FileUploaderOptions = { url: url };
        _uploaderOptions.autoUpload = false;
        _uploaderOptions.authToken = token;
        _uploaderOptions.removeAfterUpload = true;

        uploader.onAfterAddingFile = (file) => {
            //! Validations handled in BE

            // if (file._file.size > 10 * 1024 * 1024) {
            //     uploader.removeFromQueue(file);
            //     return;
            // }

            file.withCredentials = false;
        };

        uploader.onSuccessItem = (item, response, status) => {
            const resp = <IAjaxResponse>JSON.parse(response);

            if (!resp.success) this.message.error(resp.error.message);

            const fileToken = resp.result.fileToken;

            if (resp.result.output) {
                var resData = JSON.parse(resp.result.output.replaceAll('**', ''));
                var extractedString = resData.choices[0].message.content.match(/\{[\s\S]*\}/);
                var content = JSON.parse(extractedString); // could be null as some documents dont need ocr
                onSuccess({ fileToken, content });
            } else {
                onSuccess(fileToken);
            }
            this.caseInsuredPersonForm.form.markAsDirty();
        };

        uploader.onErrorItem = (item, response, status, headers) => {
            let errorMessage = this.l('AnErrorOccurredDuringFileUpload');
            try {
                const resp = JSON.parse(response);
                if (resp && resp.error && resp.error.message) {
                    errorMessage = resp.error.message;
                }
            } catch (e) {
                errorMessage = this.l('Extraction Failed');
            }

            Swal.fire({
                icon: 'error',
                title: this.l('Extraction Failed'),
                text: errorMessage,
            });
        };

        uploader.setOptions(_uploaderOptions);
        return uploader;
    }

    updateHospitalAddress(hospitalId: number): void {
        if (hospitalId) {
            this._CommonDropdownService.getHospitalByIdForTableDropdown(hospitalId).subscribe((result) => {
                if (!result || !result.displayName) return;
                this.hospitalAddress = result.address;
            });
        } else {
            this.hospitalAddress = '';
        }
    }

    save(): void {
        if (this.jpjRegisterDateString instanceof Date) {
            console.log('I am a date');
            this.insuredPerson.jpjRegisterDate = DateTime.fromJSDate(this.jpjRegisterDateString);
        } else if (typeof this.jpjRegisterDateString === 'string' && this.jpjRegisterDateString) {
            console.log('I am a string');
            const [day, month, year] = this.jpjRegisterDateString.split('/').map(Number);
            this.insuredPerson.jpjRegisterDate = DateTime.fromJSDate(new Date(year, month - 1, day));
        } else {
            this.insuredPerson.jpjRegisterDate = null;
        }

        if (this.licenseFromDateString instanceof Date) {
            console.log('I am a date');

            this.insuredPerson.licenseDateFrom = DateTime.fromJSDate(this.licenseFromDateString);
        } else if (typeof this.licenseFromDateString === 'string' && this.licenseFromDateString) {
            console.log('I am a string');

            const [day, month, year] = this.licenseFromDateString.split('/').map(Number);
            this.insuredPerson.licenseDateFrom = DateTime.fromJSDate(new Date(year, month - 1, day));
        } else {
            this.insuredPerson.licenseDateFrom = null;
        }

        if (this.licenseToDateString instanceof Date) {
            this.insuredPerson.licenseDateTo = DateTime.fromJSDate(this.licenseToDateString);
        } else if (typeof this.licenseToDateString === 'string' && this.licenseToDateString) {
            const [day, month, year] = this.licenseToDateString.split('/').map(Number);
            this.insuredPerson.licenseDateTo = DateTime.fromJSDate(new Date(year, month - 1, day));
        } else {
            this.insuredPerson.licenseDateTo = null;
        }

        if (this.driverLicenseFrontUploaded) {
            // NRIC and License checking
            if (this.insuredPerson.identicationNo != null && this.licenseNRIC != null) {
                const NRICIdentityNo = this.insuredPerson.identicationNo.replace(/-/g, '');
                const LicenseIdentityNo = this.licenseNRIC.replace(/-/g, '');
                if (NRICIdentityNo != LicenseIdentityNo) {
                    this.message.error(this.l('ICDoesNotMatch'));
                    return;
                }
            }
        }

        this.saving = true;
        this.insuredPerson.isOwner = true;

        this._insuredPersonsServiceProxy
            .createOrEdit(this.insuredPerson)
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
                            title: 'Identity number is inconsistent with Claimant Report in Step 6. Kindly reupload and update the Claimant Police Report in Step 6 to regenerate the statement summary.',
                            icon: 'warning',
                            showCancelButton: false,
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'Ok',
                        })
                        .then(() => {
                            this.notify.info(this.l('SavedSuccessfully'));
                        });
                } else {
                    this.notify.info(this.l('SavedSuccessfully'));
                }
                this.caseInsuredPersonForm.control.markAsPristine();
                this.show(Number(this.registerId));
            });
    }

    onSelectDriverICFrontFile(fileInput: any): void {
        const selectedFile = <File>fileInput.target.files[0];
        this.driverICFrontFileUploader.clearQueue();
        this.driverICFrontFileUploader.addToQueue([selectedFile]);
        this.driverICFrontFileUploader.uploadAll();

        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }

    removeDriverICFrontFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._insuredPersonsServiceProxy.removeDriverICFrontFile(this.insuredPerson.id).subscribe(() => {
                    abp.notify.success(this.l('SuccessfullyDeleted'));
                    this.driverICFrontFileName = null;
                    this.driverICFrontFileUploaded = false;
                });
            }
        });
    }

    onSelectDriverICBackFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.driverICBackFileUploader.clearQueue();
        this.driverICBackFileUploader.addToQueue([selectedFile]);
        this.driverICBackFileUploader.uploadAll();
        this.driverICBackFileUploaded = true;
    }

    removeDriverICBackFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._insuredPersonsServiceProxy.removeDriverICBackFile(this.insuredPerson.id).subscribe(() => {
                    abp.notify.success(this.l('SuccessfullyDeleted'));
                    this.driverICBackFileName = null;
                    this.driverICBackFileUploaded = false;
                });
            }
        });
    }

    onSelectDriverLicenseFrontFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];

        this.driverLicenseFrontFileUploader.clearQueue();
        this.driverLicenseFrontFileUploader.addToQueue([selectedFile]);
        this.driverLicenseFrontFileUploader.uploadAll();

        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }

    removeDriverLicenseFrontFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._insuredPersonsServiceProxy.removeDriverLicenseFrontFile(this.insuredPerson.id).subscribe(() => {
                    abp.notify.success(this.l('SuccessfullyDeleted'));
                    this.driverLicenseFrontFileName = null;
                    this.driverLicenseBackFileUploaded = false;
                });
            }
        });
    }

    onSelectDriverLicenseBackFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];

        this.driverLicenseBackFileUploader.clearQueue();
        this.driverLicenseBackFileUploader.addToQueue([selectedFile]);
        this.driverLicenseBackFileUploader.uploadAll();

        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }

    removeDriverLicenseBackFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._insuredPersonsServiceProxy.removeDriverLicenseBackFile(this.insuredPerson.id).subscribe(() => {
                    abp.notify.success(this.l('SuccessfullyDeleted'));
                    this.driverLicenseBackFileName = null;
                    this.driverLicenseBackFileUploaded = false;
                });
            }
        });
    }

    onSelectDriverEmploymentDetailFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.driverEmploymentDetailFileUploader.clearQueue();
        this.driverEmploymentDetailFileUploader.addToQueue([selectedFile]);
        this.driverEmploymentDetailFileUploader.uploadAll();

        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }

    removeDriverEmploymentDetailFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._insuredPersonsServiceProxy
                    .removeDriverEmploymentDetailFile(this.insuredPerson.id)
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.driverEmploymentDetailFileName = null;
                        this.driverEmploymentDetailFileuploaded = false;
                    });
            }
        });
    }

    onSelectDriverHospitalDetailFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.driverHospitalDetailFileUploader.clearQueue();
        this.driverHospitalDetailFileUploader.addToQueue([selectedFile]);
        this.driverHospitalDetailFileUploader.uploadAll();
        this.driverHospitalDetailFileUploaded = true;
    }

    removeDriverHospitalDetailFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._insuredPersonsServiceProxy.removeDriverHospitalDetailFile(this.insuredPerson.id).subscribe(() => {
                    abp.notify.success(this.l('SuccessfullyDeleted'));
                    this.driverHospitalDetailFileName = null;
                    this.driverHospitalDetailFileUploaded = false;
                });
            }
        });
    }

    onSelectDriverCarGrantFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.driverCarGrantFileUploader.clearQueue();
        this.driverCarGrantFileUploader.addToQueue([selectedFile]);
        this.driverCarGrantFileUploader.uploadAll();

        this.swalAlert.fire({
            title: this.l('UploadingAndExtractingContent'),
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }

    removeDriverCarGrantFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._insuredPersonsServiceProxy.removeDriverCarGrantFile(this.insuredPerson.id).subscribe(() => {
                    abp.notify.success(this.l('SuccessfullyDeleted'));
                    this.driverCarGrantFileName = null;
                    this.driverCarGrantFileUploaded = false;
                });
            }
        });
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFileFromFileOrg?id=' + id;
    }

    onCountrySelect(parentLocationId): void {
        this._CommonDropdownService.getAllLocationByStateForTableDropdown(parentLocationId).subscribe((result) => {
            this.allStateLocations = result;
        });
    }

    canDeactivate(): Promise<boolean> | boolean {
        this.hideMainSpinner();

        if (this.caseInsuredPersonForm.dirty) {
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

    showModal() {
        this.fileUploadModal.show();
    }

    close() {
        this.fileUploadModal.hide();
    }
}
