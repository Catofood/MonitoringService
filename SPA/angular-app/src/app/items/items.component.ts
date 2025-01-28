import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.less']
})

export class ItemsComponent implements OnInit {
  items: any[] = [];

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
      this.loadItems();
  }

  loadItems(): void{
    this.apiService.getItems().subscribe(data => {
      this.items = data;
    });
  }

  refreshData(): void {
    this.apiService.refreshData().subscribe(data =>{
      this.items = data;
    })
  }
}
