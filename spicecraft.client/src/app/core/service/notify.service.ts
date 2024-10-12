import { Injectable } from '@angular/core';
import notify from 'devextreme/ui/notify';

@Injectable({
  providedIn: 'root',
})
export class NotifyService {

  // Generic notification method
  private showNotification(message: string, type: string) {
    notify(
      {
        message: message,
        position: {
          my: 'top center',  // Display at the top center
          at: 'top center',
          of: '#content-container',
        },
      },
      type,
      3000 // Duration of 3 seconds
    );
  }

  // Success notification
  showSuccess(message: string) {
    this.showNotification(message, 'success');
  }

  // Error notification
  showError(message: string) {
    this.showNotification(message, 'error');
  }

  // Warning notification
  showWarning(message: string) {
    this.showNotification(message, 'warning');
  }

  // Information notification
  showInfo(message: string) {
    this.showNotification(message, 'info');
  }
}
