/// <summary>
/// De <c>AnnuleringLogic</c> class zorgt ervoor dat annuleringen kunnen worden aangemaakt,
/// maar ook dat er annuleringen kunnen worden ingezien.
/// <para>
/// Als de class wordt geïnitialiseerd worden eerst drie verschillende lijsten gemaakt namelijk: 
/// <see cref="MovieModel"/>, <see cref="AnnuleringModel"/> en de <see cref="ChairReservationModel"/> objecten die allemaal
/// in een eigen lijst worden gestopt zodat hier makkelijker mee gewerkt kan worden. De drie lijsten zijn ook alleen maar in deze class te gebruiken.
/// </para>
/// <para>
/// De <see cref="Annuleringen"/> method laten alle annuleringen zien van een bepaalde emailadres, de <see cref="AnnuleringID"/> method maakt een
/// nieuwe annulering aan op basis van <see cref="ChairReservationModel.ID"/> en een <see cref="ChairReservationModel.EmailAdress"/>
/// en de <see cref="Movie"/> zorgt ervoor dat de <see cref="_movies"/> buiten de class nog kan worden gezien."
/// </para>
/// </summary>
public class AnnuleringLogic
{
    private List<ChairReservationModel> _chairReservation = new List<ChairReservationModel>();
    protected static List<MovieModel> _movies = new();
    private static ChairReservationAccess ChairReservationAccess = new ChairReservationAccess();
    private static MovieAccess MovieAccess = new MovieAccess();
    private static AnnuleringAccess AnnuleringAccess = new AnnuleringAccess();

    private List<AnnuleringModel> _annulering = new List<AnnuleringModel>();
    private static MovieLogic _movieLogic = new MovieLogic();
    public AnnuleringLogic()
    {
        _chairReservation = ChairReservationAccess.LoadAll();
        _movies = MovieAccess.LoadAll();
        _annulering = AnnuleringAccess.LoadAll();
    }

    /// <summary>
    /// Dit is de method om alle annuleringen te tonen aan een gebruiker die hierdoor dan kan kiezen welke film ticket hij of zij wil annuleren.
    /// </summary>
    /// <param name="email"></param>
    /// <returns>List<see cref="ChairReservationModel"/></returns>
    public List<ChairReservationModel> Annuleringen(string email)
    {
        List<ChairReservationModel> reservations = new();
        foreach (ChairReservationModel reservation in _chairReservation)
        {
            if (reservation.EmailAdress == email) reservations.Add(reservation);
        }
        return reservations;
    }

    /// <summary>
    /// Deze method zorgt ervoor dat er een nieuwe annulering wordt aangemaakt als deze nog niet bestaat.
    /// Om dan vervolgens dit aan de List<see cref="AnnuleringModel"/> toe te voegen en vervolgens de json file te updaten.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="email"></param>
    /// <returns><see cref="bool"/></returns>
    public bool AnnuleringID(int id, string email)
    {
        int lengte = _annulering.Count();
        if (!_chairReservation.Any(item => item.ReserveringsCode == id))
        {
            Console.WriteLine("Deze reserveringscode bestaat niet.");
            PresentationLogic.CurrentMessage = "Deze reserveringscode bestaat niet.";
            return false;
        }
        foreach (ChairReservationModel reservation in _chairReservation)
        {
            // reserveringscode, emailadres en tijd moeten goed overeenkomen
            if (reservation.ReserveringsCode == id && reservation.EmailAdress == email)
            {
                AnnuleringModel ann = new AnnuleringModel(email, id, DateTime.Now);
                bool containItem = _annulering.Any(item => item.ReservationID == ann.ReservationID);
                if (containItem)
                {
                    Console.WriteLine("U heeft voor deze film al een annulering aangemaakt.");
                    PresentationLogic.CurrentMessage = "U heeft voor deze film al een annulering aangemaakt.";
                    return false;
                }
                _annulering.Add(ann);
                break;
            }
        }

        // Als er niet een nieuwe annulering is aangemaakt dan wordt er met false gereturned
        if (lengte == _annulering.Count())
        {
            Console.WriteLine("\nDe combinatie van emailadres en reserveringscode kloppen niet.");
            Console.WriteLine("Check of de reservering ook echt is gemaakt met de gegeven email.");
            Console.WriteLine("Het kan ook zijn dat de film al is geweest. Deze reservering kan je natuurlijk niet annuleren.");
            return false;
        }
        AnnuleringAccess.WriteAll(_annulering);
        return true;
    }

    /// <summary>
    /// Method om een prive lijst van <see cref="MovieModel"/> te returnen. Omdat deze lijst privé is.
    /// </summary>
    /// <returns>List<see cref="MovieModel"/></returns>
    public List<MovieModel> Movie() => _movies;

