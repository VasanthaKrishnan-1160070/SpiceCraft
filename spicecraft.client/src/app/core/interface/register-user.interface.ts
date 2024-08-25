export interface RegisterModel {
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  profileImage?: string;  // Optional because it's initialized with a default value
  dateOfBirth?: Date | undefined;
  phoneNumber: string;
  roleId: number;

  streetAddress1: string;
  streetAddress2: string;
  city: string;
  state: string;
  postalCode: string;
}
