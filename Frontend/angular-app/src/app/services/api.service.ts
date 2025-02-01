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

  getDeviceSessionsBackup(): Observable<DeviceSession[]> {
    return this.http.get<DeviceSession[]>(`${this.apiUrl}/backup`);
  }

  downloadDeviceSessionsBackup(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/backupFile`, {responseType: 'blob'});
  }

  deleteDeviceSessionsBackup(): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/backup`, {});
  }

  saveDeviceSessionsBackup(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/backup`, {});
  }
}
