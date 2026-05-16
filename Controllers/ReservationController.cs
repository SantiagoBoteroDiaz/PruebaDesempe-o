using GestionDeEspacios.Interfaces;
using GestionDeEspacios.Models;
using GestionDeEspacios.ModelView;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeEspacios.Controllers;

public class ReservationController : Controller
{
    private readonly IReservationService _reservationService; 
    private readonly IUserService _userService;
    private readonly ISportSpacesService _sportSpacesService;

    public ReservationController(IReservationService user, IUserService userService,
        ISportSpacesService sportSpacesService)
    {
        _reservationService = user;
        _userService = userService;
        _sportSpacesService = sportSpacesService;
    }
    
    public async Task<IActionResult> Index()
    {
        var reservations = await _reservationService.GetAllReservations();
        if (!reservations.Success)
        {
            TempData["Message"] = reservations.Message;
            return View(new List<Reservation>());
        }
        return View(reservations.Data);
    }
        
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _reservationService.Delete(id);
        TempData["Message"] = result.Message;
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Create()
    {   
        var users = await _userService.GetAllUsers();
        var sportSpaces = await _sportSpacesService.GetAllSportSpaces();
        var viewModel = new ReservationViewModel()
        {
            Users = users.Data,
            SportSpaces = sportSpaces.Data,
            Reservation =  new Reservation()
        }; 
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Save(Reservation reservation)
    {
        var response = await _reservationService.Create(reservation);
        TempData["Message"] = response.Message;
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _reservationService.GetReservationById(id);
        if (!response.Success)
        {
            TempData["Message"] = response.Message;
            return RedirectToAction("Index");
        }
        var users = await _userService.GetAllUsers();
        var sportSpaces = await _sportSpacesService.GetAllSportSpaces();
        var modelView = new ReservationViewModel
        {
            Reservation = response.Data,
            Users = users.Data ,
            SportSpaces = sportSpaces.Data
        };
        return View(modelView);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Reservation reservation)
    {
        var response = await _reservationService.Update(reservation); 
        TempData["Message"] = response.Message;
        return RedirectToAction("Index");
    }
}