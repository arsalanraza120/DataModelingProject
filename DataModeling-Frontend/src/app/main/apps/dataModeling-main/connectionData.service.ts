
import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root',
})

export class connectionDataService 
{
  private connectionData: any;
  setConnectionData(data: any) {
    this.connectionData = data;
  }

  getConnectionData() {
    return this.connectionData;
  }


}