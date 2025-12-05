namespace LibraryWebApp.Models;

public static class BookingStatus
{
    public const string Pending = "Pending";
    public const string Approved = "Approved";
    public const string Refused = "Refused";
    public const string Completed = "Completed";

    public static bool IsActive(string status) => status is Pending or Approved;

    public static bool IsFinal(string status) => status is Refused or Completed;
}
