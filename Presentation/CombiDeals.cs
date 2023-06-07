


class CombiDeal : IPresentation
{
    private static List<CombiDeal> _productList = new List<CombiDeal>();
    private static CombiDealLogic _combiDeal = new CombiDealLogic();
    // Laat de gebruiker de opties zien waar uit hin kan kiezen
    public static void Start()
    {
        bool choice = false;
        while (choice == false)
        {
            // laat de gebruiker alle combideals zien en kiezen.
            Console.WriteLine(@"Je kan de volgende Combideals kiezen.");
            _combiDeal.PrintCombiDeals();
            try
            {
                Console.Write("> ");
                int idUser = Convert.ToInt32(Console.ReadLine());
                
                // controleren of de combideal bestaat
                bool valid = _combiDeal.ExistCombiDeal(idUser);
                if (valid == true)
                // laat de gebruiker zien wat die heeft gekozen en vraagt een bevestiging.
                {
                    Console.WriteLine("U heeft gekozen Voor");
                    CombiDealModel selectedCombiDeal = _combiDeal.GetCombiDeal(idUser);
                    _combiDeal.ShowCombiDeal(idUser);
            
                    Console.WriteLine();
                    Console.WriteLine($"Saldo is {Math.Round(AccountsLogic.TotaalPrijs + selectedCombiDeal.Price,2)}.");
                    Console.WriteLine();
                    Console.WriteLine("Weet u zeker dat u deze combideal wilt Y/N. ");
                    Console.Write("> ");
                
                    string inputUser = Console.ReadLine().ToUpper();
                    // bij de juiste handelingen gaat de gebruiker door naar betalingen
                    if (inputUser == "Y")
                    {
                        AccountsLogic.TotaalPrijs += selectedCombiDeal.Price;
                        choice = true;
                        Auditorium.ChooseParkingTicket();
                        break;
                    }
                    // bij verkeerde keuze kan de gebruiker dit opniew doen.
                    else if (inputUser == "N")
                    {
                        CombiDeal.Start();
                    }
                    else
                    {
                        Console.WriteLine("Verkeerde invoer probeer het opnieuw. ");
                        CombiDeal.Start();
                    }
                }            
            }
            catch
            {
                Console.WriteLine("Verkeerde invoer probeer het opnieuw. ");
            }             
        }
    }
    public static void StartManager()
    {
        bool askUser = false;
        while(askUser == false)
        {
            PresentationLogic.CurrentPresentation = "combideals";
            PresentationLogic.WriteMenu(AccountsLogic.UserPresentationModels, true);
            string inputUser = Console.ReadLine().ToLower();

            switch(inputUser)
            {
                case "a":

                    Console.WriteLine("Product beschrijving. ");
                    Console.Write("> ");
                    string productDescription = Console.ReadLine();
                    
                    Console.WriteLine("Product prijs. ");
                    Console.Write("> ");
                    double priceDescription = Convert.ToDouble(Console.ReadLine());
                    bool add = _combiDeal.AddCombi(productDescription, priceDescription);
                    if (add == true)
                        {
                            Console.WriteLine("Succesvol toegevoegd.");
                        }
                    break;
                case "r":
                    _combiDeal.PrintCombiDeals();
                    try
                    {
                        Console.WriteLine("ID van de Combideal die u wilt verwijderen.  ");
                        Console.Write("> ");
                        int idUser = Convert.ToInt32(Console.ReadLine());
                        _combiDeal.ShowCombiDeal(idUser);
                        bool delete = _combiDeal.DeleteCombi(idUser);
                        if (delete == true)
                        {
                            Console.WriteLine("Succesvol Verwijderd.");
                        }
                        else if (delete = false)
                        {
                            Console.WriteLine("ID niet gevonden.");
                        }
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("geen geldige invoer. ");
                    }
                    break;
                case "e":
                    _combiDeal.PrintCombiDeals();
                    Console.WriteLine("ID van de Combideal die u wilt bewerken.  ");
                    Console.Write("> ");
                    try
                    {
                        int id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Product beschrijving. ");
                        Console.Write("> ");
                        string newProductDescription = Console.ReadLine();

                        Console.WriteLine("Product prijs. ");
                        Console.Write("> ");
                        double newPriceDescription = Convert.ToDouble(Console.ReadLine());
                        bool edit = _combiDeal.EditCombi(id, newProductDescription, newPriceDescription);
                        if (edit == true)
                        {
                            Console.WriteLine("Succesvol bewerkt.");
                        }
                        else if (edit = false)
                        {
                            Console.WriteLine("ID niet gevonden.");
                        }
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("Geen geldige invoer");
                    }
                    break;
                case "o":
                    _combiDeal.PrintCombiDeals();
                    break;
                case "b": 
                    askUser = true;
                    Menu.Start();
                    return;
                default:
                    Console.WriteLine("Verkeerde invoer");
                    Menu.Start();
                    return;
            }
        }
    }
}
