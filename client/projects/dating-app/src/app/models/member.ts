import { Photo } from "./photo";

export interface Member {
  id: string;
  userName: string;
  knownAs: string;
  photoUrl: string;
  age: number;
  dob: Date;
  introduction: string;
  gender: string;
  country: string;
  city: string;
  interests: string;
  lookingFor: string;
  photos: Photo[];
  createdAt: Date;
  lastActive: Date;
}
