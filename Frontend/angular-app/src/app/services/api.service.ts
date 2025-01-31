import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {DeviceSession} from '../models/device-session';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = "http://localhost:5262/api/DeviceSessions";
  constructor(private http: HttpClient) { }

  getDeviceSessions(): Observable<DeviceSession[]> {
    return this.http.get<DeviceSession[]>(this.apiUrl);
  }

  deleteDeviceSession(sessionId: number): Observable<DeviceSession[]> {
    return this.http.delete<DeviceSession[]>(`${this.apiUrl}/sessionId/${sessionId}`);
  }
}
