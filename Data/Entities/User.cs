using System.ComponentModel.DataAnnotations.Schema;
using Core.Enum;
using Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Data.Entities;

[Index(nameof(Username), IsUnique = true)]
public class User : BaseEntities
{
    [Column("username")]
    public string Username { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
    
}