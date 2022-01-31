import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {
  public userDetails: any;
  constructor(private authService: AuthService) {
  }

  ngOnInit() {
    this.userDetails = this.authService.user;
    console.log(this.userDetails);
  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
  logout(): void {
    this.authService.logout();
  }
}
