import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DocumentSettingsServiceProxy, GetDocumentSettingForViewDto, DocumentSettingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ActivatedRoute } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { BreadcrumbItem } from '@app/shared/common/sub-header/sub-header.component';
@Component({
    templateUrl: './view-documentSetting.component.html',
    animations: [appModuleAnimation()]
})
export class ViewDocumentSettingComponent extends AppComponentBase implements OnInit {

    active = false;
    saving = false;

    item: GetDocumentSettingForViewDto;

breadcrumbs: BreadcrumbItem[]= [
                        new BreadcrumbItem(this.l("DocumentSetting"),"/app/admin/common/documentSettings"),
                        new BreadcrumbItem(this.l('DocumentSettings') + '' + this.l('Details')),
                    ];
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
         private _documentSettingsServiceProxy: DocumentSettingsServiceProxy
    ) {
        super(injector);
        this.item = new GetDocumentSettingForViewDto();
        this.item.documentSetting = new DocumentSettingDto();        
    }

    ngOnInit(): void {
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(documentSettingId: number): void {
      this._documentSettingsServiceProxy.getDocumentSettingForView(documentSettingId).subscribe(result => {      
                 this.item = result;
                this.active = true;
            });       
    }
    
    
}