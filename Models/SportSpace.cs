using GestionDeEspacios.Enums;

namespace GestionDeEspacios.Models;

public class SportSpace
{
    public int Id { get; set; }
    public string Name { get; set; }
    public SportSpacesType Type { get; set; }
    public int Capacity { get; set; }
    
    //Navegation 
    public IEnumerable<Reservation> Reservations { get; set; } = new List<Reservation>();
}