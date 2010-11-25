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
using System.Security.Cryptography;

namespace Turn.Message
{
	public enum TurnMessageRfc
	{
		Rfc3489,
		Rfc5389,
		MsTurn,
	}

	public enum CreditalsType
	{
		LongTerm,
		ShortTerm,
		MsAvedgea,
	}

	public class TurnMessage
	{
		private const int HeaderLength = 20;
		private int messageIntegrityStartOffset = -1;
		private int fingerprintStartOffset = -1;
		private byte[] storedBytes = null;
		private Attribute[] allAttributes = null;

		//public static byte[] ServerErrorMessage = new byte[] { 0x00, 0x00, 0x00, 0x00 };

		public TurnMessage()
		{
		}

		public MessageType MessageType { get; set; }

		/// <summary>
		/// This length does not include the 20 byte header.
		/// </summary>
		public UInt16 MessageLength { get; set; }
		public TransactionId TransactionId { get; set; }

		public AlternateServer AlternateServer { get; set; }
		public Bandwidth Bandwidth { get; set; }
		public Data Data { get; set; }
		public DestinationAddress DestinationAddress { get; set; }
		public ErrorCodeAttribute ErrorCodeAttribute { get; set; }
		public Fingerprint Fingerprint { get; set; }
		public Lifetime Lifetime { get; set; }
		public MagicCookie MagicCookie { get; set; }
		public MappedAddress MappedAddress { get; set; }
		public MessageIntegrity MessageIntegrity { get; set; }
		public MsVersion MsVersion { get; set; }
		public MsSequenceNumber MsSequenceNumber { get; set; }
		public Nonce Nonce { get; set; }
		public Realm Realm { get; set; }
		public RemoteAddress RemoteAddress { get; set; }
		public Software Software { get; set; }
		public UnknownAttributes UnknownAttributes { get; set; }
		public Username Username { get; set; }
		public XorMappedAddress XorMappedAddress { get; set; }
		public MsUsername MsUsername { get; set; }

		// rfc3489
		public ChangedAddress ChangedAddress { get; set; }
		public ChangeRequest ChangeRequest { get; set; }
		public ResponseAddress ResponseAddress { get; set; }
		public SourceAddress SourceAddress { get; set; }
		public ReflectedFrom ReflectedFrom { get; set; }
		public Password Password { get; set; }

		public static TurnMessage Parse(byte[] bytes, TurnMessageRfc rfc)
		{
			return Parse(bytes, 0, bytes.Length, rfc);
		}

		public static TurnMessage Parse(byte[] bytes, int length, TurnMessageRfc rfc)
		{
			return Parse(bytes, 0, length, rfc);
		}

		public static TurnMessage Parse(byte[] bytes, int startIndex, int length, TurnMessageRfc rfc)
		{
			TurnMessage message = new TurnMessage();

			message.ParseHeader(bytes, startIndex, length);
			message.ParseAttributes(bytes, startIndex, length, rfc);

			return message;
		}

		public static MessageType? SafeGetMessageType(byte[] bytes, int length, int startIndex)
		{
			if (length < 2)
				return null;

			UInt16 messageType = bytes.BigendianToUInt16(startIndex);

			if (Enum.IsDefined(typeof(MessageType), (Int32)messageType) == false)
				return null;

			return (MessageType)messageType;
		}

		public static TransactionId SafeGetTransactionId(byte[] bytes, int length)
		{
			TransactionId id = null;

			try
			{
				if (length >= HeaderLength)
					id = new TransactionId(bytes, TransactionId.DefaultStartIndex);
			}
			catch
			{
			}

			return id;
		}

		public static bool IsTurnMessage(byte[] bytes, int startIndex, int length)
		{
			if (length < HeaderLength + 0x0008)
				return false;

			if (bytes.BigendianToUInt16(HeaderLength + startIndex) != (UInt16)AttributeType.MagicCookie)
				return false;

			if (bytes.BigendianToUInt16(HeaderLength + startIndex + 0x0002) != 0x0004)
				return false;

			if (bytes.BigendianToUInt32(HeaderLength + startIndex + 0x0004) != MagicCookie.MagicCookieValue)
				return false;

			return true;
		}

		public byte[] GetBytes(byte[] key2)
		{
			return GetBytes(null, 0, null, key2, CreditalsType.MsAvedgea, 0);
		}

		public void GetBytes(byte[] bytes, int startIndex, byte[] key2)
		{
			GetBytes(bytes, startIndex, null, key2, CreditalsType.MsAvedgea, 0);
		}

