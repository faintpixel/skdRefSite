import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { LogsService } from '../logs.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  profile: any;
  logCount = 0;


  constructor(public authService: AuthService, private logsService: LogsService) { }

  ngOnInit() {
    this.updateLogCount();
    setInterval(() => { this.updateLogCount(); }, 300000); // 5 minutes
  }

  updateLogCount() {
    if (this.loggedIn() && this.authService.isAdmin()) {
      this.logsService.getLogCount().subscribe(count => this.logCount = count);
    }
  }

  login() {
    this.authService.login();
  }

  logout() {
    this.authService.logout();
  }

  loggedIn() {
    const isAuthenticated = this.authService.isAuthenticated();


    return isAuthenticated;
  }

  updateProfile(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      this.profile = null;
    } else {
      if (this.authService.userProfile) {
        this.profile = this.authService.userProfile;
      } else {
        this.authService.getProfile((err, profile) => { this.profile = profile; });
      }
    }
  }

}
