using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;//Added for access to data annotations and validation.

namespace StoreFront.Data.EF//.StoreFrontMetadata
{

    #region Product Metadata

    public class ProductMetadata
    {
        //PK/Identity: Commented out.
        //public int ProductID { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "*Must be 100 characters or less")]
        [Display(Name = "Product")]
        public string ProductName { get; set; }

        [DisplayFormat(NullDisplayText = "N/A")]
        [UIHint("MultilineText")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, double.MaxValue, ErrorMessage = "*Must be a valid number 0 or larger.")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "*")]
        public int ProductStatusID { get; set; }

        [DisplayFormat(NullDisplayText = "N/A")]
        [Range(0, int.MaxValue, ErrorMessage = "*Must be a valid number 0 or larger.")]
        [Display(Name = "Units in Stock")]
        public Nullable<int> UnitsInStock { get; set; }

        [DisplayFormat(NullDisplayText = "N/A", DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Released")]
        public Nullable<System.DateTime> DateReleased { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Image")]
        public string ProductImage { get; set; }
    }

    [MetadataType(typeof(ProductMetadata))]
    public partial class Product { }

    #endregion

    #region Category Metadata

    public class CategoryMetadata
    {
        //public int CategoryID { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "*Must be 50 characters or less")]
        [Display(Name = "Category")]
        [UIHint("MultilineText")]
        public string CategoryName { get; set; }
    }

    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category { }

    #endregion

    #region ProductStatus Metadata

    public class ProductStatusMetadata
    {
        //public int ProductStatusID { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(25, ErrorMessage = "*Must be 25 characters or less")]
        [Display(Name = "Product Status")]
        public string Status { get; set; }
    }

    [MetadataType(typeof(ProductStatusMetadata))]
    public partial class ProductStatus { }

    #endregion

    #region Employee Metadata

    public class EmployeeMetadata
    {
        //public int EmployeeID { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(25, ErrorMessage = "*Must be 25 characters or less")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(25, ErrorMessage = "*Must be 25 characters or less")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DisplayFormat(NullDisplayText = "N/A")]
        [Range(1, int.MaxValue, ErrorMessage = "Must be a valid number 0 or larger.")]
        public Nullable<int> DirectReportID { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birthday")]
        public System.DateTime BirthDate { get; set; }

        [DisplayFormat(NullDisplayText = "N/A", DataFormatString = "{0:d}", ApplyFormatInEditMode = true),]
        [Display(Name = "Hire Date")]
        public Nullable<System.DateTime> HireDate { get; set; }

        //TODO: RegEx for Phone Number?
        [Required(ErrorMessage = "*")]
        [Range(1, int.MaxValue, ErrorMessage = "Must be a valid number 0 or larger.")]
        [Display(Name = "Phone Number")]
        public int Phone { get; set; }

        //TODO: Fix Employee Photo Datatype and re-import
        //public byte[] Photo { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(1, int.MaxValue, ErrorMessage = "Must be a valid number 0 or larger.")]
        public int DepartmentID { get; set; }
    }

    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee { }

    #endregion

    #region Department Metadata

    public class DepartmentMetadata
    {
        //public int DepartmentID { get; set; }
        //TODO: Fix the name of the Department Name Column

        [Required(ErrorMessage = "*")]
        [StringLength(25, ErrorMessage = "*Must be 12500 characters or less")]
        [Display(Name = "Department")]
        public string Department1 { get; set; }
    }

    [MetadataType(typeof(DepartmentMetadata))]
    public partial class Department { }

    #endregion

}
