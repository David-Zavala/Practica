import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, map } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { User, UserLogin } from '../_models/user';
import { Router } from '@angular/router';
import { Doc } from '../_models/doc';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  login(model: User) {
    return this.http.post<User>(this.baseUrl + 'users/login', model).subscribe(
      (user: any) => {
        model = {
          Name: user.name,
          Email: user.email,
          BirthDate: user.birthDate,
        };
        this.setCurrentUser(model);
        this.router.navigate(['/'])
      },
      (error) => {
        console.error('Error en la solicitud HTTP:', error);
      }
    )
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    )
  }

  setCurrentUser(user: User) {
    localStorage.setItem('email', JSON.stringify(user.Email));
    localStorage.setItem('username', JSON.stringify(user.Name));
    localStorage.setItem('authorized', "True");
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('username');
    localStorage.removeItem('authorized');
    this.currentUserSource.next(null)
  }
}
