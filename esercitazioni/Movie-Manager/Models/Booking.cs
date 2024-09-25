public class Booking
{
    public int BookingId { get; set; }  // Unique identifier for the booking
    public int UserId { get; set; }  // ID of the user who made the booking
    public int MovieId { get; set; }  // ID of the movie being booked
    public DateTime BookingDate { get; set; }  // Date when the booking was made
}