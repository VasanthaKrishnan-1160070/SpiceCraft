import { Injectable } from '@angular/core';
import { LookupModel } from '../model/lookup.model';

@Injectable({
  providedIn: 'root'
})
export class LookupService {

  constructor() { }

  /**
   * Converts a TypeScript enum to a data source that can be used in select boxes.
   * The returned data source will be an array of objects with 'key' (enum value) and 'value' (enum name).
   * Optionally, a default option can be added with a key of 0, and 'None' can be replaced by this value.
   * @param enumType The enum to be converted.
   * @param defaultText Optional default text for the first select option.
   * @returns Array of objects with key-value pairs for the enum.
   */
  public enumToDataSource<T extends object>(enumType: T, defaultText?: string): LookupModel[] {
    const dataSource = Object.entries(enumType)
      .filter(([key, value]) => isNaN(Number(key)))  // Filter out numeric keys (since we only need string keys)
      .map(([key, value]) => ({
        key: Number(value),  // Convert the enum value to a number (key)
        value: key === 'None' && defaultText ? defaultText : this.formatEnumKey(key)  // Replace 'None' with defaultText if provided
      }));

    // If defaultText is provided and 'None' isn't found, prepend a default option with key 0
    if (defaultText && !dataSource.some(item => item.value === defaultText)) {
      dataSource.unshift({ key: 0, value: defaultText });
    }

    return dataSource;
  }

  /**
   * Formats the enum key by adding spaces before capital letters to separate words in PascalCase.
   * Example: 'EnumValue' becomes 'Enum Value'
   * @param key The original enum key in PascalCase.
   * @returns A formatted, human-readable string with spaces between words.
   */
  private formatEnumKey(key: string): string {
    return key
      .replace(/([a-z])([A-Z])/g, '$1 $2')  // Add space before each capital letter
      .replace(/([A-Z])([A-Z][a-z])/g, '$1 $2')  // Add space between groups of capital letters and following lowercase letters
      .replace(/\b\w/g, char => char.toUpperCase());  // Capitalize the first letter of each word
  }
}
