namespace Homies.Data.ValidationConstants
{
	public static class EntityValidationConstants
	{
		public static class Event
		{
			public const int EventNameMaxLength = 20;
			public const int EventNameMinLength = 5;

			public const int EventDescriptionMaxLength = 150;
			public const int EventDescriptionMinLength = 15;
		}

		public static class Type
		{
			public const int TypeNameMaxLength = 15;
			public const int TypeNameMinLength = 5;
		}
	}
}