using GestionDeEspacios.Models;
using GestionDeEspacios.Response;

namespace GestionDeEspacios.Interfaces;

public interface IUserService
{
    Task<SystemResponse<IEnumerable<User>>> GetAllUsers(); 
    Task<SystemResponse<User>> Create(User user);
    Task<SystemResponse<User>> Update(User user);
    Task<SystemResponse<User>> Delete(int id);
    Task<SystemResponse<User>>GetUserById(int id);
}