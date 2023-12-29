import { HttpClient, } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { environment } from 'environments/environment';
import { BehaviorSubject, Observable} from 'rxjs';


@Injectable()
export class dbService implements Resolve<any> {
  public rows: any;
  public onUserListChanged: BehaviorSubject<any>;
 /**
   * Constructor
   *
   * @param {HttpClient} _httpClient
   */
  constructor(private _httpClient: HttpClient) {
    // Set the defaults
    this.onUserListChanged = new BehaviorSubject({});
  }

  /**
   * Resolver
   *
   * @param {ActivatedRouteSnapshot} route
   * @param {RouterStateSnapshot} state
   * @returns {Observable<any> | Promise<any> | any}
   */
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise<void>((resolve, reject) => {
      Promise.all([this.getDataTableRows()]).then(() => {
        resolve();
      }, reject);
    });
  }

configurationSave(configData: any): Observable<any> {
    return this._httpClient
      .post<any>(`${environment.apiUrl}/Configuration/SaveConnectionStringParams`, configData)
  }

getAllCredential(): Observable<any> {
      return this._httpClient
      .get<any>(`${environment.apiUrl}/Configuration/GetCredentialsByCredentialName`);
}

removeCredential(id: number): Observable<any> {
  return this._httpClient.delete<any>(`${environment.apiUrl}/Configuration/RemoveConnectionById?id=${id}`);
}

fetchAllTable(credentials: any): Observable<any> {
  return this._httpClient
    .post<any>(`${environment.apiUrl}/Configuration/GetAllTablesMetaData`, credentials)
}

getCredentialById(id: number): Observable<any> {
  return this._httpClient.get<any>(`${environment.apiUrl}/Configuration/GetCredentialById?id=${id}`);
}

getTableNames(credentials:any):Observable<any>{
  return this._httpClient.post<any>(`${environment.apiUrl}/Configuration/GetTableNames`,credentials)
}

getMetaDataTableByName(tblName: string, credentials: any): Observable<any> {
  const params = { tblName: tblName };
  const body = credentials;
  return this._httpClient.post<any>(`${environment.apiUrl}/Configuration/GetMetaDataTableByName`, body, { params: params });
}

createTable(data: any): Observable<any> {
  return this._httpClient.post<any>(`${environment.apiUrl}/Configuration/CreateTable`,data)
}

getMetaDataMultipleTableByName(tableNames: string[], credentials: any): Observable<any> {
  const body = { tableNames: tableNames, conn: credentials };
  return this._httpClient.post<any>(`${environment.apiUrl}/Configuration/GetMetaDataMultipleTableByName`, body);
}

  getDataTableRows(): Promise<any[]> {
    return new Promise((resolve, reject) => {
      this._httpClient.get('api/users-data').subscribe((response: any) => {
        this.rows = response;
        this.onUserListChanged.next(this.rows);
        resolve(this.rows);
      }, reject);
    });
  }
}

