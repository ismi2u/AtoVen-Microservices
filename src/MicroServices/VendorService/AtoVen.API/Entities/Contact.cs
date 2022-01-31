﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AtoVen.API.Entities
{
    public class Contact 
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }
        public int CompanyID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FormofAddress { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }

       
    }

    public class ContactDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FormofAddress { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }


    }
}
