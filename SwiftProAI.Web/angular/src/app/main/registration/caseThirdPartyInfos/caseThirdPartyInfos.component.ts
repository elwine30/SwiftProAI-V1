import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CaseThirdPartyInfosServiceProxy, CaseThirdPartyInfoDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';


import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CaseThirdPartyInfosDataService } from './caseThirdPartyInfosDataService';
import { AppConsts } from '@shared/AppConsts';


@Component({
    selector: 'case-third-party-infos',
    templateUrl: './caseThirdPartyInfos.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class CaseThirdPartyInfosComponent extends AppComponentBase implements OnInit {




  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  isHidden = AppConsts.isComponentDisabled;
  advancedFiltersAreShown = false;
  filterText = '';
  maxAgeFilter: number;
  maxAgeFilterEmpty: number;
  minAgeFilter: number;
  minAgeFilterEmpty: number;
  sexFilter = '';
  maritalStatusFilter = '';
  thirdPartyTypeFilter = '';
  maxAdmittedDate1Filter: DateTime;
  minAdmittedDate1Filter: DateTime;
  maxAdmittedDate2Filter: DateTime;
  minAdmittedDate2Filter: DateTime;
  maxAdmittedDate3Filter: DateTime;
  minAdmittedDate3Filter: DateTime;
  maxDischargeDate1Filter: DateTime;
  minDischargeDate1Filter: DateTime;
  maxDischargeDate2Filter: DateTime;
  minDischargeDate2Filter: DateTime;
  maxDischargeDate3Filter: DateTime;
  minDischargeDate3Filter: DateTime;
  employerPriorFilter = '';
  maxEmployedDateFromFilter: DateTime;
  minEmployedDateFromFilter: DateTime;
  maxEmployedDateToFilter: DateTime;
  minEmployedDateToFilter: DateTime;
  maxEPFFilter: number;
  maxEPFFilterEmpty: number;
  minEPFFilter: number;
  minEPFFilterEmpty: number;
  maxSOCSOFilter: number;
  maxSOCSOFilterEmpty: number;
  minSOCSOFilter: number;
  minSOCSOFilterEmpty: number;
  medicalBenefitFilter = '';
  maxIncomeLossFilter: number;
  maxIncomeLossFilterEmpty: number;
  minIncomeLossFilter: number;
  minIncomeLossFilterEmpty: number;
  employerAdministrativeFilter = '';
  afterAccidentEmployerNameFilter = '';
  maxAfterAccidentEmployerIncomeFilter: number;
  maxAfterAccidentEmployerIncomeFilterEmpty: number;
  minAfterAccidentEmployerIncomeFilter: number;
  minAfterAccidentEmployerIncomeFilterEmpty: number;
  maxAfterAccidentEmployerIncomeReductionFilter: number;
  maxAfterAccidentEmployerIncomeReductionFilterEmpty: number;
  minAfterAccidentEmployerIncomeReductionFilter: number;
  minAfterAccidentEmployerIncomeReductionFilterEmpty: number;
  afterAccidentEmployerAddressFilter = '';
  afterAccidentEmployerJobFilter = '';
  injuriesSustainedFilter = '';
  medicalLeaveFilter = '';
  maxDisablementPeriodFromFilter: DateTime;
  minDisablementPeriodFromFilter: DateTime;
  maxDisablementPeriodToFilter: DateTime;
  minDisablementPeriodToFilter: DateTime;
  presentConditionFilter = '';
  currentDisabilitiesFilter = '';
  solicitorNameFilter = '';
  solicitorAddressFilter = '';
  solicitorContactFilter = '';
  solicitorReferenceNoFilter = '';
  otherMedicalBenefitFilter = '';
  fatalCaseCheckFilter = -1;
  vehicleNoFilter = '';
  mainRegistrationVehicleNoFilter = '';
  hospitalNameFilter = '';
  hospitalName2Filter = '';
  hospitalName3Filter = '';
  caseInsuredPersonNameFilter = '';

  constructor(
    injector: Injector,
    private _caseThirdPartyInfosServiceProxy: CaseThirdPartyInfosServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private dataService: CaseThirdPartyInfosDataService,
    private _dateTimeService: DateTimeService
  ) {
    super(injector);

  }
  ngOnInit(): void {
    

    this.dataService.refreshPage$.subscribe(() => {
      this.reloadPage();
    });

  }

  getCaseThirdPartyInfos(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      if (this.primengTableHelper.records &&
        this.primengTableHelper.records.length > 0) {
        return;
      }
    }

    this.primengTableHelper.showLoadingIndicator();

      var registerId = this._activatedRoute.snapshot.queryParams['id'];

      this._caseThirdPartyInfosServiceProxy.getAll(registerId,
      this.primengTableHelper.getSorting(this.dataTable),
      this.primengTableHelper.getSkipCount(this.paginator, event),
      this.primengTableHelper.getMaxResultCount(this.paginator, event)
    ).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }

  reloadPage(): void {
    this.paginator.changePage(this.paginator.getPage());
  }

  selectItem(item: number) {
    this.dataService.selectItem(item);
    window.scrollTo(0, 0);

  }

  deleteCaseThirdPartyInfo(caseThirdPartyInfo: CaseThirdPartyInfoDto): void {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._caseThirdPartyInfosServiceProxy.delete(caseThirdPartyInfo.id)
            .subscribe(() => {
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
              this.selectItem(null);
            });
        }
      }
    );
  }

}
