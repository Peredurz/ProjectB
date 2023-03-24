class AuditoriumLogic
{
    public static int AuditoriumID;
    private ChairLogic _chairLogic;
    private List<AuditoriumModel> _auditoriums;

    public AuditoriumLogic(int auditoriumID)
    {
        _auditoriums = AuditoriumAccess.LoadAll();
        AuditoriumID = auditoriumID;
        _chairLogic = new ChairLogic();
    }

    public void Write(List<AuditoriumModel> auditoriums)
    {
        AuditoriumAccess.WriteAll(auditoriums);
    }

    public string ChairPrint()
    {
        List<int> chairs = _auditoriums[--AuditoriumID].Chairs;
        int length = _auditoriums[AuditoriumID].TotalCols;
        int pos = 1;
        string chairPrint = "";
        for (int i = 0; i < chairs.Count; i++)
        {
            if (pos == length)
            {
                chairPrint += "\n";
                pos = 1;
            }
            ChairModel chair = _chairLogic.Chairs[i];
            if (chair.Status == Status.Available)
            {
                chairPrint += "# ";
            }
            else if (chair.Status == Status.Pending)
            {
                chairPrint += "? ";
            }
            else if (chair.Status == Status.Reserved)
            {
                chairPrint += "X ";
            }
            pos++;
        }
        return chairPrint;
    }
}