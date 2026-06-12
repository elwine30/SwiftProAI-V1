import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';



@Component({
    selector: 'createInvestigationOfficer',
    templateUrl: './create-or-edit-caseInvestigationOfficer.component.html',
    animations: [appModuleAnimation()]
})
export class CreateOrEditCaseInvestigationOfficerComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;
    registerId = '';
    
    


    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,        
        private _router: Router,
        public navigationService: NavigationService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'].toString();
        this.show(this._activatedRoute.snapshot.queryParams['id']);
        
    }

    show(registerId?: number): void {

                  	
    }
    
    save(): void {
        this.saving = true;
        
    }
    
    // saveAndNew(): void {
    //     this.saving = true;
        
    //     this._caseInvestigationOfficersServiceProxy.createOrEdit(this.caseInvestigationOfficer)
    //         .pipe(finalize(() => {
    //             this.saving = false;
    //         }))
    //         .subscribe(x => {
    //             this.saving = false;               
    //             this.notify.info(this.l('SavedSuccessfully'));
    //             this.caseInvestigationOfficer = new CreateOrEditCaseInvestigationOfficerDto();
    //         });
    // }

















}
