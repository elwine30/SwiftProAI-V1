import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class DataService {
    private data: any;

    setNotOnboardData(data: any) {
        this.data = data;
    }

    getNotOnboardData() {
        return this.data;
    }
}