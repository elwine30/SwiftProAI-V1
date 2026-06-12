import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CaseClaimDataService {
    private statusIdSource = new BehaviorSubject<number | null>(null);
    currentStatusId = this.statusIdSource.asObservable();
  
    constructor() { }
  
    changeStatusId(statusId: number | null) {
      this.statusIdSource.next(statusId);
    }
}
