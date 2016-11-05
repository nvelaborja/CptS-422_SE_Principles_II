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
using System.Numerics;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;

namespace HW10
{
	public class BigNum
	{
		private BigInteger m_number;
		private BigInteger m_power;
		private bool m_undefined;

		// Criteria for number string
		//	The string must start with a minus sign, numeric char, or decimal point
		//	No whitespace
		//	No null or empty string
		//	No more than one - or .
		public BigNum (string number)
		{
			if (number == null)
				throw new ArgumentException ("Given number string was empty");

			Match match = Regex.Match(number, "^-?[0-9]*[.]?[0-9]+$");

			if (!match.Success)
				throw new ArgumentException ("Given number string was not in proper form");

			m_power = new BigInteger (0);								// Initialize power to 0
			m_undefined = false;										// Any successful use of this constructor should have undefined=false

			if (number.Contains (".")) {								// Adjust power based on a possible decimal
				int dIndex = number.IndexOf (".");
				m_power = -((number.Length - 1) - dIndex);

				number = number.Remove (dIndex, 1);						// Remove the decimal
			}

			//m_number = new BigInteger (Convert.ToDouble (number));
			BigInteger.TryParse (number, out m_number);
		}

		public BigNum(double value, bool useDoubleToString)
		{
			m_number = new BigInteger (0);								// Initialize members
			m_power = new BigInteger (0);
			m_undefined = false;

			if (double.IsNaN (value) || double.IsInfinity (value)) {
				m_undefined = true;
				return;
			}

			#region DoubleToStringTRUE

			if (useDoubleToString) {										// If true, we will use same method as string constructor
				string number = Convert.ToString (value);

				if (number.Contains (".")) {								// Adjust power based on a possible decimal
					int dIndex = number.IndexOf (".");
					m_power = -((number.Length - 1) - dIndex);

					number = number.Remove (dIndex, 1);						// Remove the decimal
				}

				m_number = new BigInteger (Convert.ToInt64 (number));
			} 

			#endregion

			#region DoubleToString FALSE

			else {															// Must use the value itself
				// TODO: Figure out how to do this crap

				BitArray RawBits = new BitArray (BitConverter.GetBytes (value));
				bool[] bits = new bool[64];

				for (int i = 0; i < 64; i++) 								// Bits are backwards, flip them!
					bits [i] = RawBits [63 - i];

				bool[] exponent = new bool[11];
				bool[] fraction = new bool[52];
				bool sign = bits[0];

				// Get exponent bits and get value 
				long exp = (long) 0;
				for (int i = 0; i < 11; i++) {
					exponent [i] = bits [i+1];
				}

				Array.Reverse (exponent);

				for (int i = 0; i < 11; i++) {
					if (exponent[i]) {
						long mask = (long)(1 << i);
						exp |= mask;
					}
				}

				exp -= 1023;												// Shift exponent to it's actual value (0 = 1023 in binary)

				// Get fraction bits 
				for (int i = 0; i < 52; i++) {
					fraction [i] = bits [i+12];
				}

				// Make BigNum for 2^exp
				// Make BigNum for each bit==true
					// 2^(-index)*2^exp
				// Add them all up
				// Make negative if sign==true
				// Voila

				double num = 1.0;

				if (exp == 0) 
					num = 1.0;
				else if (exp == 1) 
					num = (double)exp;
				else{
					long absExp = Math.Abs (exp);

					for (int i = 0; i < absExp; i++) {
						num *= 2;
					}

					if (exp < 0)
						num = 1.0 / num;
				}

				BigNum twoExp = new BigNum(num.ToString());
				BigNum number = new BigNum (0.0, true);							// double.tostring() here is guarunteed to be lossless

				Array.Reverse (fraction);

				for (int i = 0; i < 51; i++) {
					if (fraction [51 - i])										// Skip zero bits, no need to add them!
					{
						double val = 1.0;

						for (int j = 0; j < (i + 1); j++) {
							val *= 2;
						}

						val = 1.0 / val;

						number = number + new BigNum (val.ToString ("0." + new string('#', 1000)));
					}
				}

				number = number + new BigNum("1");
				number = number * twoExp;
				m_number = sign ? -number.m_number : number.m_number;
				m_power = number.m_power;
			}

			#endregion
		}

