public class Payment : IPresentation
{
    public static void Start()
    {
        PresentationLogic.CurrentPresentation = "payment";
        Console.WriteLine($"Het totaal bedrag is {Math.Round(AccountsLogic.TotaalPrijs, 2)}");
        PresentationLogic.WriteMenu(AccountsLogic.UserPresentationModels, true);
        string chosenOption = Console.ReadLine().ToLower();
        switch (chosenOption)
        {
            case "i":
                Console.WriteLine("Vul hier jou Iban in.");
                Console.Write(">");
                string userIBAN = Console.ReadLine();
                bool validIBAN = PaymentLogic.ValidateIBAN(userIBAN);
                if (validIBAN == true)
                {
                    Console.WriteLine("Betaling voltooid");
                    MailLogic.GenerateQRCode();
                    MailLogic.SendMail();
                    ChairReservationLogic.UpdateChairReservation();
                    Menu.Start();
                    return;
                }
                else
                {
                    for (int i = 1; i < 3; i++)
                    {
                        Console.WriteLine("Invalid invoer");
                        Console.Write(">");
                        string userIBAN1 = Console.ReadLine();
                        bool checkIBAN = PaymentLogic.ValidateIBAN(userIBAN1);
                        if (checkIBAN == true)
                        {
                            Console.WriteLine("Betaling voltooid");
                            MailLogic.GenerateQRCode();
                            MailLogic.SendMail();
                            ChairReservationLogic.UpdateChairReservation();
                            Menu.Start();
                            return;
                        }
                    }
                    Console.WriteLine("U heeft het te vaak verkeerd gedaan.\n Reservering Gannuleerd");
                    Menu.Start();
                    return;
                }
                break;

            case "b":
                Movie.Start();
                return;
        }
    }
}