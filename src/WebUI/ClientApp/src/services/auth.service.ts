import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import jwt_decode from "jwt-decode";
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
//import { constants } from 'crypto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();
  private _user: User;

  constructor(private http: HttpClient,
              private router: Router,
              private toastrService: ToastrService)
              { }

  login(userName: string, password: string): void {
    this.http.post<any>('/api/authenticate', { UserName: userName, Password: password })
      .subscribe(
        res => {
          localStorage.setItem('token', res.token);
          this._user = this.createUserFromToken(res.token);
          this.router.navigate(['/home']);
          this.toastrService.success('User loggged In Successfully !', 'Success');
          this.isLoggedInSubject.next(true);
        },
        (err: HttpErrorResponse) => {
          console.log(err);
          if (err.status == 401) {
            this.toastrService.info("Unauthorized!")
          }
        });
  }

  decodeToken(): any {
    let rawToken = localStorage.getItem('token');
    if (rawToken != null)
      return jwt_decode(rawToken);
    else
      return null;
  }

  showLoginPageIfTokenExpries(): void {
    if (this.isTokenExpired())
      this.isLoggedInSubject.next(false); // push to subscribers of observable
    else
      this.isLoggedInSubject.next(true);  // push to subscribers of observable
  }

  public getTokenExpirationDate(): Date {
    let rawToken = localStorage.getItem('token');
    let decoded: any = {};
     decoded = jwt_decode(rawToken);

    if (decoded.exp === undefined)
      return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

  public isTokenExpired(): boolean {
    let rawToken = localStorage.getItem('token');
    if (rawToken == null)
      return true;

    const date = this.getTokenExpirationDate();
    if (date === undefined) return false;
    return !(date.valueOf() > new Date().valueOf());
  }


  private createUserFromToken(rawToken: string): User {
    let token: any = {};
    token = jwt_decode(rawToken);
    let user = new User();
    user.userName = token.unique_name;
    user.email = token.email;
    user.customerType = token.CustomerType;
    user.userId = token.sub;

    return user;
  }

  get user(): User {
    return this._user ?? this.createUserFromToken(localStorage.getItem('token'));
  }

  public logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['']);

    // push to subscribers of observable
    this.isLoggedInSubject.next(false);
  }
}

export class User {
  userName: string;
  phoneNumber: string;
  email: string;
  customerType: string;
  userId: string;
}
