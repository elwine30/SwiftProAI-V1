import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    HospitalsServiceProxy, CreateOrEditHospitalDto, 
    CommonDropdownDto,
    CommonDropdownServiceProxy
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';




@Component({
    selector: 'createOrEditHospitalModal',
    templateUrl: './create-or-edit-hospital-modal.component.html'
})
export class CreateOrEditHospitalModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;


    hospital: CreateOrEditHospitalDto = new CreateOrEditHospitalDto();

    locationName = '';
    locationName2 = '';

    allCountryLocations: CommonDropdownDto[];
    allStateLocations: CommonDropdownDto[];


    constructor(
        injector: Injector,
        private _hospitalsServiceProxy: HospitalsServiceProxy,
        private _dateTimeService: DateTimeService,
        private _CommonDropdownService: CommonDropdownServiceProxy,
    ) {
        super(injector);
    }

    show(hospitalId?: number): void {


        if (!hospitalId) {
            this.hospital = new CreateOrEditHospitalDto();
            this.hospital.id = hospitalId;
            this.locationName = '';
            this.locationName2 = '';


            this.active = true;
            this.modal.show();
        } else {
            this._hospitalsServiceProxy.getHospitalForEdit(hospitalId).subscribe(result => {
                this.hospital = result.hospital;

                this.locationName = result.locationName;
                this.locationName2 = result.locationName2;


                this.active = true;
                this.onCountrySelect(this.hospital.countryLocationId);
                this.modal.show();
            });
        }
        this._CommonDropdownService.getAllLocationByCountryForTableDropdown(0).subscribe(result => {
            this.allCountryLocations = result;
        });



    }

    onCountrySelect(parentLocationId): void {
        this._CommonDropdownService.getAllLocationByStateForTableDropdown(parentLocationId).subscribe(result => {
            this.allStateLocations = result;
        });
      }

    save(): void {
        this.saving = true;



        this._hospitalsServiceProxy.createOrEdit(this.hospital)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }



    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {

    }


}
