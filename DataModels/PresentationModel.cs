public class PresentationModel
{
	public string Name { get; set; }
	public string Description { get; set; }
	public List<string> UserClearance { get; set; }
	public bool IsSubMenu { get; set; }
	// als het all is betekent het dat het in alle menu's zit, en dat is excl. bij de submenu's
	// voorbeeld is: de terug optie, die moet overal terug te vinden zijn.
	public string PresentationName { get; set; }

	public PresentationModel(string menuName, string menuDescription, List<string> userClearance, string presentationName)
	                  : this(menuName, menuDescription, userClearance, presentationName, false) {}
	public PresentationModel(string menuName, string menuDescription, List<string> userClearance, string presentationName, bool isSubMenu)
	{
		Name = menuName;
		Description = menuDescription;
		UserClearance = userClearance;
		IsSubMenu = isSubMenu;
		PresentationName = presentationName;
	}
}