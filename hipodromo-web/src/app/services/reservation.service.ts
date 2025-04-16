import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { CreateReservationRequest, Reservation } from '../models/reservation';
import { environment } from '../../environments/environment.development';

@Injectable({
    providedIn: 'root'
})
export class ReservationService {
    private http = inject<HttpClient>(HttpClient);
    
    constructor() { }
    
    private apiUrl = `${environment.apiUrl}/reservation`;
    
    public async getReservations(): Promise<Reservation[]> {
        const url = `${this.apiUrl}/list`;
        const response = this.http.get<Reservation[]>(url);
        return lastValueFrom(response);
    }

    public async createReservation(request: CreateReservationRequest): Promise<Reservation> {
        const url = `${this.apiUrl}/create`;
        const response = this.http.post<Reservation>(url, request);
        return lastValueFrom(response);
    }

    public async deleteReservation(reservationId: number): Promise<void> {
        const url = `${this.apiUrl}/${reservationId}`;
        const response = this.http.delete<void>(url);
        return lastValueFrom(response);
    } 
}