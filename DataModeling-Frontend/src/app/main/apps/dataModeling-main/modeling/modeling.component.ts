import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { dbService } from '../db-list/db-list.service';
import { ActivatedRoute } from '@angular/router';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
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
    @BlockUI() blockUI: NgBlockUI;
    currentTableName: string;
    selectAllChecked: boolean = false;
    selectedRows: any[] = [];

    metaTblResponse: MetaTableResponse;


    ngOnInit(): void {
        this.blockUI.start();
        this.selectAllChecked = false;
    }
    toggleEdit(column: any) {
        debugger
        column.editable = !column.editable;
    }

    constructor(
        private route: ActivatedRoute,
        private connectionDataService: connectionDataService,
        private changeDetector: ChangeDetectorRef,
        private _dbService: dbService
    ) {
        this.blockUI.start('Loading...');
        this.tbl_com = false
        this.route.params.subscribe(params => {
            this.routedataId = params['id'];
            this._dbService.getCredentialById(this.routedataId).subscribe((res: any) => {
                if (res.isSuccess) {
                    const connectionData = res.data;
                    this.connectionDataService.setConnectionData(connectionData);
                    this._dbService.getTableNames(this.connectionDataService.getConnectionData()).subscribe(
                        (apiResponse: any) => {
                            if (apiResponse.isSuccess) {
                                this.tblTasks = apiResponse.data.data;
                            }
                        },
                        error => {
                            console.error('Error fetching table names:', error);
                        }
                    );
                }
                else if (res.isSuccess == false) {

                }
            })
        });
    }

    createObjTbl() {
        if (this.selectedRows.length > 0) {
            debugger
            this._dbService.createTable(this.selectedRows).subscribe((res: any) => {
                if (res.isSuccess) {
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
                    debugger;
                } else if (res.isSuccess === false) {
                    debugger;
                }
            });
    }



    selectAllColumns() {
        debugger
        const currentTable = this.metaTblResponse.data.tables[this.currentTableName];
        if (currentTable) {
            for (var i = 0; i < currentTable.columns.length; i++) {
                currentTable.columns[i].isSelected = this.selectAllChecked;
            }

            this.selectedRows = currentTable.columns.filter((item: any) => item.isSelected);
        }
    }

    isSelect() {
        debugger;
        const currentTable = this.metaTblResponse.data.tables[this.currentTableName];
        if (currentTable) {
            this.selectAllChecked = currentTable.columns.every((item: any) => item.isSelected === true);
            this.selectedRows = currentTable.columns.filter((item: any) => item.isSelected);
        }
    }

}
