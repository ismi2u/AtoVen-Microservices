using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtoVen.API.Entities
{
    public class Approver
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public virtual ApproverRole ApproverRole { get; set; }
        public int ApproverRoleID { get; set; }
        public virtual ApproverLevel ApproverLevel { get; set; }
        public int ApproverLevelID { get; set; }
        public bool IsEnabled { get; set; }

    }

    public class ApproverDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int ApproverRoleID { get; set; }
        public string ApproverRole { get; set; }
        public int ApproverLevelID { get; set; }

        public int ApproverLevel { get; set; }
        public bool IsEnabled { get; set; }

    }
}
