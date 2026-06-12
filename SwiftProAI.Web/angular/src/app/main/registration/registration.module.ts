import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { RegistrationRoutingModule } from './registration-routing.module';
import { RegistrationComponent } from './registration.component';
import { AppCommonModule } from '@app/shared/common/app-common.module';

@NgModule({
    declarations: [RegistrationComponent],
    imports: [AppSharedModule, RegistrationRoutingModule, AppCommonModule]
})
export class RegistrationModule {}
