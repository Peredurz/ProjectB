using System;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using QRCoder;

class MailLogic
{
    private static string _name;
    private static string _emailAddress;
    public static string EmailAddress 
    { 
        get { return _emailAddress; } 
        set { _emailAddress = value; } 
    } 
    public static string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public static int GenerateCode()
    {
        Random RandomInt = new Random();
        return RandomInt.Next();
    }

    public static void SendMail()
    {
        var reservationLogic = new ChairReservationLogic();
        var reservation = reservationLogic.GetChairReservation(AccountsLogic.CurrentReservationCode);
        int auditorium = reservation.Item1.AuditoriumID + 1;
        DateTime time = reservation.Item1.Time;
        int reservationCode = reservation.Item1.ReserveringsCode;
        int chairID = reservation.Item1.ChairID;
        var chairLogic = new ChairLogic();
        var chair = chairLogic.GetChairModel(chairID);
        int chairRow = chair.Row;
        int chairColumn = chair.Column;
        double totaalPrijs = AccountsLogic.TotaalPrijs;
        bool parkingticket = ParkingTicketLogic.choiseParkingTicket;
        int movieId = reservation.Item1.MovieID;
        string movieTitle = MovieLogic.GetMovie(movieId).Title;

        if (AccountsLogic.CurrentAccount == null)
        {
            var info = Mail.AskInfo();
            EmailAddress = info.Item1;
            Name = info.Item2;
        }
        else
        {
            Name = AccountsLogic.CurrentAccount.FullName;
            EmailAddress = AccountsLogic.CurrentAccount.EmailAddress;
        }
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Project Groep 1 INF1F", "projectgroep1fhr@gmail.com"));
        email.To.Add(new MailboxAddress(Name, EmailAddress));
        email.Subject = "Reservering Bioscoop Naamloos";
        var builder = new BodyBuilder();
        builder.TextBody = @$"Beste {Name},
        
Bedankt voor het reserveren bij Bioscoop Naamloos.
Uw reservering is succesvol verwerkt.
Uw reserveringscode is: {reservationCode}
Uw stoel bevind zich op: Rij {chairRow}, Stoel {chairColumn}
Uw zaal is: zaal {auditorium}
Uw film is: {movieTitle}
Uw film begint om: {time}
Uw totaalprijs is: €{totaalPrijs}

Vriendelijke Groet,
Bioscoop Naamloos

---------------------------------------
Telefoon nummer:    010 123 123 12.
adres:              Wijnhaven 107.
postcode:           3011 WN in Rotterdam.
Openings tijd:      Wij zijn dertig minuten voor de eerste film geopend 
                    De bioscoop sluit vijftien minuten na de laatste film
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

    public static void GenerateQRCode()
    {
        string reservationCode = Convert.ToString(AccountsLogic.CurrentReservationCode);
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(reservationCode, QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new QRCode(qrCodeData);
        System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);
        qrCodeImage.Save("qr-code.png", System.Drawing.Imaging.ImageFormat.Png);
    }
}