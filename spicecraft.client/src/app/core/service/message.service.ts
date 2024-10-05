import { Injectable } from '@angular/core';
import { confirm, alert } from 'devextreme/ui/dialog';

@Injectable({
  providedIn: 'root',
})
export class MessageService {

  // Confirm dialog
  showConfirmDialog(message: string, title: string = 'Confirm'): Promise<boolean> {
    return confirm(message, title).then((dialogResult: boolean) => {
      return dialogResult; // Returns true if confirmed, false otherwise
    });
  }

  // Information dialog
  showInfoMessage(message: string, title: string = 'Info'): void {
    alert(message, title);  // Simple alert dialog for informational messages
  }

  // Warning dialog
  showWarningMessage(message: string, title: string = 'Warning'): void {
    alert(message, title);  // Simple alert dialog for warning messages
  }

  // error dialog
  showErrorDialog(message: string, title: string = 'Error'): void {
    alert(message, title);  // Simple alert dialog for error messages
  }
}
