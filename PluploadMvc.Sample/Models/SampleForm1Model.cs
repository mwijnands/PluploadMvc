using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace XperiCode.PluploadMvc.Sample.Models
{
    public class SampleForm1Model
    {
        public SampleForm1Model()
        {
            UploadReference1 = Guid.NewGuid().ToString();
            UploadReference2 = "12345";
        }

        [Display(Name = "Some text")]
        public string Text { get; set; }

        [Display(Name = "Some files")]
        public string UploadReference1 { get; set; }

        [Display(Name = "Some other files")]
        public string UploadReference2 { get; set; }
    }
}
