public class UserLogin : IPresentation
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {

        bool loop = true;
        while (loop)
        {
            Console.WriteLine("Welcome to the login page");
            Console.WriteLine("L: Login");
            Console.WriteLine("N: New user?");
            Console.WriteLine("B: Back");
            Console.Write("> ");
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
                        Console.WriteLine("Welcome back " + acc.FullName);
                        Console.WriteLine("Your email number is " + acc.EmailAddress);

                        //Write some code to go back to the menu
                        //Menu.Start();
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