namespace RepositoryMemory
{
	using System;
	using System.Collections.Generic;
	using Contracts;
	using Entities;

	public class PartyInvitesR: IPartyInvitesR
	{
		private static List<GuestResponse> _Storage = new List<GuestResponse>();
		
		public void Add(GuestResponse aGuestResponse)
		{
			_Storage.Add(aGuestResponse);
		}

		public void Update(GuestResponse aGuestResponse)
		{
			throw new NotImplementedException();
		}

		public void Delete(int aGuesResponseId)
		{
			throw new NotImplementedException();
		}

		public GuestResponse Get(int aGuestResponseId)
		{
			throw new NotImplementedException();
		}

		public List<GuestResponse> GetAll() { return _Storage; }

	}
}