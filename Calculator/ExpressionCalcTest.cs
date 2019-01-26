using System.Linq;
using NUnit.Framework;

namespace Calculator
{
	[TestFixture]
	public class ExpressionCalcTest
	{
		ExpressionCalc calc;

		[SetUp]
		public void Setup()
		{
			calc = new ExpressionCalc();
		}
		[Test]
		public void Get_tokens_correctly()
		{
			Assert.AreEqual(3, ExpressionCalc.GetTokens("1+2").Count());
			Assert.AreEqual(3, ExpressionCalc.GetTokens("11+22").Count());
			Assert.AreEqual(5, ExpressionCalc.GetTokens("-11*(-22)").Count());
			Assert.AreEqual(7, ExpressionCalc.GetTokens("(1+5)-2").Count());
		}
		[Test]
		public void Zero_for_null_or_empty()
		{
			Assert.AreEqual(0, calc.Calculate(null));
			Assert.AreEqual(0, calc.Calculate(""));
		}
		[Test]
		public void Single_number()
		{
			Assert.AreEqual(5, calc.Calculate("5"));
			Assert.AreEqual(5, calc.Calculate("+5"));
			Assert.AreEqual(-5, calc.Calculate("-5"));
		}
		[Test]
		public void Simple_caclulations()
		{
			Assert.AreEqual(3, calc.Calculate("1+2"));
			Assert.AreEqual(-1, calc.Calculate("1-2"));
			Assert.AreEqual(6, calc.Calculate("2*3"));
			Assert.AreEqual(2, calc.Calculate("4/2"));
		}
		[Test]
		public void Can_deal_with_spaces()
		{
			Assert.AreEqual(3, calc.Calculate(" 1 + 2"));
			Assert.AreEqual(-5, calc.Calculate("1 - 2* 3"));
		}
		[Test]
		public void Caclulate_with_priority()
		{
			Assert.AreEqual(0, calc.Calculate("1+2*3-7"));
			Assert.AreEqual(7, calc.Calculate("1-2/2+7"));
		}
		[Test]
		public void Calculate_with_brackets()
		{
			Assert.AreEqual(10, calc.Calculate("2*(1+5)-2"));
			Assert.AreEqual(4, calc.Calculate("(1+5)-2"));
			Assert.AreEqual(12, calc.Calculate("2*(1+5)"));
		}
		[Test]
		public void Calculate_with_multi_brackets()
		{
			Assert.AreEqual(9, calc.Calculate("2+(2*(2+3)-3)"));
			Assert.AreEqual(-111, calc.Calculate("2+3*(2+3*(2-3*4)-3*2)-11"));
		}
	}
}
