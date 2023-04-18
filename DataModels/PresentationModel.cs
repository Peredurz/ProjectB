public class PresentationModel
{
	public string Name { get; set; }
	public string Description { get; set; }
	public List<string> UserClearance { get; set; }
	public bool IsSubMenu { get; set; }

	public PresentationModel(string menuName, string menuDescription, List<string> userClearance)
	                  : this(menuName, menuDescription, userClearance, false) {}
	public PresentationModel(string menuName, string menuDescription, List<string> userClearance, bool isSubMenu)
	{
		Name = menuName;
		Description = menuDescription;
		UserClearance = userClearance;
		IsSubMenu = isSubMenu;
	}
}