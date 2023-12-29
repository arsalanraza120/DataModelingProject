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
    selectedColumns: any[] = [];
    selectedTables: string[] = [];
    generatedTables: any[] = [];
    tbl_com: boolean;
    routedataId: any;
    tblTasks: any[] = [];
    currentTableName: string;
    selectAllChecked: boolean = false;
    selectedRows: any[] = [];
    loading = false;
    metaTblResponse: MetaTableResponse;
    selectAllTrigger: boolean = false;

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
                            Swal.fire({
                                icon: "error",
                                title: "Error fetching table names" + error,
                            })
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
        if (this.selectedColumns.length > 0) {
            const tableName = "abc"//this.metaTblResponse.data.tableName
            const dataToSend = {
                tableName: tableName,
                selectedRows: this.selectedColumns,
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

    onCheckboxChange(tblName: string) {
        this.loading = true;
        const index = this.selectedTables.indexOf(tblName);
        if (index === -1) {
            this.generatedTables = [] = [];
            this.selectedTables.push(tblName);
        } else {
            this.loading = false;
            this.tbl_com = false;
            this.generatedTables = [] = [];
            this.selectedTables.splice(index, 1);
        }
        this.getMetaDataForSelectedTables();
    }
    // getMetaDataForSelectedTables() {
    //     if (this.selectedTables.length > 0) {
    //         this.tbl_com = true;
    //         this._dbService.getMetaDataMultipleTableByName(this.selectedTables, this.connectionDataService.getConnectionData())
    //             .subscribe((res: MetaTableResponse) => {
    //                 if (res.isSuccess) {
    //                     debugger;
    //                     this.metaTblResponse = res;
    //                     console.log("Testing",this.metaTblResponse);
    //                 } else if (res.isSuccess === false) {

    //                 }
    //             });
    //     }
    // }
    getMetaDataForSelectedTables() {
        if (this.selectedTables.length > 0) {
            this.tbl_com = true;
            this._dbService.getMetaDataMultipleTableByName(this.selectedTables, this.connectionDataService.getConnectionData())
                .subscribe((res: any) => {
                    debugger
                    if (res.isSuccess) {
                        debugger
                        this.loading = false;
                        this.metaTblResponse = res.data;
                        this.generatedTables = [] = [];
                        for (const tableName in this.metaTblResponse) {
                            if (Object.prototype.hasOwnProperty.call(this.metaTblResponse, tableName)) {
                                const table = this.metaTblResponse[tableName];
                                this.generateTable(table);
                            }
                        }
                    } else {

                    }
                });
        }
    }

    generateTable(table: any): void {
        this.generatedTables.push({
            tableName: table.tableName,
            columns: table.tables[table.tableName].columns
        });
    }

    // onItemClick(tblName: string) {
    //     this.tbl_com = true;
    // this._dbService.getMetaDataTableByName(tblName, this.connectionDataService.getConnectionData())
    //     .subscribe((res: MetaTableResponse) => {
    //         if (res.isSuccess) {
    //             this.metaTblResponse = res;
    //             this.currentTableName = res.data.tableName;

    //         } else if (res.isSuccess === false) {

    //         }
    //     });
    // }



    selectAllColumn(): void {
        this.selectedColumns = [];
        const isAnyColumnUnselected = this.generatedTables.some(table => table.columns.some(column => !column.isSelected));
        this.generatedTables.forEach(table => {
            table.columns.forEach(column => {
                column.isSelected = isAnyColumnUnselected;
                if (isAnyColumnUnselected) {
                    this.selectedColumns.push(column);
                }
            });
        });
        this.selectAllTrigger = false;
    }

    isSelects(column: any): void {
        if (column.isSelected) {
            this.selectedColumns.push(column);
        } else {
            this.selectedColumns = this.selectedColumns.filter(selectedColumn => selectedColumn !== column);
        }
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
