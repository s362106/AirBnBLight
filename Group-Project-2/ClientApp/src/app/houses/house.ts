import { User } from '../reservations/User';
import { IReservation } from '../reservations/reservation';
export interface IHouse {
  HouseId: number,
  Title: string,
  Description: string,
  HouseImageUrl: string,
  BedroomImageUrl: string,
  BathroomImageUrl: string,
  Location: string,
  PricePerNight: number,
  Bedrooms: number,
  Bathrooms: number,
  UserId: string,
  User?: User,
  Reservations?: IReservation[];
}
