import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
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
import { dbListComponent } from './db-list/db-list.component';
import { dbService } from './db-list/db-list.service';
import { ConfigurationComponent } from './configuration/configuration.component';

// routing
const routes: Routes = [
  {
    path: 'db-list',
    component: dbListComponent,
  },
  {
    path: 'configuration/:id',
    component: ConfigurationComponent,
  },

  {
    path: 'configuration',
    component: ConfigurationComponent,
  },

];

@NgModule({
  declarations: [ConfigurationComponent,dbListComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
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
  providers: [dbService]
})
export class dbListModule {}
