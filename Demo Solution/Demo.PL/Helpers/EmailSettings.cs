using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
	public static class EmailSettings
	{
		//zylwvleqracuylzu

		public static void SendEmail(Email email) 
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("esmailhelal2929@gmail.com", "zylwvleqracuylzu");
			client.Send("esmailhelal2929@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
