import { Injectable } from '@angular/core';
import {LookupModel} from "../model/lookup.model";

@Injectable({
  providedIn: 'root'
})
export class LookupService {

  constructor() { }

  /**
   * Converts a TypeScript enum to a data source that can be used in select boxes.
   * The returned data source will be an array of objects with 'key' (enum value) and 'value' (enum name).
   * @param enumType The enum to be converted.
   * @returns Array of objects with key-value pairs for the enum.
   */
  public enumToDataSource<T extends object>(enumType: T): LookupModel[] {
    return Object.entries(enumType)
      .filter(([key, value]) => isNaN(Number(key)))  // Filter out numeric keys
      .map(([key, value]) => ({
        key: Number(value),  // Convert the enum value to a number (key)
        value: this.formatEnumKey(key)  // Format the key (enum name) for display (value)
      }));
  }

  /**
   * Formats the enum key by replacing underscores with spaces and capitalizing words.
   * Useful for making enum names more human-readable in a select box.
   * @param key The original enum key.
   * @returns A formatted, human-readable string.
   */
  private formatEnumKey(key: string): string {
    return key
      .replace(/_/g, ' ')  // Replace underscores with spaces
      .replace(/\w\S*/g, (word) => word.charAt(0).toUpperCase() + word.substr(1).toLowerCase());  // Capitalize words
  }
}
