// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    M.Modal.init(document.querySelectorAll('.modal'));
});

function showUser(name, documentId, phone, email) {

    document.getElementById("viewName").textContent = name;
    document.getElementById("viewDocument").textContent = documentId;
    document.getElementById("viewPhone").textContent = phone;
    document.getElementById("viewEmail").textContent = email;
}

function showSpace(id, name, type, capacity) {

    document.getElementById("viewName").textContent = name;
    document.getElementById("viewType").textContent = type;
    document.getElementById("viewCapacity").textContent = capacity + " personas";
}

function showReservation(id, user, space, date, start, end)
{
    document.getElementById("viewUser").textContent = user;
    document.getElementById("viewSpace").textContent = space;
    document.getElementById("viewDate").textContent = date;
    document.getElementById("viewStart").textContent = start;
    document.getElementById("viewEnd").textContent = end;
    
}