namespace Northwind2_v73.Services
{
	public class ValidationRulesException : Exception
	{
		public Dictionary<string, string[]> Errors { get; } = new();

		public ValidationRulesException()
		{

		}

		public ValidationRulesException(string message)
			 : base(message)
		{
		}

		public ValidationRulesException(string message, Exception inner)
			 : base(message, inner)
		{
		}
	}
}
