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

    public List<ChairReservationModel> Annuleringen(string email)
    {
        List<ChairReservationModel> reservations = new();
        foreach (ChairReservationModel reservation in _chairReservation)
        {
            if (reservation.EmailAdress == email) reservations.Add(reservation);
        }
        return reservations;
    }

    public bool AnnuleringID(int id, string email)
    {
        foreach (ChairReservationModel reservation in _chairReservation)
        {
            if (reservation.ID == id)
            {
                AnnuleringModel ann = new AnnuleringModel(email, id);
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
        AnnuleringAccess.WriteAll(_annulering);
        return true;
    }

    public List<MovieModel> Movie() => _movies;
}