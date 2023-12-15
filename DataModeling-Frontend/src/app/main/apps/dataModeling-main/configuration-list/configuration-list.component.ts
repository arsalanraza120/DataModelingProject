import { dbService } from '../db-list/db-list.service';
import Swal from 'sweetalert2';
import { Component,  OnInit } from '@angular/core';

import { Router } from '@angular/router';

@Component({
  selector: 'configuration-list',
  templateUrl: 'configuration-list.component.html',
  styleUrls: ['configuration-list.component.scss']
})

export class ConfigurationListComponent implements OnInit {


  databaseModel: any;
  totalCount: number;
  credentialsList: any[];
  loading= true;
  constructor(private _dbService: dbService,
    private router: Router,
  ) {
    this.loading = true;
    this._dbService
      .getAllCredential()
      .subscribe((res: any) => {
        if (res.isSuccess) {
          this.loading = false;
          this.credentialsList = res.data.groupedCredentials;
          const message = res.message;
          this.totalCount = res.data.totalCount;
        }
        else if (res.isSuccess == false) {
          this.loading = false;
          const message = res.data.message;
          this.totalCount = res.data.totalCount;

        }
      });
  }


  ngOnInit() {
  }



  fetchData(credentials: any) {
    console.log("Click Credential", credentials);
    console.log("test ===> ID", credentials.id);
    this.router.navigate(['apps/dataModeling-main/modeling', credentials.id])

  }




  deleteCredentials(id: number) {
    this._dbService.removeCredential(id).subscribe((res: any) => {
      if (res.isSuccess) {
        const message = res.message;
        Swal.fire({
          icon: 'success',
          title: 'Remove Success Fully',
          text: message
        });
      }
      else if (res.isSuccess == false) {
        const message = res.message;
        Swal.fire({
          icon: 'error',
          title: 'Failed',
          text: message,
        });
      }
    });
  }

  generateArray(totalCount: number): any[] {
    return Array(totalCount);
  }


}
