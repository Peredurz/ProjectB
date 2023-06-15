namespace ProjectBUnitTests;

[TestClass]
public class PaymentValidationUnitTests
{
    [TestMethod]
	[DataRow("NL20INGB0001234567")]
	[DataRow("NL20 INGB 0001 2345 67")]
	[DataRow("GB82WEST12345698765432")]
    public void TestPaymentValidationTrue(string iban)
    {
		bool result = PaymentLogic.ValidateIBAN(iban);

		Assert.IsTrue(result, $"{iban} should be a correct iban");
    }

    [TestMethod]
	[DataRow("NL01 1234 1234 1234 1234")]
	[DataRow("NL20 INGB 0001")]
	[DataRow("NL20INGB001")]
	public void TestPaymentValidationFalse(string iban)
	{
		bool result = PaymentLogic.ValidateIBAN(iban);

		Assert.IsFalse(result, $"{iban} shouldn't be a correct iban");
	}
}