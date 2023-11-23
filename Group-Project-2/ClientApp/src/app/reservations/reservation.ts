import { IHouse } from '../houses/house';
export interface IReservation {
  ReservationId: number;
  CheckInDate: Date;
  CheckOutDate: Date;
  TotalPrice: number;
  BookingDuration: number;
  DateCreated: Date;
  HouseId: number;
  House?: IHouse;
  /*
  userId: string;
  user?: User;
  */
}
