import { Injectable, EventEmitter } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UsersDataService {
  onSave: EventEmitter<any> = new EventEmitter();
  onShow: EventEmitter<any> = new EventEmitter(); 

  constructor() { }

  triggerShow(userId?: number) {  
    this.onShow.emit(userId);
  }

  triggerSave(userId?: number) {
    this.onSave.emit(userId);
  }
}
