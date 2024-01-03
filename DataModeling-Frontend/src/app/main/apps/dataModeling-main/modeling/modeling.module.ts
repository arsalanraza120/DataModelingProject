import { ModelingComponent } from './modeling.component';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgxGraphModule } from '@swimlane/ngx-graph';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgApexchartsModule } from 'ng-apexcharts';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { Ng2FlatpickrModule } from 'ng2-flatpickr';
import { CoreCommonModule } from '@core/common.module';
import { CoreDirectivesModule } from '@core/directives/directives';
import { CorePipesModule } from '@core/pipes/pipes.module';
import { InputMaskModule } from 'app/main/forms/form-elements/input-mask/input-mask.module';
import { CardSnippetModule } from '@core/components/card-snippet/card-snippet.module';
import { NgxMaskModule } from 'ngx-mask';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { CoreSidebarModule } from '@core/components';
import { dbService } from '../db-list/db-list.service';
import { connectionDataService } from '../connectionData.service';
import { BlockUIModule } from 'ng-block-ui';


const routes: Routes = [
    {
      path: 'modeling',
      component: ModelingComponent
    },
    {
        path: 'modeling/:id',
        component: ModelingComponent
    },

];
@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        CoreCommonModule,
        NgApexchartsModule,
        FormsModule,
        ReactiveFormsModule,
        InputMaskModule,
        NgSelectModule,
        Ng2FlatpickrModule,
        NgxDatatableModule,
        CorePipesModule,
        CoreDirectivesModule,
        CardSnippetModule,
        NgxGraphModule,
        NgxMaskModule.forRoot(),
        CoreSidebarModule,
        PerfectScrollbarModule,
        NgbModule,
        BlockUIModule.forRoot()
    ],
    declarations: [
        ModelingComponent,
    ],
    exports: [
        ModelingComponent,
    ],
    providers: [dbService,connectionDataService]
})
export class ModelingModule {

}
