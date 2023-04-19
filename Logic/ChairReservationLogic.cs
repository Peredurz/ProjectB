using System;

class ChairReservationLogic
{
    private List<ChairReservationModel> _chairReservation = new List<ChairReservationModel>();

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

        DateTime movieTime = MovieLogic.GetMovie(movieID).Time;
        string userEmail = "";
        if (AccountsLogic.CurrentAccount != null)
            userEmail = AccountsLogic.CurrentAccount.EmailAddress;

        _chairReservation.Add(new ChairReservationModel(userEmail, chairID, auditoriumID, movieTime));

        ChairReservationAccess.WriteAll(_chairReservation);
        return true;
    }
}