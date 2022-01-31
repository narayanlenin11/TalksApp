import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  private _isLoading: BehaviorSubject<Boolean> = new BehaviorSubject(false);
  constructor() { }

  show() {
    this._isLoading.next(true);
  }
  hide() {
    this._isLoading.next(false);
  }
  get isLoading() {
    return this._isLoading;
  }
}
