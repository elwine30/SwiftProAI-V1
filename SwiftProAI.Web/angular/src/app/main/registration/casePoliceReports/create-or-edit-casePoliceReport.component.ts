import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
    CasePoliceReportsServiceProxy,
    CreateOrEditCasePoliceReportDto,
    CasePoliceReportMainRegistrationLookupTableDto,
    CasePoliceReportDto,
    ChartDateInterval,
    GetLookupByGroupDto,
    LookupsServiceProxy,
    CommonDropdownDto,
    CasePoliceReportSummariesServiceProxy,
    CreateOrEditCasePoliceReportSummaryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FileItem, FileUploader, FileUploaderOptions } from '@node_modules/ng2-file-upload';
import { IAjaxResponse, TokenService } from '@node_modules/abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { CasePoliceReportFileUploadModalComponent } from './casePoliceReport-file-upload-modal.component';
import { CreateOrEditCaseInvestigationOfficerComponent } from '../caseInvestigationOfficers/create-or-edit-caseInvestigationOfficer.component';
import { NgForm } from '@angular/forms';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';
import { ViewCasePoliceReportModalComponent } from './view-casePoliceReport-modal.component';
import { CreateOrEditCasePoliceReportSummaryComponent } from '../casePoliceReportSummaries/create-or-edit-casePoliceReportSummary.component';
import Swal from 'sweetalert2';
import { CasePoliceReportSummaryType } from '@app/shared/common/registration/enum';

@Component({
    templateUrl: './create-or-edit-casePoliceReport.component.html',
    animations: [appModuleAnimation()],
})
export class CreateOrEditCasePoliceReportComponent extends AppComponentBase implements OnInit, DirtyFormGuard {
    active = false;
    saving = false;
    isHidden = AppConsts.isComponentDisabled;
    hasReportSummary = false;
    casePoliceReport: CreateOrEditCasePoliceReportDto = new CreateOrEditCasePoliceReportDto();
    casePoliceReportSummary: CreateOrEditCasePoliceReportSummaryDto = new CreateOrEditCasePoliceReportSummaryDto();
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('casePoliceReportFileUploadModal')
    casePoliceReportFileUploadModal: CasePoliceReportFileUploadModalComponent;
    @ViewChild('createCaseInvestigationOfficer')
    createCaseInvestigationOfficer: CreateOrEditCaseInvestigationOfficerComponent;
    @ViewChild('casePoliceReportForm') casePoliceReportForm: NgForm;
    @ViewChild('viewCasePoliceReportModal', { static: true })
    viewCasePoliceReportModal: ViewCasePoliceReportModalComponent;

    allMainRegistrations: CasePoliceReportMainRegistrationLookupTableDto[];
    types: GetLookupByGroupDto[];
    reportFileUploadFileUploader: FileUploader;
    reportFileUploadFileName: string;
    reportFileUploadFileAcceptedTypes: string = '';
    advancedFiltersAreShown = false;
    reportTypes: CommonDropdownDto[];

    uploadedFile: FileItem = null;
    invalidFileSize = false;
    uploading = false;
    result: any;
    incidentTime: any;
    reportTime: any;

    registerIdFilter = '';
    incidentTimeTo: DateTime;
    lateReport = '';
    newDateTime: DateTime;
    registerIdNum: number;

    isAllDataConsistent: boolean = true;

