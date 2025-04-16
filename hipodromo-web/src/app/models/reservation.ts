import { TableCapacityEnum, TimeSlotEnum } from "../components/reservation-create/reservation-create";
import { DateTime } from 'luxon';

export interface CreateReservationRequest {
    clientId: number;
    tableCapacity: TableCapacityEnum
    reservationDate: DateTime;
    timeSlot: TimeSlotEnum;
}

export interface Reservation {
    waitingList: boolean;
    tableId: number;
}