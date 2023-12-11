// Angular Imports
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgApexchartsModule } from 'ng-apexcharts';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { Ng2FlatpickrModule } from 'ng2-flatpickr';

import { CoreCommonModule } from '@core/common.module';
import { CoreDirectivesModule } from '@core/directives/directives';
import { CorePipesModule } from '@core/pipes/pipes.module';
import { CoreSidebarModule } from '@core/components';
import { InputMaskModule } from 'app/main/forms/form-elements/input-mask/input-mask.module';
import { CardSnippetModule } from '@core/components/card-snippet/card-snippet.module';
import { NgxMaskModule } from 'ngx-mask';
// This Module's Components
import { ConfigurationComponent } from './configuration.component';

@NgModule({
    imports: [
        CommonModule,
        CoreCommonModule,
        NgApexchartsModule,
        FormsModule,
        ReactiveFormsModule,
        NgbModule,
        InputMaskModule,
        NgSelectModule,
        Ng2FlatpickrModule,
        NgxDatatableModule,
        CorePipesModule,
        CoreDirectivesModule,
        CoreSidebarModule,
        CardSnippetModule,
        NgxMaskModule.forRoot(),

    ],
    declarations: [
        ConfigurationComponent,
    ],
    exports: [
        ConfigurationComponent,
    ]
})
export class ConfigurationModule {

}
