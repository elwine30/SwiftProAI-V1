import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, OnInit } from '@angular/core';
import {
    CaseAdjustersServiceProxy,
    GetCaseAdjusterForViewDto,
    CaseAdjusterDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
import { DateTime } from 'luxon';
import { NavigationService } from '@app/shared/common/registration/step-nav-service';

@Component({
    templateUrl: './view-caseAdjuster.component.html',
    animations: [appModuleAnimation()],
})
export class ViewCaseAdjusterComponent extends AppComponentBase implements OnInit {
    active = false;
    item: GetCaseAdjusterForViewDto = new GetCaseAdjusterForViewDto();
    caseAdj: CaseAdjusterDto = new CaseAdjusterDto();
    registerId = '';
    extendedCompletionDateString = '';

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _caseAdjustersServiceProxy: CaseAdjustersServiceProxy,
        public navigationService: NavigationService,
    ) {
        super(injector);
        this.show(this._activatedRoute.snapshot.queryParams['id']);
        const caseAdjusterId = this._activatedRoute.snapshot.queryParams['id'];
    }

    ngOnInit(): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];

        this.navigationService.registerId = this.registerId;
    }

    show(caseAdjusterId: number) {
        this._caseAdjustersServiceProxy.getCaseAdjusterForView(caseAdjusterId).subscribe((result) => {
            this.caseAdj = result.caseAdjuster;
            if (!this.caseAdj) {
                this.caseAdj = new CaseAdjusterDto();
                this.caseAdj.statusName = '';
                this.caseAdj.stateLocationId = 0;
                this.extendedCompletionDateString = '';
                this.caseAdj.extendCompletionRemark = '';
            }
            this.item.scopeAssignmentDescription = result.scopeAssignmentDescription || '';
            this.item.registrationCaseTypeId = result.registrationCaseTypeId || '';
            this.item.adjusterName = result.adjusterName || '';
            this.item.adjusterContact = result.adjusterContact || '';
            this.item.caseTypeName = result.caseTypeName || '';
            this.item.editorUserName = result.editorUserName || '';
            this.item.branchName = result.branchName || '';
            this.item.assignmentTime = result.assignmentTime;
            this.item.stateName = result.stateName || '';
            this.extendedCompletionDateString = result.caseAdjuster
                ? result.caseAdjuster.extendedCompletionDate.toLocaleString()
                : '';
            this.caseAdj.extendCompletionRemark = result.caseAdjuster ? result.caseAdjuster.extendCompletionRemark : '';
            this.item;
            console.log(this.item.assignmentTime);
        });

        console.log('result ', this.caseAdj);
        console.log('result ', this.item);
        this.active = true;
    }
}
