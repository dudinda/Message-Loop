namespace MessageLoop.Web.Common.Code.Enums
{
    [Flags]
    public enum Messages
    {
        Unknown = 0,

        Ok      = 1 << 0,
        Fail    = 1 << 1,

        Message_1 = 1 << 2,
        Message_2 = 1 << 3,
        Message_3 = 1 << 4,

        Cancel = 1 << 31
    }
}
