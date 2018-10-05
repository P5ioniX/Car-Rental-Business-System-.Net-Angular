import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  img1: string;
  img2: string;
  img3: string;

  constructor() 
  {
    this.img1 = '/assets/Images/Mini_Cooper.jpg';
    this.img2 = '/assets/Images/Subaru_Impreza_Grey.jpg';
    this.img3 = '/assets/Images/BMW_Blue.jpg';
   }

  ngOnInit() {
  }

}
