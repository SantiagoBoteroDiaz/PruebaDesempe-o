using System.Runtime.InteropServices.JavaScript;
using GestionDeEspacios.Data;
using GestionDeEspacios.Enums;
using GestionDeEspacios.Interfaces;
using GestionDeEspacios.Models;
using GestionDeEspacios.Response;
using Microsoft.EntityFrameworkCore;

namespace GestionDeEspacios.Services;

public class ReservationService : IReservationService
{
    readonly MysqlDbContext _dbContext;
    readonly EmailService _emailService;
    
    public ReservationService(MysqlDbContext dbContext,  EmailService emailService)
    {
        _dbContext = dbContext;
        _emailService = emailService;
    }


    public async Task<SystemResponse<IEnumerable<Reservation>>> GetAllReservations()
    {
        try
        {
            var reservations = await _dbContext.Reservations
                .Include(x => x.User)
                .Include(x => x.SportSpace)
                .ToListAsync();
            if (reservations.Count == 0)
            {
                return new SystemResponse<IEnumerable<Reservation>>()
                {
                    Data = reservations,
                    Success = false,
                    Message = "No se encontraron."
                };
            }

            return new SystemResponse<IEnumerable<Reservation>>()
            {
                Data = reservations,
                Success = true
            };
        }
        catch (Exception e)
        {
            return new SystemResponse<IEnumerable<Reservation>>()
            {
                Success = false,
                Message = $"Error inesperado: {e.Message}"
            }; 
        }
        throw new NotImplementedException();
    }

    public async Task<SystemResponse<Reservation>> Create(Reservation reservation)
    {
        try
        {
            //validations 
            
            var userValidation = await _dbContext.Reservations.AnyAsync(x =>
                x.UserID == reservation.UserID &&
                reservation.StartTime < x.EndTime &&
                reservation.EndTime > x.StartTime &&
                reservation.Date == x.Date 
            );
            var spaceRepeat = await _dbContext.Reservations.AnyAsync(x =>
                x.SportId == reservation.SportId &&
                reservation.StartTime < x.EndTime &&
                reservation.EndTime > x.StartTime &&
                reservation.Date == x.Date 
            );
            
            if (userValidation)
            {
                return new SystemResponse<Reservation>()
                {
                    Success = false,
                    Message = "Usuario ya tiene una reserva en ese horario"
                }; 
            }
            
            if (spaceRepeat)
            {
                return new SystemResponse<Reservation>()
                {
                    Success = false,
                    Message = "El espacio ya tiene una reserva en ese horario"
                }; 
            }

            if (reservation.Date < DateOnly.FromDateTime(DateTime.Now))
            {
                return new SystemResponse<Reservation>()
                {
                    Success = false,
                    Message = "No se pueden crear en fechas anteriores."
                };
            }

            if (reservation.StartTime >= reservation.EndTime)
            {
                return new SystemResponse<Reservation>()
                {
                    Success = false,
                    Message = "La hora de incio no puede ser mayor a la de entrada."
                };
            }
            
            _dbContext.Reservations.Add(reservation);
            
            var result = await _dbContext.SaveChangesAsync();
            
            if (result > 0)
            {
                var user = await _dbContext.Users.FindAsync(reservation.UserID);
                await _emailService.SendReservation(user.Email , user.Name);  
                return new SystemResponse<Reservation>()
                {
                    Message = $"Reservacion creada con exito",
                    Success = true,
                }; 
            }

            return new SystemResponse<Reservation>()
            {
                Message = $"No se pudo crear la reservacion",
                Success = false,
            }; 
        }
        catch (Exception e)
        {
            return new SystemResponse<Reservation>()
            {
                Message = $"Error inesperado: {e.Message}"
            };
        }
    }

