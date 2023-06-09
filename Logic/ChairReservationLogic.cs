using System;

class ChairReservationLogic
{
    private List<ChairReservationModel> _chairReservation = new List<ChairReservationModel>();
    private ChairLogic _chairLogic = new ChairLogic();

    private static ChairReservationAccess ChairReservationAccess = new ChairReservationAccess();
    public ChairReservationLogic()
    {
        _chairReservation = ChairReservationAccess.LoadAll();
    }

    /// <summary>
    /// Om alle reserveringen naar de JSON te schrijven.
    /// </summary>
    public void WriteAll()
    {
        ChairReservationAccess.WriteAll(_chairReservation);
    }

    /// <summary>
    /// Om een reservering voor een stoel te maken, deze returned True als deze gereserveerd is.
    /// </summary>
    public bool ReserveChair(int auditoriumID, int movieID, ChairModel chair)
    {
        AccountsLogic.TotaalPrijs += chair.Price;
        DateTime movieTime = MovieLogic.GetMovie(movieID).Time;
        string userEmail = "";
        if (AccountsLogic.CurrentAccount != null)
            userEmail = AccountsLogic.CurrentAccount.EmailAddress;

        if (CheckChairAvailability(chair.ID, movieTime, auditoriumID) == false)
        {
            Console.WriteLine("Deze stoel is al gereserveerd.");
            return false;
        }
        else
        {
            _chairReservation.Add(new ChairReservationModel(userEmail, chair.ID, movieID, auditoriumID, movieTime, AccountsLogic.TotaalPrijs, AccountsLogic.CurrentReservationCode));

            ChairReservationAccess.WriteAll(_chairReservation);
            return true;
        }
    }

    /// <summary>
    /// Om te checken of een stoel bezet is of niet.
    /// </summary>
    public bool CheckChairAvailability(int id, DateTime time, int auditoriumId)
    {
        bool available = true;

        foreach (var chairReservation in _chairReservation)
        {
            if (chairReservation.ChairID == id && chairReservation.Time == time && chairReservation.AuditoriumID == auditoriumId)
            {
                available = false;
                break;
            }
        }
        return available;
    }

    /// <summary>
    /// Om een reservering te krijgen, dit kan via een reserveringscode, reserverings ID of stoel ID.
    /// </summary>
    public Tuple<ChairReservationModel, int> GetChairReservation(int codeOrID)
    {
        foreach (ChairReservationModel _reservation in _chairReservation)
        {
            if (_reservation.ID == codeOrID || _reservation.ReserveringsCode == codeOrID || _reservation.ChairID == codeOrID)
                return Tuple.Create(_reservation, _reservation.ID);
        }
        return null;
    }

    /// <summary>
    /// Om meerdere stoelen te krijgen en dat als een lijst terug te geven.
    /// </summary>
    public List<Tuple<ChairReservationModel, int>> GetChairReservations(int codeOrID)
    {
        List<Tuple<ChairReservationModel, int>> chairReservations = new List<Tuple<ChairReservationModel, int>>();
        foreach (ChairReservationModel _reservation in _chairReservation)
        {
            // check if the codeOrID is equal to the ID, ReserveringsCode or ChairID and if its not already in the list of chairReservations add it to the list
            if (_reservation.ID == codeOrID || _reservation.ReserveringsCode == codeOrID || _reservation.ChairID == codeOrID && !chairReservations.Contains(Tuple.Create(_reservation, _reservation.ID)))
                chairReservations.Add(Tuple.Create(_reservation, _reservation.ID));
        }
        return chairReservations;
    }

    /// <summary>
    /// Om de index te krijgen van een reservering in de _chairReservation lijst.
    /// </summary>
    public int GetChairReservationIndex(int reservationID)
    {
        return _chairReservation.FindIndex(x => x.ID == reservationID);
    }

    /// <summary>
    /// Om een stoel te updaten op een bepaalde index.
    /// </summary>
    public void UpdateChairReservationAtIndex(Tuple<ChairReservationModel, int> reservationInfo)
    {
        _chairReservation[reservationInfo.Item2] = reservationInfo.Item1;
    }

    /// <summary>
    /// Om alle stoelen die gereserveerd zijn te updaten met nieuwe en gefinaliseerde data, deze verwijderd ook alle,
    /// tijdelijke stoelen en update statische variables naar hun 0 waardes.
    /// </summary>
    public static void UpdateChairReservation()
    {
        ChairReservationLogic chairReservationLogic = new ChairReservationLogic();
        foreach (ChairModel _chair in AccountsLogic.ChosenChairs)
        {
            var chairReservationModel = chairReservationLogic.GetChairReservation(_chair.ID);
            chairReservationModel.Item1.TotaalPrijs = AccountsLogic.TotaalPrijs;
            chairReservationModel.Item1.IsCompleted = true;
            chairReservationModel.Item1.EmailAdress = MailLogic.EmailAddress;
            chairReservationModel.Item1.Name = MailLogic.Name;
            int reservationIndex = chairReservationLogic.GetChairReservationIndex(chairReservationModel.Item2);
            chairReservationLogic.UpdateChairReservationAtIndex(Tuple.Create(chairReservationModel.Item1, reservationIndex));
        }
        chairReservationLogic.RemoveNotCompletedReservations();
        chairReservationLogic.WriteAll();
        AccountsLogic.CurrentReservationCode = MailLogic.GenerateCode();
        AccountsLogic.ChosenChairs.Clear();
        AccountsLogic.TotaalPrijs = 0;
    }

    /// <summary>
    /// Om alle niet afgemaakte reserveringen te verwijderen.
    /// </summary>
    public void RemoveNotCompletedReservations()
    {
        List<ChairReservationModel> chairReservations = new List<ChairReservationModel>();
        foreach (ChairReservationModel _reservation in _chairReservation)
            if (_reservation.IsCompleted == true)
                chairReservations.Add(_reservation);

        _chairReservation = chairReservations;
    }

    /// <summary>
    /// Om de reserveringen van de gebruiker te krijgen.
    /// </summary>
    public List<ChairReservationModel> GetUserReservations(string emailAddress)
    {
        List<ChairReservationModel> chairReservations = new List<ChairReservationModel>();
        foreach (ChairReservationModel _reservation in _chairReservation)
            if (_reservation.EmailAdress == emailAddress)
                chairReservations.Add(_reservation);

        return chairReservations;
    }
}
