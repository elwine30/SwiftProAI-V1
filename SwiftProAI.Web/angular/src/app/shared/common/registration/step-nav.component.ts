import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { NavigationService } from './step-nav-service';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
  selector: 'step-nav',
  templateUrl: './step-nav.component.html',
  styleUrls: ['./step-nav.component.less'],
  animations: [appModuleAnimation()]

})
export class StepNavComponent extends AppComponentBase implements OnInit {
  showMe = false;
  currentMode = '';
  selectedStepUrl = '';

  steps = [
    { label: 'General', url: this.navigationService.step1Url, queryParams: { id: this.navigationService.registerId, stepId: 1 } },
    { label: 'Insured Owner', url: this.navigationService.step2Url, queryParams: { id: this.navigationService.registerId, stepId: 2 } },
    { label: 'Insured Driver', url: this.navigationService.step3Url, queryParams: { id: this.navigationService.registerId, stepId: 3 } },
    { label: 'Lawyers, Insurers and Workshop', url: this.navigationService.step4Url, queryParams: { id: this.navigationService.registerId, stepId: 4 } },
    { label: 'Incident Details', url: this.navigationService.step5Url, queryParams: { id: this.navigationService.registerId, stepId: 5 } },
    { label: 'Police Report', url: this.navigationService.step6Url, queryParams: { id: this.navigationService.registerId, stepId: 6 } },
    { label: 'Third Party Vehicle Details', url: this.navigationService.step7Url, queryParams: { id: this.navigationService.registerId, stepId: 7 } },
    { label: 'Third Party Personal Details', url: this.navigationService.step8Url, queryParams: { id: this.navigationService.registerId, stepId: 8 } }
  ];

  constructor(
    injector: Injector,
    private router: Router,
    private _activatedRoute: ActivatedRoute,
    public navigationService: NavigationService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.showMe = this.shouldShowMenu(this._activatedRoute.snapshot.queryParams);
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.updateSelectedStep();
        this._activatedRoute.queryParams.subscribe(params => {

          // Check for the specific query parameter
          this.showMe = this.shouldShowMenu(params);
          this.navigationService.viewOnly = this.isViewMode();
          

        });
      }
    });
    this.navigationService.viewOnly = this.isViewMode();
    this.updateStepUrls();
  }

  shouldShowMenu(params: any): boolean {
    // Check if the 'showMenu' query parameter exists
    return params.hasOwnProperty('stepId');
  }
  isDarkModeActive() {
    return this.appSession.theme.baseSettings.layout.darkMode ? 'dark' :'light';
}

updateSelectedStep(): void {
  const stepId = this._activatedRoute.snapshot.queryParams['stepId'];
  const id = this._activatedRoute.snapshot.queryParams['id'];

  const currentStep = this.steps.find(step => step.queryParams.stepId.toString() === stepId);

  if (currentStep) {
    this.selectedStepUrl = currentStep.url;
  } else {
    this.selectedStepUrl = '';
  }
}

navigateToStep(selectedStepUrl?: string): void {
  const selectedStep = this.steps.find(step => step.url === selectedStepUrl);
  if (selectedStep) {
    this.router.navigate([selectedStep.url], { 
      queryParams: { 
        id: this.navigationService.registerId,
        stepId: selectedStep.queryParams.stepId
      }
    });
  }
}

// Helper method to check if the current URL is in 'view' mode
isViewMode(): boolean {
  return this.router.url.split('/').pop().split('?')[0].trim().toLowerCase() === 'view';
}

// Method to update step URLs
updateStepUrls(): void {
  const isViewMode = this.isViewMode();
  this.steps.forEach(step => {
    step.url = isViewMode ? step.url.replace('createOrEdit', 'view') : step.url.replace('view', 'createOrEdit');
  });
}

}
