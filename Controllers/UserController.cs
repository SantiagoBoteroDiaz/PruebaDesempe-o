using GestionDeEspacios.Interfaces;
using GestionDeEspacios.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeEspacios.Controllers;

public class UserController : Controller
{
    private readonly IUserService _user; 
    public UserController(IUserService user)
    {
        _user = user;
    }

    public async Task<IActionResult> Index()
    {
        var response = await _user.GetAllUsers();
        if (!response.Success)
        {
            TempData["Message"] = response.Message;
            return View();
        }
        return View(response.Data);
    }

    public async Task<IActionResult> edit(int id)
    {
        var response = await _user.GetUserById(id);
        if (!response.Success)
        {
            TempData["Message"] = response.Message;
            return RedirectToAction("Index");
        }
        return View(response.Data);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var response = await _user.Delete(id);
        TempData["Message"] = response.Message;
        return RedirectToAction("Index");
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Save(User user)
    {
        var saveUser = await _user.Create(user);
        TempData["Message"] = saveUser.Message;
        return RedirectToAction("Index"); 
    }

    public async Task<IActionResult> Update(int id)
    {
        var response = await _user.GetUserById(id);
        if (!response.Success)
        {
            TempData["Message"] = response.Message;
            return RedirectToAction("Index");
        }
        return View(response.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(User user)
    {
        var userUpd = await _user.Update(user);
        TempData["Message"] = userUpd.Message;
        return RedirectToAction("Index");
    }
}