		public byte[] GetBytes(string password, bool longOrShortTerm)
		{
			return GetBytes(null, 0, password, null, longOrShortTerm ? CreditalsType.LongTerm : CreditalsType.ShortTerm, 0);
		}

		public byte[] GetBytes(string password, bool longOrShortTerm, byte paddingByte)
		{
			return GetBytes(null, 0, password, null, longOrShortTerm ? CreditalsType.LongTerm : CreditalsType.ShortTerm, paddingByte);
		}

		public bool IsMessageIntegrityValid(string password, bool longOrShortTerm)
		{
			if (MessageIntegrity == null)
				return false;

			byte[] messageIntegrity = ComputeMessageIntegrity(password, longOrShortTerm);

			return messageIntegrity.AreArraysEqual(MessageIntegrity.Value);
		}

		public bool IsValidMessageIntegrity(byte[] key2)
		{
			if (MessageIntegrity == null)
				return false;
			if (MsUsername == null)
				return false;

			byte[] messageIntegrity = ComputeMessageIntegrity(key2);

			return messageIntegrity.AreArraysEqual(MessageIntegrity.Value);
		}

		public bool IsValidMsUsername(byte[] key1)
		{
			if (MsUsername == null)
				return false;

			using (HMACSHA1 sha1 = new HMACSHA1(key1))
			{
				sha1.ComputeHash(MsUsername.Value, 0, MsUsername.TokenBlobLength);
				return sha1.Hash.AreArraysEqual(MsUsername.Value, MsUsername.TokenBlobLength, MsUsername.HashOfTokenBlobLength);
			}
		}

		public byte[] ComputeMsPasswordBytes(byte[] key2)
		{
			return ComputeMsPasswordBytes(key2, MsUsername.Value);
		}

		public static byte[] ComputeMsPasswordBytes(byte[] key2, byte[] msUsername)
		{
			using (HMACSHA1 sha1 = new HMACSHA1(key2))
			{
				sha1.ComputeHash(msUsername);
				return sha1.Hash;
			}
		}

		public byte[] ComputeMessageIntegrity(string password, bool longOrShortTerm)
		{
			if (longOrShortTerm)
				return ComputeLongTermMessageIntegrity(storedBytes, messageIntegrityStartOffset, password);
			else
				return ComputeShortTermMessageIntegrity(storedBytes, messageIntegrityStartOffset, password);
		}


		public byte[] ComputeMessageIntegrity(byte[] key2)
		{
			return ComputeMsAvedgeaMessageIntegrity(storedBytes, 0, storedBytes.Length, key2, Realm.Value, MsUsername.Value);
		}

		public UInt32 ComputeFingerprint()
		{
			return ComputeFingerprint(storedBytes, fingerprintStartOffset);
		}

		public bool IsRfc3489(AttributeType attributeType)
		{
			switch (attributeType)
			{
				case AttributeType.ChangedAddress:
				case AttributeType.ChangeRequest:
				case AttributeType.ResponseAddress:
				case AttributeType.SourceAddress:
				case AttributeType.ReflectedFrom:
				case AttributeType.Password:
					return true;
			}

			return false;
		}

		public bool IsAttributePaddingEnabled()
		{
			return MsVersion == null && IsAttributePaddingDisabled == false;
		}

		public bool IsAttributePaddingDisabled
		{
			private get;
			set;
		}

		private byte[] GetBytes(byte[] bytes, int startIndex, string password, byte[] key2, CreditalsType creditalsType, byte paddingByte)
		{
			if (bytes == null)
			{
				ComputeMessageLength();
				bytes = new byte[MessageLength + HeaderLength];
			}

			GetHeaderBytes(bytes, ref startIndex);
			GetAttributesBytes(bytes, ref startIndex, password, key2, creditalsType, paddingByte);

			return bytes;
		}

		private void CreateAttributesArray()
		{
			allAttributes = new Attribute[]
			{
				MagicCookie,

				MsVersion,
				AlternateServer,
				Bandwidth,
				RemoteAddress,
				Data,
				DestinationAddress,
				ErrorCodeAttribute,
				Lifetime,
				MappedAddress,
				Software,
				UnknownAttributes,
				Username,
				MsUsername,
				Nonce,
				Realm,
				XorMappedAddress,
				MsSequenceNumber,

				// rfc3489
				ChangedAddress,
				ChangeRequest,
				ResponseAddress,
				SourceAddress,
				ReflectedFrom,
				Password,

				// message signatures
				MessageIntegrity,
				Fingerprint,
			};
		}

