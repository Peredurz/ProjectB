class ChairLogic
{
    public List<ChairModel> Chairs;
    public ChairLogic()
    {
        Chairs = ChairAccess.LoadAll();
        // Chairs = new();
    }

    public void AddChair(ChairModel chair)
    {
        Chairs.Add(chair);
        ChairAccess.WriteAll(Chairs);
    }
}