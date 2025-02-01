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

  downloadBackup(): void {
    const dataStr = JSON.stringify(this.deviceSessions, null, 2);
    const blob = new Blob([dataStr], {type: 'application/json'});
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = 'device_sessions_backup.json';
    link.click();
  }
}
