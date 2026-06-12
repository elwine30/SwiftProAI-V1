import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CreateRemarkModalComponent } from '@app/main/registration/create-remark-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'add-remark-button',
    templateUrl: './add-remark-button.component.html'
})
export class AddRemarkButtonComponent extends AppComponentBase implements OnInit {
    @ViewChild('createRemarkModal', {static:true}) createRemarkModal: CreateRemarkModalComponent;
    
    registerId :number;
    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.registerId = this._activatedRoute.snapshot.queryParams['id'];
    }


    createNewRemark() :void
    {
        this.createRemarkModal.show(this.registerId);
    }

}