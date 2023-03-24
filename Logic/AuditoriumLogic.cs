public class AuditoriumLogic
{
    protected int AuditoriumID;
    private ChairLogic chairLogic;
    public List<ChairModel> Chairs;
    public AuditoriumLogic(int auditoriumID)
    {
        AuditoriumID = auditoriumID;
    }
}