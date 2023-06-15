namespace Testen;

[TestClass]
public class UnitTestsAnnulerlingLogic
{
    [TestMethod]
    public void TestAnnuleringCalculator()
    { 
        DateTime date1 = DateTime.Now;
        DateTime date2 = date1.AddHours(-24);
        MovieModel movie = new MovieModel(1, 1, "Matrix", "Description", date1, 144, "Actie", 18);
        AnnuleringModel annulering = new AnnuleringModel("redwane1999@live.nl", 1, date2);
        ChairReservationModel reservation = new ChairReservationModel("redwane1999@live.nl", 1, 1, 1, date1, 20.00, 1234556789, true);
        var annuleringLogic = new AnnuleringLogic();
        double actual = annuleringLogic.AnnuleringCalculator(movie, annulering, reservation);
        double expected = 20.00;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestAnnuleringCalculatorTwee()
    {
        DateTime date3 = DateTime.Now;
        DateTime date4 = date3.AddHours(-12);
        MovieModel movie = new MovieModel(1, 1, "Matrix", "Description", date3, 144, "actie", 18);
        AnnuleringModel annulering = new AnnuleringModel("redwane1999@live.nl", 1, date4);
        ChairReservationModel reservation = new ChairReservationModel("redwane1999@live.nl", 1, 1, 1, date3, 20.00, 1234556789, true);
        var annuleringLogic = new AnnuleringLogic();
        double actual = annuleringLogic.AnnuleringCalculator(movie, annulering, reservation);
        double expected = 10.00;
        Assert.AreEqual(expected, actual);
    }
}
