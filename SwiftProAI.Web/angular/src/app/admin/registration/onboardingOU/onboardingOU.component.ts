import { Component, Injector, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DataService } from '@app/shared/common/data-service/data-service';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    CreateOrEditDocumentSettingDto,
    CreateOrganizationUnitInput,
    CreateOrUpdateUserInput,
    CreateOUOnboardingInput,
    OrganizationUnitDto,
    OUOnboardingServiceProxy,
    RoleServiceProxy,
    UserEditDto,
    UserServiceProxy,
} from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'app-onboardingOU',
    templateUrl: './onboardingOU.component.html',
    styleUrls: ['./onboardingOU.component.css'],
})
export class OnboardingOUComponent extends AppComponentBase implements OnInit {
    onboardingForm: FormGroup;

    adminId: number;

    constructor(
        injector: Injector,
        private fb: FormBuilder,
        private _roleService: RoleServiceProxy,
        private _ouOnboardingService: OUOnboardingServiceProxy,
        private router: Router,
        private dataService: DataService,
    ) {
        super(injector);
    }

    emailDotValidator(control: AbstractControl): ValidationErrors | null {
        const email = control.value;
        if (email && email.includes('@') && !email.split('@')[1].includes('.')) {
            return { emailDotError: true };
        }
        return null;
    }

    ngOnInit() {
        this.onboardingForm = this.fb.group({
            organization: this.fb.group({
                displayName: ['', Validators.required],
            }),
            documentSetting: this.fb.group({
                businessRegistrationNo: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(50)]],
                companyLegalName: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(100)]],
                address: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(200)]],
                telNo: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(15)]],
                companyType: ['', Validators.required],
                invoiceRefNoPrefix: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(10)]],
                invoiceRefNoLength: ['', [Validators.required, Validators.min(3), Validators.max(10)]],
                debitRefNoPrefix: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(10)]],
                debitRefNoLength: ['', [Validators.required, Validators.min(3), Validators.max(10)]],
                creditRefNoPrefix: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(10)]],
                creditRefNoLength: ['', [Validators.required, Validators.min(3), Validators.max(10)]],
                caseRefNoPrefix: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(10)]],
                caseRefNoLength: ['', [Validators.required, Validators.min(3), Validators.max(10)]],
            }),
            admin: this.fb.group({
                user: this.fb.group({
                    userName: ['', Validators.required],
                    emailAddress: ['', [Validators.required, Validators.email, this.emailDotValidator]],
                    name: ['', Validators.required],
                    surname: ['', Validators.required],
                    phoneNumber: ['', Validators.required],
                    password: ['', Validators.required],
                }),
                sendActivationEmail: [true],
            }),
        });

        this.onboardingForm.get('organization.displayName').valueChanges.subscribe((value) => {
            this.onboardingForm.get('documentSetting.companyLegalName').setValue(value);
        });

        const approvalNotOnboard = this.dataService.getNotOnboardData();

        if (approvalNotOnboard != undefined || approvalNotOnboard != null) {
            this.onboardingForm.get('organization.displayName').setValue(approvalNotOnboard.name);
            this.onboardingForm.get('documentSetting.companyLegalName').setValue(approvalNotOnboard.name);
            this.onboardingForm
                .get('documentSetting.businessRegistrationNo')
                .setValue(approvalNotOnboard.businessRegistrationNo);
        }
    }

    getAdminId() {
        var roleLists;
        this._roleService.getRoles(undefined).subscribe((result) => {
            roleLists = result;
        });
        this.adminId = roleLists.find((role) => role.name === 'admin').id;
    }

    submitForm() {
        if (this.onboardingForm.invalid) {
            this.onboardingForm.markAllAsTouched();
            this.checkFormFields(); // Log the validity of each field
            return;
        }

        console.log('Submit');

        const formValues = this.onboardingForm.value;

        const organizationData = new CreateOrganizationUnitInput(formValues.organization);
        organizationData.companyType = formValues.documentSetting.companyType;
        const documentSettingData = new CreateOrEditDocumentSettingDto(formValues.documentSetting);

        const user = new UserEditDto(formValues.admin.user);

        const adminData = new CreateOrUpdateUserInput({
            user: user,
            assignedRoleNames: ['3rd Party Admin'],
            sendActivationEmail: formValues.admin.sendActivationEmail,
            setRandomPassword: false,
            organizationUnits: [],
        });

        documentSettingData.companyLegalName = organizationData.displayName;

        const onboardingOUInput = new CreateOUOnboardingInput({
            organizationDto: organizationData,
            documentSettingDto: documentSettingData,
            adminDto: adminData,
        });

        this._ouOnboardingService.createOnboardingOu(onboardingOUInput).subscribe(
            (result) => {
                this.message.success('Registered Successfully');
                this.resetForm();
            },
            (error) => {
                console.error('Error creating Organization Unit:', error);
            },
        );
    }

    isFieldInvalid(groupName: string, fieldName: string): boolean {
        const control = this.onboardingForm.get(`${groupName}.${fieldName}`);
        return control && control.invalid && (control.dirty || control.touched);
    }

    checkFormFields() {
        Object.keys(this.onboardingForm.controls).forEach((groupKey) => {
            const group = this.onboardingForm.get(groupKey) as FormGroup;

            if (group) {
                // Loop through each form group control
                Object.keys(group.controls).forEach((controlKey) => {
                    const control = group.get(controlKey);
                    if (control instanceof FormGroup) {
                        // Recursively check nested form groups
                        this.checkFormControls(control, `${groupKey}.${controlKey}`);
                    } else {
                        console.log(`${groupKey}.${controlKey} - Valid: ${control.valid}, Invalid: ${control.invalid}`);
                    }
                });
            }
        });
    }

    resetForm() {
        this.onboardingForm.reset({
            organization: {
                displayName: '',
            },
            documentSetting: {
                businessRegistrationNo: '',
                companyLegalName: '',
                address: '',
                telNo: '',
                companyType: '',
                invoiceRefNoPrefix: '',
                invoiceRefNoLength: '',
                debitRefNoPrefix: '',
                debitRefNoLength: '',
                creditRefNoPrefix: '',
                creditRefNoLength: '',
                caseRefNoPrefix: '',
                caseRefNoLength: '',
            },
            admin: {
                user: {
                    userName: '',
                    emailAddress: '',
                    name: '',
                    surname: '',
                    phoneNumber: '',
                    password: '',
                },
                sendActivationEmail: true,
            },
        });
    }

    checkFormControls(controlGroup: FormGroup, path: string) {
        Object.keys(controlGroup.controls).forEach((controlKey) => {
            const control = controlGroup.get(controlKey);
            if (control instanceof FormGroup) {
                this.checkFormControls(control, `${path}.${controlKey}`);
            } else {
                console.log(`${path}.${controlKey} - Valid: ${control.valid}, Invalid: ${control.invalid}`);
            }
        });
    }
}
