import { Component, OnInit, ViewChild } from '@angular/core';
import { dbService } from '../db-list/db-list.service';
import { ActivatedRoute } from '@angular/router';
import { connectionDataService } from '../connectionData.service';
import Swal from 'sweetalert2';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ContextMenuComponentComponent } from '../context-menu-component/context-menu-component.component';


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
    @ViewChild('modalContent') modalContent: any;
    newRow: any = { columnName: '', dataType: '', size: '', allowNull: false };

    contextMenuOptions = [
        { label: 'Insert Column', action: 'insert' },
        { label: 'Delete Column', action: 'delete' }
    ];
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
    selectedColumn: any;

    ngOnInit(): void {
        this.selectAllChecked = false;
    }

    toggleEdit(column: any) {
        column.editable = !column.editable;
    }

    constructor(
        private route: ActivatedRoute,
        private connectionDataService: connectionDataService,
        private _dbService: dbService,
        private modalService: NgbModal,
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
        if (!this.currentTableName) {

            Swal.fire({
                icon: 'warning',
                title: 'Table Name Required',
                text: 'Please enter a table name before creating the table.',
            });
            this.loading = false;
            return;
        }

        if (this.selectedColumns.length > 0) {
            const tableName = this.currentTableName;
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
                        title: 'Table Successfully Created',
                        text: message,
                    });
                } else {
                    this.loading = false;
                    const message = res.message || 'An error occurred while creating the table.';
                    Swal.fire({
                        icon: 'error',
                        title: 'Table Creation Error',
                        text: message,
                    });
                }
            });
        } else {
            Swal.fire({
                icon: 'warning',
                title: 'No Rows Selected',
                text: 'Please select at least one row before creating the table.',
            });
            this.loading = false;
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

    getMetaDataForSelectedTables() {
        if (this.selectedTables.length > 0) {
            this.tbl_com = true;
            this._dbService.getMetaDataMultipleTableByName(this.selectedTables, this.connectionDataService.getConnectionData())
                .subscribe((res: any) => {
                    if (res.isSuccess) {
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
        this.selectedColumns = [];
        this.generatedTables.push({
            tableName: table.tableName,
            columns: table.tables[table.tableName].columns
        });
    }


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

    isSelect(column: any): void {
        if (column.isSelected) {
            this.selectedColumns.push(column);
        } else {
            this.selectedColumns = this.selectedColumns.filter(selectedColumn => selectedColumn !== column);
        }

    }

    openModal(): void {
        const modalRef = this.modalService.open(this.modalContent, { size: 'lg' });
        modalRef.componentInstance.selectedColumns = this.selectedColumns;
       

    }

    onRightClick(event: MouseEvent, column: any) {
        event.preventDefault();
        this.openContextMenu(column);
    }


    openContextMenu(column: any) {
        debugger
        const modalRef = this.modalService.open(ContextMenuComponentComponent,
            { backdrop: 'static', keyboard: false });
        modalRef.componentInstance.column = column;
        modalRef.result.then((result) => {
            if (result === 'Delete Column clicked') {
                this.deleteColumn(column); // Pass the entire column object
            }
        }, (reason) => {
            console.log(`Context menu dismissed: ${reason}`);
        });
    }


    deleteColumn(column: any) {
        debugger;
        const index = this.selectedColumns.indexOf(column);
        if (index !== -1) {
            this.selectedColumns.splice(index, 1);
        }
        
    }
}
