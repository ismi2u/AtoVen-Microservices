using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataService.Entities
{
    public class Contact
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }
        public int CompanyID { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        public string LastName { get; set; }


        [Column(TypeName = "varchar(max)")]
        public string Address { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Designation { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Department { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string PhoneNo { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string MobileNo { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string FaxNo { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Language { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Country { get; set; }


    }

    public class ContactDTO
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }


    }

    public class ContactPostDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }


    }


    public class ContactPutDTO
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }


    }

    public class ContactVM
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }


    }
}
    
