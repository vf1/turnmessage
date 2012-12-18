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
	public struct ConnectionId
		: IEquatable<ConnectionId>
	{
		public Int64 Value1;
		public Int64 Value2;
		public Int32 Value3;

		public void GetBytes(byte[] bytes, ref int startIndex)
		{
			Buffer.BlockCopy(BitConverter.GetBytes(Value1), 0, bytes, startIndex + 0, 8);
			Buffer.BlockCopy(BitConverter.GetBytes(Value2), 0, bytes, startIndex + 8, 8);
			Buffer.BlockCopy(BitConverter.GetBytes(Value3), 0, bytes, startIndex + 16, 4);

			startIndex += 20;
		}

		public void Parse(byte[] bytes, ref int startIndex)
		{
			Value1 = BitConverter.ToInt64(bytes, startIndex + 0);
			Value2 = BitConverter.ToInt64(bytes, startIndex + 8);
			Value3 = BitConverter.ToInt32(bytes, startIndex + 16);

			startIndex += 20;
		}

		public bool Equals(ConnectionId other)
		{
			return Value1 == other.Value1
				&& Value2 == other.Value2
				&& Value3 == other.Value3;
		}

		public static bool operator ==(ConnectionId id1, ConnectionId id2)
		{
			return id1.Equals(id2);
		}

		public static bool operator !=(ConnectionId id1, ConnectionId id2)
		{
			return !id1.Equals(id2);
		}

		public override bool Equals(Object obj)
		{
			return obj != null && obj is ConnectionId && Equals((ConnectionId)obj);
		}

		public override int GetHashCode()
		{
			return Value1.GetHashCode() ^ Value2.GetHashCode() ^ Value3.GetHashCode();
		}

        public override string ToString()
        {
            return string.Format("{0:x16}{1:x16}{2:x8}", Value1, Value2, Value3);
        }
	}
}
