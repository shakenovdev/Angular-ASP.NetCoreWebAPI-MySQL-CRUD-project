import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'date',
  templateUrl: './date.component.html'
})
export class DateComponent implements OnInit {

  @Input() date;
  isFull = false;

  constructor() { }

  ngOnInit() {
  }

  showFull() {
    this.isFull = true;
  }

  showShort() {
    this.isFull = false;
  }

}