    /// <summary>
    /// optie om alle annuleringen te zien
    /// </summary>
    public void ShowAnnulering()
    {
        foreach (AnnuleringModel annulering in _annulering)
        {
            ConsoleColor color = Console.ForegroundColor;
            ChairReservationModel? reservation = _chairReservation.Find(item => item.ReserveringsCode == annulering.ReservationID);
            if (reservation == null)
            {
                continue;
            }
            // Half uur van te voren of later is rood
            if (annulering.AnnuleringDatum > reservation.Time - TimeSpan.FromMinutes(30))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            // Tussen 30 minuten en 24 uur van te voren is geel
            else if (annulering.AnnuleringDatum > reservation.Time - TimeSpan.FromMinutes(1440))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            // Anders is het groen
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine($"ID: {annulering.ID}\nEmailAddress: {annulering.EmailAddress}\nReservationID: {annulering.ReservationID}\nAnnuleringDatum: {annulering.AnnuleringDatum}\nFilmdatum: {reservation.Time}\n");
            Console.WriteLine();
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Rood: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("nadat de film was gespeeld geannuleerd.");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Geel: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("tussen 30 minuten en 24 uur voor de film was gespeeld geannuleerd.");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Groen: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Meer dan 24 uur van te voren geannuleerd.\n");
    }

    /// <summary>
    /// controleerd of annuleringen bestaat
    /// </summary>
    public AnnuleringModel GetAnnulering(int id)
    {
        foreach (AnnuleringModel annulering in _annulering)
        {
            if (annulering.ID == id)
            {
                Console.WriteLine($"\nU heeft gekozen voor \nID: {annulering.ID}\nEmailAddress: {annulering.EmailAddress}\nReservationID: {annulering.ReservationID}\nAnnuleringDatum: {annulering.AnnuleringDatum}\n");
                return annulering;
            }
        }
        return null;
    }

    /// <summary>
    /// annuleringen accepteren
    /// </summary>
    public bool AnnuleringAccept(int id)
    {
        string movieOuput = _movieLogic.ShowMovies();
        while (true)
        {
            Console.WriteLine("Wilt u de anunulering accepteren of weigeren. A/W ");
            Console.Write(">");
            string answerUser = Console.ReadLine().ToUpper();
            foreach (AnnuleringModel annulering in _annulering)
            {
                switch (answerUser)
                {
                    case "A":
                        if (annulering.ID == id)
                        {
                            ChairReservationModel reservationModel = GetResservationByID(annulering);
                            MovieModel movie = MovieLogic.GetMovie(reservationModel.MovieID);
                            double moneyReturn = AnnuleringCalculator(movie, annulering, reservationModel);
                            _annulering.Remove(annulering);
                            Console.WriteLine("Annulering succesvol geaccepteerd.\n");
                            PresentationLogic.CurrentMessage = "Annulering succesvol geaccepteerd";
                            MailLogic.SendCancelationMail(annulering, movie, reservationModel, moneyReturn);
                            AnnuleringLogic.DeleteCanceledChair(annulering.ReservationID);
                            AnnuleringAccess.WriteAll(_annulering);
                            return true;
                        }
                        break;
                    case "W":
                        if (annulering.ID == id)
                        {
                            ChairReservationModel reservationModel = GetResservationByID(annulering);
                            MovieModel movie = MovieLogic.GetMovie(reservationModel.MovieID);
                            _annulering.Remove(annulering);
                            Console.WriteLine("Annulering succesvol geweigerd.\n");
                            PresentationLogic.CurrentMessage = "Annulering succesvol geweigerd";
                            MailLogic.SendCancelationMail(annulering, movie, reservationModel, isRejected: true);
                            AnnuleringAccess.WriteAll(_annulering);
                            return false;
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Berekening terug geven van het geld
    /// </summary>
    public double AnnuleringCalculator(MovieModel movie, AnnuleringModel annulering, ChairReservationModel reservation)
    {
        DateTime timeNow = annulering.AnnuleringDatum;
        DateTime dateTime = movie.Time;
        TimeSpan difference = dateTime - timeNow;
        if (difference.TotalHours >= 24)
        {
            return reservation.TotaalPrijs;
        }
        else
        {
            return reservation.TotaalPrijs / 2;
        }
    }

    /// <summary>
    /// Het verkijgen van de Resservation ID
    /// </summary>
    public ChairReservationModel GetResservationByID(AnnuleringModel annulering)
    {
        foreach (ChairReservationModel reservation in _chairReservation)
        {
            if (annulering.ReservationID == reservation.ReserveringsCode)
            {
                return reservation;
            }
        }
        return null;
    }

    /// <summary>
    /// Verwijdert de stoel in chairreservation
    /// </summary>
    public static void DeleteCanceledChair(int reserveringsCode)
    {
        List<ChairReservationModel> _chairReservations = ChairReservationAccess.LoadAll();
        List<ChairReservationModel> chairReservations = new List<ChairReservationModel>();
        foreach (ChairReservationModel chair in _chairReservations)
        {
            if (chair.ReserveringsCode != reserveringsCode)
                chairReservations.Add(chair);
        }
        _chairReservations = chairReservations;
        ChairReservationAccess.WriteAll(_chairReservations);
    }
}