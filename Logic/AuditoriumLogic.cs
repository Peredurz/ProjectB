public class AuditoriumLogic
{
    public static int AuditoriumID;
    //private ChairLogic _chairLogic = new ChairLogic();
    private List<AuditoriumModel> _auditoriums = new List<AuditoriumModel>();
    // public List<ChairModel> Chairs = ChairAccess.LoadAll();
    public AuditoriumLogic(int auditoriumID)
    {
        _auditoriums = AuditoriumAccess.LoadAll();
        AuditoriumID = auditoriumID;
    }


    // public string ChairPrint()
    // {
    //     List<int> chairs = _auditoriums[AuditoriumID].Chairs;
    //     int length = _auditoriums[AuditoriumID].TotalCols;
    //     int pos = 1;
    //     string chairPrint = "";
    //     foreach (int id in chairs)
    //     {
    //         if (pos == length)
    //         {
    //             chairPrint += "\n";
    //             pos = 1;
    //         }
    //         ChairModel chair = _chairLogic.Chairs[id];
    //         if (chair.Status == Status.Available)
    //         {
    //             chairPrint += "# ";
    //         }
    //         else if (chair.Status == Status.Pending)
    //         {
    //             chairPrint += "? ";
    //         }
    //         else if (chair.Status == Status.Reserved)
    //         {
    //             chairPrint += "X ";
    //         }
    //         pos++;
    //     }
    //     return chairPrint;
    // }
}