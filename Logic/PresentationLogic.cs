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

	public static bool IsEmpty() => _presentations.Any() != true;

	public static List<PresentationModel> GetPresentationModels(bool inSubMenu)
	{
		return AccountsLogic.UserPresentationModels
		      .Where(x => x.IsSubMenu == inSubMenu && (CurrentPresentation == x.PresentationName || x.PresentationName == "all"))
		      .ToList();
	}

	public static string GetUserInputFromMenu(bool isSubMenu)
	{
        List<PresentationModel> presentations = GetPresentationModels(isSubMenu);
        int index = 0;
        string inputUser = "";

        ConsoleKeyInfo keyinfo;
        do
        {
            Console.Clear();
            PresentationLogic.WriteMenu(presentations, isSubMenu, presentations[index].Name);
            keyinfo = Console.ReadKey();

            if (keyinfo.Key == ConsoleKey.DownArrow)
            {
                if (index + 1 < presentations.Count)
                {
                    index++;
                }
            }
            if (keyinfo.Key == ConsoleKey.UpArrow)
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

		return inputUser;
	}
}
