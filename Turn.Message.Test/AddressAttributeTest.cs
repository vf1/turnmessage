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
using System.Net;
using System.Net.Sockets;
using Turn.Message;
using NUnit.Framework;

namespace TestTurnMessage
{
	[TestFixture()]
	public class AddressAttributeTest
	{
		[Test()]
		public void ParseIp4Test()
		{
			byte[] bytes = new byte[] 
			{
				0x00, 0x01, 0x00, 0x08, 
				0xff, 0x01, 0x12, 0x34,
				0x01, 0x02, 0x03, 0x04,
			};

			int startIndex = 0;
			AddressAttribute target = new MappedAddress();
			target.Parse(bytes, ref startIndex);
			Assert.AreEqual(12, startIndex);
			Assert.AreEqual(AttributeType.MappedAddress, target.AttributeType);
			Assert.AreEqual(0x1234, target.Port);
			Assert.AreEqual(@"1.2.3.4", target.IpAddress.ToString());
		}

		[Test()]
		public void ParseIp6Test()
		{
			byte[] bytes = new byte[] 
			{
				0x00, 0x01, 0x00, 20, 
				0x00, 0x02, 0x12, 0x34,
				0x11, 0x22, 0x33, 0x44,
				0x55, 0x66, 0x77, 0x88,
				0x99, 0xaa, 0xbb, 0xcc,
				0xdd, 0xee, 0xff, 0xf2,
			};

			int startIndex = 0;
			AddressAttribute target = new MappedAddress();
			target.Parse(bytes, ref startIndex);
			Assert.AreEqual(24, startIndex);
			Assert.AreEqual(AttributeType.MappedAddress, target.AttributeType);
			Assert.AreEqual(0x1234, target.Port);
			Assert.AreEqual(@"1122:3344:5566:7788:99aa:bbcc:ddee:fff2", target.IpAddress.ToString());
		}

		[Test()]
		public void GetIpV4BytesTest()
		{
			AddressAttribute target = new MappedAddress()
			{
				IpAddress = IPAddress.Loopback,
				Port = 0x1234,
			};

			byte[] expected = new byte[] 
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x01, 0x00, 0x08, 
				0x00, 0x01, 0x12, 0x34,
				0x7f, 0x00, 0x00, 0x01,
			};

			byte[] actual = new byte[]
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
			};

			int startIndex = 4;
			target.GetBytes(actual, ref startIndex);

			Assert.AreEqual(16, startIndex);
			Helpers.AreArrayEqual(expected, actual);
			Assert.AreEqual(AddressFamily.InterNetwork, target.IpEndPoint.AddressFamily);
		}

		[Test()]
		public void GetIpV6BytesTest()
		{
			AddressAttribute target = new MappedAddress()
			{
				IpAddress = IPAddress.Parse("0102:0304:0506:0708:090a:0b0c:0d0e:0f10"),
				Port = 0x1234,
			};

			byte[] expected = new byte[] 
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x01, 0x00, 20, 
				0x00, 0x02, 0x12, 0x34,
				0x01, 0x02, 0x03, 0x04,
				0x05, 0x06, 0x07, 0x08,
				0x09, 0x0a, 0x0b, 0x0c,
				0x0d, 0x0e, 0x0f, 0x10,
			};

			byte[] actual = new byte[]
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
			};

			int startIndex = 4;
			target.GetBytes(actual, ref startIndex);

			Assert.AreEqual(28, startIndex);
			Helpers.AreArrayEqual(expected, actual);

			Assert.AreEqual(AddressFamily.InterNetworkV6, target.IpEndPoint.AddressFamily);
		}
	}
}
