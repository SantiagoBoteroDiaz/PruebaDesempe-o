using GestionDeEspacios.Enums;
using GestionDeEspacios.Interfaces;
using GestionDeEspacios.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeEspacios.Controllers;

public class SportSpacesController : Controller
{
    private readonly ISportSpacesService _sportSpace; 
    
    public SportSpacesController(ISportSpacesService user)
    {
        _sportSpace = user;
    }

    public async Task<IActionResult> Index()
    {
        var response = await _sportSpace.GetAllSportSpaces();

        if (!response.Success)
        {
            TempData["Message"] = response.Message;

            return View(new List<SportSpace>());
        }

        return View(response.Data);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SportSpace sportSpace)
    {
        var response = await _sportSpace.Create(sportSpace);
        TempData["Message"] = response.Message;
        return RedirectToAction("Index");
    }

    public async Task<IActionResult>  Edit(int id)
    {
        var findUser = await _sportSpace.GetSportSpaceById(id);
        if (!findUser.Success)
        {
            TempData["Message"] = findUser.Message;
            return RedirectToAction("Index");
        }
        return View(findUser.Data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(SportSpace sportSpace)
    {
        var response = await _sportSpace.Update(sportSpace);
        
        TempData["Message"] =  response.Message;
        
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _sportSpace.Delete(id); 
        TempData["Message"] = response.Message;
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> FilterByType(SportSpacesType type)
    {
        var response = await _sportSpace.GetSportSpaceByType(type);
    
        if (!response.Success)
        {
            TempData["Error"] = response.Message;
            return RedirectToAction("Index");
        }
    
        return View("Index", response.Data);
    }
}