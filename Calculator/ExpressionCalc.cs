using System;
using System.Collections.Generic;

namespace Calculator
{
	class ExpressionCalc
	{
		public int Calculate(string exp)
		{
			if (exp == null || exp == string.Empty)
			{
				return 0;
			}
			exp = exp.Replace(" ", "");

			var output = new Queue<ExpressionToken>();
			var stack = new Stack<ExpressionToken>();

			var tokens = GetTokens(exp);
			foreach (var token in tokens)
			{
				token.Process(output, stack);
			}
			while (stack.Count > 0)
			{
				output.Enqueue(stack.Pop());
			}
			while (output.Count > 0)
			{
				var token = output.Dequeue();
				if (token.IsOperation)
				{
					var right = stack.Pop();
					var left = stack.Pop();
					stack.Push(token.Calc(left, right));
				}
				else
				{
					stack.Push(token);
				}
			}
			if (stack.Count == 1)
			{
				return stack.Peek().Value;
			}
			throw new ArgumentException("Invalid expression!");
		}

		public static IEnumerable<ExpressionToken> GetTokens(string expression)
		{
			List<ExpressionToken> tokens = new List<ExpressionToken>();

			int pos = 0;
			while (pos < expression.Length)
			{
				char symbol = expression[pos];
				bool tryGetNumber = 
					Char.IsDigit(symbol) ||
					((symbol == '+' || symbol == '-') && (pos == 0 || expression[pos - 1] == '('));
				if (tryGetNumber)
				{
					int toPos = pos + 1;
					while (toPos < expression.Length && Char.IsDigit(expression[toPos]))
					{
						toPos++;
					}
					string numStr = expression.Substring(pos, toPos - pos);
					tokens.Add(new ExpressionTokenValue(Convert.ToInt32(numStr)));
					pos = toPos;
				}
				else
				{
					switch (symbol)
					{
						case '+':
							tokens.Add(new ExpressionTokenAdd());
							break;
						case '-':
							tokens.Add(new ExpressionTokenSub());
							break;
						case '*':
							tokens.Add(new ExpressionTokenMul());
							break;
						case '/':
							tokens.Add(new ExpressionTokenDiv());
							break;
						case '(':
							tokens.Add(new ExpressionTokenLeftBracket());
							break;
						case ')':
							tokens.Add(new ExpressionTokenRightBracket());
							break;
						default:
							throw new ArgumentException(string.Format("Bad symbol \'{0}\' in expression.", expression[pos]));
					}
					pos++;
				}
			}

			return tokens;
		}
	}
}
