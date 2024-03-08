import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../_services/user.service';
import { UserLogin } from '../_models/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  loginForm!: FormGroup;
  emailErrorMessage: string = "";
  passwordErrorMessage: string = "";
  loginErrorMessage: string = "";

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      email: new FormControl('',Validators.email),
      password: new FormControl('', Validators.required),
    })
  }

  login() {
    if (this.loginForm.valid) {
      const email = this.loginForm.get('email')!.value;
      const password = this.loginForm.get('password')!.value;

      var user: UserLogin;

      if (email && password) {
        user = {
          Email: email,
          Password: password
        };
        this.userService.login(user);
      }
    }
    else
      this.loginErrorMessage = "Disculpe las molestias!\nAlgo parece aber salido mal, vuelve a intentarlo, de lo contrario contacta a alguien de soporte";
  }

  emailValidator() {
    const email = this.loginForm.get('email');
    this.emailErrorMessage = "";
    
    if (email && email.touched)
    this.emailErrorMessage = "El correo es requerido";
    if (email && email.invalid){
      var errors = email.errors;
      if (errors && errors['email'])
        this.emailErrorMessage = "El correo ingresado no es válido";
    }
  }

  passwordValidator() {
    const password = this.loginForm.get('password');
    
    this.passwordErrorMessage = "";
    
    if (password && password.errors && password.errors['required']){
      this.passwordErrorMessage = "Este campo es requerido";
    }
  }
}
