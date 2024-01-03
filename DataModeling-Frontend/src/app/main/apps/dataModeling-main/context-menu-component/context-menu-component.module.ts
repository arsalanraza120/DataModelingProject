// Angular Imports
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ContextMenuComponentComponent } from './context-menu-component.component';

@NgModule({
    imports: [
        CommonModule,
        NgbModule
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
