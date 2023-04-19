public class UserLogin : IPresentation
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {

        PresentationLogic.WriteMenu(Menu.presentationModels, true);
        
        bool loop = true;
        while (loop)
        {
            string input = Console.ReadLine();
            switch (input.ToLower())
            {
                case "l":
                    Console.WriteLine("Email.");
                    string email = Console.ReadLine();
                    Console.WriteLine("Wachtwoord.");
                    string password = Console.ReadLine();
                    AccountModel acc = accountsLogic.CheckLogin(email, password);
                    if (acc != null)
                    {
                        AccountsLogic.CurrentAccount = acc;
                        Console.WriteLine("Ingelogd als: " + acc.FullName);
                        loop = false;
                    }
                    else
                    {
                        Console.WriteLine("Geen account gevonden met dit email adres en wachtwoord.");
                    }
                    break;
                case "n":
                    Console.WriteLine("Volledige naam ");
                    string fullName = Console.ReadLine();
                    Console.Write("> ");
                    Console.WriteLine("Email");
                    string emailAddress = Console.ReadLine();
                    Console.Write("> ");
                    Console.WriteLine("Wachtwoord");
                    string _password = Console.ReadLine();
                    accountsLogic.NewAccount(fullName, emailAddress, _password);
                    Console.WriteLine("Account succesvol aangemaakt. ");
                    loop = false;
                    break;
                case "b":
                    loop = false;
                    break;
                default:
                    Console.WriteLine("Verkeerde invoer.");
                    break;
            }
        }
        Menu.Start();
    }
}