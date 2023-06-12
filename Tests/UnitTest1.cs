using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectB_test
{
    [TestClass]
    public class ProjectB_Test
    {
        [TestMethod]
        public void Test1()
        {
            string[] inputs = new string[] { "L", "L", "Admin@admin.nl", "admin", "a", "r", "6", "Y", "c", "2023-05-24T12:00:00", "n", "b", "q" };
            using (StringReader inputReader = new StringReader(string.Join(Environment.NewLine, inputs)))
            using (StringWriter outputWriter = new StringWriter())
            {
                Console.SetIn(inputReader);
                Console.SetOut(outputWriter);

                Menu.Start(); // Moved here to set the input redirection before calling the method

                string output = outputWriter.ToString();
                List<MovieModel> _movies = MovieLogic.GetMovies();
                //Assert.IsTrue(output.Contains("Deze tijd is al bezet."));

                Assert.IsTrue(_movies[5].Time == DateTime.Parse("1970-01-01T00:00:00"));
            }
        }
    }
}
