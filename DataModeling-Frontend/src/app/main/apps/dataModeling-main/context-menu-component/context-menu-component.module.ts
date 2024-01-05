
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ContextMenuComponentComponent } from './context-menu-component.component';
import { FormsModule }   from '@angular/forms';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [
        NgbModule,
        FormsModule,
        CommonModule
    ],
    declarations: [  
        ContextMenuComponentComponent,
    ],
    exports: [
        ContextMenuComponentComponent,
    ],
    entryComponents: [
        ContextMenuComponentComponent,
      ],
})
export class ContextMenuComponentModule {

}
