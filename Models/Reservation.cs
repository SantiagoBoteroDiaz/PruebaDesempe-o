using GestionDeEspacios.Enums;

namespace GestionDeEspacios.Models;

public class Reservation
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; } 
    public ReservationStatus Status { get; set; }
    
    //relations 
    public int UserID { get; set; }
    public User User { get; set; } 
    
    public int SportId { get; set; }
    public SportSpace SportSpace { get; set; }
}