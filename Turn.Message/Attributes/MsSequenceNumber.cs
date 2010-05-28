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
	public class MsSequenceNumber : Attribute
	{
		public MsSequenceNumber()
		{
			AttributeType = AttributeType.MsSequenceNumber;
			ValueLength = 0x0018;
		}

		public byte[] ConnectionId { get; set; }

		public UInt32 SequenceNumber { get; set; }

		public override void GetBytes(byte[] bytes, ref int startIndex)
		{
			base.GetBytes(bytes, ref startIndex);

#if DEBUG
			if (ConnectionId.Length != 20)
				throw new ArgumentException("ConnectionId must be a 20 bytes length.");
#endif

			CopyBytes(bytes, ref startIndex, ConnectionId);
			CopyBytes(bytes, ref startIndex, SequenceNumber.GetBigendianBytes());
		}

		public override void Parse(byte[] bytes, ref int startIndex)
		{
			ParseValidateHeader(bytes, ref startIndex);

			ConnectionId = new byte[20];
			Array.Copy(bytes, startIndex, ConnectionId, 0, ConnectionId.Length);
			startIndex += ConnectionId.Length;

			SequenceNumber = bytes.BigendianToUInt32(ref startIndex);
		}
	}
}
