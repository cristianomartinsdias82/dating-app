import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'dta-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss']
})
export class ErrorComponent {

  error: any;

  constructor(private router: Router) {
    const navData = router.getCurrentNavigation();
    this.error = navData?.extras?.state['error'];
  }
}
