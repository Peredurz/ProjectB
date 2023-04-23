class Annulering
{
    private static readonly List<string> _allClearance = new List<string>() { "Manager", "Worker", "Customer" };
    private static readonly List<PresentationModel> _presentationModels = new()
    {
        new PresentationModel("R", "Annulering", _allClearance, true),
        new PresentationModel("B", "Terug", _allClearance, true),
    };
    public static void Start()
    {
        PresentationLogic.WriteMenu(_presentationModels, true);
        System.Console.WriteLine("");
    }
}