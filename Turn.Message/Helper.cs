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
	static class Helpers
	{
		public static bool AreArraysEqual(this byte[] array1, byte[] array2)
		{
			return AreArraysEqual(array1, array2, 0, array2.Length);
		}

		public static bool AreArraysEqual(this byte[] array1, byte[] array2, int startIndex2, int length2)
		{
			if (array1.Length != length2)
				return false;

			for (int i = 0; i < array1.Length; i++)
				if (array1[i] != array2[startIndex2 + i])
					return false;

			return true;
		}
	}
}
