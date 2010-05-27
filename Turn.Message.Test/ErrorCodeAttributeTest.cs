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
using Turn.Message;
using NUnit.Framework;

namespace TestTurnMessage
{
	[TestFixture()]
	public class ErrorCodeAttributeTest
	{
		[Test()]
		public void ParseTest()
		{
			byte[] bytes = new byte[] 
			{
				0x00, 0x09, 0x00, 0x0A, 
				0x00, 0x00, 3, 21,
				(byte)'H', (byte)'e', (byte)'l', (byte)'l', (byte)'o', (byte)'!',   
			};

			int startIndex = 0;
			ErrorCodeAttribute target = new ErrorCodeAttribute();
			target.Parse(bytes, ref startIndex);
			Assert.AreEqual(14, startIndex);
			Assert.AreEqual(AttributeType.ErrorCode, target.AttributeType);
			Assert.AreEqual(3 * 100 + 21, target.ErrorCode);
			Assert.AreEqual("Hello!", target.ReasonPhrase);
		}

		[Test()]
		public void GetBytesTest()
		{
			ErrorCodeAttribute target = new ErrorCodeAttribute()
			{
				ErrorCode = 123,
				ReasonPhrase = @"Test!",
			};

			byte[] expected = new byte[] 
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x09, 0x00, 0x09, 
				0x00, 0x00,    1,   23,
				(byte)'T', (byte)'e', (byte)'s', (byte)'t', (byte)'!',
			};

			byte[] actual = new byte[]
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00, 0x00,
			};

			int startIndex = 4;
			target.GetBytes(actual, ref startIndex);

			Assert.AreEqual(17, startIndex);
			Helpers.AreArrayEqual(expected, actual);
		}

		[Test()]
		public void ErrorCodeAttributeConstructorTest()
		{
			ErrorCodeAttribute target = new ErrorCodeAttribute();
			Assert.AreEqual(AttributeType.ErrorCode, target.AttributeType);
		}
	}
}
