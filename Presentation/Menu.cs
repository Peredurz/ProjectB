public class Menu : IPresentation
{
    private static AuditoriumLogic _auditoriumLogic = new AuditoriumLogic();

    // de clearances die op een menu item kunnen zitten dus dat betekent wie er wel en niet bij kan
    private static List<string> _allClearance = new List<string>() { "Manager", "Worker", "Customer" };
    private static List<string> _managerClearance = new List<string>() { "Manager" };
    private static List<string> _nonCustomerClearance = new List<string>() { "Manager", "Worker" };
    // definieer alle menu items die wij in onze applicatie hebben
    public static List<PresentationModel> presentationModels = new List<PresentationModel>()
    {
        new PresentationModel("L", "Login", _allClearance),
        new PresentationModel("I", "Info", _allClearance),
        new PresentationModel("M", "Films", _allClearance),
        new PresentationModel("Q", "Afsluiten", _allClearance),
        new PresentationModel("L", "Login", _allClearance, true),
        new PresentationModel("N", "Nieuwe Gebruiker", _allClearance, true),
        new PresentationModel("B", "Terug", _allClearance, true),
        new PresentationModel("A", "Film Toevoegen (Nog niet te gebruiken)", _managerClearance),
        new PresentationModel("R", "Anuleringen (Nog niet te gebruiken)", _nonCustomerClearance),
    };

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    public static void Start()
    {
        // maak de stoelen voor de zalen
        if (ChairAccess.LoadAll().Count == 0)
            _auditoriumLogic.InitializeSeats();
        // geef deze aan de presentation logic om daar verder mee te kunnen werken
        if (PresentationLogic.IsEmpty() == true)
            PresentationLogic.SetPresentations(presentationModels);

        if (AccountsLogic.CurrentAccount != null)
        {
            // als een gebruiker is ingelogd kunnen de opties aangepast worden op basis van de type gebruiker
            PresentationLogic.WriteMenu(PresentationLogic.GetUserOptions(AccountsLogic.CurrentAccount.UserType), false);
        }
        else
        {
            // de basis menu opties zijn die van een customer.
            PresentationLogic.WriteMenu(PresentationLogic.GetUserOptions("Customer"), false);
        }

        string inputUser = Console.ReadLine().ToUpper();
        if (inputUser == "L")
        {
            UserLogin.Start();
        }
        else if (inputUser == "I")
        {

            Console.WriteLine(@"
            Telefoon nummer:   010 123 123 12.
            adres:            Wijnhaven 107.
            postcode:           3011 WN in Rotterdam.
            Openings tijd:      Wij zijn dertig minuten voor de eerste film geopend 
                                De bioscoop sluit vijftien minuten na de laatste film");
            Menu.Start();
        }
        else if (inputUser == "M")
        {
            Console.WriteLine("");
            Movie.Start();

            // terug naar menu start
            Menu.Start();
        }
        else if (inputUser == "Q")
        {

            Console.WriteLine("Thanks for visiting us.");
        }
        else
        {
            Console.WriteLine("Invalid input");
            Menu.Start();
        }

    }
}
