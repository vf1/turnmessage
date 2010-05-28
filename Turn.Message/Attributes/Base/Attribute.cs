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
	public abstract class Attribute
	{
		public Attribute()
		{
			Ignore = false;
		}

		public const UInt16 AttributeHeaderLength = 4;
		public AttributeType AttributeType { get; protected set; }
		public virtual UInt16 ValueLength { get; protected set; }

		public UInt16 TotalLength
		{
			get
			{
				return (UInt16)(ValueLength + AttributeHeaderLength);
			}
		}

		public bool Ignore
		{
			get;
			set;
		}

		public virtual void GetBytes(byte[] bytes, ref int startIndex)
		{
			CopyBytes(bytes, ref startIndex, ((UInt16)AttributeType).GetBigendianBytes());
			CopyBytes(bytes, ref startIndex, ValueLength.GetBigendianBytes());
		}

		public abstract void Parse(byte[] bytes, ref int startIndex);

		public static void Skip(byte[] bytes, ref int startIndex)
		{
			UInt16 length = ParseHeader(bytes, ref startIndex);
			// !bug! do NOT combine these lines
			startIndex += length;
		}

		protected static void CopyBytes(byte[] target, ref int startIndex, byte[] source)
		{
			CopyBytes(target, ref startIndex, source, 0, source.Length);
		}

		protected static void CopyBytes(byte[] target, ref int startIndex, byte[] source, int offset, int length)
		{
			Array.Copy(source, offset, target, startIndex, length);
			startIndex += source.Length;
		}

		protected static UInt16 ParseHeader(byte[] bytes, ref int startIndex)
		{
			startIndex += 2;
			return bytes.BigendianToUInt16(ref startIndex);
		}

		protected void ParseValidateHeader(byte[] bytes, ref int startIndex)
		{
			if (ParseHeader(bytes, ref startIndex) != ValueLength)
				throw new TurnMessageException(ErrorCode.BadRequest, @"Invalid attribute length - " + AttributeType.ToString());
		}
	}
}
