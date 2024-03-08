import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Doc } from '../_models/doc';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class DocsService {
  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) { }

  registerDoc(model: Doc) {
    return this.http.post(this.baseUrl + 'docs/docregister', model).subscribe(
      (response) => {
        this.setRegisterDocSuccesful();
      },
      (error) => {
        this.setRegisterDocError();
      }
    )
  }

  setRegisterDocSuccesful() {
    localStorage.setItem('RegisteredDoc', "1");
  }

  setRegisterDocError() {
    localStorage.setItem('RegisteredDoc', "0");
  }
}
