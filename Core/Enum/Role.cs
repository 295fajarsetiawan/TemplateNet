using System.Runtime.Serialization;

namespace Core.Enum;
public enum UserType
{
    [EnumMember(Value = "ADMIN")]
    ADMIN = 1,

    [EnumMember(Value = "TEACHER")]
    TEACHER = 2,

    [EnumMember(Value = "STUDENT")]
    STUDENT = 3,

    [EnumMember(Value = "PARENT")]
    PARENT = 4,

    [EnumMember(Value = "ACCOUNTANT")]
    ACCOUNTANT = 5,

    [EnumMember(Value = "LIBRARIAN")]
    LIBRARIAN = 6,

    [EnumMember(Value = "RECEPTIONIST")]
    RECEPTIONIST = 7
}