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
                bool validIBAN;
                if (userIBAN.Length <= 0)
                    validIBAN = false;
                else
                    validIBAN = PaymentLogic.ValidateIBAN(userIBAN);
                if (validIBAN == true)
                {
                    Console.WriteLine("Betaling voltooid");
                    PresentationLogic.CurrentMessage = "Betaling voltooid";
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
                        if (userIBAN1.Length <= 0)
                            continue;
                        bool checkIBAN = PaymentLogic.ValidateIBAN(userIBAN1);
                        if (checkIBAN == true)
                        {
                            Console.WriteLine("Betaling voltooid");
                            PresentationLogic.CurrentMessage = "Betaling voltooid";
                            MailLogic.GenerateQRCode();
                            MailLogic.SendMail();
                            ChairReservationLogic.UpdateChairReservation();
                            Menu.Start();
                            return;
                        }
                    }
                    ChairReservationLogic chairReservationLogic = new ChairReservationLogic();
                    chairReservationLogic.RemoveNotCompletedReservations();
                    chairReservationLogic.WriteAll();
                    AccountsLogic.CurrentReservationCode = MailLogic.GenerateCode();
                    AccountsLogic.ChosenChairs.Clear();
                    AccountsLogic.TotaalPrijs = 0;
                    Console.WriteLine("U heeft het te vaak verkeerd gedaan.\n Reservering Gannuleerd");
                    PresentationLogic.CurrentMessage = "U heeft het te vaak verkeerd gedaan.\n Reservering Gannuleerd";
                    Menu.Start();
                    return;
                }
            case "b":
                Movie.Start();
                return;
        }
    }
}