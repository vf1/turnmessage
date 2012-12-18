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
    class ConnectionIdTest
    {
        [Test()]
        public void ToStringTest()
        {
            {
                var connectionId = new ConnectionId() { Value1 = 1, Value2 = 2, Value3 = 3, };
                Assert.AreEqual("0000000000000001000000000000000200000003", connectionId.ToString());
            }

            {
                var connectionId = new ConnectionId() { Value1 = -1, Value2 = -1, Value3 = -1, };
                Assert.AreEqual("ffffffffffffffffffffffffffffffffffffffff", connectionId.ToString());
            }
        }
    }
}