		public void ComputeMessageLength()
		{
			CreateAttributesArray();

			MessageLength = 0;
			foreach (var attribute in allAttributes)
				if (attribute != null && attribute.Ignore == false)
				{
					MessageLength += attribute.TotalLength;

					if (IsAttributePaddingEnabled())
					{
						if (MessageLength % 4 > 0)
							MessageLength += (UInt16)(4 - MessageLength % 4);
					}
				}
		}

		public int TotalMessageLength
		{
			get
			{
				return MessageLength + HeaderLength;
			}
		}

		private void GetAttributesBytes(byte[] bytes, ref int startIndex, string password, byte[] key2, CreditalsType creditalsType, byte paddingByte)
		{
			int messageStartIndex = startIndex - HeaderLength;

			foreach (var attribute in allAttributes)
			{
				if (attribute != null && attribute.Ignore == false)
				{
					if (attribute is MessageIntegrity)
					{
						switch (creditalsType)
						{
							case CreditalsType.ShortTerm:
								(attribute as MessageIntegrity).Value = ComputeShortTermMessageIntegrity(bytes, startIndex, password);
								break;
							case CreditalsType.LongTerm:
								(attribute as MessageIntegrity).Value = ComputeLongTermMessageIntegrity(bytes, startIndex, password);
								break;
							case CreditalsType.MsAvedgea:
								(attribute as MessageIntegrity).Value = ComputeMsAvedgeaMessageIntegrity(bytes, messageStartIndex, startIndex, key2, Realm.Value, MsUsername.Value);
								break;
						}
					}

					if (attribute is Fingerprint)
						(attribute as Fingerprint).Value = ComputeFingerprint(bytes, startIndex);

					if (attribute is XorMappedAddress)
						(attribute as XorMappedAddress).GetBytes(bytes, ref startIndex, TransactionId);
					else
						attribute.GetBytes(bytes, ref startIndex);

					if (IsAttributePaddingEnabled())
					{
						if (startIndex % 4 > 0)
						{
							if (paddingByte != 0)
								PadAttribute(paddingByte, bytes, startIndex);
							startIndex += 4 - startIndex % 4;
						}
					}
				}
			}
		}

		private void PadAttribute(byte paddingByte, byte[] bytes, int startIndex)
		{
			while (startIndex % 4 > 0)
			{
				bytes[startIndex] = paddingByte;
				startIndex++;
			}
		}

		private void GetHeaderBytes(byte[] bytes, ref int startIndex)
		{
#if DEBUG
			if (Enum.IsDefined(typeof(MessageType), MessageType) == false)
				throw new NullReferenceException("Message type was not setted");
#endif

			Array.Copy(((UInt16)MessageType).GetBigendianBytes(), 0, bytes, startIndex + 0, 2);
			Array.Copy(((UInt16)MessageLength).GetBigendianBytes(), 0, bytes, startIndex + 2, 2);
			Array.Copy(TransactionId.Value, 0, bytes, startIndex + 4, TransactionId.Length);

			startIndex += HeaderLength;
		}

		private void ParseHeader(byte[] bytes, int startIndex, int length)
		{
			if (length < HeaderLength)
				throw new TurnMessageException(ErrorCode.BadRequest, @"Too short message, less than 20 bytes (header size)");

			UInt16 messageType = bytes.BigendianToUInt16(startIndex + 0);
			if ((messageType & 0xC000) != 0)
				throw new TurnMessageException(ErrorCode.BadRequest, @"The most significant two bits of Message Type MUST be set to zero");
			if (Enum.IsDefined(typeof(MessageType), (Int32)messageType) == false)
				throw new TurnMessageException(ErrorCode.BadRequest, @"Unknow message type");
			this.MessageType = (MessageType)messageType;

			this.MessageLength = bytes.BigendianToUInt16(startIndex + 2);
			if (this.MessageLength != (length - HeaderLength))
				throw new TurnMessageException(ErrorCode.BadRequest, string.Format(@"Wrong message length, wait for {0} actual is {1}", length - HeaderLength, this.MessageLength));

			this.TransactionId = new TransactionId(bytes, startIndex + 4);
		}

