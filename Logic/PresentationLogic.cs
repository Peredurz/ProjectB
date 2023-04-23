public class PresentationLogic
{
    private static List<PresentationModel> _presentations = new List<PresentationModel>();

    public static void SetPresentations(List<PresentationModel> presentations)
    {
        foreach (PresentationModel _presentation in presentations)
            _presentations.Add(_presentation);
    }

    public static List<PresentationModel> GetUserOptions(string userType)
    {
        List<PresentationModel> userMenuOptions = new List<PresentationModel>();

        foreach (PresentationModel _presentation in _presentations)
        {
            // voeg alleen het menu toe aan de opties voor de gebruiker als die de clearance heeft.
            _presentation.UserClearance.ForEach(x => { if (x == userType) userMenuOptions.Add(_presentation); });
        }
        return userMenuOptions;
    }

    public static void WriteMenu(List<PresentationModel> presentations, bool inSubMenu)
    {
        // als je in een submenu ziet zoals het login ding toon dan alleen die menu items
        presentations = presentations.Where(x => x.IsSubMenu == inSubMenu).ToList();

        foreach (PresentationModel _presentation in presentations)
        {
            Console.WriteLine($"{_presentation.Name}: {_presentation.Description}");
        }

        Console.Write("> ");
    }

    public static bool IsEmpty() => _presentations.Any() != true;
}