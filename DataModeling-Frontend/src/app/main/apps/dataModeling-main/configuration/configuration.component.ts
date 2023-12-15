import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as snippet from 'app/main/forms/form-elements/input-groups/input-groups.snippetcode';
import Swal from 'sweetalert2';
import { dbService } from '../db-list/db-list.service';


@Component({
  selector: 'configuration',
  templateUrl: 'configuration.component.html',
  styleUrls: ['configuration.component.scss']
})
export class ConfigurationComponent implements OnInit {
  public Host;
  public Port;
  public Password;
  public UserId;
  public InstanceName;
  public ServiceName;
  public DbName;
  public CredentialName;
  routedata: any;
  loading= false;
  public basicPwdShow = false;
  isSubmitting: boolean = false;
  public _snippetCodeBasic = snippet.snippetCodeBasic;

  constructor(
    private route: ActivatedRoute,
    private _dbService: dbService,
    private router: Router
  ) {
    this.route.params.subscribe(params => {
      this.routedata = params['id'];
    });

  }

  ngOnInit(): void {
  }

  submit(form: NgForm) {
    this.loading = true;
    if (form.valid && !this.isSubmitting) {
      this.isSubmitting = true;
      const ConfigData = {
        id: 0,
        Host: this.Host,
        Port: this.Port,
        ServiceName: this.ServiceName,
        InstanceName: this.InstanceName,
        UserId: this.UserId,
        Password: this.Password,
        DbName: this.DbName,
        CredentialName: this.CredentialName,
        dbType: this.routedata,
      };
      this._dbService
        .configurationSave(ConfigData)
        .subscribe((res: any) => {
        
          this.isSubmitting = false
          if (res.isSuccess) {
            const message = res.message;
            this.loading = false;
            Swal.fire({
              icon: 'success',
              title: 'Configuration Success Fully',
              text: message,
            });
            this.router.navigate(['apps/dataModeling-main/config-list'])
          }
          else if (res.isSuccess == false) {
            const message = res.message;
            this.loading = false;
            Swal.fire({
              icon: 'error',
              title: 'Connection Failed',
              text: message,
            });
          }
        }
        );
    }
  }

}
