public class ChairLogic
{
    public List<ChairModel> Chairs;
    public ChairLogic()
    {
        Chairs = ChairAccess.LoadAll();
    }
}