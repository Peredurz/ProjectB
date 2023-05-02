using System.Text.RegularExpressions;
using System.Numerics;

public class PaymentLogic
{
	// alle afkortingen voor geldige land codes op een iban
	private static string[] _validLandCodes = {
		"AL", "AD", "BE", "BA", "BG", "CY",
		"DK", "DE", "EE", "FO", "FI", "FR",
		"GE", "GI", "GR", "GL", "HU", "IE",
		"IS", "IL", "IT", "JO", "HR", "LV",
		"LB", "LI", "LT", "LU", "MT", "MU",
		"MC", "ME", "NL", "MK", "NO", "AT",
		"PL", "PT", "RO", "SM", "SA", "RS",
		"SK", "SI", "ES", "CZ", "TR", "TN",
		"GB", "AE", "SE", "CH"
	};

	/// <summary>
	///	Method om een iban te kunnen checken dit is ongeveer hoe dat ook in het echt werkt.
	/// </summary>
	/// <return>
	/// boolean waar false niet valid is en true wel
	/// </return>
	/// <param name="iban">
	/// iban om te checken of die correct is
	/// </param>
	/// <seealso href="https://nl.wikipedia.org/wiki/International_Bank_Account_Number#Valideren">
	/// Nederlandse wiki pagina over valideren
	/// </seealso>
	public static bool ValidateIBAN(string iban)
	{
		Regex ibanRegex = new Regex(@"\b[A-Z]{2}[0-9]{2}(?:[ ]?[0-9]{4}){4}(?!(?:[ ]?[0-9]){3})(?:[ ]?[0-9]{1,2})?\bgm",
		                            RegexOptions.Compiled | RegexOptions.IgnoreCase);
		// verwijder spaties is makkelijker te parsen
		iban = iban.Trim().Replace(" ", "");

		// check via regex of het een geldig iban nummer is
		// maar we gaan onder ook nog echt het hele ding individueel checken
		if (ibanRegex.IsMatch(iban) == true)
			return false;

		// pak de land code om te checken of die goed is.
		string landCode = iban.Substring(0, 2).ToUpper();
		if (_validLandCodes.Contains(landCode) == false)
			return false;

		int controlNumber;
		try
		{
			controlNumber = Convert.ToInt32(iban.Substring(2, 2));
			if (controlNumber < 2 || controlNumber > 98)
				return false;
		}
		catch (FormatException ex)
		{
			Console.WriteLine("Controle nummer is geen nummer.");
			return false;
		}

		// verander de volgorde van de iban om met mod 97 te checken of ie correct is.
		string newIbanString = iban.Substring(4) + iban.Substring(0, 4);
		string newIban = "";
		foreach (char _c in newIbanString)
		{
			// als _c een letter is moet het naar een nummer veranderen waarbij A = 10, B = 11, ... , Z = 35
			if (Char.IsLetter(_c) == true)
				newIban += ((int)(_c - 'A' + 10)).ToString();
			else
				newIban += _c;
		}

		// maak er een ontzettend groot getal van waar we mod 97 moeten doen
		BigInteger newIbanNumber = BigInteger.Parse(newIban);
		if (newIbanNumber % 97 != 1)
			return false;

		return true;
	}
}