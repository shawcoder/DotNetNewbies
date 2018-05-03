namespace Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class GuestResponse
	{
		public Guid Id { get; set; }

		[Required(ErrorMessage = "Please enter your name")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please enter your email address")]
		[RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email address")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Please enter your phone number")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Please specify whether you'll attend or not")]
		public bool? WillAttend { get; set; }

	}

	public static class GuestResponseHelper
	{
		private static void FromTo(GuestResponse aFrom, GuestResponse aTo)
		{
			aTo.Id = aFrom.Id;
			aTo.Name = aFrom.Name;
			aTo.Email = aFrom.Email;
			aTo.Phone = aFrom.Phone;
			aTo.WillAttend = aFrom.WillAttend;
		}

		public static void AssignTo(this GuestResponse aFrom, GuestResponse aTo)
		{
			FromTo(aFrom, aTo);
		}

		public static void AssignFrom(this GuestResponse aTo, GuestResponse aFrom)
		{
			FromTo(aFrom, aTo);
		}

	}
}