    @ViewChild('CasePoliceReport_reportFileUploadLabel') casePoliceReport_reportFileUploadLabel: ElementRef;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _casePoliceReportsServiceProxy: CasePoliceReportsServiceProxy,
        private _casePoliceReportsSummaryServiceProxy: CasePoliceReportSummariesServiceProxy,
        private _router: Router,
        private _dateTimeService: DateTimeService,
        private _tokenService: TokenService,
        private _http: HttpClient,
        public navigationService: NavigationService,
        private _lookupService: LookupsServiceProxy,
    ) {
        super(injector);
    }

    ngAfterViewInit(): void {
        if (AppConsts.isComponentDisabled) {
            setTimeout(() => {
                if (this.casePoliceReportForm && this.casePoliceReportForm.controls) {
                    this.disableForm();
                }
            });
        }
    }

    disableForm(): void {
        if (this.casePoliceReportForm && this.casePoliceReportForm.controls) {
            Object.keys(this.casePoliceReportForm.controls).forEach((controlName) => {
                const control = this.casePoliceReportForm.controls[controlName];
                control.disable();
            });
        } else {
            console.log('Form or controls not available');
        }
    }

    ngOnInit(): void {
        this.show(); //insert the case
        this.registerIdFilter = this._activatedRoute.snapshot.queryParams['id'].toString();
        this.registerIdNum = this._activatedRoute.snapshot.queryParams['id'];
        this.navigationService.registerId = this.registerIdFilter;
        this._lookupService.getByGroup('InvolvedPartyType').subscribe((result) => {
            this.types = result;
        });
        this._http.get(AppConsts.remoteServiceBaseUrl + '/fileOcr/GetFileAllowedTypes').subscribe((data: any) => {
            if (!data || !data.result) {
                return;
            }

            let list = data.result as string[];
            if (list.length == 0) {
                return;
            }

            for (let i = 0; i < list.length; i++) {
                this.reportFileUploadFileAcceptedTypes += '.' + list[i] + ',';
            }
        });

        this._casePoliceReportsServiceProxy.getAllPoliceReportTypeDropdown().subscribe((result) => {
            this.reportTypes = result;
        });
    }

    //Function to send file and get response from openAI

    show(casePoliceReportId?: number): void {
        this._casePoliceReportsServiceProxy
            .getCasePoliceReportForEdit(this._activatedRoute.snapshot.queryParams['id'], casePoliceReportId)
            .subscribe((result) => {
                this.result = result;
                this.casePoliceReport = result.casePoliceReport;
                this.reportFileUploadFileName = result.reportFileUploadFileName;
                if (!this.casePoliceReport) {
                    this.casePoliceReport = new CreateOrEditCasePoliceReportDto();
                    this.casePoliceReport.id = casePoliceReportId;
                    this.reportTime = this._dateTimeService.getStartOfDay();
                    this.incidentTime = this._dateTimeService.getStartOfDay();
                    this.reportFileUploadFileName = null;
                }
                if (result.incidentTimeTo != null) {
                    this.incidentTimeTo =
                        this.casePoliceReport.incidentTime =
                        this.incidentTime =
                            result.incidentTimeTo;
                }

                this.casePoliceReport.registerId = this._activatedRoute.snapshot.queryParams['id'];
                this.active = true;

                this.markFormAsPristine(this.casePoliceReportForm);
            });

        this.getReportSummary();
    }

    getReportSummary(): void {
        this._casePoliceReportsSummaryServiceProxy
            .getCasePoliceReportSummaryForEdit(this._activatedRoute.snapshot.queryParams['id'])
            .subscribe((result) => {
                console.log(result);
                if (result.casePoliceReportSummary != null) {
                    this.casePoliceReportSummary = result.casePoliceReportSummary;
                    console.log(this.casePoliceReportSummary);
                    this.hasReportSummary = true;
                }
            });
    }

    updateLateReport(): void {
        if (this.casePoliceReport.incidentTime && this.casePoliceReport.reportTime) {
            const reportTime = DateTime.fromISO(this.casePoliceReport.reportTime.toString());
            // Calculate the difference in milliseconds
            const diff = reportTime.diff(this.incidentTime, 'milliseconds').milliseconds;
            // Convert milliseconds to hours
            const day = 1000 * 60 * 60 * 24;

            // Check if the report is late
            if (diff > day) {
                this.casePoliceReport.lateReport = 'Yes';
            } else {
                this.casePoliceReport.lateReport = 'No';
                this.casePoliceReport.lateReason = null;
            }
        }
        this.markFormAsPristine(this.casePoliceReportForm);
    }

    addPoliceReport(policeReport: any) {
        this.casePoliceReport = policeReport;
        this.casePoliceReportForm.form.markAsDirty();
    }

    save(): void {
        if (this.incidentTime > this.reportTime) {
            this.message.warn('Incident time cannot be later than report time.');
            return;
        }

        this.casePoliceReport.reportTime = DateTime.fromJSDate(
            new Date(this.casePoliceReport.reportTime.toLocaleString()),
        );
        this.casePoliceReport.incidentTime = DateTime.fromJSDate(
            new Date(this.casePoliceReport.incidentTime.toLocaleString()),
        );

        this.saving = true;
        console.log(this.casePoliceReport);
        this._casePoliceReportsServiceProxy
            .createOrEdit(this.casePoliceReport)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.casePoliceReport = new CreateOrEditCasePoliceReportDto();
                this.getCasePoliceReports();
                this.getReportSummary();
                this.casePoliceReportFileUploadModal.resetUploader();
            });
    }

    saveReportSummary(): void {
        this.casePoliceReportSummary.summaryType = CasePoliceReportSummaryType.Edited;

        this._casePoliceReportsSummaryServiceProxy.createOrEdit(this.casePoliceReportSummary).subscribe((result) => {
            this.saving = false;
            this.notify.info(this.l('SavedSuccessfully'));
            this.getReportSummary();
        });
    }

    saveAndNew(): void {
        if (this.incidentTime > this.reportTime) {
            this.message.warn('Incident time cannot be later than report time.');
            return;
        }

        this.casePoliceReport.reportTime = DateTime.fromJSDate(
            new Date(this.casePoliceReport.reportTime.toLocaleString()),
        );
        this.casePoliceReport.incidentTime = DateTime.fromJSDate(
            new Date(this.casePoliceReport.incidentTime.toLocaleString()),
        );

        this.saving = true;
        this._casePoliceReportsServiceProxy
            .createOrEdit(this.casePoliceReport)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe((x) => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.show();
                this.casePoliceReport = new CreateOrEditCasePoliceReportDto();
                this.getCasePoliceReports();
                document.getElementById('CasePoliceReport_ReportFileUpload')['value'] = ''; //clear value to upload again
                this.uploadedFile = null;
                this.casePoliceReportFileUploadModal.resetUploader();
            });
    }

    // Add the methods from casePoliceReports.component.ts here
    getCasePoliceReports(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._casePoliceReportsServiceProxy
            .getAll(
                this.registerIdFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                50,
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();

                result.items.forEach((item) => {
                    if (!item.casePoliceReport.isDataConsistent) {
                        this.isAllDataConsistent = item.casePoliceReport.isDataConsistent;
                        return;
                    }
                });
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    deleteCasePoliceReport(casePoliceReport: CasePoliceReportDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._casePoliceReportsServiceProxy.delete(casePoliceReport.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    editCasePoliceReport(record: CasePoliceReportDto): void {
        this.active = true;
        this.show(record.id);
    }

    onSelectReportFileUploadFile(fileInput: any): void {
        let selectedFile = <File>fileInput.target.files[0];
        this.reportFileUploadFileUploader.clearQueue();
        this.reportFileUploadFileUploader.addToQueue([selectedFile]);
        this.reportFileUploadFileUploader.uploadAll();
        this.casePoliceReportForm.form.markAsDirty();
    }

    removeReportFileUploadFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._casePoliceReportsServiceProxy
                    .removeReportFileUploadFile(this.casePoliceReport.id)
                    .subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.reportFileUploadFileName = null;
                    });
            }
        });
    }

    checkFiles(file: FileItem) {
        const img = new Image();

        this.uploadedFile = file;

        //! Validations handled in BE

        // if (file._file.size > 10 * 1024 * 1024) {
        //     uploader.removeFromQueue(file);
        //     return;
        // }

        img.src = URL.createObjectURL(
            this.reportFileUploadFileUploader.queue[this.reportFileUploadFileUploader.queue.length - 1]._file,
        );
    }

    //This uploader will handle the file uploading to openAI
    openAiUploader(url: string, success?: (result: any) => void): FileUploader {
        var token = 'Bearer ' + this._tokenService.getToken();
        const uploaderOptions: FileUploaderOptions = {
            url: AppConsts.remoteServiceBaseUrl + url,
            removeAfterUpload: true,
            authToken: token,
        };
        const uploader = new FileUploader(uploaderOptions);

        uploader.onAfterAddingFile = (file) => {
            this.checkFiles(file);
            file.withCredentials = false;
            this.casePoliceReportForm.form.markAsDirty();
        };
        uploader.onSuccessItem = (item, response, status) => {
            const ajaxResponse = <IAjaxResponse>JSON.parse(response);
            if (ajaxResponse.success) {
                if (success) {
                    success(ajaxResponse.result);
                }
            } else {
                this.message.error(ajaxResponse.error.message);
            }
            document.getElementById('CasePoliceReport_ReportFileUpload')['value'] = ''; //clear value to upload again
            this.uploadedFile = null;
            this.uploading = false;
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
        return uploader;
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFileFromFileOrg?id=' + id;
    }

    showFileUpload(): void {
        this.result.casePoliceReport = this.casePoliceReport;
        this.casePoliceReportFileUploadModal.show(this.result, this.registerIdNum);
    }

    next(): void {
        this._router.navigate(['/app/main/registration/caseIncidentDetails/createOrEdit'], {
            queryParams: { id: this._activatedRoute.snapshot.queryParams['id'] },
        });
    }

    canDeactivate(): Promise<boolean> | boolean {
        this.hideMainSpinner();

        if (this.casePoliceReportForm.dirty) {
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
    generateCasePoliceReportSummary(): void {
        // checking for only allow one claimant and one police report is done at appservice
        // here we just check for data consistency
        if (this.isAllDataConsistent) {
            this.swalAlert.fire({
                title: this.l('Generating Report Summary'),
                didOpen: () => {
                    Swal.showLoading();
                },
            });
            this._casePoliceReportsSummaryServiceProxy.generateReportSummary(this.registerIdNum).subscribe(
                (response) => {
                    const resp = JSON.parse(response);

                    // Extract the JSON object from the content string
                    const extractedString = resp.choices[0].message.content.match(/\{[\s\S]*\}/);
                    console.log(extractedString);
                    if (extractedString) {
                        var content = JSON.parse(extractedString[0]);

                        // cater for array and string format
                        if (Array.isArray(content.discrepancies)) {
                            // console.log('discrepencies is array')
                            // console.log('content.discrepancies: ', content.discrepancies)
                            const stringifiedArr = content.discrepancies
                                .map((item) => `${item.title}\n${item.description}`)
                                .join('\n\n');
                            this.casePoliceReportSummary.reportInconsistency = stringifiedArr;
                        } else if (typeof content.discrepancies == 'string') {
                            // console.log('discrepencies is string')
                            // console.log('content.discrepancies: ', content.discrepancies)
                            this.casePoliceReportSummary.reportInconsistency = content.discrepancies;
                        }

                        this.casePoliceReportSummary.reportSummary = content.reportSummary;
                        this.casePoliceReportSummary.registerId = this.registerIdNum;
                        this.casePoliceReportSummary.summaryType = CasePoliceReportSummaryType.Generated;

                        // console.log('this.casePoliceReportSummary:', this.casePoliceReportSummary )

                        //Save or Update casePoliceReportSummary
                        this._casePoliceReportsSummaryServiceProxy.createOrEdit(this.casePoliceReportSummary).subscribe(
                            (result) => {
                                this.hasReportSummary = true; //  enable report summary and case discrepancies field and show save button
                                this.message.success(this.l('Successfully Generated Report'));
                                this.getReportSummary();
                            },
                            (error) => {
                                console.error('Failed to save report summary. Error:', error);
                                this.message.error(
                                    this.l('Failed to save the report, please regenerate summary try again.'),
                                );
                            },
                        );
                    }
                },
                (error) => {
                    console.error('Failed to generate report. Error:', error);
                },
            );
        } else {
            this.message.error(
                this.l('Reporter NRIC number is inconsistent. Please update the claimant/third party IC number.'),
            );
        }
    }
}
