namespace RepositoryMemory.Test
{
	using System;
	using System.Collections.Generic;
	using Contracts;
	using Entities;
	using FluentAssertions;
	using Xunit;

	public class TestRepositoryMemory
	{

		/*
		int Count();
		void Add(GuestResponse aGuestResponse);
		void Update(GuestResponse aGuestResponse);
		void Delete(Guid aGuesResponseId);
		GuestResponse Get(Guid aGuestResponseId);
		List<GuestResponse> GetAll();
		// Discovered the need for the following during testing
		void DeleteAll();
		bool Exists(Guid aId);
		*/

		private GuestResponse MakeNewRecord(bool aWillAttend = true)
		{
			GuestResponse vResult =
				new GuestResponse
				{
					Id = Guid.NewGuid()
					, Email = "Someone@email.com"
					, Name = "Some Name"
					, Phone = "123-4567"
					, WillAttend = aWillAttend
				};
			return vResult;
		}

		private void PrepareStorage()
		{
			IPartyInvitesR vRepository = new PartyInvitesR();
			vRepository.DeleteAll();
		}

		[Fact]
		public void TestCount()
		{
			// Arrange
			const int EXPECTED = 1;

			PrepareStorage();
			IPartyInvitesR vRepository = new PartyInvitesR();
			int vInitialCount = vRepository.Count();
			vRepository.Add(MakeNewRecord());

			// Act
			int vResult = vRepository.Count();

			// Assert
			vInitialCount.Should().Be(0, "Initial count. Repository was cleared");
			vResult.Should().Be(EXPECTED);
		}

		[Fact]
		public void TestExists()
		{
			// Arrange
			PrepareStorage();
			IPartyInvitesR vRepository = new PartyInvitesR();
			GuestResponse vNewRec = MakeNewRecord();
			vRepository.Add(vNewRec);

			// Act
			bool vResult = vRepository.Exists(vNewRec.Id);

			// Assert
			vResult.Should().BeTrue();
		}

		[Fact]
		public void TestAdd()
		{
			// Arrange
			PrepareStorage();
			int vExpected = 1;
			IPartyInvitesR vRepository = new PartyInvitesR();
			vRepository.DeleteAll();
			int vCount = vRepository.Count();
			GuestResponse vRecord = MakeNewRecord();

			// Act
			vRepository.Add(vRecord);
			int vResult = vRepository.Count();

			// Assert
			vCount.Should().Be(0);
			vResult.Should().Be(vExpected);
		}

		[Fact]
		public void TestUpdate()
		{
			// Arrange
			const bool EXPECTED = false;

			PrepareStorage();
			IPartyInvitesR vRepository = new PartyInvitesR();
			vRepository.DeleteAll();
			GuestResponse vNewRec = MakeNewRecord(true);
			vRepository.Add(vNewRec);
			GuestResponse vUpdatedRec = new GuestResponse();
			vUpdatedRec.AssignFrom(vNewRec);
			vUpdatedRec.WillAttend = EXPECTED;

			// Act
			vRepository.Update(vUpdatedRec);
			GuestResponse vResult = vRepository.Get(vUpdatedRec.Id);

			// Assert
			vResult.WillAttend.Should().Be(EXPECTED);
		}

		[Fact]
		public void TestGet()
		{
			// Arrange
			PrepareStorage();
			IPartyInvitesR vRepository = new PartyInvitesR();
			GuestResponse vNewRec = MakeNewRecord();
			vRepository.Add(vNewRec);

			// Act
			GuestResponse vResult = vRepository.Get(vNewRec.Id);

			// Assert
			vResult.Id.Should().Be(vNewRec.Id);
			vResult.Email.Should().Be(vNewRec.Email);
			vResult.Name.Should().Be(vNewRec.Name);
			vResult.Phone.Should().Be(vNewRec.Phone);
			vResult.WillAttend.Should().Be(vNewRec.WillAttend);
		}

		[Fact]
		public void TestGetAll()
		{
			// Arrange
			const int COUNT = 5;

			PrepareStorage();
			IPartyInvitesR vRepository = new PartyInvitesR();
			GuestResponse[] vNewRec = new GuestResponse[5];
			for (int vLcv = 0; vLcv < COUNT; vLcv++)
			{
				vNewRec[vLcv] = MakeNewRecord();
				vRepository.Add(vNewRec[vLcv]);
			}

			// Act
			List<GuestResponse> vResult = vRepository.GetAll();

			// Assert
			for (int vLcv = 0; vLcv < COUNT; vLcv++)
			{
				vResult[vLcv].Id.Should().Be(vNewRec[vLcv].Id);
				vResult[vLcv].Email.Should().Be(vNewRec[vLcv].Email);
				vResult[vLcv].Name.Should().Be(vNewRec[vLcv].Name);
				vResult[vLcv].Phone.Should().Be(vNewRec[vLcv].Phone);
				vResult[vLcv].WillAttend.Should().Be(vNewRec[vLcv].WillAttend);
			}
		}

		[Fact]
		public void TestDelete()
		{
			// Arrange
			const int EXPECTED = 0;

			PrepareStorage();
			GuestResponse vNewRec = MakeNewRecord();
			IPartyInvitesR vRepository = new PartyInvitesR();
			int vInitialCount = vRepository.Count();
			vRepository.Add(vNewRec);
			int vStartCount = vRepository.Count();

			// Act
			vRepository.Delete(vNewRec.Id);
			int vResult = vRepository.Count();

			// Assert
			vInitialCount.Should().Be(0);
			vStartCount.Should().Be(1);
			vResult.Should().Be(EXPECTED);
		}

		[Fact]
		public void TestDeleteAll()
		{
			// Arrange
			const int EXPECTED = 0;
			const int START_COUNT = 5;
			IPartyInvitesR vRepository = new PartyInvitesR();
			List<GuestResponse> vList = vRepository.GetAll();
			foreach (GuestResponse vRec in vList)
			{
				vRepository.Delete(vRec.Id);
			}
			int vInitialCount = vRepository.Count();  // Should be zero
			GuestResponse vNewRec = MakeNewRecord();
			vRepository.Add(vNewRec);
			vNewRec = MakeNewRecord();
			vRepository.Add(vNewRec);
			vNewRec = MakeNewRecord();
			vRepository.Add(vNewRec);
			vNewRec = MakeNewRecord();
			vRepository.Add(vNewRec);
			vNewRec = MakeNewRecord();
			vRepository.Add(vNewRec);
			int vStartCount = vRepository.Count();

			// Act
			vRepository.DeleteAll();

			// Assert
			vInitialCount.Should().Be(0);
			vStartCount.Should().Be(START_COUNT);
			vRepository.Count().Should().Be(EXPECTED);
		}

	}
}