import { Component, OnInit, OnChanges } from '@angular/core';
import { ColDef, GridApi, ICellRendererParams, GridReadyEvent } from 'ag-grid-community';
import { StoriesService } from 'src/shared/services/stories.service';
import { FormControl } from '@angular/forms';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  private gridApi!: GridApi;
  public gridData: any;
  stories: any;
  constructor(
    private storiesService: StoriesService
  ) {
  }

  filterControl = new FormControl('');

  ngOnInit(): void {    
    this.stories = this.storiesService.getStories('priority', 200).subscribe({
      next: this.handleGetStoriesSuccess.bind(this),
      error: this.handleGetStoriesError.bind(this)
    });

    this.filterControl.valueChanges.subscribe(value => 
      {
        this.gridApi.setGridOption("quickFilterText", value?.toString());
      }
    );
  }

  handleGetStoriesSuccess(data: any){
    this.gridData = data;
  }

  handleGetStoriesError(data: any){
    console.log('error');
  }

  // Column Definitions: Defines & controls grid columns.
  colDefs: ColDef[] = [
    { field: 'title',
      cellRenderer: function(params: ICellRendererParams ) {
        if (params.data.url)
              return `<a href="${params.data.url}" target="_blank">`+ params.data.title+'</a>';
        else
          return params.data.title;
    }
  }
  ];
  
  defaultColDef: ColDef = {
    flex: 1,
  }

  onGridReady(params: GridReadyEvent) {
    this.gridApi = params.api;
  }
}
