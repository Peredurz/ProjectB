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
class AnnuleringLogic
{
    private List<ChairReservationModel> _chairReservation = new List<ChairReservationModel>();
    protected static List<MovieModel> _movies = new();

    private List<AnnuleringModel> _annulering = new List<AnnuleringModel>();
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
            return false;
        }
        foreach (ChairReservationModel reservation in _chairReservation)
        {
            // reserveringscode, emailadres en tijd moeten goed overeenkomen
            if (reservation.ReserveringsCode == id && reservation.EmailAdress == email && DateTime.Now <= reservation.Time)
            {
                AnnuleringModel ann = new AnnuleringModel(email, id, DateTime.Now);
                bool containItem = _annulering.Any(item => item.ReservationID == ann.ReservationID);
                if (containItem)
                {
                    Console.WriteLine("U heeft voor deze film al een annulering aangemaakt.");
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

    // optie om alle annuleringen te zien
    public void ShowAnnulering()
    {
        foreach(AnnuleringModel annulering in _annulering)
        {
            Console.WriteLine($"ID: {annulering.ID}\nEmailAddress: {annulering.EmailAddress}\nReservationID: {annulering.ReservationID}\nAnnuleringDatum: {annulering.AnnuleringDatum}");
        Console.WriteLine();
        }
    }

    //controleerd of annuleringen bestaat 
    public AnnuleringModel GetAnnulering(int id)
    {
        foreach(AnnuleringModel annulering in _annulering)
        {
            if (annulering.ID == id)
            {
                Console.WriteLine($"\nU heeft gekozen voor \nID: {annulering.ID}\nEmailAddress: {annulering.EmailAddress}\nReservationID: {annulering.ReservationID}\nAnnuleringDatum: {annulering.AnnuleringDatum}\n");
                return annulering;
            }
        }
        return null;
    }

    //annuleringen accepteren
    public bool AnnuleringAccept(int id)
    {
        while(true)
        {
            Console.WriteLine("Wilt u de anunulering accepteren of weigeren. A/W ");
            Console.Write(">");
            string answerUser = Console.ReadLine().ToUpper();
            foreach(AnnuleringModel annulering in _annulering)
            {
                switch (answerUser)
                {
                    case "A":
                        if (annulering.ID == id)
                        {
                            _annulering.Remove(annulering);
                            Console.WriteLine("Annulering succesvol geaccepteerd.\n");
                            AnnuleringAccess.WriteAll(_annulering);
                            return true;
                        }
                        break;
                    case "W":
                        if (annulering.ID == id)
                        {
                            _annulering.Remove(annulering);
                            Console.WriteLine("Annulering succesvol geweigerd.\n");
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
}