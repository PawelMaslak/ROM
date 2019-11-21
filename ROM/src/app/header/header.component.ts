import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styles: []
})
export class HeaderComponent implements OnInit {

  public title: string = 'Restaurant Managment Application';
  
  constructor() { }

  ngOnInit() {
  }

}
