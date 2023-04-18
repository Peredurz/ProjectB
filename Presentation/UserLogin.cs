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
                    Console.WriteLine("Please enter your email address");
                    string email = Console.ReadLine();
                    Console.WriteLine("Please enter your password");
                    string password = Console.ReadLine();
                    AccountModel acc = accountsLogic.CheckLogin(email, password);
                    if (acc != null)
                    {
                        AccountsLogic.CurrentAccount = acc;
                        Console.WriteLine("Welcome back " + acc.FullName);
                        Console.WriteLine("Your email number is " + acc.EmailAddress);
                        loop = false;
                    }
                    else
                    {
                        Console.WriteLine("No account found with that email and password");
                    }
                    break;
                case "n":
                    Console.WriteLine("Full name");
                    string fullName = Console.ReadLine();
                    Console.Write("> ");
                    Console.WriteLine("Email");
                    string emailAddress = Console.ReadLine();
                    Console.Write("> ");
                    Console.WriteLine("Password");
                    string _password = Console.ReadLine();
                    accountsLogic.NewAccount(fullName, emailAddress, _password);
                    break;
                case "b":
                    loop = false;
                    break;
                default:
                    Console.WriteLine("Wrong input");
                    break;
            }
        }
        Menu.Start();
    }
}