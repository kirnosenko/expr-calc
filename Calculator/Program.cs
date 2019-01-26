using System;

namespace Calculator
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Enter the expression!");
			var expression = Console.ReadLine();
			try
			{
				ExpressionCalc calc = new ExpressionCalc();
				Console.WriteLine("The result is {0}", calc.Calculate(expression));
			}
			catch (Exception e)
			{
				Console.WriteLine("Ooops! {0}", e.Message);
			}
			Console.ReadKey();
		}
	}
}
