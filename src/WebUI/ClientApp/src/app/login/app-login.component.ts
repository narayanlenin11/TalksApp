import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder, AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms'
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoaderService } from '../../services/loader.service';
import { UserService } from '../users/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './app-login.component.html',
  styleUrls:['./app-login.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  addCustomerForm: FormGroup;
  siginInsubmitted = false;
  submitted = false;
  isNewUser = false;
  customerTypes : Array<any> =[
    { value: 'Corporate', type:'Corporate Customer' },
    { value: 'HomeOrOffice', type:'Home / Office Customer' },
    { value: 'Student', type:'Student Customer' },
    { value: 'Admin', type:'Admin' },

  ];
  constructor(private http: HttpClient,
    private router: Router,
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private loaderService: LoaderService,
    private _userService: UserService,
    private toastrService: ToastrService) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      userName: ['', Validators.required],
      password: ['', Validators.required],

    });
    this.addCustomerForm = this.formBuilder.group({
      userName: ['', Validators.required],
      email: ['', Validators.required],
      address: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      companyName: ['',],
      companyAddress: ['',],
      companyPhone: [''],
      companyPreferred: [''],
      fax: [''],
      shippingMethod: [''],
      hOCompanyName: [''],
      hOFax: [''],
      school: [''],
      password: [''],
      confirmPassword: [''],
      customerType: ['', Validators.required],
      passwordGroup: this.formBuilder.group({
        password: ['', Validators.required],
        confirmPassword: ['', Validators.required],
      }, { validator: passwordMatcher }),
    },
    );

    this.addCustomerForm.get('customerType').valueChanges.subscribe(value => {
      const companyNameControl = this.addCustomerForm.get('companyName');
      const companyAddressControl = this.addCustomerForm.get('companyAddress');
      const companyPhoneControl = this.addCustomerForm.get('companyPhone');
      const companyPreferredControl = this.addCustomerForm.get('companyPreferred');
      const faxControl = this.addCustomerForm.get('fax');
      const shippingMethodControl = this.addCustomerForm.get('shippingMethod');
      const hOCompanyNameControl = this.addCustomerForm.get('hOCompanyName');
      const hOFaxControl = this.addCustomerForm.get('hOFax');
      const schoolControl = this.addCustomerForm.get('school');

      switch (value) {
        case 'Corporate': {
          companyNameControl.setValidators(Validators.required);
          companyAddressControl.setValidators(Validators.required);
          shippingMethodControl.setValidators(Validators.required);
          companyPhoneControl.setValidators(Validators.required);
          companyPreferredControl.setValidators(Validators.required);
          faxControl.setValidators(Validators.required);

          hOCompanyNameControl.clearValidators();
          hOFaxControl.clearValidators();
          schoolControl.clearValidators();
          break;
        }
        case 'HomeOrOffice': {
          hOCompanyNameControl.setValidators(Validators.required);
          hOFaxControl.setValidators(Validators.required);
          companyNameControl.clearValidators();
          companyAddressControl.clearValidators();
          shippingMethodControl.clearValidators();
          companyPhoneControl.clearValidators();
          companyPreferredControl.clearValidators();
          faxControl.clearValidators();
          schoolControl.clearValidators();
          break;
        }
        case 'Student': {
          schoolControl.setValidators(Validators.required);
          companyNameControl.clearValidators();
          companyAddressControl.clearValidators();
          shippingMethodControl.clearValidators();
          companyPhoneControl.clearValidators();
          companyPreferredControl.clearValidators();
          faxControl.clearValidators();
          hOCompanyNameControl.clearValidators();
          hOFaxControl.clearValidators();
          break;
        }
      } 
      this.addCustomerForm.updateValueAndValidity();
    });

  }

  get g() { return this.loginForm.controls; }

  get f() { return this.addCustomerForm.controls; }

  onSubmit(): void {

    this.siginInsubmitted= true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    this.authService.login(this.loginForm.controls.userName.value, this.loginForm.controls.password.value);
    console.log(this.authService.user);;
  }

  get isLoading() {
    return this.loaderService.isLoading;
  }
  Changetype(type) {
    if (type) {
      this.isNewUser = true;
    }
    else{
      this.isNewUser = false;
    }
  }
  onCreateNewCustomer(): void {
    this.submitted = true;
    const password = this.addCustomerForm.controls.passwordGroup.get('password').value;
    const confirmPassword = this.addCustomerForm.controls.passwordGroup.get('confirmPassword').value;
    this.addCustomerForm.controls.password.setValue(password);
    this.addCustomerForm.controls.confirmPassword.setValue(confirmPassword);
    if (this.addCustomerForm.invalid) {
      return;
    }
    switch (this.addCustomerForm.value.customerType) {
      case 'Corporate': {
        this._userService.createCorporateCustomer(this.addCustomerForm.value)
      .subscribe((res: HttpResponse<void>) => {
        if (res.status === 201) {
          this.addCustomerForm.reset();
          this.submitted = false;
          this.toastrService.success('Corporate Customer Successfully Created !', 'Success', {
            timeOut: 3000,
            positionClass: 'toast-bottom-right'
          });
        };
      },
        (err: HttpErrorResponse) => {
          if (err.status === 400) {
            this.toastrService.error(err.error[0], 'error', {
              timeOut: 3000,
              positionClass: 'toast-bottom-right'
            });
          }
          else {
          }
        }
      )
        break;
      }
      case 'HomeOrOffice': {
        this._userService.createHomeorOfficeCustomer(this.addCustomerForm.value)
          .subscribe((res: HttpResponse<void>) => {
            if (res.status === 201) {
              this.addCustomerForm.reset();
              this.submitted = false;
              this.toastrService.success('Home / Office Customer Successfully Created !', 'Success', {
                timeOut: 3000,
                positionClass: 'toast-bottom-right'
              });
            };
          },
            (err: HttpErrorResponse) => {
              if (err.status === 400) {
                this.toastrService.error(err.error[0], 'error', {
                  timeOut: 3000,
                  positionClass: 'toast-bottom-right'
                });
              }
              else {
              }
            }
          )
        break;
      }
      case 'Student': {
        this._userService.createStudentCustomer(this.addCustomerForm.value)
          .subscribe((res: HttpResponse<void>) => {
            if (res.status === 201) {
              this.addCustomerForm.reset();
              this.submitted = false;
              this.toastrService.success('Student Customer Successfully Created !', 'Success', {
                timeOut: 3000,
                positionClass: 'toast-bottom-right'
              });
            };
          },
            (err: HttpErrorResponse) => {
              if (err.status === 400) {
                this.toastrService.error(err.error[0], 'error', {
                  timeOut: 3000,
                  positionClass: 'toast-bottom-right'
                });
              }
              else {
              }
            }
          )
        break;
      }
      case 'Admin': {
        this._userService.createAdminUser(this.addCustomerForm.value)
          .subscribe((res: HttpResponse<void>) => {
            if (res.status === 201) {
              this.addCustomerForm.reset();
              this.submitted = false;
              this.toastrService.success('Admin User Successfully Created !', 'Success', {
                timeOut: 3000,
                positionClass: 'toast-bottom-right'
              });
            };
          },
            (err: HttpErrorResponse) => {
              if (err.status === 400) {
                this.toastrService.error(err.error[0], 'error', {
                  timeOut: 3000,
                  positionClass: 'toast-bottom-right'
                });
              }
              else {
              }
            }
          )
        break;
      }
      
    } 

    //this._userService.createUser(this.addCustomerForm.value)
    //  .subscribe((res: HttpResponse<void>) => {
    //    if (res.status === 200) {
    //      this.addCustomerForm.reset();
    //      this.submitted = false;
    //      this.toastrService.success('User Successfully Created !', 'Success', {
    //        timeOut: 3000,
    //        positionClass: 'toast-bottom-right'
    //      });
    //    };
    //  },
    //    (err: HttpErrorResponse) => {
    //      if (err.status === 422) {
    //      }
    //      else {
    //      }
    //    }
    //  )

  }
}
function passwordMatcher(c: AbstractControl) {
  const passwordControl = c.get('password');
  const confirmControl = c.get('confirmPassword');

  if (passwordControl.pristine || confirmControl.pristine) {
    // validation passess
    return null;
  }

  if (passwordControl.value === confirmControl.value) {
    // validation passess
    return null;
  }
  // validation fails
  return { 'match': true }
}

