public class CombiDealLogic
{
    private List<CombiDealModel> _productList = new List<CombiDealModel>();

    private static CombiDealAccess CombiDealAccess = new CombiDealAccess();
    public CombiDealLogic()
    {
        _productList = CombiDealAccess.LoadAll();
    }

    // return de juiste com combideal 
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

    // laat de juiste combideal zien
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


    // Voegd items toe aan de CombiDeals lijst
    public void AddProduct(int id, string product, double price)
    {
        CombiDealModel item = new CombiDealModel(id, product, price);
        _productList.Add(item);
        CombiDealAccess.WriteAll(_productList);
    }

    // laat alle CombiDeals zien
    public void PrintCombiDeals()
    {
        foreach (CombiDealModel combi in _productList)
        {
            Console.WriteLine($"ID: {combi.ID} \nOmschrijving: {combi.Product} \nPrijs: {combi.Price}");
            Console.WriteLine();
        }
    }

    // Controleert of de combideal bestaat  
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
}
