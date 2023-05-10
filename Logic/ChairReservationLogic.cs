using System;

class ChairReservationLogic
{
    private List<ChairReservationModel> _chairReservation = new List<ChairReservationModel>();
    private ChairLogic _chairLogic = new ChairLogic();

    public ChairReservationLogic()
    {
        _chairReservation = ChairReservationAccess.LoadAll();
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
            _chairReservation.Add(new ChairReservationModel(userEmail, chairID, auditoriumID, movieTime, AccountsLogic.TotaalPrijs, MailLogic.GenerateCode()));

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
}
