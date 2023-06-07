public class Payment : IPresentation
{
    public static void Start()
    {
        PresentationLogic.CurrentPresentation = "payment";
        Console.WriteLine($"Het totaal bedrag is {AccountsLogic.TotaalPrijs}");
        PresentationLogic.WriteMenu(AccountsLogic.UserPresentationModels, true);
        string chosenOption = Console.ReadLine().ToLower();
        switch (chosenOption)
        {
            case "i":
                Console.WriteLine("Vul hier jou Iban in.");
                Console.Write(">");
                string UserIBAN = Console.ReadLine();
                bool validIBAN = PaymentLogic.ValidateIBAN(UserIBAN);
                if (validIBAN == true)
                {
                    Console.WriteLine("Betaling voltooid");
                    MailLogic.GenerateQRCode();
                    MailLogic.SendMail();
                    ChairReservationLogic.UpdateChairReservation();
                    Menu.Start();
                }
                else if (validIBAN == false)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine("Invalid invoer");
                        bool checkIBAN = PaymentLogic.ValidateIBAN(UserIBAN);
                        if (checkIBAN == true)
                        {
                            Console.WriteLine("Betaling voltooid");
                            MailLogic.GenerateQRCode();
                            MailLogic.SendMail();
                            ChairReservationLogic.UpdateChairReservation();
                            Menu.Start();
                        }
                        Console.WriteLine("U heeft het te vaak verkeerd gedaan.\n Reservering Gannuleerd");
                        Menu.Start();
                    }
                }
                break;
            
            case "p":
                Console.WriteLine("Niet beschikbaar");
                break;
            
            case "c":
                Console.WriteLine("Niet beschikbaar");
                break;
        }
    }
}