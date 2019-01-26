using System;
using System.Collections.Generic;

namespace Calculator
{
	abstract class ExpressionToken
	{
		public virtual int Value
		{
			get { throw new InvalidOperationException(string.Format("Getting value for {0}", this.GetType())); }
		}
		public virtual int Precedence
		{
			get { return 0; }
		}
		public virtual bool IsOperation
		{
			get { return false; }
		}
		public abstract void Process(Queue<ExpressionToken> output, Stack<ExpressionToken> stack);
		public virtual ExpressionToken Calc(ExpressionToken left, ExpressionToken right)
		{
			throw new InvalidOperationException(string.Format("Calculating for {0}", this.GetType()));
		}
	}

	class ExpressionTokenValue : ExpressionToken
	{
		int value;

		public ExpressionTokenValue(int value)
		{
			this.value = value;
		}
		public override int Value
		{
			get { return value; }
		}
		public override void Process(Queue<ExpressionToken> output, Stack<ExpressionToken> stack)
		{
			output.Enqueue(this);
		}
	}

	abstract class ExpressionTokenOperation : ExpressionToken
	{
		public override bool IsOperation => true;
		public override void Process(Queue<ExpressionToken> output, Stack<ExpressionToken> stack)
		{
			while (
				stack.Count > 0 &&
				stack.Peek().GetType() != typeof(ExpressionTokenLeftBracket) &&
				stack.Peek().Precedence >= this.Precedence
			)
			{
				output.Enqueue(stack.Pop());
			}
			stack.Push(this);
		}
	}

	class ExpressionTokenMul : ExpressionTokenOperation
	{
		public override int Precedence => 3;
		public override ExpressionToken Calc(ExpressionToken left, ExpressionToken right)
		{
			return new ExpressionTokenValue(left.Value * right.Value);
		}
	}

	class ExpressionTokenDiv : ExpressionTokenOperation
	{
		public override int Precedence => 3;
		public override ExpressionToken Calc(ExpressionToken left, ExpressionToken right)
		{
			return new ExpressionTokenValue(left.Value / right.Value);
		}
	}

	class ExpressionTokenAdd : ExpressionTokenOperation
	{
		public override int Precedence => 2;
		public override ExpressionToken Calc(ExpressionToken left, ExpressionToken right)
		{
			return new ExpressionTokenValue(left.Value + right.Value);
		}
	}

	class ExpressionTokenSub : ExpressionTokenOperation
	{
		public override int Precedence => 2;
		public override ExpressionToken Calc(ExpressionToken left, ExpressionToken right)
		{
			return new ExpressionTokenValue(left.Value - right.Value);
		}
	}

	class ExpressionTokenLeftBracket : ExpressionToken
	{
		public override void Process(Queue<ExpressionToken> output, Stack<ExpressionToken> stack)
		{
			stack.Push(this);
		}
	}

	class ExpressionTokenRightBracket : ExpressionToken
	{
		public override void Process(Queue<ExpressionToken> output, Stack<ExpressionToken> stack)
		{
			while (
				stack.Count > 0 &&
				stack.Peek().GetType() != typeof(ExpressionTokenLeftBracket)
			)
			{
				output.Enqueue(stack.Pop());
			}
			stack.Pop();
		}
	}
}
