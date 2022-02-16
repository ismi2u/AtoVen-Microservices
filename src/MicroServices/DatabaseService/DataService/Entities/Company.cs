using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataService.Entities
{
    public class Company
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(300)")]
        public string CompanyName { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string CommercialRegistrationNo { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Language { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Country { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Region { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string District { get; set; }


        [Column(TypeName = "varchar(20)")]
        public string PostalCode { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string City { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Street { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string HouseNo { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Building { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Floor { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Room { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string? POBox { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string PhoneNo { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string FaxNumber { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string MobileNo { get; set; }


        [Column(TypeName = "varchar(200)")]
        public string Website { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string VendorType { get; set; }


        [Column(TypeName = "varchar(200)")]
        public string AccountGroup { get; set; }


        [Column(TypeName = "varchar(max)")]
        public string Notes { get; set; }


        [Column(TypeName = "varchar(100)")]
        public string VatNo { get; set; }
        //public string DocumentIDs { get; set; }


        public bool? IsVendorInitiated { get; set; }
        public DateTime RecordDate { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }


        public List<Contact> ListOfCompanyContacts { get; set; }

        public List<Bank> ListOfCompanyBanks { get; set; }
    }

    public class CompanyDTO
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CommercialRegistrationNo { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNo { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string? POBox { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Website { get; set; }
        public string VendorType { get; set; }
        public string AccountGroup { get; set; }
        public string Notes { get; set; }
        public string VatNo { get; set; }

        //public string DocumentIDs { get; set; }

        public bool? IsVendorInitiated { get; set; }
        public DateTime RecordDate { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public List<ContactDTO> ListOfCompanyContacts { get; set; }

        public List<BankDTO> ListOfCompanyBanks { get; set; }
    }


    public class CompanyPostDTO
    {
        public string CompanyName { get; set; }

        public string CommercialRegistrationNo { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNo { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string? POBox { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Website { get; set; }
        public string VendorType { get; set; }
        public string AccountGroup { get; set; }
        public string Notes { get; set; }
        public string VatNo { get; set; }

        //public string DocumentIDs { get; set; }

        public bool? IsVendorInitiated { get; set; }

        public List<ContactPostDTO> ListOfCompanyContacts { get; set; }

        public List<BankPostDTO> ListOfCompanyBanks { get; set; }
    }


    public class CompanyPutDTO
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CommercialRegistrationNo { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNo { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string? POBox { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNumber { get; set; }
        public string MobileNo { get; set; }
        public string Website { get; set; }
        public string VendorType { get; set; }
        public string AccountGroup { get; set; }
        public string Notes { get; set; }
        public string VatNo { get; set; }

        //public string DocumentIDs { get; set; }

        public int ApprovalFlowID { get; set; }

        public bool? IsVendorInitiated { get; set; }

        public List<ContactPutDTO> ListOfCompanyContacts { get; set; }

        public List<BankPutDTO> ListOfCompanyBanks { get; set; }
    }


    public class CompanyVM
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CommercialRegistrationNo { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNo { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string? POBox { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Website { get; set; }
        public string VendorType { get; set; }
        public string AccountGroup { get; set; }
        public string Notes { get; set; }
        public string VatNo { get; set; }


        public List<ContactVM> ListOfCompanyContacts { get; set; }

        public List<BankVM> ListOfCompanyBanks { get; set; }
    }
}
