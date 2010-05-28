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
	public enum ErrorCode
	{
		BadRequest = 400,
		Unauthorized = 401,
		UnknownAttribute = 420,

		/// <summary>
		/// The Binding Request did contain a MESSAGE-INTEGRITY
		/// attribute, but it used a shared secret that has expired.
		/// </summary>
		StaleCredentials = 430,

		/// <summary>
		/// The Binding Request contained a MESSAGE-INTEGRITY 
		/// attribute, but the HMAC failed verification.
		/// </summary>
		IntegrityCheckFailure = 431,

		/// <summary>
		/// The USERNAME attribute was not present in the request.
		/// </summary>
		MissingUsername = 432,

		/// <summary>
		/// The REALM attribute was not present in the request.
		/// </summary>
		MissingRealm = 434,

		/// <summary>
		/// The NONCE attribute was not present in the request.
		/// </summary>
		MissingNonce = 435,

		/// <summary>
		/// The USERNAME supplied in the Shared Secret Request is 
		/// not known in the given REALM.
		/// </summary>
		UnknownUsername= 436,

		/// <summary>
		/// A Send Request was received by the server, but there 
		/// is no binding in place for the source 5-tuple. 
		/// </summary>
		NoBinding = 437,


		/// <summary>
		/// Nonce value was not valid
		/// </summary>
		StaleNonce = 438,
		

		/// <summary>
		/// A Send Request was received by the server over TCP, but 
		/// the server wasn't able to transmit the data to the
		/// requested destination.
		/// </summary>
		// SendFailed=438, overlaped by StaleNonce

		/// <summary>
		/// A Set Active Destination request was received
		/// by the server.  However, a previous request was sent within the
		/// last three seconds, and the server is still transitioning to that
		/// active destination.  Please repeat the request once three seconds
		/// have elapsed.
		/// </summary>
		Transitioning = 439,

		/// <summary>
		/// A Set Active Destination request was
		/// received by the server.  However, the requested destination has
		/// not been one corresponding to the destination of a Send Request,
		/// and has not been one for which packets or a connection attempt
		/// have been received.
		/// </summary>
		NoDestination = 440,

		/// <summary>
		/// A TURN request was received for an allocated
		/// binding, but it did not use the same username and password that
		/// were used in the allocation.  The client must supply the proper
		/// credentials, and if it cannot, it should teardown its binding,
		/// allocate a new one time password, and try again.
		/// </summary>
		WrongUsername = 441,

		/// <summary>
		/// The Shared Secret request has to be sent over TLS, but was not received over TLS.
		/// </summary>
		UseTLS = 433,

		/// <summary>
		/// The server has suffered a temporary error. The client should try again.
		/// </summary>
		ServerError = 500,

		/// <summary>
		/// The server is refusing to fulfill the request. The client should not retry.
		/// </summary>
		GlobalFailure = 600,
	}

	public static class ReasonPhrase
	{
		public static string GetReasonPhrase(this ErrorCode errorCode)
		{
			switch(errorCode)
			{
				case ErrorCode.BadRequest:
					return @"Bad Request";
				case ErrorCode.Unauthorized:
					return @"Unauthorized";
				case ErrorCode.UnknownAttribute:
					return @"Unknown Attribute";
				case ErrorCode.StaleCredentials:
					return @"Stale Credentials";
				case ErrorCode.IntegrityCheckFailure:
					return @"Integrity Check Failure";
				case ErrorCode.MissingUsername:
					return @"Missing Username";
				case ErrorCode.MissingRealm:
					return @"Missing Realm";
				case ErrorCode.MissingNonce:
					return @"Missing Nonce";
				case ErrorCode.UnknownUsername:
					return @"Unknown Username";
				case ErrorCode.NoBinding:
					return @"No Binding";
				case ErrorCode.StaleNonce:
					return @"Stale Nonce";
				case ErrorCode.Transitioning:
					return @"Transitioning";
				case ErrorCode.NoDestination:
					return @"No Destination";
				case ErrorCode.WrongUsername:
					return @"Wrong Username";
				case ErrorCode.UseTLS:
					return @"Use TLS";
				case ErrorCode.ServerError:
					return @"Server Error";
				case ErrorCode.GlobalFailure:
					return @"Global Failure";
			}

			return @"";
		}
	}
}
