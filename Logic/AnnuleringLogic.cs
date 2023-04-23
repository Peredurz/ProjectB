class AnnuleringLogic
{
    private List<ChairReservationModel> _chairReservation = new List<ChairReservationModel>();

    public AnnuleringLogic()
    {
        _chairReservation = ChairReservationAccess.LoadAll();
    }

    public List<ChairReservationModel> Annulering(string email)
    {
        List<ChairReservationModel> reservations = new();
        foreach (ChairReservationModel reservation in _chairReservation)
        {
            if (reservation.EmailAdress == email) reservations.Add(reservation);
        }
        return reservations;
    }
}