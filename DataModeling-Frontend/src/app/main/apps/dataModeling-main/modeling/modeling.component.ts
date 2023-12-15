import { Component, OnInit } from '@angular/core';
import { dbService } from '../db-list/db-list.service';
import { ActivatedRoute } from '@angular/router';
import { connectionDataService } from '../connectionData.service';
import Swal from 'sweetalert2';


interface Column {
    columnName: string;
    dataType: string;
    size: string;
    allowNull: boolean;
    isSelected?: boolean;

}

interface Table {
    columns: Column[];
    foreignKeys: {
        constraintName: string,
        foreignKeyColumnName: string,
        referencedConstraintName: string,
        referencedTableName: string
    }[];
    primaryKeys: { columnName: string }[];
}

interface MetaTableResponse {
    data: {
        tableName: string;
        tables: {
            [tableName: string]: Table;
        };
    };
    isSuccess: boolean;
}

@Component({
    selector: 'modeling',
    templateUrl: 'modeling.component.html',
    styleUrls: ['modeling.component.scss']
})


export class ModelingComponent implements OnInit {

    tbl_com: boolean;
    routedataId: any;
    tblTasks: any[] = [];
    currentTableName: string;
    selectAllChecked: boolean = false;
    selectedRows: any[] = [];
    loading = true;
    metaTblResponse: MetaTableResponse;



    ngOnInit(): void {
       this.selectAllChecked = false;
    }
    toggleEdit(column: any) {

        column.editable = !column.editable;
    }

    constructor(
        private route: ActivatedRoute,
        private connectionDataService: connectionDataService,
        private _dbService: dbService
    ) {

        this.tbl_com = false;
        this.loading = true;
        this.route.params.subscribe(params => {
            this.routedataId = params['id'];
            this._dbService.getCredentialById(this.routedataId).subscribe((res: any) => {
                if (res.isSuccess) {
                    this.loading = false;
                    const connectionData = res.data;
                    this.connectionDataService.setConnectionData(connectionData);
                    this._dbService.getTableNames(this.connectionDataService.getConnectionData()).subscribe(
                        (apiResponse: any) => {
                            if (apiResponse.data.isSuccess) {
                                this.loading = false;
                                this.tblTasks = apiResponse.data.data;

                            }
                            else if (apiResponse.data.isSuccess == false) {
                                const message = apiResponse.data.message;
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error Fetching Table',
                                    text: message,
                                });
                            }
                        },
                        error => {
                            console.error('Error fetching table names:', error);
                        }
                    );
                }
                else if (res.isSuccess == false) {
                    this.loading = false;
                }
            })
        });
    }

    createObjTbl() {
        this.loading = true;
        if (this.selectedRows.length > 0) {
            const tableName = this.metaTblResponse.data.tableName
            const dataToSend = {
                tableName: tableName,
                selectedRows: this.selectedRows,
            };
            this._dbService.createTable(dataToSend).subscribe((res: any) => {
                if (res.isSuccess) {
                    this.loading = false;
                    const message = res.message;
                    Swal.fire({
                        icon: 'success',
                        title: 'Table SuccessFully',
                        text: message,
                    });
                }
                else if (res.isSuccess == false) {
                    this.loading = false;
                    const message = res.message;
                    Swal.fire({
                        icon: 'error',
                        title: 'Table Error',
                        text: message,
                    });
                }
            });
        } else {
            Swal.fire({
                icon: 'warning',
                title: 'No Rows selected',
            });
        }
    }


    onItemClick(tblName: string) {
        this.tbl_com = true;
        this._dbService.getMetaDataTableByName(tblName, this.connectionDataService.getConnectionData())
            .subscribe((res: MetaTableResponse) => {
                if (res.isSuccess) {
                    this.metaTblResponse = res;
                    this.currentTableName = res.data.tableName;

                } else if (res.isSuccess === false) {

                }
            });
    }



    selectAllColumns() {
        const currentTable = this.metaTblResponse.data.tables[this.currentTableName];
        if (currentTable) {
            for (var i = 0; i < currentTable.columns.length; i++) {
                currentTable.columns[i].isSelected = this.selectAllChecked;
            }
            this.selectedRows = currentTable.columns.filter((item: any) => item.isSelected);
        }
    }

    isSelect() {
        const currentTable = this.metaTblResponse.data.tables[this.currentTableName];
        if (currentTable) {
            this.selectAllChecked = currentTable.columns.every((item: any) => item.isSelected === true);
            this.selectedRows = currentTable.columns.filter((item: any) => item.isSelected);
        }
    }

}
