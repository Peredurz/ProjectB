public class CombiDealLogic
{
    private List<CombiDealModel> _productList = new List<CombiDealModel>();

    private static CombiDealAccess CombiDealAccess = new CombiDealAccess();
    public CombiDealLogic()
    {
        _productList = CombiDealAccess.LoadAll();
    }

    /// <summary>
    /// return de juiste com combideal
    /// </summary>
    public CombiDealModel GetCombiDeal(int id)
    {
        foreach (CombiDealModel combi in _productList)
        {
            if (combi.ID == id)
            {
                return combi;
            }
        }
        return null;
    }

    /// <summary>
    /// laat de juiste combideal zien
    /// </summary>
    public void ShowCombiDeal(int id)
    {
        foreach (CombiDealModel combi in _productList)
        {
            if (combi.ID == id)
            {
                Console.WriteLine($"ID: {combi.ID} \nOmschrijving: {combi.Product} \nPrijs: {combi.Price}");
            }
        }
    }


    /// <summary>
    /// Voegd items toe aan de CombiDeals lijst
    /// </summary>
    public bool AddCombi(string product, double price)
    {
        CombiDealModel item = new CombiDealModel(product, price);
        _productList.Add(item);
        CombiDealAccess.WriteAll(_productList);
        return true;
    }

    /// <summary>
    /// Combideals verwijderen
    /// </summary>
    public bool DeleteCombi(int id)
    {
        foreach (CombiDealModel combi in _productList)
        {
            if (combi.ID == id)
            {
                ShowCombiDeal(combi.ID);
                Console.WriteLine("Weet u zeker dat u het wilt verwijderen. Y/N");
                Console.Write("> ");
                string input = Console.ReadLine().ToUpper();
                if (input == "Y")
                {
                    _productList.Remove(combi);
                    CombiDealAccess.WriteAll(_productList);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Combideals bewerken
    /// </summary>
    public bool EditCombi(int id, string product, double price)
    {
        foreach (CombiDealModel combi in _productList)
        {
            if (combi.ID == id)
            {
                combi.Product = product;
                combi.Price = price;
                CombiDealAccess.WriteAll(_productList);
                return true;
            }
        }        
        return false;
    }

    /// <summary>
    /// laat alle CombiDeals zien
    /// </summary>
    public void PrintCombiDeals()
    {
        foreach (CombiDealModel combi in _productList)
        {
            Console.WriteLine($"ID: {combi.ID} \nOmschrijving: {combi.Product} \nPrijs: {combi.Price}");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Controleert of de combideal bestaat
    /// </summary>
    public bool ExistCombiDeal(int id)
    {
        foreach (CombiDealModel combi in _productList)
        {
            if (combi.ID == id)
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Om de lijst met combideals terug te krijgen, zodat je deze kan tonen aan de gebruiker.
    /// </summary>
    public List<CombiDealModel> Deals => _productList;
}
