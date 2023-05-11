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

    public static void SendMail()
    {
      string name;
      string emailaddress;
      List<ChairReservationModel> allReservations = new List<ChairReservationModel>();
      allReservations = ChairReservationAccess.LoadAll();
      ChairReservationModel lastReservation = allReservations[allReservations.Count - 1];
      int auditorium = lastReservation.AuditoriumID + 1;
      DateTime time = lastReservation.Time;
      int reservationCode = lastReservation.ReserveringsCode;
      int chairID = lastReservation.ChairID;
      double totaalPrijs = AccountsLogic.TotaalPrijs;
      bool parkingticket = ParkingTicketLogic.choiseParkingTicket;

      if (AccountsLogic.CurrentAccount == null)
      {
        var info = Mail.AskInfo();
        emailaddress = info.Item1;
        name = info.Item2;
      }
      else
      {
        name = AccountsLogic.CurrentAccount.FullName;
        emailaddress = AccountsLogic.CurrentAccount.EmailAddress;
      }
      var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Project Groep 1 INF1F", "projectgroep1fhr@gmail.com"));
        email.To.Add(new MailboxAddress(name, emailaddress));
        email.Subject = "Reservering Bioscoop Naamloos";
        var builder = new BodyBuilder();
        builder.TextBody = @$"Beste {name},
        
Bedankt voor het reserveren bij Bioscoop Naamloos.
Uw reservering is succesvol verwerkt.
Uw reserveringscode is: {reservationCode}
Uw stoelnummer is: {chairID}
Uw zaalnummer is: {auditorium}
Uw film is: blank
Uw film begint om: {time}
Uw totaalprijs is: {totaalPrijs}

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
      ParkingTicketLogic.choiseParkingTicket = false;
    }

    public static bool ValidateMailAddress(string mailAddress)
    {
      try
      {
        var addr = new System.Net.Mail.MailAddress(mailAddress);
        return addr.Address == mailAddress;
      }
      catch
      {
        return false;
      }
    }
}