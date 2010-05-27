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
	public class UInt32AttributeTest
	{
		[Test()]
		public void ParseTest()
		{
			byte[] bytes = new byte[] 
			{
				0xff, 0xff, 0xff,
				0x00, 0x10, 0x00, 0x04, 
				0x12, 0x34, 0x56, 0x78,
			};

			int startIndex = 3;
			UInt32Attribute target = new Bandwidth();
			target.Parse(bytes, ref startIndex);
			Assert.AreEqual(11, startIndex);
			Assert.AreEqual(0x12345678, target.Value);
		}

		[Test()]
		public void GetBytesTest()
		{
			UInt32Attribute target = new Bandwidth()
			{
				Value = 0x87654321,
			};

			byte[] expected = new byte[] 
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x10, 0x00, 0x04, 
				0x87, 0x65, 0x43, 0x21,
			};

			byte[] actual = new byte[]
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
			};

			int startIndex = 4;
			target.GetBytes(actual, ref startIndex);

			Assert.AreEqual(12, startIndex);
			Helpers.AreArrayEqual(expected, actual);
		}
	}
}
