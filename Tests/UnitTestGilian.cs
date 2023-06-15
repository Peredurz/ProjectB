namespace UnitTestGilian;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestShowMovies()
    {
        // test if movies shown in the program are the same as the ones in the json file
        string expected = "ID: 1\r\nZaal: 1\r\nTitle: The Matrix\r\nTime: 24-3-2024 12:00:00\r\nDuration: 140\r\n\r\nID: 2\r\nZaal: 2\r\nTitle: The Matrix Reloaded\r\nTime: 24-5-2023 12:00:00\r\nDuration: 132\r\n\r\nID: 3\r\nZaal: 3\r\nTitle: The Matrix Revolutions\r\nTime: 24-3-2023 12:00:00\r\nDuration: 129\r\n\r\nID: 4\r\nZaal: 2\r\nTitle: The Matrix Resurrections\r\nTime: 24-6-2023 12:00:00\r\nDuration: 142\r\n\r\n\r\n\r\n";
        var movieLogic = new MovieLogic();
        string actual = movieLogic.ShowMovies();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestShowFutureMovies()
    {
        // test if future movies shown in the program are the same as the ones in the json file
        string expected = "ID: 5\r\nZaal: 1\r\nTitle: The Lord of the Rings: The Fellowship of the Ring\r\nTime: Nog niet bekend\r\nDuration: 178\r\n\r\nID: 6\r\nZaal: 2\r\nTitle: Berserk\r\nTime: Nog niet bekend\r\nDuration: 90\r\n\r\n";
        var movieLogic = new MovieLogic();
        string actual = movieLogic.ShowFutureMovies();
        Assert.AreEqual(expected, actual);
    } 
}