		public override string ToString ()
		{
			if (m_undefined)
				return "undefined";

			string number = m_number.ToString();
			int power = (int) BigInteger.Abs (m_power);
			int count = 0;

			if (m_power < 0) {
				if (number.Length < power) {
					count = power - number.Length;
					number = Multiply ("0", count) + number;
				}
				number = number.Insert (number.Length + (int)m_power, ".");
			}

			if (number [0] == '.')										// If number starts with decimal, give it a zero to look pretty
				number = "0" + number;

			// format fix for "-.4829234" -> give it a zero!
			if (number [0] == '-' && number [1] == '.')
				number = number.Insert (1, "0");

			return number;
		}

		public bool IsUndefined
		{
			get {
				return m_undefined;
			}
		}

		public static BigNum operator+(BigNum lhs, BigNum rhs)
		{
			if (lhs.IsUndefined || rhs.IsUndefined) {
				return new BigNum (double.PositiveInfinity, true);
			}

			// Find the number with smaller power and pad with zeros, adjust the power
			int pdif = Math.Abs ((int)lhs.m_power - (int)rhs.m_power);
			BigNum larger;
			BigNum smaller;

			if (lhs.m_power < rhs.m_power) {
				smaller = new BigNum(lhs.ToString());
				larger = new BigNum(rhs.ToString());
			} 
			else {
				smaller = new BigNum(rhs.ToString());
				larger = new BigNum(lhs.ToString());
			}

			string largerString = larger.ToString ();

			if (!largerString.Contains (".") && pdif > 0)
				largerString += ".";

			largerString = largerString + Multiply ("0", pdif);

			larger = new BigNum (largerString);

			string newValue = (larger.m_number + smaller.m_number).ToString ();				// Get string for added values
			int power = (int) BigInteger.Abs (larger.m_power);

			if (larger.m_power < 0) {
				if (newValue.Length < power) {
					int count = power - newValue.Length;
					newValue = Multiply ("0", count) + newValue;
				}
				newValue = newValue.Insert (newValue.Length + (int)larger.m_power, ".");
			}

			return new BigNum (newValue);
		}

		public static BigNum operator-(BigNum lhs, BigNum rhs)								// Identical to addition asside from the newValue assignment line
		{
			if (lhs.IsUndefined || rhs.IsUndefined) {
				return new BigNum (double.PositiveInfinity, true);
			}

			bool left = false;

			// Find the number with smaller power and pad with zeros, adjust the power
			int pdif = Math.Abs ((int)lhs.m_power - (int)rhs.m_power);
			BigNum larger;
			BigNum smaller;

			if (lhs.m_power < rhs.m_power) {
				smaller = new BigNum(lhs.ToString());
				larger = new BigNum(rhs.ToString());
				left = true;
			} 
			else {
				smaller = new BigNum(rhs.ToString());
				larger = new BigNum(lhs.ToString());
			}

			string largerString = larger.ToString ();

			if (!largerString.Contains (".") && pdif > 0)
				largerString += ".";

			largerString = largerString + Multiply ("0", pdif);

			larger = new BigNum (largerString);

			string newValue = (larger.m_number - smaller.m_number).ToString ();				// Get string for added values
			int power = (int) BigInteger.Abs (larger.m_power);

			if (larger.m_power < 0) {
				if (newValue.Length < power) {
					int count = power - newValue.Length;
					newValue = Multiply ("0", count) + newValue;
				}
				newValue = newValue.Insert (newValue.Length + (int)larger.m_power, ".");
			}

			// Now we gotta adjust the sign accordingly
			if (left && !newValue.StartsWith ("-"))
				newValue = "-" + newValue;
			

			return new BigNum (newValue);
		}

