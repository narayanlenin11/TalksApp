import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private _httpClient: HttpClient) {
  }
  
  public createAdminUser(formData: any): Observable<any> {
    return this._httpClient.post('/api/users/admin', formData, { observe: "response" });
  }
  public createCorporateCustomer(formData: any): Observable<any> {
    return this._httpClient.post('/api/users/corporateCustomer', formData, { observe: "response" });
  }
  public createHomeorOfficeCustomer(formData: any): Observable<any> {
    return this._httpClient.post('/api/users/homeOrOfficeCustomer', formData, { observe: "response" });
  }
  public createStudentCustomer(formData: any): Observable<any> {
    return this._httpClient.post('/api/users/studentCustomer', formData, { observe: "response" });
  }
 
}
