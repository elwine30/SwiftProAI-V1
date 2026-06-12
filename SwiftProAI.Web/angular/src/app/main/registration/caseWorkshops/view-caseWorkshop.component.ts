import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { finalize } from 'rxjs/operators';
import { CaseWorkshopsServiceProxy, CreateOrEditCaseWorkshopDto ,CaseWorkshopWorkshopLookupTableDto, GetCaseWorkshopForViewDto, CaseWorkshopDto
					} from '@shared/service-proxies/service-proxies';
import { WorkshopsServiceProxy, WorkshopDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StepNavComponent } from '@app/shared/common/registration/step-nav.component';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { CreateOrEditCaseInsurerComponent } from '../caseInsurers/create-or-edit-caseInsurer.component';
import { NgForm } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';




@Component({
    selector : 'viewCaseWorkshop',
    templateUrl: './view-caseWorkshop.component.html',
    animations: [appModuleAnimation()]
})
export class ViewCaseWorkshopComponent extends AppComponentBase implements OnInit {
    

    
    item: CaseWorkshopDto = new CaseWorkshopDto();

    caseWorkshopName = '';
    caseWorkshopAddress = '';

	allWorkshops: CaseWorkshopWorkshopLookupTableDto[];


    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,        
        private _caseWorkshopsServiceProxy: CaseWorkshopsServiceProxy,
        private _workshopsServiceProxy: WorkshopsServiceProxy,
        private _router: Router,
        public navigationService: NavigationService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }


    
    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(registerId?: number): void {

        this._caseWorkshopsServiceProxy.getCaseWorkshopForView(registerId).subscribe(result => {
            this.item = result.caseWorkshop;
            if (!this.item) {
                this.item = new CaseWorkshopDto()
                this.caseWorkshopName = '';
                this.caseWorkshopAddress = '';
            }
            else {
                this.caseWorkshopName = result.workshopWorkshopName;
 
                this._workshopsServiceProxy.getWorkshopForView(result.caseWorkshop.workshopId).subscribe(w => {						
                    this.caseWorkshopAddress = w.workshop.address;
                });
            }
 
            this.item.registerId = registerId;

        });
        this._caseWorkshopsServiceProxy.getAllWorkshopForTableDropdown().subscribe(result => {						
						this.allWorkshops = result;
					});
    }


    


}
