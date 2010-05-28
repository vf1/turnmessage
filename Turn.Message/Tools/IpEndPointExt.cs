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

public static class IpEndPointExt
{
	public static void CopyFrom(this IPEndPoint to, IPEndPoint from)
	{
		to.Address = from.Address;
		to.Port = from.Port;
	}

	public static IPEndPoint MakeCopy(this IPEndPoint from)
	{
		return new IPEndPoint(from.Address, from.Port);
	}

	public static bool IsEqual(this IPEndPoint ip1, IPEndPoint ip2)
	{
		return 
			ip1.AddressFamily == ip2.AddressFamily &&
			ip1.Port == ip2.Port &&
			ip1.Address.Equals(ip2.Address);
	}
}
