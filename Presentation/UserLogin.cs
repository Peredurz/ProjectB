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
            string input = PresentationLogic.GetUserInputFromMenu(true);
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
                        PresentationLogic.CurrentMessage = "Welkom terug " + acc.FullName + "\nJouw email is " + acc.EmailAddress; 
                        loop = false;
                    }
                    else
                    {
                        Console.WriteLine("Geen account gevonden met dat e-mailadres en wachtwoord");
                        PresentationLogic.CurrentMessage = "Geen account gevonden met dat e-mailadres en wachtwoord";
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
                            PresentationLogic.CurrentMessage = "Email is niet geldig";
                            continue;
                        }
                        else
                        {
                            loop2 = false;
                        }
                    }
                    Console.WriteLine("Wachtwoord");
                    Console.Write("> ");

                    var _password = string.Empty;
                    ConsoleKey k1;
                    do
                    {
                        var keyInfo = Console.ReadKey(intercept: true);
                        k1 = keyInfo.Key;

                        if (k1 == ConsoleKey.Backspace && _password.Length > 0)
                        {
                            Console.Write("\b \b");
                            password = _password[0..^1];
                        }
                        else if (!char.IsControl(keyInfo.KeyChar))
                        {
                            Console.Write("*");
                            _password += keyInfo.KeyChar;
                        }
                    } while (k1 != ConsoleKey.Enter);

                    Console.WriteLine("\nHerhaal het wactwoord.");
                    Console.Write("> ");
                    var _password2 = string.Empty;
                    ConsoleKey k2 = 0;
                    do
                    {
                        var keyInfo = Console.ReadKey(intercept: true);
                        k2 = keyInfo.Key;

                        if (k2 == ConsoleKey.Backspace && _password2.Length > 0)
                        {
                            Console.Write("\b \b");
                            _password2 = _password2[0..^1];
                        }
                        else if (!char.IsControl(keyInfo.KeyChar))
                        {
                            Console.Write("*");
                            _password2 += keyInfo.KeyChar;
                        }
                    } while (k2 != ConsoleKey.Enter);

                    if (_password != _password2)
                    {
                        Console.WriteLine("\nWachtwoorden komen niet overeen\n");
                        PresentationLogic.CurrentMessage = "Wachtwoorden komen niet overeen";
                        break;
                    }
                    Console.WriteLine();
                    accountsLogic.NewAccount(fullName, emailAddress, _password);
                    break;
                case "f":
                    loop = false;
                    Console.WriteLine("Met deze keuze gaat u het wachtwoord van uw account resetten. Weet u dit zeker? (y/n)");
                    bool valid = false;
                    while (valid == false)
                    {
                        Console.Write("> ");
                        string pwChoice = Console.ReadLine().ToLower();
                        if (pwChoice == "y")
                        {
                            accountsLogic.GenerateTempPassword();
                            valid = true;
                        }
                        else if (pwChoice == "n")
                        {
                            Console.WriteLine("U gaat terug naar het inlogscherm.");
                            valid = true;
                        }
                        else
                        {
                            Console.WriteLine("Verkeerde invoer");
                        }
                    }
                    break;
                case "b":
                    loop = false;
                    break;
                case "a":
                    if (AccountsLogic.CurrentAccount != null)
                    {
                        Console.WriteLine("Huidige Wachtwoord");
                        Console.Write("> ");

                        var _password3 = string.Empty;
                        ConsoleKey k3;
                        do
                        {
                            var keyInfo = Console.ReadKey(intercept: true);
                            k3 = keyInfo.Key;

                            if (k3 == ConsoleKey.Backspace && _password3.Length > 0)
                            {
                                Console.Write("\b \b");
                                _password3 = _password3[0..^1];
                            }
                            else if (!char.IsControl(keyInfo.KeyChar))
                            {
                                Console.Write("*");
                                _password3 += keyInfo.KeyChar;
                            }
                        } while (k3 != ConsoleKey.Enter);

                        accountsLogic.ChangePassword(AccountsLogic.CurrentAccount.EmailAddress, _password3);
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
                            PresentationLogic.CurrentMessage = "U bent uitgelogd";
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
                        Console.WriteLine("Druk een toets om verder te gaan");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("U moet ingelogd zijn om uw geschiednis te kunnen zien.");
                        PresentationLogic.CurrentMessage = "U moet ingelogd zijn om uw geschiedenis te kunnen zien.";
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