/***********************************\
 * Nathan VelaBorja
 * November 4, 2016
 * Cpts 422 HW 9
 * Collaborators: 
 * 	Worked on algorithms with Gene Lee,
 * 	Luke Holbert, and Kameron Haramoto
 * Notes:
 * 	Had a very intense week of school,
 * 	please don't be disgusted by my lack
 * 	of comments and bad coding
\***********************************/

using System;

namespace HW10
{
	class MainClass
	{
		public static void Main (string[] args)
		{

			//Console.WriteLine (BigNum.Pow (2, -3));
			//Console.WriteLine (BigNum.Pow (2, -2));
			//Console.WriteLine (BigNum.Pow (2, -1));
			//Console.WriteLine (BigNum.Pow (2, 0));
			//Console.WriteLine (BigNum.Pow (2, 1));
			//Console.WriteLine (BigNum.Pow (2, 2));
			//Console.WriteLine (BigNum.Pow (2, 3));
			//Console.WriteLine (BigNum.Pow (2, 4));

			double lf = -127.6948;
			//BigNum num = new BigNum (lf, false);
			//BigNum num2 = new BigNum ("0");
			//BigNum num3 = new BigNum (lf, false);
			//Console.WriteLine (num3);
			BigNum small = new BigNum ("0.0000026");
			Console.WriteLine (small);
			BigNum small2 = new BigNum ("00000026");
			Console.WriteLine (small2);
			BigNum small3 = new BigNum ("15.0000026");
			Console.WriteLine (small3);
			BigNum small4 = new BigNum ("-84051.511");
			Console.WriteLine (small4);
			BigNum small5 = new BigNum ("-85242");
			Console.WriteLine (small5);
			BigNum small6 = new BigNum ("0");
			Console.WriteLine (small6);
			BigNum small7 = new BigNum ("1");
			Console.WriteLine (small7);
			BigNum small8 = new BigNum ("3");
			Console.WriteLine (small8);

			BigNum added = small + small4;
			Console.WriteLine (added);

			BigNum multiplied = small * small4;
			Console.WriteLine (multiplied);

			BigNum divided = small7 / small8;
			Console.WriteLine (divided);

			Console.WriteLine ("******");
			BigNum doubleTest = new BigNum (lf, false);
			Console.WriteLine (doubleTest);
			Console.WriteLine(BigNum.IsToStringCorrect (lf));
			Console.WriteLine(BigNum.IsToStringCorrect (0.5));
			Console.WriteLine ("******");
			Console.WriteLine ((small < small2).ToString() + " --- [True]");
			Console.WriteLine ((small2 > small3).ToString() + " --- [True]");
			Console.WriteLine ((small == small2).ToString() + " --- [False]");
			Console.WriteLine ((small != small2).ToString() + " --- [True]");
			Console.WriteLine ((small == small).ToString() + " --- [True]");
			Console.WriteLine ((small <= small2).ToString() + " --- [True]");
			Console.WriteLine ((small >= small2).ToString() + " --- [False]");
			Console.WriteLine ((small <= small).ToString() + " --- [True]");
			Console.WriteLine ((small >= small).ToString() + " --- [True]");

			BigNum small9 = new BigNum ("-10.48");
			BigNum small10 = new BigNum ("12.11");
			Console.WriteLine ((small9 + small10).ToString ());

		}
	}
}
