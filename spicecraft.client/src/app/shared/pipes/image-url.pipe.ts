import { Pipe, PipeTransform } from '@angular/core';
import {environment} from "../../../environments/environment";

@Pipe({
  standalone: true,
  name: 'imageUrl'
})
export class ImageUrlPipe implements PipeTransform {
  transform(fileName: string, type: 'item' | 'profile' = 'item'): string {
    if (!fileName) {
      return ''; // Return empty string if no file name
    }

    // Construct the image URL based on the type
    const baseUrl = environment.backend_url;  // Adjust base URL as needed
    if (type === 'item') {
      return `${baseUrl}/uploads/items/${fileName}`;
    } else if (type === 'profile') {
      return `${baseUrl}/uploads/profiles/${fileName}`;
    }
    return '';
  }
}

