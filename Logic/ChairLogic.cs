public class ChairLogic
{
    private List<ChairModel> Chairs;
    public ChairLogic()
    {
        Chairs = ChairAccess.LoadAll();
    }
}