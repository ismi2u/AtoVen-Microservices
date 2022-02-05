using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AtoVen.API.Entities
{
    public class ApprovalFlow
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Company Company { get; set; }
        public int CompanyID { get; set; }
        public DateTime RecordDate { get; set; }
        public string ApproverEmail { get; set; }
        public int ApproverLevel{ get; set; }
        public DateTime? LevelApprovedDate { get; set; }

    }
}
