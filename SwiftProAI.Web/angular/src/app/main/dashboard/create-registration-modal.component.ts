import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { MainRegistrationServiceProxy, CreateMainRegistrationInput, CommonDropdownDto, RegistrationCaseTypeLookupTableDto, CommonDropdownServiceProxy, CommonAdjusterDropdownDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'createRegistrationModal',
    templateUrl: './create-registration-modal.component.html'
})
export class CreateRegistrationModalComponent extends AppComponentBase {

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    @ViewChild('createModal', { static: true }) modal: ModalDirective;
    @ViewChild('nameInput' , { static: false }) nameInput: ElementRef;

    registrationDetails: CreateMainRegistrationInput = new CreateMainRegistrationInput();

    today = new Date();


    modesOfAssignment = [
        { value: "Merimen" },
        { value: "Email" },
        { value: "Fax" },
        { value: "By Hand" },
        { value: "Portal" }
        ];

    allAdjusters : CommonAdjusterDropdownDto[];
    allCompanies : CommonDropdownDto[];
    allCaseTypes : RegistrationCaseTypeLookupTableDto[];
    allBranches : CommonDropdownDto[];

    tenants = [
        { value: 1, display: "Tenant 1" },
        ];


    active: boolean = false;
    saving: boolean = false;
    remarkInputActive : boolean = false;

    constructor(
        injector: Injector,
        private _MainRegistrationService: MainRegistrationServiceProxy,
        private _CommonDropdownService: CommonDropdownServiceProxy,
    ) {
        super(injector);
    }
    
    show(): void {
        this.active = true;
        this.modal.show();
        this.registrationDetails = new CreateMainRegistrationInput();

        this._CommonDropdownService.getAllCompanyForTableDropdown().subscribe(result => {						
            this.allCompanies = result;
        });

        this._MainRegistrationService.getAllCaseTypeForTableDropdown().subscribe(result => {						
            this.allCaseTypes = result;
        });

        this._CommonDropdownService.getAllBranchForTableDropdown().subscribe(result => {						
            this.allBranches = result;
        });
    }

    onShown(): void {
        this.nameInput.nativeElement.focus();
    }

    updateAdjusterslist(branchId: number): void {
        this.registrationDetails.adjusterMemberId = null;
        this._CommonDropdownService.getAllAdjusterByBranchForTableDropdown(branchId).subscribe(result => {
            this.allAdjusters = result;
        });
    }

    save(): void {
        this.saving = true;
        if(!this.remarkInputActive){
            this.registrationDetails.remarkDescription = "";
        }
        this._MainRegistrationService.createMainRegistration(this.registrationDetails)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.modalSave.emit(this.registrationDetails);
            });
        this.close();
    }

    close(): void {
        this.modal.hide();
        this.active = false;
    }
}