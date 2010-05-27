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

namespace Turn.Message
{
	class XorMappedAddress : AddressAttribute
	{
		public XorMappedAddress(TurnMessageRfc rfc)
		{
			if (rfc == TurnMessageRfc.MsTurn)
				AttributeType = AttributeType.XorMappedAddress;
			else
				AttributeType = AttributeType.XorMappedAddressStun;
		}

		public override void GetBytes(byte[] bytes, ref int startIndex)
		{
			throw new NotSupportedException();
		}

		public override void Parse(byte[] bytes, ref int startIndex)
		{
			throw new NotSupportedException();
		}

		public void GetBytes(byte[] bytes, ref int startIndex, TransactionId transactionId)
		{
			base.GetBytes(bytes, ref startIndex, transactionId.Value);
		}

		public void Parse(byte[] bytes, ref int startIndex, TransactionId transactionId)
		{
			base.Parse(bytes, ref startIndex, transactionId.Value);
		}
	}
}
