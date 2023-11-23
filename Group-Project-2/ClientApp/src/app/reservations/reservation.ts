export interface IReservation {
  ReservationId: number;
  CheckInDate: Date;
  CheckOutDate: Date;
  TotalPrice: number;
  BookingDuration: number;
  DateCreated: Date;
  HouseId: number;
  /*
  userId: string;
  user?: User;
  */
}
