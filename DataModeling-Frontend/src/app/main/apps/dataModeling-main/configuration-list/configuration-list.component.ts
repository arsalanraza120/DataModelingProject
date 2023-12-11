import { dbService } from '../db-list/db-list.service';
import Swal from 'sweetalert2';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

import { Router } from '@angular/router';

interface Table {
  tableName: string;
  columns: Column[];
  foreignKeys: ForeignKeys[];
}

interface Column {
  columnName: string;
  dataType: string;
  size: string;
  isPrimaryKey?: boolean;
  isForeignKey?: boolean;

}

interface ForeignKeys {
  constraintName: string
  ColumnName: string
  ReferencedTable: string
  ReferencedConstraint: string
}

@Component({
  selector: 'configuration-list',
  templateUrl: 'configuration-list.component.html',
  styleUrls: ['configuration-list.component.scss']
})

export class ConfigurationListComponent implements OnInit {


  databaseModel: any;
  erdData: any = {
    "tables": {
      "User": {
        "tableName": "User",
        "columns": [
          {
            "columnName": "UserId",
            "dataType": "bigint",
            "size": "",
            "isPrimaryKey": true,
            "isForeignKey": false
          },
          {
            "columnName": "Username",
            "dataType": "nvarchar",
            "size": "50",
            "isPrimaryKey": false,
            "isForeignKey": false
          },
          {
            "columnName": "Email",
            "dataType": "nvarchar",
            "size": "50",
            "isPrimaryKey": false,
            "isForeignKey": false
          },
          {
            "columnName": "Password",
            "dataType": "nvarchar",
            "size": "50",
            "isPrimaryKey": false,
            "isForeignKey": false
          },
          {
            "columnName": "CompanyName",
            "dataType": "nvarchar",
            "size": "50",
            "isPrimaryKey": false,
            "isForeignKey": false
          },
          {
            "columnName": "IsActive",
            "dataType": "bit",
            "size": "",
            "isPrimaryKey": false,
            "isForeignKey": false
          }
        ],
        "foreignKeys": [],
        "primaryKeys": [
          "UserId"
        ]
      },
      "MachineRegister": {
        "tableName": "MachineRegister",
        "columns": [
          {
            "columnName": "Id",
            "dataType": "bigint",
            "size": ""
          },
          {
            "columnName": "MachineIp",
            "dataType": "nvarchar",
            "size": "50"
          },
          {
            "columnName": "MachineId",
            "dataType": "nvarchar",
            "size": "50"
          },
          {
            "columnName": "MachinePassword",
            "dataType": "nvarchar",
            "size": "50"
          },
          {
            "columnName": "MachinePort",
            "dataType": "nvarchar",
            "size": "50"
          }
        ],
        "foreignKeys": [],
        "primaryKeys": [
          "Id"
        ]
      },
      "Orders": {
        "tableName": "Orders",
        "columns": [
          {
            "columnName": "OrderID",
            "dataType": "bigint",
            "size": ""
          },
          {
            "columnName": "OrderNumber",
            "dataType": "varchar",
            "size": "20"
          },
          {
            "columnName": "CustomerID",
            "dataType": "bigint",
            "size": ""
          },
          {
            "columnName": "OrderDate",
            "dataType": "date",
            "size": ""
          }
        ],
        "foreignKeys": [
          {
            "constraintName": "FK__Orders__Customer__4BAC3F29",
            "columnName": "CustomerID",
            "referencedTable": "Orders",
            "referencedConstraint": ""
          }
        ],
        "primaryKeys": [
          "OrderID"
        ]
      },
      "Customers": {
        "tableName": "Customers",
        "columns": [
          {
            "columnName": "CustomerID",
            "dataType": "bigint",
            "size": ""
          },
          {
            "columnName": "FirstName",
            "dataType": "varchar",
            "size": "50"
          },
          {
            "columnName": "LastName",
            "dataType": "varchar",
            "size": "50"
          },
          {
            "columnName": "Email",
            "dataType": "varchar",
            "size": "100"
          }
        ],
        "foreignKeys": [],
        "primaryKeys": [
          "CustomerID"
        ]
      }
    }
  };

  tables: Table[] = [];
  
  totalCount: number;
  credentialsList: any[];
  constructor(private _dbService: dbService,
    private router: Router,
    ) {
    this._dbService
      .getAllCredential()
      .subscribe((res: any) => {
        if (res.isSuccess) {
          this.credentialsList = res.data.groupedCredentials;
          const message = res.message;
          this.totalCount = res.data.totalCount;
        }
        else if (res.isSuccess == false) {
          const message = res.data.message;
          this.totalCount = res.data.totalCount;

        }
      });
  }


  ngOnInit() {
  }



