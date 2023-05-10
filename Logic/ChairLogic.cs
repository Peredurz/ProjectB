class ChairLogic
{
    public List<ChairModel> Chairs = new List<ChairModel>();

    public ChairLogic()
    {
        Chairs = ChairAccess.LoadAll();
    }

    public void AddChair(ChairModel chair)
    {
        Chairs.Add(chair);
        ChairAccess.WriteAll(Chairs);
    }

    public static int FindChairID(int row, int col, int auditoriumID)
    {
        // maak een lijst met alle stoelen die we kunnen bekijken.
        List<ChairModel> chairs = new List<ChairModel>();
        chairs = ChairAccess.LoadAll();

        // haal de ids van de stoelen op die je nu in de zaal heb
        List<int> chairIDs = AuditoriumLogic.GetChairIDs(auditoriumID);
        // bepaal een range om in te zoeken in de chairs lijst
        int minID = chairIDs.Min();
        int maxID = chairIDs.Max();

        // loop door de lijst heen waar we dan zoeken naar de stoel, als die gevonden is 
        // krijg je het id terug anders 0 om daarvoor te kunnen checken
        for (int i = minID - 1; i < maxID; i++)
        {
            ChairModel currentChair = chairs[i];
            // de stoel kan niet wit zijn en moet overeenkomen met de stoel kolom en rij
            if (currentChair.Row == row && currentChair.Column == col - 1 && currentChair.Color != "White")
                return currentChair.ID;
        }
        return 0;
    }

    public ChairModel GetChairModel(int chairID)
    {
        foreach (ChairModel chair in Chairs)
        {
            if (chairID == chair.ID)
            {
                return chair;
            }
        }
        return null;
    }
}