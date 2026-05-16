using GestionDeEspacios.Data;
using GestionDeEspacios.Enums;
using GestionDeEspacios.Interfaces;
using GestionDeEspacios.Models;
using GestionDeEspacios.Response;
using Microsoft.EntityFrameworkCore;

namespace GestionDeEspacios.Services;

public class SportSpacesService : ISportSpacesService
{
    readonly MysqlDbContext _dbContext;
    
    public SportSpacesService(MysqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<SystemResponse<IEnumerable<SportSpace>>> GetAllSportSpaces()
    {
        try
        {
            var sportSpaces = _dbContext.SportSpaces.ToList();
            if (sportSpaces.Count == 0)
            {
                return new SystemResponse<IEnumerable<SportSpace>>
                {
                    Success = false,
                    Message = "No hay espacios"
                }; 
            }

            return new SystemResponse<IEnumerable<SportSpace>>
            {
                Data = sportSpaces,
                Success = true
            }; 

        }
        catch (Exception e)
        {
            return new SystemResponse<IEnumerable<SportSpace>>
            {
                Success = false,
                Message = $"Erro desconocido: {e.Message}"
            }; 
        }
    }

    public async Task<SystemResponse<SportSpace>> GetSportSpaceById(int id)
    {
        try
        {
            var space = await _dbContext.SportSpaces.FindAsync(id);
            if (space == null)
            {
                return new SystemResponse<SportSpace>
                {
                    Success = false,
                    Message = "No se pudo encontrar el espacio"
                };
            }

            return new SystemResponse<SportSpace>()
            {
                Data = space,
                Success = true
            };
        }
        catch (Exception e)
        {
            return new SystemResponse<SportSpace>
            {
                Success = false,
                Message = $"Error inesperado : {e.Message}"
            };
        }
    }

    public async Task<SystemResponse<IEnumerable<SportSpace>>>  GetSportSpaceByType(SportSpacesType type)
    {
        try
        {
            var spaces = await _dbContext.SportSpaces.Where(x => x.Type == type).ToListAsync<SportSpace>();
            if (spaces.Count == 0)
                return new SystemResponse<IEnumerable<SportSpace>>
                {
                    Success = false,
                    Message = "No se encontraron espacios de ese tipo"
                };

            return new SystemResponse<IEnumerable<SportSpace>>
            {
                Data = spaces,
                Success = true
            };
        }
        catch (Exception e)
        {
            return new SystemResponse<IEnumerable<SportSpace>>()
            {
                Success = false,
                Message = $"Erro desconocido: {e.Message}"
            };
        }
    }

    public async Task<SystemResponse<SportSpace>> Create(SportSpace sportSpace)
    {
        try
        {
            var spaceRepeat = await _dbContext.SportSpaces.FirstOrDefaultAsync(x => x.Name == sportSpace.Name);
            if (spaceRepeat != null)
            {
                return new SystemResponse<SportSpace>
                {
                    Success = false,
                    Message = "Espacio ya creado"
                };
            }

            _dbContext.SportSpaces.Add(sportSpace);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return new SystemResponse<SportSpace>
                {
                    Success = true,
                    Message = "Espacio creado correctamente"
                };
            }

            return new SystemResponse<SportSpace>
            {
                Success = false,
                Message = "No se pudo guardar el espacio"
            }; 
        }
        catch (Exception e)
        {
            return new SystemResponse<SportSpace>()
            {
                Success = false,
                Message = $"Erro desconocido: {e.Message}"
            };
        }
        
        
        throw new NotImplementedException();
    }

    public async Task<SystemResponse<SportSpace>> Update(SportSpace sportSpace)
    {
        try
        {
            var spaceUpdate = await _dbContext.SportSpaces.FindAsync(sportSpace.Id);
            if (spaceUpdate == null)
            {
                return new SystemResponse<SportSpace>
                {
                    Success = false,
                    Message = "No se pudo encontrar el espacio"
                };
            }

            var spaceRepeat = await _dbContext.SportSpaces.FirstOrDefaultAsync(x =>
                    x.Id != sportSpace.Id && (x.Name == sportSpace.Name));
            if (spaceRepeat != null)
            {
                return new SystemResponse<SportSpace>
                {
                    Success = false,
                    Message = "Espacio ya creado anteriormente"
                };
            }

            spaceUpdate.Name = sportSpace.Name;
            spaceUpdate.Type = sportSpace.Type;
            spaceUpdate.Capacity = sportSpace.Capacity;

            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return new SystemResponse<SportSpace>
                {
                    Success = true,
                    Message = "Espacio actualizado correctamente"
                };
            }

            return new SystemResponse<SportSpace>()
            {
                Success = false,
                Message = "No se pudo actualizar el espacio"
            };
        }
        catch (Exception e)
        {
            return new SystemResponse<SportSpace>()
            {
                Success = false,
                Message = $"Error desconocido: {e.Message}"
            };
        }
    }

    public async Task<SystemResponse<SportSpace>> Delete(int id)
    {
        try
        {
            var userDelete = await _dbContext.SportSpaces.FindAsync(id);
            if (userDelete == null)
            {
                return new SystemResponse<SportSpace>
                {
                    Success = false,
                    Message = "No se pudo encontrar el espacio"
                };
            }
            _dbContext.SportSpaces.Remove(userDelete);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return new SystemResponse<SportSpace>
                {
                    Success = true,
                    Message = "Espacio eliminado correctamente"
                };
            }

            return new SystemResponse<SportSpace>()
            {
                Success = false,
                Message = "No se pudo eliminar el espacio"
            };
        }
        catch (Exception e)
        {
            return new SystemResponse<SportSpace>()
            {
                Success = false,
                Message = $"Error desconocido: {e.Message}"
            };
        }
    }
}