		private void ParseAttributes(byte[] bytes, int startIndex, int length, TurnMessageRfc rfc)
		{
			int currentIndex = startIndex + HeaderLength;
			int endIndex = startIndex + length;

			while (currentIndex < endIndex)
			{
				UInt16 attributeType1 = bytes.BigendianToUInt16(currentIndex);
				if (Enum.IsDefined(typeof(AttributeType), (Int32)attributeType1) == false)
					throw new TurnMessageException(ErrorCode.UnknownAttribute);
				AttributeType attributeType = (AttributeType)attributeType1;

				if (rfc == TurnMessageRfc.Rfc3489)
					if (IsRfc3489(attributeType))
						throw new TurnMessageException(ErrorCode.UnknownAttribute);



				if (attributeType == AttributeType.Fingerprint)
				{
					if (Fingerprint == null)
					{
						if (storedBytes == null)
						{
							storedBytes = new byte[length];
							Array.Copy(bytes, startIndex, storedBytes, 0, length);
						}
						fingerprintStartOffset = currentIndex - startIndex;
						Fingerprint = new Fingerprint();
						Fingerprint.Parse(bytes, ref currentIndex);
					}
					else
					{
						Attribute.Skip(bytes, ref currentIndex);
					}
				}
				else if (MessageIntegrity != null)
				{
					Attribute.Skip(bytes, ref currentIndex);
				}
				else
				{
					switch (attributeType)
					{
						case AttributeType.AlternateServer:
							AlternateServer = new AlternateServer();
							AlternateServer.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.Bandwidth:
							Bandwidth = new Bandwidth();
							Bandwidth.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.Data:
							Data = new Data();
							Data.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.DestinationAddress:
							DestinationAddress = new DestinationAddress();
							DestinationAddress.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.ErrorCode:
							ErrorCodeAttribute = new ErrorCodeAttribute();
							ErrorCodeAttribute.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.Fingerprint:
							break;

						case AttributeType.Lifetime:
							Lifetime = new Lifetime();
							Lifetime.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.MagicCookie:
							MagicCookie = new MagicCookie();
							MagicCookie.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.MappedAddress:
							MappedAddress = new MappedAddress();
							MappedAddress.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.MessageIntegrity:
							messageIntegrityStartOffset = currentIndex - startIndex;
							if (storedBytes == null)
							{
								if (rfc == TurnMessageRfc.MsTurn)
								{
									storedBytes = new byte[GetPadded64(messageIntegrityStartOffset)];
									Array.Copy(bytes, startIndex, storedBytes, 0, messageIntegrityStartOffset);
								}
								else
								{
									storedBytes = new byte[length];
									Array.Copy(bytes, startIndex, storedBytes, 0, length);
								}
							}
							MessageIntegrity = new MessageIntegrity();
							MessageIntegrity.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.MsVersion:
							MsVersion = new MsVersion();
							MsVersion.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.MsSequenceNumber:
							MsSequenceNumber = new MsSequenceNumber();
							MsSequenceNumber.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.RemoteAddress:
							RemoteAddress = new RemoteAddress();
							RemoteAddress.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.Software:
							Software = new Software();
							Software.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.UnknownAttributes:
							UnknownAttributes = new UnknownAttributes();
							// Not Implemented
							UnknownAttributes.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.Username:
							if (MsVersion != null)
							{
								MsUsername = new MsUsername();
								MsUsername.Parse(bytes, ref currentIndex);
							}
							else
							{
								Username = new Username();
								Username.Parse(bytes, ref currentIndex);
							}
							break;


						// ietf-mmusic-ice
						case AttributeType.Priority:
						case AttributeType.UseCandidate:
						case AttributeType.IceControlled:
						case AttributeType.IceControlling:
							Attribute.Skip(bytes, ref currentIndex);
							break;


						// rfc3489
						case AttributeType.ChangedAddress:
							ChangedAddress = new ChangedAddress();
							ChangedAddress.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.ChangeRequest:
							ChangeRequest = new ChangeRequest();
							ChangeRequest.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.ResponseAddress:
							ResponseAddress = new ResponseAddress();
							ResponseAddress.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.SourceAddress:
							SourceAddress = new SourceAddress();
							SourceAddress.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.ReflectedFrom:
							ReflectedFrom = new ReflectedFrom();
							ReflectedFrom.Parse(bytes, ref currentIndex);
							break;

						case AttributeType.Password:
							Password = new Password();
							Password.Parse(bytes, ref currentIndex);
							break;



						default:
							if (rfc == TurnMessageRfc.MsTurn)
							{
								switch (attributeType)
								{
									case AttributeType.Nonce:
										Nonce = new Nonce(rfc);
										Nonce.Parse(bytes, ref currentIndex);
										break;

									case AttributeType.Realm:
										Realm = new Realm(rfc);
										Realm.Parse(bytes, ref currentIndex);
										break;

									case AttributeType.XorMappedAddress:
										XorMappedAddress = new XorMappedAddress(rfc);
										XorMappedAddress.Parse(bytes, ref currentIndex, TransactionId);
										break;

									default:
										throw new NotImplementedException();
								}
							}
							else
							{
								switch (attributeType)
								{
									case AttributeType.NonceStun:
										Nonce = new Nonce(rfc);
										Nonce.Parse(bytes, ref currentIndex);
										break;

									case AttributeType.RealmStun:
										Realm = new Realm(rfc);
										Realm.Parse(bytes, ref currentIndex);
										break;

									case AttributeType.XorMappedAddressStun:
										XorMappedAddress = new XorMappedAddress(rfc);
										XorMappedAddress.Parse(bytes, ref currentIndex, TransactionId);
										break;

									default:
										throw new NotImplementedException();
								}
							}
							break;
					}
				}

				if (rfc != TurnMessageRfc.MsTurn)
				{
					if (currentIndex % 4 > 0)
						currentIndex += 4 - currentIndex % 4;
				}
			}
		}

