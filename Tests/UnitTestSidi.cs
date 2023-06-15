namespace projectBTests;

[TestClass]
public class UnitTestCombiDealLogic
{
    [TestMethod]
    public void TestGetCombiDeal()
    {
        var combiDealLogic = new CombiDealLogic();
        List<CombiDealModel> models = combiDealLogic.Deals;
        CombiDealModel output = combiDealLogic.GetCombiDeal(1);
        Assert.AreEqual(models[0].Product, output.Product);



    }
}