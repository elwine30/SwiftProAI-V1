import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CaseThirdPartyInfosDataService {
  private selectedItemSubject = new BehaviorSubject<number>(null);
  selectedItem$ = this.selectedItemSubject.asObservable();

  private refreshPageSubject = new BehaviorSubject<void>(null);
  refreshPage$ = this.refreshPageSubject.asObservable();

  constructor() {}

  selectItem(itemId: number) {
    this.selectedItemSubject.next(itemId);
  }

  refreshPage() {
    this.refreshPageSubject.next();
  }
}
