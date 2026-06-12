import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { OcrComponent } from './ocr/ocr.component';
import { OCRRoutingModule } from './ocr-routing.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';

@NgModule({
    declarations: [OcrComponent],
    imports: [AppSharedModule, OCRRoutingModule, AdminSharedModule]
})
export class OCRModule {}
