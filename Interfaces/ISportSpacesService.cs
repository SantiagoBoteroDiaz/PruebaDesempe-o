using GestionDeEspacios.Enums;
using GestionDeEspacios.Models;
using GestionDeEspacios.Response;

namespace GestionDeEspacios.Interfaces;

public interface ISportSpacesService
{
    Task<SystemResponse<IEnumerable<SportSpace>>> GetAllSportSpaces();
    Task<SystemResponse<SportSpace>> GetSportSpaceById(int id);
    Task<SystemResponse<IEnumerable<SportSpace>>> GetSportSpaceByType(SportSpacesType type);
    Task<SystemResponse<SportSpace>> Create(SportSpace sportSpace);
    Task<SystemResponse<SportSpace>> Update(SportSpace sportSpace);
    Task<SystemResponse<SportSpace>> Delete(int id);
}