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
	public class ChangeRequestTest
	{
		[Test()]
		public void ChangeRequestConstructorTest()
		{
			ChangeRequest target = new ChangeRequest();
			Assert.AreEqual(AttributeType.ChangeRequest, target.AttributeType);
		}

		[Test()]
		public void ParseTest()
		{
			byte[] bytes = new byte[]
			{
				0x00, 0x04, 0x00, 0x04,
				0x00, 0x00, 0x00, 0x00,
			};
			
			int startIndex = 0;
			ChangeRequest target = new ChangeRequest();
			target.Parse(bytes, ref startIndex);

			Assert.AreEqual(8, startIndex);
			Assert.AreEqual(false, target.ChangeIp);
			Assert.AreEqual(false, target.ChangePort);

			startIndex = 0;
			bytes[7] = 0x04;
			target.Parse(bytes, ref startIndex);

			Assert.AreEqual(true, target.ChangeIp);
			Assert.AreEqual(false, target.ChangePort);

			startIndex = 0;
			bytes[7] = 0x02;
			target.Parse(bytes, ref startIndex);

			Assert.AreEqual(false, target.ChangeIp);
			Assert.AreEqual(true, target.ChangePort);

			startIndex = 0;
			bytes[7] = 0x06;
			target.Parse(bytes, ref startIndex);

			Assert.AreEqual(true, target.ChangeIp);
			Assert.AreEqual(true, target.ChangePort);
		}
	}
}
