using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SOSTransito.Repositories
{
    public class MailService
    {
        public static bool sendMail(string email, string titulo, string texto)
        {
            try
            {
                //Credenciais do e-mail                  
                MailMessage objeto_mail = new MailMessage();
                SmtpClient mail = new SmtpClient();
                //hostMail.Port = 2550 ou 465; Usar quando online
                mail.Port = 587; //usar quando localhost
                mail.Host = "mail50.redehost.com.br";
                mail.Timeout = 90000;
                mail.DeliveryMethod = SmtpDeliveryMethod.Network;
                mail.UseDefaultCredentials = false;
                mail.Credentials = new System.Net.NetworkCredential("admin@fretare.com.br", "#AdmFretare7");
                //Informações do e-mail
                objeto_mail.From = new MailAddress("admin@fretare.com.br");
                objeto_mail.To.Add(new MailAddress(email));
                objeto_mail.Subject = titulo;
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Body = texto;
                mail.Send(objeto_mail);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
