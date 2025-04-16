import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { WaitingList } from '../../models/waiting-list';

@Component({
    selector: 'app-waiting-list',
    standalone: true,
    imports: [CommonModule, ButtonModule, TableModule],
    templateUrl: './waiting-list.html',
})
export class WaitingListComponent {
    constructor() {}

@Input()
waitingList: WaitingList[] = [];

}
