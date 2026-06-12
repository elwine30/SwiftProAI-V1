import { Component, ViewChild, Injector, Output, EventEmitter, OnInit} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StaffsServiceProxy, CreateOrEditStaffDto,
					StaffGroupLookupTableDto
					} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { UsersDataService } from '@app/admin/users/usersDataService';
import { Subscription } from 'rxjs';
                    

@Component({
    selector: 'createOrEditStaffModal',
    templateUrl: './create-or-edit-staff-modal.component.html'
})
export class CreateOrEditStaffModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    selectedItem: StaffGroupLookupTableDto;
    groups: StaffGroupLookupTableDto[];
    allGroups: StaffGroupLookupTableDto[];
    filterText = '';
    private onSaveSubscription: Subscription;
    private onShowSubscription: Subscription;
    staff: CreateOrEditStaffDto = new CreateOrEditStaffDto();
    
    groupName = '';					

    constructor(
        injector: Injector,
        private _staffsServiceProxy: StaffsServiceProxy,
        private _dateTimeService: DateTimeService,
        private _usersDataService: UsersDataService,

    ) {
        super(injector);
    }
    
    show(userId?: number): void {
        this._staffsServiceProxy.getStaffForEdit(userId).subscribe(result => {
            this.staff = result.staff;
            if (!result.staff) {
                this.staff = new CreateOrEditStaffDto();
                this.groupName = '';
            }
            this.groupName = result.groupName;
            this.active = true;
        });
        this._staffsServiceProxy.getAllGroupForTableDropdown().subscribe(result => {						
            this.allGroups = result;
        });
        
    }

    save(userId?: number): void {
        this.saving = true;	
        this.staff.userId = userId;
        this._staffsServiceProxy.createOrEdit(this.staff)
            .pipe(finalize(() => { this.saving = false;}))
            .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.staff = new CreateOrEditStaffDto();
            this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
    ngOnInit(): void {
        this.onSaveSubscription = this._usersDataService.onSave.subscribe((userId: number) => {
            this.save(userId);
        });
    
        this.onShowSubscription = this._usersDataService.onShow.subscribe((userId: number) => {
            this.show(userId);
        });
     }
     
    ngOnDestroy(): void {
        this.onSaveSubscription.unsubscribe();
        this.onShowSubscription.unsubscribe();
    }
}
