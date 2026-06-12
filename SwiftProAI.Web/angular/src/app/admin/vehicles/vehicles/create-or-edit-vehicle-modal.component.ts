import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { VehiclesServiceProxy, CreateOrEditVehicleDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';




@Component({
    selector: 'createOrEditVehicleModal',
    templateUrl: './create-or-edit-vehicle-modal.component.html'
})
export class CreateOrEditVehicleModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;


    vehicle: CreateOrEditVehicleDto = new CreateOrEditVehicleDto();




    constructor(
        injector: Injector,
        private _vehiclesServiceProxy: VehiclesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(vehicleId?: number): void {


        if (!vehicleId) {
            this.vehicle = new CreateOrEditVehicleDto();
            this.vehicle.id = vehicleId;


            this.active = true;
            this.modal.show();
        } else {
            this._vehiclesServiceProxy.getVehicleForEdit(vehicleId).subscribe(result => {
                this.vehicle = result.vehicle;



                this.active = true;
                this.modal.show();
            });
        }


    }

    save(): void {
        this.saving = true;



        this._vehiclesServiceProxy.createOrEdit(this.vehicle)
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
