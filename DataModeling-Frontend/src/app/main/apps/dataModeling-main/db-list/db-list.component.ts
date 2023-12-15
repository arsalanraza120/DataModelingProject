import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'db-list',
  templateUrl: './db-list.component.html',
  styleUrls: ['./db-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class dbListComponent implements OnInit {

  /**
   * Constructor
   *
   * @param {CoreConfigService} _coreConfigService
   * @param {UserListService} _userListService
   * @param {CoreSidebarService} _coreSidebarService
   */
  

  constructor() {    
  }

  ngOnInit(): void {
  }

}
