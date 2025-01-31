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

  loadDeviceSessions(): void {
    console.log('Load device sessions');
    this.apiService.getDeviceSessions().subscribe((deviceSessions)=>{
      this.deviceSessions = deviceSessions;
    })
  }
}
