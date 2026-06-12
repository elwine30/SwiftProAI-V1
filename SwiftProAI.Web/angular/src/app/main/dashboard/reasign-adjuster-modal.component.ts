import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { MainRegistrationServiceProxy,
    MainRegistrationDto,
    ReassignCaseAdjusterDto, 
    RegistrationCaseTypeLookupTableDto,
    CommonDropdownServiceProxy,
    CommonDropdownDto,
    CommonAdjusterDropdownDto,
    RemarkDto
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'reasignCaseAdjusterModal',
    templateUrl: './reasign-adjuster-modal.component.html',
})
export class ReasignCaseAdjusterModalComponent extends AppComponentBase {
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    @ViewChild('createModal', { static: true }) modal: ModalDirective;
    @ViewChild('nameInput', { static: false }) nameInput: ElementRef;

    registrationDetails: MainRegistrationDto = new MainRegistrationDto();
    saveInput: ReassignCaseAdjusterDto = new ReassignCaseAdjusterDto();
    remark : RemarkDto = new RemarkDto();

    today = new Date();
    allCaseTypes : RegistrationCaseTypeLookupTableDto[];
    allCompanies : CommonDropdownDto[];
    allBranches : CommonDropdownDto[];
    allAdjusters : CommonAdjusterDropdownDto[];
    remarkDescription : string = "";
    


    modesOfAssignment = [
        { value: 'Merimen' },
        { value: 'Email' },
        { value: 'Fax' },
        { value: 'By Hand' },
        { value: 'Portal' },
    ];

    tenants = [{ value: 1, display: 'Tenant 1' }];

    active: boolean = false;
    saving: boolean = false;

    constructor(
        injector: Injector,
        private _MainRegistrationService: MainRegistrationServiceProxy,
        private _CommonDropdownService: CommonDropdownServiceProxy,
        // private _usersService: UserServiceProxy
    ) {
        super(injector);

    }

    show(registerId: number): void {
        this._MainRegistrationService.getMainRegistrationDetailsByRegisterId(registerId).subscribe((data) => {
            this.registrationDetails = data;
            this._CommonDropdownService.getAllAdjusterByBranchForTableDropdown(this.registrationDetails.branchId).subscribe(result => {						
                this.allAdjusters = result;
            });
        });

        this._CommonDropdownService.getAllCompanyForTableDropdown().subscribe(result => {						
            this.allCompanies = result;
        });

        this._MainRegistrationService.getAllCaseTypeForTableDropdown().subscribe(result => {						
            this.allCaseTypes = result;
        });

        this._CommonDropdownService.getAllBranchForTableDropdown().subscribe(result => {						
            this.allBranches = result;
        });

        
        this.active = true;
        this.modal.show();
    }

    onShown(): void {
        this.nameInput.nativeElement.focus();
    }

    save(): void {
        this.saving = true;
        this.saveInput.registrationId = this.registrationDetails.id;
        this.saveInput.adjusterId = Number(this.registrationDetails.adjusterMemberId);
        this.remark.description = this.remarkDescription;
        this.remark.registerId = this.registrationDetails.id;
        this.saveInput.remark = this.remark;
        this._MainRegistrationService
            .updateCaseAdjuster(this.saveInput)
            .pipe(finalize(() => (this.saving = false)))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.modalSave.emit(this.saveInput);
            });
        this.close();
    }

    close(): void {
        this.modal.hide();
        this.active = false;
    }
}
