import { DateTime } from "luxon";
import { TableCapacityEnum, TimeSlotEnum } from "../components/reservation-create/reservation-create";

export interface WaitingListEntryRequest {
    clientId: number;
    tableCapacity: TableCapacityEnum
    reservationDate: DateTime;
    timeSlot: TimeSlotEnum;
}

export interface WaitingList {
    clientName: string;
    reservationDateTime: DateTime;
}