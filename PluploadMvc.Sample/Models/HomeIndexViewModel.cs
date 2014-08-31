using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace XperiCode.PluploadMvc.Sample.Models
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel()
        {
            UploadReference = Guid.NewGuid();
        }

        public string Text { get; set; }

        [Display(Name = "Files")]
        public Guid UploadReference { get; set; }
    }
}
