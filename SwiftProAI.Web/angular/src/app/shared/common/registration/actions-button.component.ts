import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CreateOrEditCaseDeclarationAnswerModalComponent } from '@app/main/registration/caseDeclarationAnswers/create-or-edit-caseDeclarationAnswer-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateAndViewExpensesModal } from '@app/main/registration/caseExpenses/create-and-view-caseExpenses.component';
import { MainRegistrationServiceProxy, RegistrationExporterServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CreateRemarkModalComponent } from '@app/main/registration/create-remark-modal.component';
import { NavigationService } from './step-nav-service';
import { ViewCaseDeclarationAnswerModalComponent } from '@app/main/registration/caseDeclarationAnswers/view-caseDeclarationAnswer-modal.component';

@Component({
    selector: 'actions-button',
    templateUrl: './actions-button.component.html',
})
export class ActionsButtonComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditCaseDeclarationAnswerModal', { static: true })
    createOrEditCaseDeclarationAnswerModal: CreateOrEditCaseDeclarationAnswerModalComponent;
    @ViewChild('viewCaseDeclarationAnswerModal', { static: true })
    viewCaseDeclarationAnswerModal: ViewCaseDeclarationAnswerModalComponent;
    @ViewChild('createAndViewExpensesModal', { static: true }) createAndViewExpensesModal: CreateAndViewExpensesModal;
    @ViewChild('createRemarkModal', { static: true }) createRemarkModal: CreateRemarkModalComponent;

    registerId = '';
    fileRefNo = '';
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _exportRegistrationProxies: RegistrationExporterServiceProxy,
        private _mainRegistrationService: MainRegistrationServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _router: Router,
        public navigationService: NavigationService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        if (this.registerId!=null) {
            this._mainRegistrationService.getMainRegistrationFileRefNo(this._activatedRoute.snapshot.queryParams['id']).subscribe(result => {
                if (result!=null) {
                    this.fileRefNo = "[" + result + "]";
                }
            });
        }
    }

    goToAddExpense(): void {
        this.createAndViewExpensesModal.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    goToAddClaims(): void {
        this._router.navigate(['/app/main/registration/caseClaims/createOrEdit'], {
            queryParams: { id: this.registerId },
        });
    }

    openDeclarationForm(): void {
        //Check if the user is in viewOnly mode display viewModal if not display editable one
        const modal = this.navigationService.viewOnly ? this.viewCaseDeclarationAnswerModal : this.createOrEditCaseDeclarationAnswerModal;
        modal.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    downloadRegistration(): void {
        const self = this;
        self._exportRegistrationProxies.postExportRegistration(Number(this.registerId)).subscribe((result) => {
            self._fileDownloadService.downloadTempFile(result);
        });
    }

    goToFileOrganizer(): void {
        this._router.navigate(['/app/main/common/fileOrgs'], { queryParams: { id: this.registerId } });
    }

    createNewRemark(): void {
        this.createRemarkModal.show(Number(this.registerId));
    }
}
