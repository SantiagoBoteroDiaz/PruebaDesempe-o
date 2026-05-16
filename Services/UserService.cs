using GestionDeEspacios.Data;
using GestionDeEspacios.Interfaces;
using GestionDeEspacios.Models;
using GestionDeEspacios.Response;
using Microsoft.EntityFrameworkCore;

namespace GestionDeEspacios.Services;

public class UserService : IUserService
{
    readonly MysqlDbContext _dbContext;
    
    public UserService(MysqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<SystemResponse<IEnumerable<User>>> GetAllUsers()
    {
        try
        {
           var users = _dbContext.Users.ToList();
           if (users.Count == 0)
           {
               return new SystemResponse<IEnumerable<User>>
               {
                   Success = false,
                   Message = "Users list empty"
               }; 
           }

           return new SystemResponse<IEnumerable<User>>
           {
               Data = users,
               Success = true,
               Message = "Users list"
           }; 

        }
        catch (Exception e)
        {
            return new SystemResponse<IEnumerable<User>>
            {
                Success = false,
                Message = e.Message
            }; 
        }
    }

    public async Task<SystemResponse<User>> Create(User user)
    {
        try
        {
            // validacion
            if (!long.TryParse(user.Phone, out _) || !long.TryParse(user.Document, out _))
                return new SystemResponse<User>
                {
                    Success = false,
                    Message = "El documento y el teléfono solo permiten números"
                };

            // 2. Validar duplicados
            var userRepeat = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Email == user.Email || x.Phone == user.Phone || x.Document == user.Document);

            if (userRepeat != null)
                return new SystemResponse<User>
                {
                    Success = false,
                    Message = "Documento, teléfono o email ya registrados"
                };

            // 3. Guardar
            _dbContext.Users.Add(user);
            var result = await _dbContext.SaveChangesAsync();

            if (result > 0)
                return new SystemResponse<User>
                {
                    Success = true,
                    Message = "Usuario creado exitosamente"
                };

            return new SystemResponse<User>
            {
                Success = false,
                Message = "No se pudo crear el usuario"
            };
        }
        catch (Exception e)
        {
            return new SystemResponse<User>
            {
                Success = false,
                Message = $"Error inesperado: {e.Message}"
            };
        }
    }

    public async Task<SystemResponse<User>> Update(User user)
    {
        try
        {
            var userUpdate = await _dbContext.Users.FindAsync(user.Id);
            if (userUpdate == null)
            {
                return new SystemResponse<User>
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                };
            }
            if (!long.TryParse(user.Phone, out _) || !long.TryParse(user.Document, out _))
            {
                return new SystemResponse<User>
                {
                    Success = false,
                    Message = "El documento o el telefono solo permiten numeros"
                };
            }
            var userRepeat = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id != user.Id && (x.Phone == user.Phone || x.Email == user.Email || x.Document == user.Document));
            if (userRepeat != null)
            {
                return new SystemResponse<User>
                {
                    Success = false,
                    Message = "Numero o email ya usados"
                }; 
            }
            userUpdate.Name = user.Name;
            userUpdate.Email = user.Email;
            userUpdate.Phone = user.Phone;
            userUpdate.Document = user.Document;
            
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return new SystemResponse<User>
                {
                    Success = true,
                    Message = "Usuario actualizado"
                }; 
            }

            return new SystemResponse<User>
            {
                Success = false,
                Message = "Falla al actualizar usuario"
            }; 
        }
        catch (Exception e)
        {
            return new SystemResponse<User>()
            {
                Success = false,
                Message = $"Error inesperado: {e.Message}"
            };
        }
    }

    public async Task<SystemResponse<User>> Delete(int id)
    {
        try
        {
            var userDelete = await _dbContext.Users.FindAsync(id);
            if (userDelete == null)
            {
                return new SystemResponse<User>()
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                };
            }

            _dbContext.Users.Remove(userDelete);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return new SystemResponse<User>
                {
                    Success = true,
                    Message = "Usuario eliminado"
                }; 
            }

            return new SystemResponse<User>
            {
                Success = false,
                Message = "Usuario no elinado"
            }; 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<SystemResponse<User>> GetUserById(int id)
    {
        try
        {
            var userFind = await _dbContext.Users.FindAsync(id);
            if (userFind == null)
            {
                return new SystemResponse<User>()
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                }; 
            }

            return new SystemResponse<User>
            {
                Data = userFind,
                Success = true,
            };
        }
        catch (Exception e)
        {
            return new SystemResponse<User>()
            {
                Success = false,
                Message = $"Error inesperado: {e.Message}"
            };
        }
    }
}