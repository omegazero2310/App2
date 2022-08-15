using SQLite;
namespace App2.Models
{

    /// <summary>
    ///     Save Employee common info
    ///   <br />
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// annv3 8/9/2022 created
    /// annv3 15/08/2022 add attribute for sqllite
    /// </Modified>
    [Table("Employee")]
    public class Employee
    {
        [PrimaryKey]
        public string Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        [MaxLength(250), Unique]
        public string ExtraInfo { get; set; }

    }
}