		public static BigNum operator*(BigNum lhs, BigNum rhs)
		{
			if (lhs.IsUndefined || rhs.IsUndefined) {
				return new BigNum (double.PositiveInfinity, true);
			}

			string newValue = (lhs.m_number * rhs.m_number).ToString ();
			int newPow = ((int)lhs.m_power + (int)rhs.m_power);
			int power = Math.Abs (newPow);

			if (newPow < 0) {
				if (newValue.Length < power) {
					int count = power - newValue.Length;
					newValue = Multiply ("0", count) + newValue;
				}
				newValue = newValue.Insert (newValue.Length + (int)newPow, ".");
			}

			return new BigNum (newValue);
		}

		public static BigNum operator/(BigNum lhs, BigNum rhs)
		{
			if (lhs.IsUndefined || rhs.IsUndefined) {
				return new BigNum (double.PositiveInfinity, true);
			}

			BigInteger left = lhs.m_number * BigInteger.Pow (10, 30);
			BigInteger power = lhs.m_power - 30;

			BigInteger divide = left / rhs.m_number;
			power = power - rhs.m_power;

			string divideString = divide.ToString ();
			string result = divideString.Insert (divideString.Length + (int)power, ".");

			return new BigNum (result);
		}

		public static bool operator>(BigNum lhs, BigNum rhs)
		{
			if (lhs.IsUndefined || rhs.IsUndefined) {
				return false;
			}

			BigNum result = lhs - rhs;

			if (result.m_number > 0)
				return true;
			return false;
		}

		public static bool operator<=(BigNum lhs, BigNum rhs)
		{
			if (lhs.IsUndefined || rhs.IsUndefined) {
				return false;
			}

			BigNum result = lhs - rhs;

			if (result.m_number <= 0)
				return true;
			return false;
		}

		public static bool operator<(BigNum lhs, BigNum rhs)
		{
			if (lhs.IsUndefined || rhs.IsUndefined) {
				return false;
			}

			BigNum result = lhs - rhs;

			if (result.m_number < 0)
				return true;
			return false;
		}

		public static bool operator>=(BigNum lhs, BigNum rhs)
		{
			if (lhs.IsUndefined || rhs.IsUndefined) {
				return false;
			}

			BigNum result = lhs - rhs;

			if (result.m_number >= 0)
				return true;
			return false;
		}

		public static bool operator==(BigNum lhs, BigNum rhs)
		{
			if (lhs.IsUndefined || rhs.IsUndefined) {
				return false;
			}

			string left = lhs.ToString ();
			string right = rhs.ToString ();

			return (left == right);
		}

		public static bool operator!=(BigNum lhs, BigNum rhs)
		{
			if (lhs.IsUndefined || rhs.IsUndefined) {
				return false;
			}

			string left = lhs.ToString ();
			string right = rhs.ToString ();

			return (left != right);
		}

		public static BigNum operator-(BigNum num)
		{
			return (num * new BigNum("-1"));
		}

		public static bool IsToStringCorrect(double value)
		{
			BigNum testNum = new BigNum (value, false);
			string thisString = testNum.ToString ();
			string thatString = value.ToString ();
			return (thisString == thatString);
		}

		private static string Multiply(string str, int mult)
		{
			if (mult == 0)
				return "";

			string local = str;

			for (int i = 1; i < mult; i++)
				local += str;

			return local;
		}

		private double Pow(long x, long y)
		{
			double one = 1.0;
			double xd = (double)x;

			if (y == 0) {
				return 1.0;
			}
			if (y == 1) {
				return xd;
			}

			long absy = Math.Abs (y);
			double answer = one;

			for (int i = 0; i < absy; i++) {
				answer *= x;
			}

			if (y < 0)
				answer = one / answer;

			return answer;
		}
	}
}

