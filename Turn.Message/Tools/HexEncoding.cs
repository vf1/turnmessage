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
using System.Text;

public static class HexEncoding
{
	public static string ToHexString(this byte[] bytes)
	{
		StringBuilder sb = new StringBuilder(bytes.Length * 2);

		for (int i = 0; i < bytes.Length; i++)
		{
			sb.Append(GetHexChar((byte)(bytes[i] >> 4)));
			sb.Append(GetHexChar(bytes[i]));
		}

		return sb.ToString();
	}

	private static char GetHexChar(this byte b)
	{
		b &= 0x0f;

		switch (b)
		{
			case 0x00:
				return '0';
			case 0x01:
				return '1';
			case 0x02:
				return '2';
			case 0x03:
				return '3';
			case 0x04:
				return '4';
			case 0x05:
				return '5';
			case 0x06:
				return '6';
			case 0x07:
				return '7';
			case 0x08:
				return '8';
			case 0x09:
				return '9';
			case 0x0A:
				return 'a';
			case 0x0B:
				return 'b';
			case 0x0C:
				return 'c';
			case 0x0D:
				return 'd';
			case 0x0E:
				return 'e';
			case 0x0F:
			default:
				return 'f';
		}
	}
}
