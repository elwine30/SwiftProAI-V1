import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { RemarkServiceProxy, RemarkDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { LazyLoadEvent } from 'primeng/api';
import { NgForm } from '@angular/forms';

@Component({
    selector: 'createRemarkModal',
    styleUrls: ['./create-remark-modal.component.less'],
    templateUrl: './create-remark-modal.component.html'
})
export class CreateRemarkModalComponent extends AppComponentBase{
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('createModal', { static: true }) modal: ModalDirective;
    @ViewChild('nameInput' , { static: false }) nameInput: ElementRef;
    @ViewChild('remarkCreateForm') remarkCreateForm: NgForm;

    cols = [
        { field: 'description', header: 'Remarks' },
        { field: 'creationTime', header: 'Created By' },
        { field: 'creationUserId', header: 'Created Date' },
    ];

    constructor(
        injector: Injector,
        private _RemarkService: RemarkServiceProxy
    ) {
        super(injector);
    }
    remarkDetails: RemarkDto = new RemarkDto();
    registerId: number; 
    today = new Date();
    saving: boolean = false;

    show(registerId: number): void {
        this.registerId = registerId;
        this.primengTableHelper.defaultRecordsCountPerPage = 5;
        this.remarkDetails = new RemarkDto();
        this.modal.show();
        this.getRemarkDetails();
    }

    onShown(): void {
    }

    save(): void {
        if (this.isRemarkDescriptionEmpty()) {
            this.notify.error(this.l('Cannot leave remarks empty. Please add some remarks.'));
            return;
        }
        this.saving = true;
        this.remarkDetails.registerId = this.registerId;
        this._RemarkService.createRemark(this.remarkDetails)
            .pipe(finalize(() => this.saving = false))
            .subscribe(
                () => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.modalSave.emit(this.remarkDetails);
                    this.markFormAsPristine(this.remarkCreateForm);
                    this.close();
                },
                error => {
                    this.notify.error(this.l('SaveFailed'));
                    console.error('Error saving remark:', error);
                }
            );
    }

    close(): void {
        console.log(this.remarkCreateForm);
        if(!this.remarkCreateForm.dirty) {
            this.modal.hide();
        } else {
            confirm('Are you sure you want to leave? Your changes will be lost.') ? this.modal.hide() : '';
        }
    }

    private isRemarkDescriptionEmpty(): boolean {
        //Function will also check if theres is empty spaces or lines
        return !this.remarkDetails.description || this.remarkDetails.description.trim() === "";
    }

    getRemarkDetails(event?: LazyLoadEvent) {
        this.primengTableHelper.showLoadingIndicator();
        this._RemarkService.getAllRemarkByRegistrationId(this.registerId,
                this.primengTableHelper.getMaxResultCount(this.paginator, event),
                this.primengTableHelper.getSkipCount(this.paginator, event)
        ).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
        .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
        })
}    
}