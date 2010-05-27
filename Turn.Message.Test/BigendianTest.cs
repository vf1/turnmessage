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
using NUnit.Framework;

namespace TestTurnMessage
{
	[TestFixture()]
	public class BigendianTest
	{
		[Test()]
		public void GetBigendianBytesTest1()
		{
			byte[] expected = new byte[] { 0x44, 0x33, 0x22, 0x11 };
			byte[] actual = Bigendian.GetBigendianBytes(0x44332211);
			Helpers.AreArrayEqual(expected, actual);
		}

		[Test()]
		public void GetBigendianBytesTest()
		{
			byte[] expected = new byte[] { 0x22, 0x11 };
			byte[] actual = Bigendian.GetBigendianBytes(0x2211);
			Helpers.AreArrayEqual(expected, actual);
		}

		[Test()]
		public void BigendianToUInt16Test()
		{
			byte[] bytes = new byte[] { 0x22, 0x11 };
			int startIndex = 0;
			UInt16 actual = Bigendian.BigendianToUInt16(bytes, ref startIndex);
			Assert.AreEqual(2, startIndex);
			Assert.AreEqual(0x2211, actual);
		}

		[Test()]
		public void BigendianToUInt32Test()
		{
			byte[] bytes = new byte[] { 0x44, 0x33, 0x22, 0x11 };
			int startIndex = 0;
			UInt32 expected = 0x44332211;
			UInt32 actual = Bigendian.BigendianToUInt32(bytes, ref startIndex);
			Assert.AreEqual(4, startIndex);
			Assert.AreEqual(expected, actual);
		}

		[Test()]
		public void UInt32Test()
		{
			UInt32 expected = 0x12345678;
			byte[] bytes = Bigendian.GetBigendianBytes(expected);
			UInt32 actual = Bigendian.BigendianToUInt32(bytes, 0);
			Assert.AreEqual(expected, actual);
		}

		[Test()]
		public void UInt16Test()
		{
			UInt16 expected = 0x1234;
			byte[] bytes = Bigendian.GetBigendianBytes(expected);
			UInt16 actual = Bigendian.BigendianToUInt16(bytes, 0);
			Assert.AreEqual(expected, actual);
		}
	}
}
