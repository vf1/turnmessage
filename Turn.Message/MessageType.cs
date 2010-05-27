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
	enum MessageType
	{
		AllocateRequest = 0x0003,
		AllocateResponse = 0x0103,
		AllocateErrorResponse = 0x0113,
		SendRequest = 0x0004,
		DataIndication = 0x0115,
		SetActiveDestinationRequest = 0x0006,
		SetActiveDestinationResponse = 0x0106,
		SetActiveDestinationErrorResponse = 0x0116,

		// The following TURN message types are not supported
		// by this extension and the server MUST NOT send them: 
		SendRequestResponse = 0x0104,
		SendRequestErrorResponse = 0x0114,

		// In addition, this extension does not support the
		// shared secret authentication mechanism. 
		// The following shared secret messages MUST NOT be used
		// by either the client or server: 
		SharedSecretRequest = 0x0002,
		SharedSecretResponse = 0x0102,
		SharedSecretErrorResponse = 0x0112,

		// STUN message types
		BindingRequest = 0x0001,
		BindingResponse = 0x0101,
		BindingErrorResponse = 0x0111,
	}

	static class MessageTypeHelpers
	{
		public static MessageType GetErrorResponseType(this MessageType requestType)
		{
			switch (requestType)
			{
				case MessageType.AllocateRequest:
					return MessageType.AllocateErrorResponse;
				case MessageType.SendRequest:
					return MessageType.SendRequestErrorResponse;
				case MessageType.BindingRequest:
					return MessageType.BindingErrorResponse;
				case MessageType.SetActiveDestinationRequest:
					return MessageType.SetActiveDestinationErrorResponse;
				case MessageType.SharedSecretRequest:
					return MessageType.SharedSecretErrorResponse;
			}

			throw new NotImplementedException();
		}
	}
}
