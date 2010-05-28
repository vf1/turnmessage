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
	public enum AttributeType
	{
		MappedAddress = 0x0001,
		Username = 0x0006,
		MessageIntegrity = 0x0008,
		ErrorCode = 0x0009,
		UnknownAttributes = 0x000A,
		Lifetime = 0x000D,
		AlternateServer = 0x000E,
		MagicCookie = 0x000F,
		Bandwidth = 0x0010,
		DestinationAddress = 0x0011,
		RemoteAddress = 0x0012,
		Data = 0x0013,
		Nonce = 0x0014,
		Realm = 0x0015,
		XorMappedAddress = 0x8020,

		XorMappedAddressStun = 0x0020,
		RealmStun = 0x0014, 
		NonceStun = 0x0015,

		// The following optional attributes are also supported in
		// this extension. Any other attributes from the optional
		// attribute space SHOULD be ignored. 
		MsVersion = 0x8008,
		MsSequenceNumber = 0x8050,
		MsServiceQuality = 0x8055,

		// rfc 5389
		Software = 0x8022,
		Fingerprint = 0x8028,

		// ietf-mmusic-ice
		Priority = 0x0024,			// 32 bit unsigned integer
		UseCandidate = 0x0025,		// has no content
		IceControlled = 0x8029,		// 64 bit unsigned integer
		IceControlling = 0x802a,	// 64 bit unsigned integer

		// rfc3489 - obsolete
		ResponseAddress = 0x0002,
		ChangeRequest = 0x0003,
		SourceAddress = 0x0004,
		ChangedAddress = 0x0005,
		Password = 0x0007,
		ReflectedFrom = 0x000B,
	}
}
