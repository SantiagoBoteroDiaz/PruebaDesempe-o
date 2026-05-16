using GestionDeEspacios.Models;

namespace GestionDeEspacios.ModelView;

public class ReservationViewModel
{
    public IEnumerable<User>? Users { get; set; }
    public IEnumerable<SportSpace>? SportSpaces { get; set; }
    
    public Reservation Reservation { get; set; } 
}