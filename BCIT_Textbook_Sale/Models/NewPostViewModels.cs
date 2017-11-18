using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BCIT_Textbook_Sale.Models
{
    //TODO:
    public class NewPostViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} can not be empty.", MinimumLength = 1)]
        [Display(Name = "Post Title")]
        public string PostTitle { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} can not be empty.", MinimumLength = 1)]
        [Display(Name = "Course Number")]
        public string CourseNumber { get; set; }

        [Required]
        [Display(Name = "Buy or Sell")]
        public string BuyOrSell { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} can not be empty.", MinimumLength = 1)]
        [Display(Name = "Detail")]
        public string NewPostDetail { get; set; }
    }
    

}
