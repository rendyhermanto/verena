import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  private REST_API_SERVER_CHECK = "https://localhost:5001/api/v1/employee";
  //private REST_API_SERVER = "https://localhost:5001/api/v1/employee/";

  constructor(private httpClient: HttpClient) { }

  public checkAPI() {
    return this.httpClient.get(this.REST_API_SERVER_CHECK);
  }
  public inquiryEmployee(data: any): Observable<any> {
    return this.httpClient.post(this.REST_API_SERVER_CHECK + "/inquiryEmployee", data);
  }
  public InsertEmployee(data: any): Observable<any> {
    return this.httpClient.post(this.REST_API_SERVER_CHECK + "/InsertEmployee", data);
  }
  public UpdateEmployee(data: any): Observable<any> {
    return this.httpClient.post(this.REST_API_SERVER_CHECK + "/UpdateEmployee", data);
  }
  public DeleteEmployee(data: any): Observable<any> {
    return this.httpClient.post(this.REST_API_SERVER_CHECK + "/DeleteEmployee", data);
  }
}
