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

namespace System
{
	public static class Bigendian
	{
		public static UInt16 BigendianToUInt16(this byte[] bytes, int startIndex)
		{
			UInt16 result = 0;

			result |= bytes[startIndex];
			result <<= 8;
			result |= bytes[startIndex + 1];

			return result;
		}

		public static UInt32 BigendianToUInt32(this byte[] bytes, int startIndex)
		{
			UInt32 result = 0;

			result |= bytes[startIndex];
			result <<= 8;
			result |= bytes[startIndex + 1];
			result <<= 8;
			result |= bytes[startIndex + 2];
			result <<= 8;
			result |= bytes[startIndex + 3];

			return result;
		}

		public static UInt16 BigendianToUInt16(this byte[] bytes, ref int startIndex)
		{
			UInt16 result = BigendianToUInt16(bytes, startIndex);
			startIndex += sizeof(UInt16);
			return result;
		}

		public static UInt32 BigendianToUInt32(this byte[] bytes, ref int startIndex)
		{
			UInt32 result = BigendianToUInt32(bytes, startIndex);
			startIndex += sizeof(UInt32);
			return result;
		}

		private static byte[] Correct(byte[] data)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse(data);
			return data;
		}

		public static byte[] GetBigendianBytes(this UInt32 value)
		{
			return Correct(BitConverter.GetBytes(value));
		}

		public static byte[] GetBigendianBytes(this UInt16 value)
		{
			return Correct(BitConverter.GetBytes(value));
		}
	}
}