  fetchData(credentials: any) {
    debugger
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

  // createDiagram(): void {
  //   const $ = go.GraphObject.make;

  //   this.diagram = $(go.Diagram, 'myDiagramDiv', {
  //     'undoManager.isEnabled': true
  //   });

  //   const createNode = (tableData: any) => {
  //     return $(go.Node, 'Auto',
  //       $(go.Shape, 'Rectangle', { fill: 'lightblue' }),
  //       $(go.Panel, 'Table',
  //         $(go.RowColumnDefinition, { row: 0, background: 'lightgray' }),
  //         $(go.TextBlock, { row: 0, columnSpan: 2, margin: 10, font: 'bold 12px sans-serif', editable: true },
  //           new go.Binding('text', 'columnName')),
  //         $(go.Panel, 'Vertical', { row: 1, margin: 4 },
  //           $(go.TextBlock, 'Attributes:', { font: 'italic 10px sans-serif' }),
  //           $(go.TextBlock, new go.Binding('text', 'attributes'))
  //         )
  //       )
  //     );
  //   };

  //   const createLink = (foreignKey: any) => {
  //     return $(go.Link,
  //       $(go.Shape),
  //       $(go.Shape, { toArrow: 'Standard' }),
  //       $(go.TextBlock, new go.Binding('text', 'text'))
  //     );
  //   };

  //   for (const tableName in this.erdData.tables) {
  //     if (this.erdData.tables.hasOwnProperty(tableName)) {
  //       const tableData = this.erdData.tables[tableName];
  //       this.diagram.nodeTemplateMap.add(tableName, createNode(tableData));

  //       tableData.foreignKeys.forEach((foreignKey: any) => {
  //         this.diagram.linkTemplateMap.add(`${tableName}_${foreignKey.referencedTable}`, createLink(foreignKey));
  //       });
  //     }
  //   }


  //   const nodeDataArray = Object.keys(this.erdData.tables).map(tableName => {
  //     const tableData = this.erdData.tables[tableName];
  //     return {
  //       key: tableName,
  //       columnName: tableData.tableName,
  //       attributes: tableData.columns.map((column: any) =>
  //         `${column.columnName}: ${column.dataType}${column.size ? `(${column.size})` : ''}`).join('\n')
  //     };
  //   });

  //   const linkDataArray = [];
  //   for (const tableName in this.erdData.tables) {
  //     if (this.erdData.tables.hasOwnProperty(tableName)) {
  //       const tableData = this.erdData.tables[tableName];
  //       tableData.foreignKeys.forEach((foreignKey: any) => {
  //         linkDataArray.push({
  //           from: tableName,
  //           to: foreignKey.referencedTable,
  //           text: foreignKey.constraintName
  //         });
  //       });
  //     }
  //   }

  //   this.diagram.model = new go.GraphLinksModel(nodeDataArray, linkDataArray);
  // }





  // private createERDiagram(responseData: any) {
  //   const width = 800;
  //   const height = 600;

  //   const svg = d3
  //     .select(this.erDiagramContainer.nativeElement)
  //     .append('svg')
  //     .attr('width', width)
  //     .attr('height', height);

  //   // Draw entities
  //   const entityGroup = svg
  //     .selectAll('.entity')
  //     .data(responseData.entities)
  //     .enter()
  //     .append('g')
  //     .attr('class', 'entity')
  //     .attr('transform', (d, i) => `translate(${i * 150}, 150)`);

  //   entityGroup
  //     .append('rect')
  //     .attr('width', 100)
  //     .attr('height', 80) // Increased height to accommodate attributes
  //     .attr('fill', 'lightblue');

  //   entityGroup
  //     .append('text')
  //     .attr('x', 50)
  //     .attr('y', 25)
  //     .attr('dy', '0.35em')
  //     .attr('text-anchor', 'middle')
  //     .text((d) => d.name);

  //   // Display attributes for each entity
  //   entityGroup
  //     .selectAll('.attribute')
  //     .data((d) => d.attributes)
  //     .enter()
  //     .append('text')
  //     .attr('class', 'attribute')
  //     .attr('x', 50)
  //     .attr('y', (d, i) => 40 + i * 15) // Adjust the spacing based on your preference
  //     .attr('dy', '0.35em')
  //     .attr('text-anchor', 'middle')
  //     .text((d) => d);

  //   // Draw relationships
  //   svg
  //     .selectAll('.relationship')
  //     .data(responseData.relationships)
  //     .enter()
  //     .append('line')
  //     .attr('class', 'relationship')
  //     .attr('x1', (d) => getEntityPosition(responseData.entities, d.source))
  //     .attr('y1', 200)
  //     .attr('x2', (d) => getEntityPosition(responseData.entities, d.target))
  //     .attr('y2', 200)
  //     .attr('stroke', 'black');

  //   function getEntityPosition(entities: any[], entityName: string) {
  //     const entity = entities.find((e) => e.name === entityName);
  //     if (entity) {
  //       const index = entities.indexOf(entity);
  //       return index * 150 + 50;
  //     }
  //     return 0;
  //   }
  // }

}
