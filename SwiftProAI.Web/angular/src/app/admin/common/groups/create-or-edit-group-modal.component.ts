import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { GroupsServiceProxy, CreateOrEditGroupDto, CommonDropdownServiceProxy, CommonDropdownDto
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { EnumGroupType } from '@app/shared/common/registration/enum';




@Component({
    selector: 'createOrEditGroupModal',
    templateUrl: './create-or-edit-group-modal.component.html'
})
export class CreateOrEditGroupModalComponent extends AppComponentBase implements OnInit{

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;


    group: CreateOrEditGroupDto = new CreateOrEditGroupDto();

    branchName = '';

	allBranchs: CommonDropdownDto[];

    constructor(
        injector: Injector,
        private _groupsServiceProxy: GroupsServiceProxy,
        private _CommonDropdownService: CommonDropdownServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);

    }

    show(groupId?: number): void {
        if (!groupId) {
            this.group = new CreateOrEditGroupDto();
            this.group.id = groupId;
            this.branchName = '';
            this.group.isActive = true;
            this.active = true;
            this.modal.show();
        } else {
            this._groupsServiceProxy.getGroupForEdit(groupId).subscribe(result => {
                this.group = result.group;
                this.branchName = result.branchName;
                this.active = true;
                this.modal.show();
            });
        }
        this._CommonDropdownService.getAllBranchForTableDropdown().subscribe(result => {						
						this.allBranchs = result;
					});

    }

    save(): void {
            this.saving = true;

            this._groupsServiceProxy.createOrEdit(this.group)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }

     ngOnInit(): void {

     }
}
