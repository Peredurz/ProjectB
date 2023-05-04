using System;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

class MailLogic
{
    static int GenerateCode()
    {
      Random RandomInt = new Random(); 
      return RandomInt.Next();
    }

    public static void SendMail()
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Project Groep 1 INF1F", "projectgroep1fhr@gmail.com"));
        email.To.Add(new MailboxAddress("Gilian", "gilkranendonk@gmail.com"));
        email.Subject = "Test";
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) 
        { 
          Text = "<b>Example HTML Message Body</b>" 
        };

        using (var smtp = new SmtpClient())
        {
          smtp.Connect("smtp.gmail.com", 587, false);

          smtp.Authenticate("projectgroep1fhr@gmail.com", "gadaklozkmjfzcih");
          smtp.Send(email);
          Console.WriteLine("The mail has been sent successfully !!");
          smtp.Disconnect(true);
    }
    }
}