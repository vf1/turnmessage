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
using Turn.Message;
using NUnit.Framework;

namespace TestTurnMessage
{
	[TestFixture()]
	public class DataTest
	{
		[Test()]
		public void GetBytesTest()
		{
			byte[] bytes = new byte[] 
			{
				0x00, 0x13, 0x00, 0x09, 
				0x01, 0x02, 0x03, 0x04,
				0x05, 0x06, 0x07, 0x08,
				0x09
			};

			int startIndex = 0;
			Data target = new Data();
			target.Parse(bytes, ref startIndex);

			Assert.AreEqual(13, startIndex);
			Assert.AreEqual(AttributeType.Data, target.AttributeType);
			Assert.AreEqual(9, target.ValueLength);
			
			
			byte[] expectedValue = new byte[9];
			Array.Copy(bytes, 4, expectedValue, 0, 9);

			Helpers.AreArrayEqual(expectedValue, 0, target.Value, 4);
		}

		[Test()]
		public void DataConstructorTest()
		{
			Data target = new Data();
			Assert.AreEqual(AttributeType.Data, target.AttributeType);
		}

		[Test()]
		public void ParseTest()
		{
			Data target = new Data()
			{
				Value = new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 },
			};

			byte[] expected = new byte[] 
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x13, 0x00, 0x09, 
				0x09, 0x08, 0x07, 0x06,
				0x05, 0x04, 0x03, 0x02,
				0x01,
			};

			byte[] actual = new byte[]
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00,
			};

			int startIndex = 4;
			target.GetBytes(actual, ref startIndex);

			Assert.AreEqual(17, startIndex);
			Helpers.AreArrayEqual(expected, actual);
		}
	}
}
