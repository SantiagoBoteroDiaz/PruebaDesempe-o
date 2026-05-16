using GestionDeEspacios.Models;
using GestionDeEspacios.Response;

namespace GestionDeEspacios.Interfaces;

public interface IReservationService
{
    Task<SystemResponse<IEnumerable<Reservation>>> GetAllReservations(); 
    Task<SystemResponse<Reservation>> Create(Reservation reservation);
    Task<SystemResponse<Reservation>>  Update(Reservation reservation);
    Task<SystemResponse<Reservation>> Delete(int id);
    Task<SystemResponse<IEnumerable<Reservation>>> GetReservationByUser(int id);
    Task<SystemResponse<Reservation>> GetReservationById(int id);
    Task<SystemResponse<IEnumerable<Reservation>>> GetReservationBySpace(int id);
}