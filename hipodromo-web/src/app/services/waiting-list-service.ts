import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { WaitingList, WaitingListEntryRequest } from '../models/waiting-list';
import { environment } from '../../environments/environment.development';

@Injectable({
    providedIn: 'root'
})
export class WaitingListService {
    private http = inject<HttpClient>(HttpClient);

    constructor() { }
    
    private apiUrl = `${environment.apiUrl}/waitingList`;

    public async getWaitingList(): Promise<WaitingList[]> {
        const url = `${this.apiUrl}/list`;
        const response = this.http.get<WaitingList[]>(url);
        return lastValueFrom(response);
    }

    public async addClientToWaitingList(request: WaitingListEntryRequest): Promise<WaitingList> {
        const url = `${this.apiUrl}/addClientToWaitingList`;
        const response = this.http.post<WaitingList>(url, request);
        return lastValueFrom(response);
    }
}