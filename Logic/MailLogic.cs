using System;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using QRCoder;
using MimeKit.Utils;

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
        var pathQrCode = Path.Combine("DataDocs/QrTicket", "qr-code.png");
        var qrImage = builder.LinkedResources.Add(pathQrCode);

        qrImage.ContentId = MimeUtils.GenerateMessageId();
        
        builder.HtmlBody = string.Format(@$"<h1>Beste {Name},</h1>
        <p>Bedankt voor uw reservering bij Bioscoop Naamloos. <br>
        Uw reserveringscode is de onderstaande QR-code. <br>
        <img src=""cid:{qrImage.ContentId}"" alt=""QR-code"" width=""200"" height=""200""> <br>
        Uw reserveringscode is: {reservationCode} <br>
        Uw reservering is voor de film: {movieTitle} <br>
        Uw film vind plaats in zaal: {auditorium} <br>
        Uw film begint om: {time} <br>
        Uw stoel is: Rij {chairRow}, Stoel {chairColumn} <br>
        Uw totaalprijs is: €{totaalPrijs} <br>
        U kunt uw reservering annuleren tot half uur voor de film begint. <br>
        Wij wensen u veel plezier bij de film. <br> <br>
        Met vriendelijke groet, <br>
        Bioscoop Naamloos <br> <br>
        ----------------Contact gegevens---------------- <br>
        Telefoon nummer: <br>    
            <strong>010 123 123 12</strong> <br>
        Adres: <br>             
            <strong>Wijnhaven 107</strong> <br>
        Postcode: <br>       
            <strong>3011 WN in Rotterdam</strong>  <br>
        Openings tijd: <br>     
            <strong> Wij zijn dertig minuten voor de eerste film geopend <br>
            De bioscoop sluit vijftien minuten na de laatste film. </strong></p>
        <p><strong> Deze mail is automatisch gegenereerd, u kunt hier niet op reageren. </strong></p>
        ");

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
        // save the image file to DataDocs/QrTicket/qr-code.png
        using (System.IO.Stream stream = File.OpenWrite(Path.Combine("DataDocs/QrTicket", "qr-code.png")))
        {
            qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        }
    }

    public static void SendCancelationMail(AnnuleringModel annulering,
                                           MovieModel movie,
                                           ChairReservationModel reservation,
                                           double price = 0.0,
                                           bool isRejected = false)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Project Groep 1 INF1F", "projectgroep1fhr@gmail.com"));
        email.To.Add(new MailboxAddress(reservation.Name, annulering.EmailAddress));
        email.Subject = isRejected == true ? "Uw annulering is geweigerd" : "Uw annulering is goedgekeurd";
        var builder = new BodyBuilder();
        if (isRejected == true)
        {
            builder.HtmlBody = string.Format(@$"<h1>Beste {reservation.Name},</h1>
            <p> Helaas is uw aanvraag om uw kaartje voor de film {movie.Title} te annuleren geweigerd door 
            een van onze medewerkers.</p><br> <br>
            Met vriendelijke groet, <br>
            Bioscoop Naamloos <br> <br>
            ----------------Contact gegevens---------------- <br>
            Telefoon nummer: <br>    
                <strong>010 123 123 12</strong> <br>
            Adres: <br>             
                <strong>Wijnhaven 107</strong> <br>
            Postcode: <br>       
                <strong>3011 WN in Rotterdam</strong>  <br>
            Openings tijd: <br>     
                <strong> Wij zijn dertig minuten voor de eerste film geopend <br>
                De bioscoop sluit vijftien minuten na de laatste film. </strong></p>
            <p><strong> Deze mail is automatisch gegenereerd, u kunt hier niet op reageren. </strong></p>");
        }
        else
        {
            builder.HtmlBody = string.Format(@$"<h1>Beste {reservation.Name},</h1>
            <p> Uw aanvraag om de film {movie.Title} te annuleren is geaccepteerd door een van onze medewerkers.<br>
            U kunt uw geld binnen een schappelijke tijd terug verwachten.<br>
            Uw reserveringscode is: {reservation.ReserveringsCode} <br>
            Uw bedrag wat u terug krijgt is: €{price} <br>
            </p><br> <br>
            Met vriendelijke groet, <br>
            Bioscoop Naamloos <br> <br>
            ----------------Contact gegevens---------------- <br>
            Telefoon nummer: <br>    
                <strong>010 123 123 12</strong> <br>
            Adres: <br>             
                <strong>Wijnhaven 107</strong> <br>
            Postcode: <br>       
                <strong>3011 WN in Rotterdam</strong>  <br>
            Openings tijd: <br>     
                <strong> Wij zijn dertig minuten voor de eerste film geopend <br>
                De bioscoop sluit vijftien minuten na de laatste film. </strong></p>
            <p><strong> Deze mail is automatisch gegenereerd, u kunt hier niet op reageren. </strong></p>");
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