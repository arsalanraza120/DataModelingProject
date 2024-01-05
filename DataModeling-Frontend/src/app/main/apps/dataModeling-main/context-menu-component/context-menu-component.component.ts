import { Component,  Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'context-menu-component',
    templateUrl: 'context-menu-component.component.html',
    styleUrls: ['context-menu-component.component.scss']
})
export class ContextMenuComponentComponent {
  @Input() column: any;
  constructor(public activeModal: NgbActiveModal) {}
  newRow: any = {}; 
 


insertNewRow() {
    const newRow = {
      columnName: '',
      dataType: '',
      size: '',
      allowNull: false,
      isSelected:true,
    };
   this.column.push(newRow);
    this.activeModal.close('Insert Column clicked');
}

deleteColumn() {
  debugger;
    console.log("Delete column:", this.column);
    this.activeModal.close('Delete Column clicked');
  }
}
