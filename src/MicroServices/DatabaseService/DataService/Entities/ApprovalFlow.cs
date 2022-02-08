using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DataService.Entities
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
        public int ApprovalStatus { get; set; } 

        public bool IsDuplicateEntry{ get; set; }
        public DateTime? LevelApprovedDate { get; set; }

    }


    public class ApprovalFlowDTO
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }

        public string CompanyRegisterNo { get; set; }

        public DateTime RecordDate { get; set; }

        public int ApprovalStatus { get; set; }
        public bool IsDuplicateEntry { get; set; }


    }

  
    public enum ApprovalStatusType
    {
        Pending =1,
        Approved,
        Rejected
 
    }


}
