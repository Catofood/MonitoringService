import { Component } from '@angular/core';
import {DeviceSession} from './models/device-session';
import {ApiService} from './services/api.service';
import {Observable} from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = 'Сервис мониторинга стороннего приложения';
  deviceSessions: DeviceSession[] = [];
  constructor(private apiService: ApiService) { }

  ngOnInit() : void {
    this.loadDeviceSessions();
  }
  loadDeviceSessions(): void {
    console.log('Load device sessions');
    this.apiService.getDeviceSessions().subscribe((deviceSessions)=>{
      this.deviceSessions = deviceSessions;
    });
  }

  removeDeviceSession(sessionId: number): void {
    this.apiService.deleteDeviceSession(sessionId).subscribe((deviceSessions)=>{
      this.deviceSessions = deviceSessions;
    });
  }

  downloadDeviceSessionsBackup(): void {
    this.apiService.downloadDeviceSessionsBackup().subscribe((response: Blob) => {
      const link = document.createElement('a');
      link.href = URL.createObjectURL(response);
      link.download = 'backup_device_sessions.json';
      link.click();
    });
  }

  saveDeviceSessionsBackup(): void {
    this.apiService.saveDeviceSessionsBackup().subscribe((response) => {});
  }

  deleteDeviceSessionsBackup(): void {
    this.apiService.deleteDeviceSessionsBackup().subscribe((response) => {});
  }

  loadDeviceSessionsBackup(): void {
    console.log('Load device sessions from backup');
    this.apiService.getDeviceSessionsBackup().subscribe((deviceSessions)=>{
      this.deviceSessions = deviceSessions;
    });
  }
}
