import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InsuredPersonDto, InsuredPersonsServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'app-view-caseInsuredDriver',
    templateUrl: './view-caseInsuredDriver.component.html',
})
export class ViewCaseInsuredDriverComponent extends AppComponentBase implements OnInit {
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
        this._insuredPersonsServiceProxy.getInsuredPersonForView(registerId, false).subscribe((result) => {
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

    isDateValid(date: any): boolean {
        if (!date || typeof date.toISOString !== 'function') {
            return false;
        }
        return date.toISOString() !== '0001-01-01T00:00:00.000Z';
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFileFromFileOrg?id=' + id;
    }
}
