import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RegistrationComponent} from './registration.component';
import { DirtyFormGuard } from '@app/shared/common/auth/dirty-form.guard';

const routes: Routes = [{
    path: '',
    component: RegistrationComponent,
    pathMatch: 'full',
    canDeactivate: [DirtyFormGuard]
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RegistrationRoutingModule {}
