import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { Reservation } from '../../models/reservation';

@Component({
    selector: 'app-reservation-list',
    standalone: true,
    imports: [CommonModule, ButtonModule, TableModule], 
    templateUrl: './reservation-list.html'
})
export class ReservationListComponent {
    
    constructor() {}
    
    @Input() 
    reservations: Reservation[] = [];
    
    @Output() 
    deleteReservation = new EventEmitter<number>();

    onDelete(id: number) {
        this.deleteReservation.emit(id);
    }
}