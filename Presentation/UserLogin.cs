public class UserLogin : IPresentation
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {

        
        bool loop = true;
        while (loop)
        {
            PresentationLogic.WriteMenu(Menu.presentationModels, true);
            string input = Console.ReadLine();
            switch (input.ToLower())
            {
                case "l":
                    Console.WriteLine("Voer uw e-mail in");
                    Console.Write("> ");
                    string email = Console.ReadLine();
                    
                    Console.WriteLine("Voer uw wachtwoord in");
                    Console.Write("> ");
                    string password = Console.ReadLine();
                    
                    AccountModel acc = accountsLogic.CheckLogin(email, password);
                    if (acc != null)
                    {
                        AccountsLogic.CurrentAccount = acc;
                        Console.WriteLine("Welkom terug " + acc.FullName);
                        Console.WriteLine("Jou email is " + acc.EmailAddress);
                        loop = false;
                    }
                    else
                    {
                        Console.WriteLine("Geen account gevonden met dat e-mailadres en wachtwoord");
                    }
                    break;
                case "n":
                    Console.WriteLine("Voor-en achternaam");
                    Console.Write("> ");
                    string fullName = Console.ReadLine();
                    
                    Console.WriteLine("Email");
                    Console.Write("> ");
                    string emailAddress = Console.ReadLine();
                    
                    
                    Console.WriteLine("Wachtwoord");
                    Console.Write("> ");
                    string _password = Console.ReadLine();
                    accountsLogic.NewAccount(fullName, emailAddress, _password);
                    break;
                case "b":
                    loop = false;
                    break;
                default:
                    Console.WriteLine("Verkeerde invoer");
                    break;
            }
        }
        PresentationLogic.CurrentPresentation = "menu";
        Menu.Start();
    }
}