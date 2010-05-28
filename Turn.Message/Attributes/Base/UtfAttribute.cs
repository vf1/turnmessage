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

namespace Turn.Message
{
	public abstract class UtfAttribute : Attribute
	{
		private byte[] utf8Value;
		private string stringValue;
		private UTF8Encoding utf8Encoding;

		public UtfAttribute()
		{
		}

		protected string StringValue
		{
			get
			{
				return stringValue;
			}
			set
			{
				utf8Value = null;
				stringValue = value;
			}
		}

		protected byte[] Utf8Value
		{
			get
			{
				if (utf8Value == null)
					utf8Value = Utf8Encoding.GetBytes(stringValue);

				return utf8Value;
			}
		}

		protected UTF8Encoding Utf8Encoding
		{
			get
			{
				if (utf8Encoding == null)
					utf8Encoding = new UTF8Encoding();

				return utf8Encoding;
			}
		}

		protected void ParseUtf8String(byte[] bytes, ref int startIndex, int length)
		{
			StringValue = Utf8Encoding.GetString(bytes, startIndex, length);
			startIndex += length;
		}
	}
}
