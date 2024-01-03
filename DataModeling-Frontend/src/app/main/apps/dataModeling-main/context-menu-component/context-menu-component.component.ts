import { Component, ElementRef, HostListener, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'context-menu-component',
    templateUrl: 'context-menu-component.component.html',
    styleUrls: ['context-menu-component.component.scss']
})
export class ContextMenuComponentComponent {
  
  @Input() column: any;
  constructor(public activeModal: NgbActiveModal) {}

  InsertColumn() {
    debugger;
    const newRow = {
        columnName: 'New Column', 
        dataType: 'New Data Type',
        size: 'New Size',
        allowNull: true,
      };
  
      this.column.push(newRow);
    this.activeModal.close('Option 1 clicked');
  }

  DeleteColumn() {
    debugger
    const indexToDelete = this.column.findIndex((item) => item.columnName === this.column.columnName);

    if (indexToDelete !== -1) {
      this.column.splice(indexToDelete, 1); // Remove 1 element at the found index
    }

    this.activeModal.close('Option 2 clicked');
  }
 
}
