export interface RegisterUser {
  userName: string;
  password: string;
  knownAs:string;
  dob:Date;
  city: string;
  country: string;
  termsAccepted: boolean;
}
