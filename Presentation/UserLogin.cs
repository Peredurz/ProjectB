public class UserLogin : IPresentation
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        if (AccountsLogic.CurrentAccount != null)
        {
            PresentationLogic.CurrentPresentation = "loginSub";
        }
        else
        {
            PresentationLogic.CurrentPresentation = "login";
        }

        bool loop = true;
        while (loop)
        {
            PresentationLogic.WriteMenu(AccountsLogic.UserPresentationModels, true);
            string input = Console.ReadLine();
            switch (input.ToLower())
            {
                case "l":
                    Console.WriteLine("Voer uw e-mail in");
                    Console.Write("> ");
                    string email = Console.ReadLine();

                    Console.WriteLine("Voer uw wachtwoord in");
                    Console.Write("> ");

                    // Om het wachtwoord niet te laten zien op het scherm
                    var password = string.Empty;
                    ConsoleKey key;
                    do
                    {
                        var keyInfo = Console.ReadKey(intercept: true);
                        key = keyInfo.Key;

                        if (key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            Console.Write("\b \b");
                            password = password[0..^1];
                        }
                        else if (!char.IsControl(keyInfo.KeyChar))
                        {
                            Console.Write("*");
                            password += keyInfo.KeyChar;
                        }
                    } while (key != ConsoleKey.Enter);
                    Console.WriteLine();

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
                    bool loop2 = true;
                    string emailAddress = "";
                    while (loop2)
                    {
                        Console.Write("> ");
                        emailAddress = Console.ReadLine();
                        if (!MailLogic.ValidateMailAddress(emailAddress))
                        {
                            Console.WriteLine("Email is niet geldig");
                            continue;
                        }
                        else
                        {
                            loop2 = false;
                        }
                    }
                    Console.WriteLine("Wachtwoord");
                    Console.Write("> ");
                    string _password = Console.ReadLine();
                    accountsLogic.NewAccount(fullName, emailAddress, _password);
                    break;
                case "f":
                    loop = false;
                    accountsLogic.GenerateTempPassword();
                    break;
                case "b":
                    loop = false;
                    break;
                case "a":
                    if (AccountsLogic.CurrentAccount != null)
                    {
                    Console.WriteLine("Huidige Wachtwoord");
                    Console.Write("> ");
                    string passwordCheck = Console.ReadLine();
                
                    accountsLogic.ChangePassword(AccountsLogic.CurrentAccount.EmailAddress,passwordCheck);
                    }
                    else
                    {
                        Console.WriteLine("Verkeerde invoer");
                    }
                    break;
                case "o": 
                    if (AccountsLogic.CurrentAccount != null)
                    {
                         Console.WriteLine("Weet u zeker dat u wilt uitloggen. (Y/N)");
                         Console.Write("> ");
                        string inputUser = Console.ReadLine().ToLower();
                        if (inputUser == "y")
                        {
                            AccountsLogic.CurrentAccount = null;
                            Console.WriteLine("Je bent uitgelogd.");
                        }
                        else
                        {
                            Console.WriteLine("Verkeerde invoer");
                        }
                    }
                    break;
                case "h": 
                    if (AccountsLogic.CurrentAccount != null)
                    {
                        Console.WriteLine("Uw aankoop geschiedenis:");
                        Console.WriteLine(accountsLogic.AankoopGeschiedenis());
                    }
                    else
                    {
                        Console.WriteLine("U moet ingelogd zijn om uw geschiednis te kunnen zien.");
                    }
                    break;
                default:
                    Console.WriteLine("Verkeerde invoer");
                    break;
            }
        }
        Menu.Start();
    }
}