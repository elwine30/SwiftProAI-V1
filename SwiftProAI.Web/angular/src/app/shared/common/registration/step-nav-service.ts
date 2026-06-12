import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {

  registerId = '';
  viewOnly: boolean = false;

  get step1Url(): string { return `/app/main/registration/caseAdjusters/` + this.getAction(); }
  get step2Url(): string { return `/app/main/registration/insuredPersons/` + this.getAction(); }
  get step3Url(): string { return `/app/main/registration/caseInsuredDrivers/` + this.getAction(); }
  get step4Url(): string { return `/app/main/registration/caseStakeholders/` + this.getAction(); }
  get step5Url(): string { return `/app/main/registration/caseIncidentDetails/` + this.getAction(); }
  get step6Url(): string { return `/app/main/registration/casePoliceReports/` + this.getAction(); }
  get step7Url(): string { return `/app/main/registration/caseThirdPartyVehicles/` + this.getAction(); }
  get step8Url(): string { return `/app/main/registration/caseThirdPartyInfos/` + this.getAction(); }

  getAction(){
    return this.viewOnly ? 'view' : 'createOrEdit';
  }

}