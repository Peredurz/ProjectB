using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BCrypt.Net;

/// <summary>
/// This class is not static so later on we can use inheritance and interfaces
/// </summary>
public class AccountsLogic
{
    private List<AccountModel> _accounts;

    public static AccountModel? CurrentAccount { get; set; } = null;
    public static List<PresentationModel> UserPresentationModels = new List<PresentationModel>();
    public static AccountsAccess AccountsAccess = new AccountsAccess();
    public static double TotaalPrijs = Math.Round(0.0, 2);
    public static int CurrentReservationCode = MailLogic.GenerateCode();
    public static List<ChairModel> ChosenChairs = new List<ChairModel>();

    public AccountsLogic()
    {
        _accounts = AccountsAccess.LoadAll();
    }

    /// <summary>
    /// Nieuw account object wordt aangemaakt en het wachtwoord wordt ook meteen gehashed
    /// </summary>
    public void NewAccount(string fullName, string email, string password)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 10);
        int id = _accounts.Count();
        AccountModel acc = new AccountModel(++id, email, passwordHash, fullName, "Customer");
        UpdateList(acc);
    }

    /// <summary>
    /// Voeg of update een gebruikersaccount aan de lijst van accounts, schrijf die ook naar de JSON.
    /// </summary>
    public void UpdateList(AccountModel acc)
    {
        //Find if there is already an model with the same id
        int index = _accounts.FindIndex(s => s.Id == acc.Id);

        if (index != -1)
        {
            //update existing model
            _accounts[index] = acc;
        }
        else
        {
            //add new model
            _accounts.Add(acc);
        }
        AccountsAccess.WriteAll(_accounts);
    }

    /// <summary>
    /// Zoek een account via een ID.
    /// </summary>
    public AccountModel GetById(int id)
    {
        return _accounts.Find(i => i.Id == id);
    }

    /// <summary>
    /// Login wordt gecheckt op emailadres en wachtwoord
    /// </summary>
    public AccountModel CheckLogin(string email, string password)
    {
        if (email == null || password == null)
        {
            return null;
        }
        CurrentAccount = _accounts.Find(i => i.EmailAddress == email);
        if (CurrentAccount == null)
        {
            return null;
        }
        bool result = BCrypt.Net.BCrypt.Verify(password, CurrentAccount.Password);
        if (result)
            return CurrentAccount;
        return null;
    }

    /// <summary>
    /// tijdelijk wachtwoord wordt aangemaakt en naar het emailadres gestuurd
    /// </summary>
    public void GenerateTempPassword()
    {
        var allAccounts = AccountsAccess.LoadAll();
        bool exists = false;

        var info = Mail.AskInfo();
        string email = info.Item1;
        string name = info.Item2;

        // update email and name in MailLogic
        MailLogic.Name = name;
        MailLogic.EmailAddress = email;
        foreach (var account in allAccounts)
        {
            if (account.EmailAddress == email)
            {
                exists = true;
            }
        }

        if (exists == false)
        {
            Console.WriteLine("Account does not exist. Try again.");
            GenerateTempPassword();
        }

        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] stringChars = new char[8];
        Random random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        string tempPassword = new String(stringChars);
        string tempPasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword, workFactor: 10);
        // change password for account associated with email
        foreach (var account in allAccounts)
        {
            if (account.EmailAddress == email)
            {
                account.Password = tempPasswordHash;
                UpdateList(account);
            }
        }
        MailLogic.SendTempPassword(tempPassword);
    }

    /// <summary>
    /// Method om een gebruikers wachtwoord te veranderen.
    /// </summary>
    public void ChangePassword(string email, string password)
    {
        // controleert of de wachtwoord klopt bij true gaaat die door
        bool result = BCrypt.Net.BCrypt.Verify(password, CurrentAccount.Password);
        bool checker = false;
        // het systeem vraagt de gebruiker om een nieuw wachtwoord tot dat de gebruiker het goed heeft. Het systeem veranderd het direct.  
        while (!checker)
        {
            if (result == true)
            {
                Console.WriteLine("\nNieuw Wachtwoord");
                Console.Write("> ");
                string password1 = Console.ReadLine();
                Console.WriteLine("Herhaal niew wachtwoord");
                Console.Write("> ");
                string passwordCheck = Console.ReadLine();
                if (password1 == passwordCheck)
                {

                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordCheck, workFactor: 10);
                    CurrentAccount.Password = passwordHash;
                    UpdateList(CurrentAccount);
                    Console.WriteLine("Wachtwoord succesvol gewijzigd! ");
                    checker = true;
                }
            }
            else
            {
                Console.WriteLine("Verkeerde Wachtwoord ingevoert probeer het opnieuw.");
                break;
            }
        }
    }

    /// <summary>
    /// Maak een aankoopgeschiedenis van de gebruiker, dit zijn alle aankopen ook van films die al gedraait zijn, maar
    /// niet degene die niet meer in de json staan.
    /// </summary>
    public string AankoopGeschiedenis()
    {
        string output = "";
        ChairReservationLogic chairReservationLogic = new ChairReservationLogic();
        ChairLogic chairLogic = new ChairLogic();
        MovieLogic movieLogic = new MovieLogic();
        List<ChairReservationModel> userReservations = chairReservationLogic.GetUserReservations(AccountsLogic.CurrentAccount.EmailAddress);
        if (userReservations.Count <= 0)
        {
            return "U heeft geen aankopen gedaan.";
        }
        foreach (ChairReservationModel  _userReservation in userReservations)
        {
            MovieModel movie = MovieLogic.GetMovie(_userReservation.MovieID);
            ChairModel chair = chairLogic.GetChairModel(_userReservation.ChairID);
            if (movie == null)
                continue;
            output += "\n";
            output += $"Film: {movie.Title}\n";
            output += $"Zaal: {_userReservation.AuditoriumID}\n";
            output += $"Rij: {chair.Row}, Stoel: {chair.Column}\n";
            output += $"Prijs: €{_userReservation.TotaalPrijs}\n";
            output += $"Reserveringscode: {_userReservation.ReserveringsCode}\n";
            output += $"Datum: {_userReservation.Time}\n";
        }
        return output;
    }
}
