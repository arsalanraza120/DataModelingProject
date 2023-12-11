using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DataBaseContext
{
    public class ConfigCredential
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CredentialName { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string? DbName { get; set; }
        public string DbType { get; set; }
        public string? ServiceName { get; set; }
        public string? InstanceName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
    }

}
