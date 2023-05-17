using System;

class ChairReservationLogic
{
    private List<ChairReservationModel> _chairReservation = new List<ChairReservationModel>();
    private ChairLogic _chairLogic = new ChairLogic();

    public ChairReservationLogic()
    {
        _chairReservation = ChairReservationAccess.LoadAll();
    }

    public void WriteAll()
    {
        ChairReservationAccess.WriteAll(_chairReservation);
    }

    public bool ReserveChair(int auditoriumID, int movieID, int chairRow, int chairCol)
    {
        int chairID = ChairLogic.FindChairID(chairRow, chairCol, auditoriumID);
        if (chairID == 0)
        {
            Console.WriteLine("Stoel met die coordinaten is niet gevonden. Of is niet te kiezen omdat het wit is.");
            return false;
        }
        ChairModel chair = _chairLogic.GetChairModel(chairID);
        AccountsLogic.TotaalPrijs += chair.Price;
        DateTime movieTime = MovieLogic.GetMovie(movieID).Time;
        string userEmail = "";
        if (AccountsLogic.CurrentAccount != null)
            userEmail = AccountsLogic.CurrentAccount.EmailAddress;

        if (CheckChairAvailability(chairID, movieTime, auditoriumID) == false)
        {
            Console.WriteLine("Deze stoel is al gereserveerd.");
            return false;
        }
        else
        {
            _chairReservation.Add(new ChairReservationModel(userEmail, chairID, movieID ,auditoriumID, movieTime, AccountsLogic.TotaalPrijs, AccountsLogic.CurrentReservationCode));

            ChairReservationAccess.WriteAll(_chairReservation);
            return true;
        }
    }

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

    public Tuple<ChairReservationModel,int> GetChairReservation(int codeOrID)
    {
        foreach (ChairReservationModel _reservation in _chairReservation)
        {
            if (_reservation.ID == codeOrID || _reservation.ReserveringsCode == codeOrID)
                return Tuple.Create(_reservation, _reservation.ID);
        }
        return null;
    }

    public int GetChairReservationIndex(int reservationID)
    {
        return _chairReservation.FindIndex(x => x.ID == reservationID);
    }

    public void UpdateChairReservationAtIndex(Tuple<ChairReservationModel,int> reservationInfo)
    {
        _chairReservation[reservationInfo.Item2] = reservationInfo.Item1;
    }

    public static void UpdateChairReservation()
    {
        ChairReservationLogic chairReservationLogic = new ChairReservationLogic();
        var chairReservationModel = chairReservationLogic.GetChairReservation(AccountsLogic.CurrentReservationCode);
        chairReservationModel.Item1.TotaalPrijs = AccountsLogic.TotaalPrijs;
        chairReservationModel.Item1.IsCompleted = true;
        int reservationIndex = chairReservationLogic.GetChairReservationIndex(chairReservationModel.Item2);
        chairReservationLogic.UpdateChairReservationAtIndex(Tuple.Create(chairReservationModel.Item1,reservationIndex));
        chairReservationLogic.RemoveNotCompletedReservations();
        chairReservationLogic.WriteAll();
        AccountsLogic.CurrentReservationCode = MailLogic.GenerateCode();
    }

    public void RemoveNotCompletedReservations()
    {
        List<ChairReservationModel> chairReservations = new List<ChairReservationModel>();
        foreach (ChairReservationModel _reservation in _chairReservation)
            if (_reservation.IsCompleted == true)
                chairReservations.Add(_reservation);

        _chairReservation = chairReservations;
    }
}