		protected static UInt32 ComputeFingerprint(byte[] bytes, int length)
		{
			// Length correction is requred like in ComputeMessageIntegrity, but
			// I assume that Fingerprint is last item always. It's true in most cases.

			using (Crc32 crc32 = new Crc32())
			{
				crc32.ComputeHash(bytes, 0, length);

				return crc32.CrcValue ^ 0x5354554e;
			}
		}

		protected static byte[] ComputeMsAvedgeaMessageIntegrity(byte[] bytes, int startIndex, int stopIndex, byte[] key2, string realm1, byte[] msUsername)
		{
			UTF8Encoding utf8 = new UTF8Encoding();

			byte[] realm = utf8.GetBytes(":" + realm1 + ":");
			byte[] password = ComputeMsPasswordBytes(key2, msUsername);

			byte[] keySource = new byte[msUsername.Length + realm.Length + password.Length];

			Array.Copy(msUsername, 0, keySource, 0, msUsername.Length);
			Array.Copy(realm, 0, keySource, msUsername.Length, realm.Length);
			Array.Copy(password, 0, keySource, keySource.Length - password.Length, password.Length);

			using (MD5 md5 = MD5.Create())
			using (HMACSHA1 sha1 = new HMACSHA1(md5.ComputeHash(keySource)))
			{
				byte[] text;

				int length = GetPadded64(stopIndex - startIndex);

				if (startIndex + length <= bytes.Length)
				{
					for (int i = stopIndex; i < startIndex + length; i++)
						bytes[i] = 0;
					text = bytes;
				}
				else
				{
					if (startIndex > 0)
						throw new NotImplementedException();

					text = new byte[GetPadded64(bytes.Length)];
					Array.Copy(bytes, text, bytes.Length);
				}

				return sha1.ComputeHash(text, startIndex, length);
			}
		}

		protected byte[] ComputeShortTermMessageIntegrity(byte[] bytes, int length, string password)
		{
			UTF8Encoding utf8 = new UTF8Encoding();

			byte[] key = utf8.GetBytes(password);

			return ComputeMessageIntegritySha1(bytes, length, key);
		}

		protected byte[] ComputeLongTermMessageIntegrity(byte[] bytes, int length, string password)
		{
			UTF8Encoding utf8 = new UTF8Encoding();

			using (MD5 md5 = MD5.Create())
			{
				byte[] key = md5.ComputeHash(utf8.GetBytes(Username.Value + ":" + Realm.Value + ":" + password.SASLprep()));
				return ComputeMessageIntegritySha1(bytes, length, key);
			}
		}

		protected static byte[] ComputeMessageIntegritySha1(byte[] bytes, int length, byte[] sha1Key)
		{
			if (bytes == null)
				throw new ArgumentNullException(@"bytes");
			if (bytes.Length < 4)
				throw new ArgumentException("Too short array", @"bytes");

			byte b3 = bytes[2];
			byte b4 = bytes[3];

			try
			{
				// 0x0018 - MessageIntegrity.TotalLength
				((UInt16)(length - HeaderLength + 0x0018)).GetBigendianBytes().CopyTo(bytes, 2);

				using (HMACSHA1 sha1 = new HMACSHA1(sha1Key))
					return sha1.ComputeHash(bytes, 0, length);
			}
			finally
			{
				bytes[2] = b3;
				bytes[3] = b4;
			}
		}

		private static int GetPadded64(int value)
		{
			return ((value + 63) / 64) * 64;
		}
	}
}
