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
using System.Net;
using Turn.Message;
using NUnit.Framework;

namespace TestTurnMessage
{
	[TestFixture()]
	public class XorMappedAddressTest
	{
		private TransactionId GetTransactionId()
		{
			return new TransactionId()
			{
				Value = new byte[]
				{
					0x21, 0x12, 0xa4, 0x42,
				},
			};
		}

		[Test()]
		public void ParseIp4Test()
		{
			byte[] bytes = new byte[] 
			{
				0x00, 0x01, 0x00, 0x08, 
				0xff, 0x01, 0x34, 0xa1,
				0xe1, 0xba, 0xa5, 0x43,
			};

			int startIndex = 0;
			XorMappedAddress target = new XorMappedAddress(TurnMessageRfc.Rfc3489);
			target.Parse(bytes, ref startIndex, GetTransactionId());
			Assert.AreEqual(12, startIndex);
			Assert.AreEqual(AttributeType.XorMappedAddressStun, target.AttributeType);
			Assert.AreEqual(0x15b3, target.Port);
			Assert.AreEqual(@"192.168.1.1", target.IpAddress.ToString());
		}


		[Test()]
		public void GetIpV4BytesTest()
		{
			XorMappedAddress target = new XorMappedAddress(TurnMessageRfc.Rfc3489)
			{
				IpAddress = IPAddress.Parse("192.168.1.1"),
				Port = 0x15b3,
			};

			byte[] expected = new byte[] 
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x20, 0x00, 0x08, 
				0x00, 0x01, 0x34, 0xa1,
				0xe1, 0xba, 0xa5, 0x43,
			};

			byte[] actual = new byte[]
			{
				0xee, 0xee, 0xee, 0xee,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
			};

			int startIndex = 4;
			target.GetBytes(actual, ref startIndex, GetTransactionId());

			Assert.AreEqual(16, startIndex);
			Helpers.AreArrayEqual(expected, actual);
		}


		[Test()]
		public void XorMappedAddressConstructorTest()
		{
			XorMappedAddress target = new XorMappedAddress(TurnMessageRfc.Rfc3489);
			Assert.AreEqual(AttributeType.XorMappedAddressStun, target.AttributeType);
		}
	}
}
