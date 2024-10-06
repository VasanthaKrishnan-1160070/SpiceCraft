export interface UserAddressModel {
  userId?: number;
  stateOrProvince: string;
  city: string;
  postalCode: string;
  streetAddress1: string;
  streetAddress2: string;
  addressType?: string | null;
}
