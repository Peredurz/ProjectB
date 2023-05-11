class Mail
{
    public static Tuple<string, string> AskInfo()
    {
        Console.WriteLine("Vul hier uw mail adres in");
        Console.Write(">");
        string mailAddress = Console.ReadLine();
        bool validMailAddress = MailLogic.ValidateMailAddress(mailAddress);
        if (validMailAddress == false)
        {
            Console.WriteLine("Uw mail adres is niet geldig");
            AskInfo();
        }

        Console.WriteLine("Vul hier uw naam in");
        Console.Write(">");
        string name = Console.ReadLine();

        return Tuple.Create(mailAddress, name);
    }

}