static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("L: Login: ");
        Console.WriteLine("I: Info: ");
        Console.WriteLine("M: Movies");
        Console.WriteLine("Q: Quit");
        Console.Write(">");

        string inputUser = Console.ReadLine().ToUpper();
        if (inputUser == "L")
        {
            UserLogin.Start();
        }
        else if (inputUser == "I")
        {

            Console.WriteLine(@"
            Telephone number:   010 123 123 12.
            Address:            Wijnhaven 107.
            Zip code:           3011 WN in Rotterdam.
            Opening hours:      We are open fifteen minutes before the start of the first performance.
                                The cinema closes 10 minutes after the start of the last performance.");
            Menu.Start();
        }
        else if (inputUser == "M")
        {
            Console.WriteLine("");
        }
        else if (inputUser == "Q")
        {
            Console.WriteLine("Thanks for visiting us.");
        }
        else
        {
            Console.WriteLine("Invalid input");
            Menu.Start();
        }

    }
}
