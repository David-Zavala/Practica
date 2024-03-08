import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title: string = 'Frontend';
  users: any;

  constructor(private http: HttpClient, private router: Router){ }
  ngOnInit(): void {
    const userName = localStorage.getItem('username');
    if (!userName) {
      this.router.navigate(['/login'])
    }
  }

}
