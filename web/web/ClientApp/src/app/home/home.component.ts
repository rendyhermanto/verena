import { Component } from '@angular/core';
import { DataService } from './data.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent{

  title = 'SkillTest';
  data: any;
  status: string = "";
  employee: any;
  department: string = "";
  jabatan: string = "";
  fasilitasId: number = 0;
  employeeId: number;

  constructor(private dataServices: DataService) { }
  ngOnInit() {
    this.inquriy();
  }

  checkAPI() {
    this.dataServices.checkAPI().subscribe((data: any) => {
      this.status = data;
      console.log(this.status);
    })
  }

  inquriy() {
    const data = {
      "Data": {
        employee: 0
      }

    };
    this.dataServices.inquiryEmployee(data).subscribe(response => {
      if (response.isSuccess == true) {
        this.employee = response.data;
      }
      else {
        console.log(response.errorDescription);
      }
    })
  }

  add() {
    console.log("masuk add?")
    const data = {
      "Data": {
        Department: this.department,
        Jabatan: this.jabatan,
        FasilitasId: this.fasilitasId
      }

    };
    this.dataServices.InsertEmployee(data).subscribe(response => {
      if (response.isSuccess == true) {
        this.employee = response;
        console.log(response);
        this.inquriy();
      }
      else {
        console.log(response.errorDescription);
      }
    })
  }

  edit() {
    console.log("masuk add?")
    const data = {
      "Data": {
        Employee: this.employeeId,
        Department: this.department,
        Jabatan: this.jabatan,
        FasilitasId: this.fasilitasId
      }

    };
    this.dataServices.UpdateEmployee(data).subscribe(response => {
      if (response.isSuccess == true) {
        this.employee = response;
        console.log(response);
        this.inquriy();
      }
      else {
        console.log(response.errorDescription);
      }
    })
  }

  delete() {
    const data = {
      "Data": {
        Employee: this.employeeId,
      }

    };
    this.dataServices.DeleteEmployee(data).subscribe(response => {
      if (response.isSuccess == true) {
        this.employee = response;
        console.log(response);
        this.inquriy();
      }
      else {
        console.log(response.errorDescription);
      }
    })
  }

}
