using System.Runtime.InteropServices;
/// <summary>
/// Dit is de <c>PresentationLogic</c> class, deze class zorgt er voor dat menu's goed worden opgebouwd.
/// <example>
/// Hoe je een menu item toevoegd, dit doe je in <c>Menu</c> hier is een lijst met <c>PresentationModels</c>
/// daar voeg je dan een nieuw item aan toe zoals:
/// <code>
/// new PresentationModel(naam, beschrijving, welke clearance, presentation naam, bool voor submenu of niet),
/// </code>
/// Zorg er voor dat als je in een nieuwe presentation komt dat je dit doet:
/// <code>
/// PresentationLogic.CurrentPresentation = "movie";
/// </code>
/// dan als je <code> PresentationLogic.WriteMenu(Menu.presentationModels, true|false); </code> aanroept
/// worden alleen de menu items van movie getoont.
/// Als je een nieuwe presentation aanmaakt zorg dan dat deze de interface IPresentation overneemt, die
/// class moet dan Start() implementeren, in deze start schrijf je het menu, maar maar je ook een "state machine",
/// dit kan heel simpel door gewoon een opties aan een switch/if statement te doen. (Tip: kijk naar de andere presentation Start() methods).
/// </example>
/// </summary>
public class PresentationLogic
{
    private static List<PresentationModel> _presentations = new List<PresentationModel>();
    public static string CurrentPresentation = "menu";
    public static string CurrentMessage = "Welkom bij onze bioscoop!";

    /// <summary>
    /// Om de presentations te maken die uiteindelijk aan de gebruiker gekoppeld worden.
    /// </summary>
    public static void SetPresentations(List<PresentationModel> presentations)
    {
        foreach (PresentationModel _presentation in presentations)
            _presentations.Add(_presentation);
    }

    /// <summary>
    /// Deze method maakt een lijst van PresentationModels die gekoppeld zijn aan de clearance van de gebruiker, dus een manager
    /// ziet alleen degene die hij als clearance heeft. En dat is ook voor de medewerker en klant.
    /// </summary>
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

    /// <summary>
    /// Om het menu te schrijven, je moet ook aangeven of dit in een submenu is of niet.
    /// </summary>
    public static void WriteMenu(List<PresentationModel> presentations, bool inSubMenu, string menuName = null)
    {
        // als je in een submenu ziet zoals het login ding toon dan alleen die menu items
        presentations = GetPresentationModels(inSubMenu);

        foreach (PresentationModel _presentation in presentations)
        {
            if (menuName == _presentation.Name)
            {
                Console.WriteLine($"> {_presentation.Name}: {_presentation.Description}");
            }
            else
            {
                Console.WriteLine($"{_presentation.Name}: {_presentation.Description}");
            }
        }
    }

    /// <summary>
    /// Om te kijken of de list van PresentationModels leeg is.
    /// </summary>
    public static bool IsEmpty() => _presentations.Any() != true;

    /// <summary>
    /// Om een lijst van PresentationModels te krijgen die gekoppeld zijn aan de gebruiker en ook aan het menu.
    /// </summary>
    public static List<PresentationModel> GetPresentationModels(bool inSubMenu)
    {
        return AccountsLogic.UserPresentationModels
              .Where(x => x.IsSubMenu == inSubMenu && (CurrentPresentation == x.PresentationName || x.PresentationName == "all"))
              .ToList();
    }

    /// <summary>
    /// Om de input te krijgen van de gebruiker die die krijgt via de pijltjes (of vi keys :) ).
    /// </summary>
    public static string GetUserInputFromMenu(bool isSubMenu)
    {
        Console.CursorVisible = false;
        List<PresentationModel> presentations = GetPresentationModels(isSubMenu);
        int index = 0;
        string inputUser = "";

        ConsoleKeyInfo keyinfo;
        do
        {
            if (CurrentPresentation != "auditorium")
            {
                Console.Clear();
            }
            else
            {
                ClearConsoleLines();
            }
            Menu.PrintCurrentMessage();
            PresentationLogic.WriteMenu(presentations, isSubMenu, presentations[index].Name);
            keyinfo = Console.ReadKey(true);

            if (keyinfo.Key == ConsoleKey.DownArrow || keyinfo.Key == ConsoleKey.J)
            {
                if (index + 1 < presentations.Count)
                {
                    index++;
                }
            }
            if (keyinfo.Key == ConsoleKey.UpArrow || keyinfo.Key == ConsoleKey.K)
            {
                if (index - 1 >= 0)
                {
                    index--;
                }
            }
            if (keyinfo.Key == ConsoleKey.Enter)
            {
                inputUser = presentations[index].Name.ToUpper();
            }
        }
        while (keyinfo.Key != ConsoleKey.Enter);

        Console.CursorVisible = true;
        return inputUser;
    }

    /// <summary>
    /// Om console lines te clearen aan de onderkant van het scherm op basis van je platform is dit anders.
    /// </summary>
    public static void ClearConsoleLines()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ClearConsoleLineWindows();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            ClearConsoleLineUnix();
        }
        else
        {
            throw new NotSupportedException("Unsupported operating system.");
        }
    }

    /// <summary>
    /// Clear lines aan de onderkant op Windows.
    /// </summary>
    private static void ClearConsoleLineWindows()
    {
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write(new string(' ', Console.BufferWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write(new string(' ', Console.BufferWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write(new string(' ', Console.BufferWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write(new string(' ', Console.BufferWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 1);
    }

    /// <summary>
    /// Clear lines aan de onderkant op unix.
    /// </summary>
    private static void ClearConsoleLineUnix()
    {
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write("\u001b[2K"); // Clear the entire line
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write("\u001b[2K"); // Clear the entire line
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write("\u001b[2K"); // Clear the entire line
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        Console.Write("\u001b[2K"); // Clear the entire line
        Console.SetCursorPosition(0, Console.CursorTop - 1);
    }
}
