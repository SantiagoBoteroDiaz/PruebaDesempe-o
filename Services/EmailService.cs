using GestionDeEspacios.Settings;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;


public class EmailService 
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    private async Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml)
    {
        var mensaje = new MimeMessage();
        mensaje.From.Add(new MailboxAddress(_settings.NombreRemitente, _settings.Email));
        mensaje.To.Add(MailboxAddress.Parse(destinatario));
        mensaje.Subject = asunto;
        mensaje.Body = new TextPart("html") { Text = cuerpoHtml };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.Host, _settings.Port, false);
        await client.AuthenticateAsync(_settings.Email, _settings.Password);
        await client.SendAsync(mensaje);
        await client.DisconnectAsync(true);
    }
    
    public async Task SendReservation(string destinatario, string nombreCuenta)
    {
        var message = "Se ha creado una reservacion";
        var body = $@" Hola {nombreCuenta} se ha realizado una reservacion a tu nombre. </h1>";
        await EnviarAsync(destinatario, message , body); 
    }
    public async Task UpdateReservation(string destinatario, string nombreCuenta)
    {
        var message = "Se ha actualizado una reservacion";
        var body = $@" Hola {nombreCuenta} se ha realizado una actualizacion a tu reserva. </h1>";
        await EnviarAsync(destinatario, message , body); 
    }
    public async Task CancelReservation(string destinatario, string nombreCuenta)
    {
        var message = "Se ha cancelado una reservacion";
        var body = $@" Hola {nombreCuenta} se ha realizado una cancelacion a tu reserva. </h1>";
        await EnviarAsync(destinatario, message , body); 
    }
}