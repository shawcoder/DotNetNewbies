namespace LambdaTutorial.LambdasDecoded
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// OK, just what the heck *is* one of these "lambda" things? Well, MS
	/// defines a lambda (or, more correctly, a "lambda expression") as follows:
	/// 
	/// "A lambda expression is an anonymous function that you can use to create 
	/// delegates or expression tree types. By using lambda expressions, you can 
	/// write local functions that can be passed as arguments or returned as the
	/// value of function calls. Lambda expressions are particularly helpful for
	/// writing LINQ query expressions."
	/// 
	/// Great! But what does that actually mean? Well, technically, a lambda is
	/// syntactic suger designed to hide and consolidate the process of creating
	/// a delagate, assigning a function to the delegate and then calling that
	/// function via the delegate. Example 1 shows the "old" way of doing things
	/// (a way, which is Just Fine to use nowadays - there are places where this
	/// syntax is perfectly acceptable!)
	/// 
	/// Here is a good alternate resource that explains the above a little 
	/// differently: http://www.tutorialsteacher.com/linq/linq-lambda-expression
	/// </summary>
	public class LambdaHost
	{

		#region Declarations

		public const int FIRST_NUMBER = 5;
		public const int SECOND_NUMBER = 10;
		public const string TEST_STRING = "Test String";
		public readonly List<int> OneThruTen =
			new List<int>
			{
				1
				, 2
				, 3
				, 4
				, 5
				, 6
				, 7
				, 8
				, 9
				, 10
			};

		#endregion

		#region Example 1 - Doing it the delegate way

		// Here is the delegate. What is a delegate? Basically a place within the
		// language that an engineer is allowed to assign the address of an
		// already-declared method whose parameter signature matches that of the
		// delegate (the "(int a, int b)" part). Why is this cool? Because it allows
		// the use of multiple functions to be called for different circumstances
		// at the same place in the code.
		public delegate int AddingTwoNumbers(int a, int b);

		/// <summary>
		/// And here is a method whose parameter signature matches that of the
		/// delegate declared above.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		private int AddTwoNumbers(int a, int b)
		{
			int vResult = a + b;
			return vResult;
		}

		public int AddTheTwoNumbersAndReturnTheResultWithADelegate(int a, int b)
		{

			AddingTwoNumbers vTheDelegateContainer;
			vTheDelegateContainer = AddTwoNumbers;
			int vResult = vTheDelegateContainer(FIRST_NUMBER, SECOND_NUMBER);
			return vResult;
		}

		#endregion

		#region Example 2 - the simplest lambda expression

		// OK, so what's all this "Func<int, int>" stuff? This is the way that MS
		// chose to hide the details of delegate declaration from the average
		// engineer. Func<T, TResult> syntax is the C# syntactic sugary way of
		// both declaring a delagate AND assigning functionality to said delegate.
		// The "Func" portion tells the compiler that what is wanted is a method
		// with a return result that is NOT void (the TResult declaration). They
		// cheated by combining this with generics such that all they need do is
		// delcare a number of Func<T, TResult> "types" and then teach the compiler
		// how to interpret them. Note that the Func<T...T16, TResult> declarations
		// only go to 16. You want more, you have to write your own. BUT, IF YOU
		// ARE WRITING SOLID CODE, THEN WHAT THE HECK ARE YOU DOING PASSING IN
		// MORE THAN 16 PARAMETERS IN THE FIRST PLACE!? DON'T DO THAT!
		// So...what do we have here? A simple method that returns a number.
		// Beyond silly? Yes, but it's here solely to demonstrate syntax, nothing 
		// more.
		public int TheSimplestLambdaExpression()
		{
			// Declare the lambda
			Func<int, int> vLambda = aNumber => aNumber;
			// Use the built-in "Invoke" method to call the method to do its thing
			// and assign the result to a return variable.
			int vResult = vLambda.Invoke(FIRST_NUMBER);
			// return the result. <= superfluous comment!
			return vResult;
		}

		/// <remarks>
		/// So, what does the above translate to? The below. This simple method has
		/// a number of characteristics that are common for all lamba expressions.
		/// It has a parameter signature - in this case NO parameters!
		/// It has some functionality built in (in this case, not much)
		/// It has a return value.
		/// All lambdas have at least this much.
		/// </remarks>
		public int TheSimplestLambdaExpressionAsAFunction()
		{
			return FIRST_NUMBER;
		}

		#endregion

		#region Examle 3 - parameter passing and processing

		/// <summary>
		/// Here we are declaring a lambda that takes in two values, adds them and
		/// then returns the result. Notice that there are no type declarations
		/// associated with the declaration, the compiler is clever enough to just
		/// figure it out from the declaration of the lambda expression variable
		/// (the Func<int, int, int> vLambda portion)
		/// </summary>
		/// <param name="aFirstNumber"></param>
		/// <param name="aSecondNUmber"></param>
		/// <returns></returns>
		public int Example3LambdaWithParameters(int aFirstNumber, int aSecondNUmber)
		{
			Func<int, int, int> vLambda = (a, b) => a + b;
			int vResult = vLambda.Invoke(aFirstNumber, aSecondNUmber);
			return vResult;
		}

		public int Example3MethodEquivalent(int aFirstNumber, int aSecondNumber)
		{
			int vResult = aFirstNumber + aSecondNumber;
			return vResult;
		}

		#endregion

		#region Example 4 - sometimes you have to help the lambda

		public int Example4MethodEquivalent(string aString, int aNumber)
		{
			int vResult = aString.Length + aNumber;
			return vResult;
		}

		public int Example4Lambda(string aString, int aNumber)
		{
			Func<string, int, int> vLambda = (string a, int b) => a.Length + b;
			int vResult = vLambda.Invoke(TEST_STRING, FIRST_NUMBER);
			return vResult;
		}

		#endregion

		#region Example 5 - Why do we care?

		/// <summary>
		/// OK, here is the method equivalent of adding up all the even numbers.
		/// </summary>
		/// <param name="aList"></param>
		/// <returns></returns>
		public int Example5SumTheListOfNumbersThatAreEvenMethod(List<int> aList)
		{
			int vResult = 0;
			foreach (int vNumber in aList)
			{
				vResult += (vNumber % 2 == 0) ? vNumber : 0;
			}
			return vResult;
		}

		// And here, finally, is where lambdas start coming in to their own. When
		// used as evaluations for processing a list of data, the short-hand syntax
		// makes both the algorithm as a whole and the processing of each item
		// much more easily understood as long as one remembers that lambdas are
		// DELEGATES IN DISGUISE! (<= theme music from "Transformers").
		// Since a lambda represents a function signature that is already attached
		// to an actual function, when processing a list of items, the function
		// will be called AGAINST EACH ITEM IN THE LIST and the results returned
		// as appropriate.
		// The short version here is that I want to add up all the even numbers in
		// the list. So, my filter is: is it even? If yes, add it to the running
		// total, if not, add 0. And that is easily seen in the function below
		// simply because it's virtually the only thing in the function. None of
		// the list processing code is declared, it's all handled by the "Sum"
		// extension method which takes in a list and processes the lambda against
		// each item in the list. Since the lambda returns only the even numbers,
		// the list gets totaled as all even numbers. Viola! Syntactic sugar that
		// makes code *WAY* easier to use!
		// <param name="aList"></param>
		// <returns></returns>
		public int Example5Lambda(List<int> aList)
		{
			int vResult =
				aList.Sum(aNumber => (aNumber % 2 == 0) ? aNumber : 0);
			return vResult;
		}

		#endregion

	}
}
