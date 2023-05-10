using System;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

class MailLogic
{
    public static int GenerateCode()
    {
      Random RandomInt = new Random(); 
      return RandomInt.Next();
    }

    public static void SendMail(string name, string emailaddress, string seat, string auditorium, string movie, bool parkingticket)
    {
      var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Project Groep 1 INF1F", "projectgroep1fhr@gmail.com"));
        email.To.Add(new MailboxAddress(name, emailaddress));
        email.Subject = "Reservering Bioscoop Naamloos";
        var builder = new BodyBuilder();
        builder.TextBody = @$"Beste {name},
        
Bedankt voor het reserveren bij Bioscoop Naamloos.
Uw reservering is succesvol verwerkt.
Uw reserveringscode is: {GenerateCode()}
Uw stoelnummer is: {seat}
Uw zaalnummer is: {auditorium}
Uw film begint om: {movie}

Vriendelijke Groet,
Bioscoop Naamloos
";
        if (parkingticket)
        {
          builder.TextBody += @$"Uw parkeerkaartje is bijgevoegd.";
          builder.Attachments.Add(Path.Combine("DataDocs/ParkeerKaart", "ticket.png"));
        }
        email.Body = builder.ToMessageBody();

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