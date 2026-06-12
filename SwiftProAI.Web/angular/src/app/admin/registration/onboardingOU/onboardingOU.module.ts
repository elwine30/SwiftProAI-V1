import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OnboardingOUComponent } from './onboardingOU.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { onboardingOURoutingModule } from './onboardingOU-routing.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';

@NgModule({
    imports: [AppSharedModule, onboardingOURoutingModule, AdminSharedModule],

    declarations: [OnboardingOUComponent],
})
export class OnboardingOUModule {}
