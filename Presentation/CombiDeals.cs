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
            double total = 0;
            Console.WriteLine(@"Je kan de volgende Combideals kiezen.");
            _combiDeal.PrintCombiDeals();
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
                Console.WriteLine($"Saldo is {selectedCombiDeal.Price + total}.");
                Console.WriteLine();
                Console.WriteLine("Weet u zeker dat u deze combideal wilt Y/N. ");
                Console.Write("> ");
                string inputUser = Console.ReadLine().ToUpper();
                // bij de juiste handelingen gaat de gebruiker door naar betalingen
                if (inputUser == "Y")
                {
                    choice = true;
                    Auditorium.ChooseParkingTicket();
                    break;
                }
                // bij verkeerde keuze kan de gebruiker dit opniew doen.
                else if (inputUser == "N")
                {
                    total = selectedCombiDeal.Price - total;
                }
            }
            else
            {
                Console.WriteLine("Bestaat niet probeer het opnieuw.");
            }
            
             
        }
    }
}
