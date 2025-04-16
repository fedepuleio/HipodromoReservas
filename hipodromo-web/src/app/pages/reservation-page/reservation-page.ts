import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReservationListComponent } from '../../components/reservation-list/reservation-list';
import { ButtonModule } from 'primeng/button';
import { RouterModule } from '@angular/router';
import { ReservationCreateComponent } from '../../components/reservation-create/reservation-create';
import { ReservationService } from '../../services/reservation.service';
import { CreateReservationRequest, Reservation } from '../../models/reservation';
import { HttpErrorResponse } from '@angular/common/http';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { WaitingList, WaitingListEntryRequest } from '../../models/waiting-list';
import { WaitingListService } from '../../services/waiting-list-service';
import { WaitingListComponent } from '../../components/waiting-list/waiting-list';
import { InplaceModule } from 'primeng/inplace';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@Component({
    selector: 'app-reservation-page',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        ButtonModule,
        ReservationListComponent,
        ReservationCreateComponent,
        ToastModule,
        ConfirmDialogModule ,
        WaitingListComponent,
        InplaceModule
    ],
    templateUrl: './reservation-page.html',
})
export class ReservationPageComponent implements OnInit {
    private reservationService = inject<ReservationService>(ReservationService);
    private waitingListService = inject<WaitingListService>(WaitingListService);
    private messageService = inject<MessageService>(MessageService);
    private confirmationService = inject<ConfirmationService>(ConfirmationService);
    
    constructor() {}
    
    reservationsList: Reservation[] = [];
    waitingList: WaitingList[] = [];

    isInitialized: boolean = false;
    isLoading = false;

    public async ngOnInit() {
        await this.loadReservations();
        await this.loadWaitingList();
        this.isInitialized = true;
    }
    
    public async loadReservations() {
        try {
            this.reservationsList = await this.reservationService.getReservations();
        } 
        catch (error) {
            if (error instanceof HttpErrorResponse) {
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: error.error?.message || 'Error inesperado al cargar las reservas.',
                    life: 3000
                });
            }
        }
    }

    public async loadWaitingList() {
        try {
            this.waitingList = await this.waitingListService.getWaitingList();
        } 
        catch (error) {
            if (error instanceof HttpErrorResponse) {
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: error.error?.message || 'Error inesperado al cargar la lista de espera.',
                    life: 3000
                });
            }
        }
    }

    protected async deleteReservation(id: number) {
        try {
            await this.reservationService.deleteReservation(id);
            await this.loadReservations();
            await this.loadWaitingList();
        } 
        catch (error) {
            if (error instanceof HttpErrorResponse) {
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: error.error?.message || 'Error inesperado al cancelar la reserva.',
                    life: 3000
                });
            }
        }
    }

    protected async handleCreateReservation(request: CreateReservationRequest) {
        try {
            this.isLoading = true;

            const response = await this.reservationService.createReservation(request);
            
            if (response.waitingList) {
                this.confirmationService.confirm({
                    message: 'No hay mesas disponibles. ¿Deseás unirte a la lista de espera?',
                    header: 'Lista de espera',
                    icon: 'pi pi-info-circle',
                    acceptLabel: 'Sí',
                    rejectLabel: 'No',
                    accept: async () => {
                        await this.addClientToWaitingList(request);
                        await this.loadWaitingList();
                    }
                });
            } 
            else {
                this.messageService.add({
                    severity: 'success',
                    summary: 'Reserva confirmada',
                    detail:  `¡Mesa N° ${response.tableId} reservada con éxito! `,
                    life: 3000
                });
            }
        } 
        catch (error) {
            if (error instanceof HttpErrorResponse) {
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: error.error?.message || 'Error inesperado al crear la reserva.',
                    life: 3000
                });
            }
        }
        finally {
            this.isLoading = false;
            await this.loadReservations();
        }
    }

    private async addClientToWaitingList(request: CreateReservationRequest) {
        try { 
            const waitingListRequest: WaitingListEntryRequest = {
                clientId: request.clientId,
                reservationDate: request.reservationDate,
                tableCapacity: request.tableCapacity,
                timeSlot: request.timeSlot
            };
            const response = await this.waitingListService.addClientToWaitingList(waitingListRequest);
            this.messageService.add({
                severity: 'info',
                summary: 'Agregado a la lista',
                detail:  `${response.clientName}, te agregamos a la lista de espera! `,
                life: 3000
            });
        } 
        catch (error) {
            if (error instanceof HttpErrorResponse) {
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: error.error?.message || 'Error inesperado al ingresarte en la lista de espera.',
                    life: 3000
                });
            }
        }
    }
}