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

namespace Turn.Message
{
	abstract class AddressAttribute : Attribute
	{
		private IPEndPoint ipEndPoint;

		public AddressAttribute()
		{
			ipEndPoint = new IPEndPoint(IPAddress.None, 0);
		}

		public override UInt16 ValueLength 
		{
			get
			{
				return (UInt16)(4 + (IpAddress.AddressFamily == AddressFamily.InterNetwork ? 4 : 16));
			}
			protected set
			{
				throw new InvalidOperationException();
			}
		}

		public IPEndPoint IpEndPoint
		{
			get
			{
				return ipEndPoint;
			}
			set
			{
				ipEndPoint.Address = value.Address;
				ipEndPoint.Port = value.Port;
			}
		}

		public UInt16 Port 
		{
			get
			{
				return (UInt16)IpEndPoint.Port;
			}
			set
			{
				IpEndPoint.Port = value;
			}
		}
		
		public IPAddress IpAddress 
		{
			get
			{
				return IpEndPoint.Address;
			}
			set
			{
				IpEndPoint.Address = value;
			}
		}

		public override void GetBytes(byte[] bytes, ref int startIndex)
		{
			this.GetBytes(bytes, ref startIndex, null);
		}

		public override void Parse(byte[] bytes, ref int startIndex)
		{
			this.Parse(bytes, ref startIndex, null);
		}

		protected void GetBytes(byte[] bytes, ref int startIndex, byte[] xorMask)
		{
			base.GetBytes(bytes, ref startIndex);

			// Reserved
			bytes[startIndex++] = 0;

			// Family
			bytes[startIndex++] = (byte)(IpAddress.AddressFamily == AddressFamily.InterNetwork ? 0x01 : 0x02);

			CopyBytes(bytes, ref startIndex, XorBytes(Port.GetBigendianBytes(), xorMask));
			CopyBytes(bytes, ref startIndex, XorBytes(IpAddress.GetAddressBytes(), xorMask));
		}

		protected void Parse(byte[] bytes, ref int startIndex, byte[] xorMask)
		{
			UInt16 length = ParseHeader(bytes, ref startIndex);

			// Reserved
			startIndex++;

			byte addressFamily = bytes[startIndex++];

			Port = (UInt16)(bytes.BigendianToUInt16(ref startIndex) ^ 
				(xorMask == null ? (UInt16)0 : xorMask.BigendianToUInt16(0)));

			if (addressFamily == 0x01)
			{
				if (length != 0x0008)
					throw new TurnMessageException(ErrorCode.BadRequest);

				IpAddress = GetAddress(AddressFamily.InterNetwork, bytes, ref startIndex, xorMask);
			}
			else if (addressFamily == 0x02)
			{
				if (length != 0x0014)
					throw new TurnMessageException(ErrorCode.BadRequest);

				IpAddress = GetAddress(AddressFamily.InterNetworkV6, bytes, ref startIndex, xorMask);
			}
			else
				throw new TurnMessageException(ErrorCode.BadRequest, @"The address family of the attribute field MUST be set to 0x01 or 0x02.");
		}

		private IPAddress GetAddress(AddressFamily addressFamily, byte[] bytes, ref int startIndex, byte[] xorMask)
		{
			byte[] ip = new byte[addressFamily == AddressFamily.InterNetwork ? 4 : 16];

			Array.Copy(bytes, startIndex, ip, 0, ip.Length);
			startIndex += ip.Length;

			XorBytes(ip, xorMask);

			return new IPAddress(ip);
		}

		private byte[] XorBytes(byte[] bytes, byte[] xorMask)
		{
			if (xorMask != null)
				for (int i = 0; i < bytes.Length; i++)
					bytes[i] ^= xorMask[i];
			return bytes;
		}
	}
}
