// 
//  Author:
//       Vitali Fomine <support@officesip.com>
// 
//  Copyright (c) 2010 OfficeSIP Communications
// 
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
// 
using System;

namespace System.Text
{
	/// <summary>
	/// SASLprep: Stringprep Profile for User Names and Passwords
	/// http://tools.ietf.org/html/rfc4013
	/// </summary>
	static class Saslprep
	{
		/// <summary>
		/// SASLprep: Stringprep Profile for User Names and Passwords
		/// http://tools.ietf.org/html/rfc4013
		/// </summary>
		public static string SASLprep(this string s)
		{
			string result = "";

			foreach (char c in s)
			{
				if (c.IsCommonlyMappedToNothing() == false)
				{
					if (c.IsNonAsciiSpace())
						result += ' ';
					else
						result += c;
				}
			}

			return result.Normalize(NormalizationForm.FormKC);
		}

		/// <summary>
		/// Preparation of Internationalized Strings ("stringprep")
		/// http://tools.ietf.org/html/rfc3454#appendix-C.1.2
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsNonAsciiSpace(this char c)
		{
			switch (c)
			{
				case '\u00A0': //; NO-BREAK SPACE
				case '\u1680': //; OGHAM SPACE MARK
				case '\u2000': //; EN QUAD
				case '\u2001': //; EM QUAD
				case '\u2002': //; EN SPACE
				case '\u2003': //; EM SPACE
				case '\u2004': //; THREE-PER-EM SPACE
				case '\u2005': //; FOUR-PER-EM SPACE
				case '\u2006': //; SIX-PER-EM SPACE
				case '\u2007': //; FIGURE SPACE
				case '\u2008': //; PUNCTUATION SPACE
				case '\u2009': //; THIN SPACE
				case '\u200A': //; HAIR SPACE
				case '\u200B': //; ZERO WIDTH SPACE
				case '\u202F': //; NARROW NO-BREAK SPACE
				case '\u205F': //; MEDIUM MATHEMATICAL SPACE
				case '\u3000': //; IDEOGRAPHIC SPACE
					return true;
			}

			return false;
		}

		/// <summary>
		/// Preparation of Internationalized Strings ("stringprep")
		/// http://tools.ietf.org/html/rfc3454#appendix-B.1
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsCommonlyMappedToNothing(this char c)
		{
			switch (c)
			{
				case '\u00AD': //; ; Map to nothing
				case '\u034F': //; ; Map to nothing
				case '\u1806': //; ; Map to nothing
				case '\u180B': //; ; Map to nothing
				case '\u180C': //; ; Map to nothing
				case '\u180D': //; ; Map to nothing
				case '\u200B': //; ; Map to nothing
				case '\u200C': //; ; Map to nothing
				case '\u200D': //; ; Map to nothing
				case '\u2060': //; ; Map to nothing
				case '\uFE00': //; ; Map to nothing
				case '\uFE01': //; ; Map to nothing
				case '\uFE02': //; ; Map to nothing
				case '\uFE03': //; ; Map to nothing
				case '\uFE04': //; ; Map to nothing
				case '\uFE05': //; ; Map to nothing
				case '\uFE06': //; ; Map to nothing
				case '\uFE07': //; ; Map to nothing
				case '\uFE08': //; ; Map to nothing
				case '\uFE09': //; ; Map to nothing
				case '\uFE0A': //; ; Map to nothing
				case '\uFE0B': //; ; Map to nothing
				case '\uFE0C': //; ; Map to nothing
				case '\uFE0D': //; ; Map to nothing
				case '\uFE0E': //; ; Map to nothing
				case '\uFE0F': //; ; Map to nothing
				case '\uFEFF': //; ; Map to nothing
					return true;
			}

			return false;
		}
	}
}
