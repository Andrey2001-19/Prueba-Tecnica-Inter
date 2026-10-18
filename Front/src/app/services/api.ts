import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
 
  private apiUrl = 'http://localhost:5263/api';

  constructor(private http: HttpClient) {}

  
  getMaterias(): Observable<any> {
    return this.http.get(`${this.apiUrl}/materias`);
  }

  
  getEstudiantes(): Observable<any> {
    return this.http.get(`${this.apiUrl}/estudiantes`);
  }

 
  registrarEstudiante(estudiante: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/estudiantes`, estudiante);
  }

  actualizarEstudiante(id: number, estudiante: any): Observable<any> {
  return this.http.put(`${this.apiUrl}/estudiantes/${id}`, estudiante);
}


  eliminarEstudiante(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/estudiantes/${id}`);
  }

  
}