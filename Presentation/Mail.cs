class Mail
{
    public static Tuple<string, string> AskInfo()
    {
        bool loop = true;
        string mailAddress = "";
        while (loop)
        {
            Console.WriteLine("Vul hier uw mail adres in");
            Console.Write(">");
            mailAddress = Console.ReadLine();
            bool validMailAddress = MailLogic.ValidateMailAddress(mailAddress);
            if (validMailAddress == false)
            {
                Console.WriteLine("Uw mail adres is niet geldig");
            }
            else
            {
                loop = false;
            }
        }
        Console.WriteLine("Vul hier uw naam in");
        Console.Write(">");
        string name = Console.ReadLine();

        return Tuple.Create(mailAddress, name);
    }

}