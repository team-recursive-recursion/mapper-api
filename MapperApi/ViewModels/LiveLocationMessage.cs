using System;

public class IncomingMessageViewModel
{
    public enum DeviceType
    {
        APPLICATION, MONITOR
    }

    public DeviceType? Device { get; set; }
    public Guid? UserID { get; set; }
    public String Location { get; set; }
}
