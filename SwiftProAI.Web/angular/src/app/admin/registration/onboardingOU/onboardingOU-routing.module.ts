import { RouterModule, Routes } from '@angular/router';
import { OnboardingOUComponent } from './onboardingOU.component';
import { NgModule } from '@angular/core';

const routes: Routes = [
    {
        path: '',
        component: OnboardingOUComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class onboardingOURoutingModule {}
