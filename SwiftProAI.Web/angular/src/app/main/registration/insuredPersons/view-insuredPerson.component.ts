import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InsuredPersonDto, InsuredPersonsServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'app-view-insuredPerson',
    templateUrl: './view-insuredPerson.component.html',
    // styleUrls: ['./view-insuredPerson.component.css'],
})
export class ViewInsuredPersonComponent extends AppComponentBase implements OnInit {
    insuredPerson: InsuredPersonDto = new InsuredPersonDto();

    registerId = '';
    hospitalName = '';
    hospitalAddress = '';
    locationName = '';
    locationName2 = '';
    locationName3 = '';
    locationName4 = '';
    mainRegistrationVehicleNo = '';

    constructor(
        private _insuredPersonsServiceProxy: InsuredPersonsServiceProxy,

        private _activatedRoute: ActivatedRoute,
        public navigationService: NavigationService,

        injector: Injector,
    ) {
        super(injector);
    }

    ngOnInit() {
        this.getData(this._activatedRoute.snapshot.queryParams['id']);
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
        this.navigationService.registerId = this.registerId;
    }

    getData(registerId?: number) {
        this._insuredPersonsServiceProxy.getInsuredPersonForView(registerId, true).subscribe((result) => {
            this.insuredPerson = result.insuredPerson;

            if (!this.insuredPerson) {
                this.insuredPerson = new InsuredPersonDto();

                this.hospitalName = '';
                this.hospitalAddress = '';
                this.locationName = '';
                this.locationName2 = '';

                this.insuredPerson.valuation = null;
                this.insuredPerson.year = null;
                this.insuredPerson.valuationEquiry = null;
            }
            this.mainRegistrationVehicleNo = result.mainRegistrationVehicleNo;
        });
    }
    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFileFromFileOrg?id=' + id;
    }
}
