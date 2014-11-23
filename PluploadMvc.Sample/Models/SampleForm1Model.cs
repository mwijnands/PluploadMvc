using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace XperiCode.PluploadMvc.Sample.Models
{
    public class SampleForm1Model
    {
        public SampleForm1Model()
        {
            UploadReference1 = Guid.NewGuid();
            UploadReference2 = Guid.NewGuid();
        }

        [Display(Name = "Some text")]
        public string Text { get; set; }

        [Display(Name = "Some files")]
        public Guid UploadReference1 { get; set; }

        [Display(Name = "Some other files")]
        public Guid UploadReference2 { get; set; }
    }
}
