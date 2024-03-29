
public class Menu : IPresentation
{
    private static AuditoriumLogic _auditoriumLogic = new AuditoriumLogic();

    // de clearances die op een menu item kunnen zitten dus dat betekent wie er wel en niet bij kan
    private static List<string> _allClearance = new List<string>() { "Manager", "Worker", "Customer" };
    private static List<string> _managerClearance = new List<string>() { "Manager" };
    private static List<string> _nonCustomerClearance = new List<string>() { "Manager", "Worker" };
    private static ChairAccess ChairAccess = new ChairAccess();
    // definieer alle menu items die wij in onze applicatie hebben
    public static List<PresentationModel> presentationModels = new List<PresentationModel>()
    {
        new PresentationModel("L", "Login", _allClearance, "menu"),
        new PresentationModel("I", "Info", _allClearance, "menu"),
        new PresentationModel("M", "Films", _allClearance, "menu"),
        new PresentationModel("A", "Filteren", _allClearance, "movies", true),
        new PresentationModel("R", "Annulering", _allClearance, "menu"),
        new PresentationModel("A", "Film Toevoegen of aanpassen", _managerClearance, "menu"),
        new PresentationModel("C", "Combideal", _managerClearance, "menu"),
        new PresentationModel("A", "Combideal toevoegen", _managerClearance, "combideals", true),
        new PresentationModel("E", "Combideal bewerken", _managerClearance, "combideals", true),
        new PresentationModel("R", "Combideal verwijderen", _managerClearance, "combideals", true),
        new PresentationModel("O", "Combideal overzicht", _managerClearance, "combideals", true),
        new PresentationModel("Q", "Afsluiten", _allClearance, "menu"),
        new PresentationModel("L", "Login", _allClearance, "login", true),
        new PresentationModel("A", "Niew wachtwoord", _allClearance, "loginSub", true),
        new PresentationModel("O", "Loguit", _allClearance, "loginSub", true),
        new PresentationModel("N", "Nieuwe Gebruiker", _allClearance, "login", true),
        new PresentationModel("F", "Wachtwoord vergeten", _allClearance, "login", true),
        new PresentationModel("A", "Film toevoegen", _managerClearance, "movie_editor", true),
        new PresentationModel("R", "Films aanpassen", _managerClearance, "movie_editor", true),
        new PresentationModel("D", "Films verwijderen", _managerClearance, "movie_editor", true),
        new PresentationModel("F", "Toekomstige Films", _allClearance, "movies", true),
        new PresentationModel("H", "Aankoop geschiedenis", _allClearance, "loginSub", true),
        new PresentationModel("C", "Huidige films", _allClearance, "movies", true),
        new PresentationModel("S", "Stoel kiezen", _allClearance, "auditorium", true),
        new PresentationModel("R", "Annulering", _allClearance, "annulering", true),
        new PresentationModel("O", "Overzicht", _nonCustomerClearance, "annulering", true),
        new PresentationModel("I", "betalen met IDeal", _allClearance, "payment", true),
        new PresentationModel("A", "AuditoriumID", _managerClearance, "editor_submenu", true),
        new PresentationModel("E", "Tijd van hoe lang de fim duurt", _managerClearance, "editor_submenu", true),
        new PresentationModel("C", "Tijd van wanneer de film begint", _managerClearance, "editor_submenu", true),
        new PresentationModel("D", "Beschrijving", _managerClearance, "editor_submenu", true),
        new PresentationModel("T", "Titel", _managerClearance, "editor_submenu", true),
        new PresentationModel("G", "Genre", _managerClearance, "editor_submenu", true),
        new PresentationModel("L", "Leeftijdsgrens", _managerClearance, "editor_submenu", true),
        new PresentationModel("O", "Overkopiëren", _managerClearance, "editor_movieAdd", true),
        new PresentationModel("N", "Nieuwe film toevoegen", _managerClearance, "editor_movieAdd", true),
        new PresentationModel("C", "Verder", _allClearance, "movie_submenu", true),
        new PresentationModel("B", "Terug", _allClearance, "all", true),
    };


    /// <summary>
    /// Het begin van het menu, deze gaat naar allerlei submenu's met verschillende functionaliteiten.
    /// </summary>
    public static void Start()
    {
        // Aan het begin van de start method aangeven in welk menu je zit.
        PresentationLogic.CurrentPresentation = "menu";
        // maak de stoelen voor de zalen
        if (ChairAccess.LoadAll().Count == 0)
            _auditoriumLogic.InitializeSeats();
        // geef deze aan de presentation logic om daar verder mee te kunnen werken
        if (PresentationLogic.IsEmpty() == true)
            PresentationLogic.SetPresentations(presentationModels);

        if (AccountsLogic.CurrentAccount != null)
        {
            AccountsLogic.UserPresentationModels = PresentationLogic.GetUserOptions(AccountsLogic.CurrentAccount.UserType);
            // als een gebruiker is ingelogd kunnen de opties aangepast worden op basis van de type gebruiker
            PresentationLogic.WriteMenu(AccountsLogic.UserPresentationModels, false);
        }
        else
        {
            AccountsLogic.UserPresentationModels = PresentationLogic.GetUserOptions("Customer");
            // de basis menu opties zijn die van een customer.
            PresentationLogic.WriteMenu(AccountsLogic.UserPresentationModels, false);
        }

        string inputUser = PresentationLogic.GetUserInputFromMenu(false);
        if (inputUser == "L")
        {
            UserLogin.Start();
        }
        else if (inputUser == "I")
        {

            Console.WriteLine(@"
            Telefoon nummer:    010 123 123 12.
            adres:              Wijnhaven 107.
            postcode:           3011 WN in Rotterdam.
            Openings tijd:      Wij zijn dertig minuten voor de eerste film geopend 
                                De bioscoop sluit vijftien minuten na de laatste film
            Je kan enter of b drukken om verder te gaan.");
            ConsoleKeyInfo keyinfo = Console.ReadKey();
            if (keyinfo.Key == ConsoleKey.B || keyinfo.Key == ConsoleKey.Enter)
                Menu.Start();
        }
        else if (inputUser == "M")
        {
            Console.WriteLine("");
            Movie.Start();
        }
        else if (inputUser == "R")
        {
            Annulering.Start();
        }
        else if (inputUser == "C")
        {
            CombiDeal.StartManager();
        }
        else if (inputUser == "A")
        {
            MovieAmendment.Start();
        }
        else if (inputUser == "Q")
        {
            Console.WriteLine("Bedankt voor uw bezoek.");
        }
        else
        {
            Console.WriteLine("Geen geldig invoer.");
            Menu.Start();
        }

    }

    /// <summary>
    /// Functie om een message te printen boven het menu.
    /// </summary>
    public static void PrintCurrentMessage()
    {
        for (int i = 0; i < 2; i++)
        {
            Console.Write("+");
            for (int j = 0; j < PresentationLogic.CurrentMessage.Length; j++)
            {
                Console.Write("-");
            }
            Console.Write("+\n");
            if (i == 0)
                Console.Write($"{PresentationLogic.CurrentMessage}\n");
        }
    }
}
