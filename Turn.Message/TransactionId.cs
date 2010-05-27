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
	class TransactionId
	{
		public const int DefaultStartIndex = 4;
		public const int Length = 16;
		public byte[] Value;

		public TransactionId()
		{
		}

		public TransactionId(byte[] value, int startIndex)
		{
			Value = new byte[Length];

			Array.Copy(value, startIndex, Value, 0, Length);
		}

		public static TransactionId Generate()
		{
			var transactionId = new TransactionId()
			{
				Value = new byte[Length],
			};

			(new Random(Environment.TickCount)).NextBytes(transactionId.Value);

			return transactionId;
		}

		public override bool Equals(Object obj)
		{
			if (Value == null)
				return false;

			if (obj == null)
				return false;

			byte[] value2;
			if (obj is TransactionId)
				value2 = (obj as TransactionId).Value;
			else if (obj is byte[])
				value2 = obj as byte[];
			else
				return false;

			if (value2 == null)
				return false;

			if (Value.Length != value2.Length)
				return false;

			for (int i = 0; i < Value.Length; i++)
				if (Value[i] != value2[i])
					return false;

			return true;
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			int startIndex = 0;

			while (Value.Length - startIndex >= 4)
			{
				hashCode ^= BitConverter.ToInt32(Value, startIndex);
				startIndex += 4;
			}

			if (Value.Length - startIndex >= 2)
			{
				hashCode ^= BitConverter.ToInt16(Value, startIndex);
				startIndex += 2;
			}

			if (startIndex < Value.Length)
				hashCode ^= (int)Value[startIndex++] << 16;

			return hashCode;
		}

		public static bool operator ==(TransactionId id1, TransactionId id2)
		{
			return Equals(id1, id2);
		}

		public static bool operator !=(TransactionId id1, TransactionId id2)
		{
			return !Equals(id1, id2);
		}

		public override string ToString()
		{
			return Value.ToHexString();
		}
	}
}
