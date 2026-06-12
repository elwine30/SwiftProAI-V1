import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { DocumentSettingsServiceProxy, CreateOrEditDocumentSettingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {Observable} from "@node_modules/rxjs";
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    templateUrl: './create-or-edit-documentSetting.component.html',
    animations: [appModuleAnimation()]
})

export class CreateOrEditDocumentSettingComponent extends AppComponentBase implements OnInit {
    active = false;
    saving = false;
    documentSetting: CreateOrEditDocumentSettingDto = new CreateOrEditDocumentSettingDto();

    breadcrumbs: BreadcrumbItem[]= [
                        new BreadcrumbItem(this.l("DocumentSetting"),"/app/admin/common/documentSettings"),
                        new BreadcrumbItem(this.l('Entity_Name_Plural_Here') + '' + this.l('Details')),
                    ];


    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,        
        private _documentSettingsServiceProxy: DocumentSettingsServiceProxy,
        private _router: Router,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.show();
    }

    show(): void {
        this._documentSettingsServiceProxy.getDocumentSettingForEdit().subscribe(result => {
            this.documentSetting = result.documentSetting;

            if(!this.documentSetting) {
                this.documentSetting = new CreateOrEditDocumentSettingDto();
            }
            this.active = true;
        });

    }
    
    save(): void {
        this.saving = true;
        this._documentSettingsServiceProxy.createOrEdit(this.documentSetting)
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(x => {
                 this.saving = false;               
                 this.notify.info(this.l('SavedSuccessfully'));
                 this._router.navigate( ['/app/admin/common/documentSettings']);
            })
    }

}
