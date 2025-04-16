import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateReservationRequest } from '../../models/reservation';
import { InputNumberModule } from 'primeng/inputnumber';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { DatePicker } from 'primeng/datepicker';
import { FloatLabel } from 'primeng/floatlabel';
import { DateTime } from 'luxon';
import { DropdownModule } from 'primeng/dropdown';
import { SelectModule } from 'primeng/select';

@Component({
    selector: 'app-reservation-create',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        InputNumberModule,
        ButtonModule,
        DatePicker,
        FloatLabel,
        DropdownModule,
        SelectModule
    ],
    templateUrl: './reservation-create.html'
})
export class ReservationCreateComponent implements OnInit {
    constructor() {}
    
    @Output() 
    createReservation = new EventEmitter<CreateReservationRequest>();
    
    formGroup!: FormGroup;
    minDate: Date = new Date(Date.now());
    tableCapacities = [
        { name: 'Mesa para 2', value: TableCapacityEnum.TWO },
        { name: 'Mesa para 4', value: TableCapacityEnum.FOUR },
        { name: 'Mesa para 6', value: TableCapacityEnum.SIX }   
    ]
    timeSlots = [
        { name: '20:00', value: TimeSlotEnum.SLOT_2000 },
        { name: '20:30', value: TimeSlotEnum.SLOT_2030 },
        { name: '21:00', value: TimeSlotEnum.SLOT_2100 },
        { name: '21:30', value: TimeSlotEnum.SLOT_2130 },
        { name: '22:00', value: TimeSlotEnum.SLOT_2200 },
        { name: '22:30', value: TimeSlotEnum.SLOT_2230 },
        { name: '23:00', value: TimeSlotEnum.SLOT_2300 },
        { name: '23:30', value: TimeSlotEnum.SLOT_2330 }
    ];
    isLoading: boolean = false;
    isInitialized: boolean = false;

    public ngOnInit(): void {
        this.initFormGroup();
        this.isInitialized = true;
    }

    
    private initFormGroup() {
        this.formGroup = new FormGroup({
            clientId: new FormControl(null, Validators.required),
            tableCapacity: new FormControl(null, Validators.required),
            reservationDate: new FormControl(null, Validators.required),
            timeSlot: new FormControl(null, Validators.required),
        });
    }

    protected onSubmit() {
        if (this.formGroup.invalid) {
            Object.values(this.formGroup.controls).forEach(control => {
                control.markAsTouched();
                control.markAsDirty();
            });
            return;
        }

        const reservationDate = DateTime.fromJSDate(this.formGroup.value.reservationDate).setZone('America/Argentina/Buenos_Aires');
        
        if (!reservationDate)
            throw new Error('La fecha de reserva no es válida');


        const reservation: CreateReservationRequest = {
            clientId: this.formGroup.value.clientId,
            tableCapacity: this.formGroup.value.tableCapacity,
            reservationDate: reservationDate!,
            timeSlot: this.formGroup.value.timeSlot
        };

            this.createReservation.emit(reservation);
            this.formGroup.reset();
    }  
}   

export enum TableCapacityEnum{
    TWO = 2,
    FOUR = 4,
    SIX = 6
}

export enum TimeSlotEnum {
    SLOT_2000 = 1,
    SLOT_2030 = 2,
    SLOT_2100 = 3,
    SLOT_2130 = 4,
    SLOT_2200 = 5,
    SLOT_2230 = 6,
    SLOT_2300 = 7,
    SLOT_2330 = 8,
}