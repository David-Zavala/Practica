import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DocsService } from '../_services/docs.service';
import { Doc } from '../_models/doc';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerDocForm!: FormGroup;
  emailErrorMessage: string = "";
  inputErrorMessage: string = "";
  DocumentoErrorMesssage: string = "";
  registerDocErrorMessage: string = "";
  educationErrorMessage = ["",""];

  constructor(private docsService: DocsService) { }

  ngOnInit(): void {
    this.registerDocForm = new FormGroup({
      Nombre: new FormControl('',Validators.required),
      Correo: new FormControl('', [Validators.email,Validators.required]),
      FechaNacimientoY: new FormControl('',[Validators.required,Validators.min(1000),Validators.max(2024),Validators.maxLength(4)]),
      FechaNacimientoM: new FormControl('',[Validators.required,Validators.min(1),Validators.max(12),Validators.maxLength(2)]),
      FechaNacimientoD: new FormControl('',[Validators.required,Validators.min(1),Validators.max(31),Validators.maxLength(2)]),
      Edad: new FormControl('',[Validators.required,Validators.min(1),Validators.max(150),Validators.maxLength(3)]),
      EducacionL: new FormControl('',[Validators.required,Validators.min(0),Validators.max(3)]),
      EducacionP: new FormControl('',Validators.pattern('[0-2]')),
      Documento: new FormControl('',Validators.required)
    });
  }

  registerDoc() {
    if (this.registerDocForm.valid) {
      const nombre = this.registerDocForm.get('Nombre')!.value;
      const correo = this.registerDocForm.get('Correo')!.value;
      const fechaNacimiento = this.registerDocForm.get('FechaNacimiento')!.value;
      const edad = this.registerDocForm.get('Edad')!.value;
      const educacion = this.registerDocForm.get('Educacion')!.value;
      const documento = this.registerDocForm.get('Documento')!.value;

      var doc: Doc;

      if (nombre && correo && fechaNacimiento && edad && educacion && documento) {
        doc = {
          Nombre: nombre,
          Correo: correo,
          FechaNacimiento: fechaNacimiento,
          Edad: edad,
          Educacion: educacion,
          Documento: documento
        };
        this.docsService.registerDoc(doc);
      }
    }
    else
      this.registerDocErrorMessage = "Disculpe las molestias!\nAlgo parece aber salido mal, vuelve a intentarlo, de lo contrario contacta a alguien de soporte";
  }

  emailValidator() {
    const email = this.registerDocForm.get('Correo');
    this.emailErrorMessage = "";
    
    // if (email && email.touched && email.value == "")
    // this.emailErrorMessage = "El correo es requerido";
    if (email && email.invalid){
      var errors = email.errors;
      if (errors && errors['email'])
        this.emailErrorMessage = "El correo ingresado no es válido";
    }
  }

  requiredValidator(event: any) {
    const input = this.registerDocForm.get(event.target.name);
    this.inputErrorMessage = "";
    if (input && input.touched && input.value == "") this.inputErrorMessage = "Este campo es requerido";
  }

  focusNext(event: any, nextInput: HTMLInputElement) {
    const inputLength = event.target.value.length;
    const maxLength = event.target.maxLength;
    
    if (inputLength >= maxLength && nextInput) {
      nextInput.focus();
    }
  }

  limitInputLength(event: any) {
    const input = this.registerDocForm.get(event.target.name);

    if (input && input.errors && input.errors['maxLength'] && input.value > input.errors['maxLength'].maxLength) input.setValue(input.errors['maxLength'].maxLength);
  }

  checkLimit(event: any) {
    const input = this.registerDocForm.get(event.target.name);
    
    if (input && input.errors && input.errors['min'] && input.value <= input.errors['min'].min) input.setValue(input.errors['min'].min);
    if (input && input.errors && input.errors['max'] && input.value >= input.errors['max'].max) input.setValue(input.errors['max'].max);
  }

  selectValidator(event: any, i: number) {
    const input = this.registerDocForm.get(event.target.name);
    this.educationErrorMessage[i] = "";

    if (input && input.errors) this.educationErrorMessage[i] = "Debe seleccionar una";
  }

  deleteDoc() {
    const input = this.registerDocForm.get('Documento');
    console.log(input);
    
  }
}
