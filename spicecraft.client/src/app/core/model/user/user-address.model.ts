export interface UserAddressModel {
  stateOrProvince: string;
  city: string;
  postalCode: string;
  streetAddress1: string;
  streetAddress2: string;
  addressType?: string | null;
}
