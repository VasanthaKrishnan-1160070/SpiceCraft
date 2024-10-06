import {UserAddressModel} from "./user-address.model";

export interface UserModel {
  userId?: number;
  userName: string  // Default to empty string
  firstName: string;
  lastName: string ;
  email: string;
  roleId: number;
  isActive?: boolean; // Optional boolean, defaults to false
  phone: string;
  profileImg: string;
  title: string;
  dateOfBirth?: Date | undefined;
  userAddress: UserAddressModel;
}