    public async Task<SystemResponse<Reservation>> Update(Reservation reservation)
    {
        try
        {
            
            var currentReservation = await _dbContext.Reservations.FindAsync(reservation.Id);

            if (currentReservation == null)
            {
                return new SystemResponse<Reservation>
                {
                    Success = false,
                    Message = "La reservación no existe."
                };
            }

            // Validations

    
            var userValidation = await _dbContext.Reservations.AnyAsync(x =>
                x.Id != reservation.Id &&
                x.UserID == reservation.UserID &&
                x.Date == reservation.Date &&
                reservation.StartTime < x.EndTime &&
                reservation.EndTime > x.StartTime
            );

       
            var spaceRepeat = await _dbContext.Reservations.AnyAsync(x =>
                x.Id != reservation.Id &&
                x.SportId == reservation.SportId &&
                x.Date == reservation.Date &&
                reservation.StartTime < x.EndTime &&
                reservation.EndTime > x.StartTime
            );

            if (userValidation)
            {
                return new SystemResponse<Reservation>
                {
                    Success = false,
                    Message = "El usuario ya tiene una reserva en ese horario."
                };
            }

            if (spaceRepeat)
            {
                return new SystemResponse<Reservation>
                {
                    Success = false,
                    Message = "El espacio ya tiene una reserva en ese horario."
                };
            }

 
            if (reservation.Date < DateOnly.FromDateTime(DateTime.Now))
            {
                return new SystemResponse<Reservation>
                {
                    Success = false,
                    Message = "No se pueden actualizar reservas en fechas anteriores."
                };
            }

 
            if (reservation.StartTime >= reservation.EndTime)
            {
                return new SystemResponse<Reservation>
                {
                    Success = false,
                    Message = "La hora de inicio debe ser menor a la hora de finalización."
                };
            }


            currentReservation.UserID = reservation.UserID;
            currentReservation.SportId = reservation.SportId;
            currentReservation.Status =  reservation.Status;
            currentReservation.Date = reservation.Date;
            currentReservation.StartTime = reservation.StartTime;
            currentReservation.EndTime = reservation.EndTime;

            var result = await _dbContext.SaveChangesAsync();

            if (result > 0)
            {
                var user = await _dbContext.Users.FindAsync(reservation.UserID);
                await _emailService.UpdateReservation(user.Email , user.Name);  
                return new SystemResponse<Reservation>
                {
                    Success = true,
                    Message = "Reservación actualizada con éxito."
                };
            }

            return new SystemResponse<Reservation>
            {
                Success = false,
                Message = "No se pudo actualizar la reservación."
            };
        }
        catch (Exception e)
        {
            return new SystemResponse<Reservation>
            {
                Success = false,
                Message = $"Error inesperado: {e.Message}"
            };
        }
    }

    public async Task<SystemResponse<Reservation>> Delete(int id)
    {
        try
        {
            var reservation = await _dbContext.Reservations.FindAsync(id);
            if (reservation != null)
            {
                reservation.Status = ReservationStatus.Canceled ;
                var result = await _dbContext.SaveChangesAsync();
                if (result > 0)
                {
                    var user = await _dbContext.Users.FindAsync(reservation.UserID);
                    await _emailService.CancelReservation(user.Email , user.Name);  
                    
                    return new SystemResponse<Reservation>()
                    {
                        Message = $"Reservacion cancelada con exito",
                        Success = true
                    };
                }
            }
            return new SystemResponse<Reservation>()
            {
                Message = $"No se pudo cancelar la reservacion",
                Success = false
            };
        }
        catch (Exception e)
        {
            return new SystemResponse<Reservation>()
            {
                Message = $"Error inesperado: {e.Message}"
            };
        }
    }

    public async Task<SystemResponse<IEnumerable<Reservation>>> GetReservationByUser(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<SystemResponse<Reservation>> GetReservationById(int id)
    {
        try
        {
            var reservation = await _dbContext.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return new SystemResponse<Reservation>()
                {
                    Success = false,
                    Message = "No se pudo encontrar la reservacion"
                }; 
            }
            return new SystemResponse<Reservation>()
            {
                Data = reservation,
                Success = true
            };
        }
        catch (Exception e)
        {
            return new SystemResponse<Reservation>()
            {
                Success = false,
                Message = $"Erro desconocido: {e.Message}"
            };
        }
    }

    public async Task<SystemResponse<IEnumerable<Reservation>>> GetReservationBySpace(int id)
    {
        throw new NotImplementedException();
    }
}