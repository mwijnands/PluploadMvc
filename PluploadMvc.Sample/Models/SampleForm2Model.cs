using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace XperiCode.PluploadMvc.Sample.Models
{
    public class SampleForm2Model
    {
        public SampleForm2Model()
        {
            Files = new PluploadFileCollection();
            OtherFiles = new PluploadFileCollection();
        }

        [Display(Name = "Some text")]
        public string Text { get; set; }

        [Display(Name = "Some files")]
        public PluploadFileCollection Files { get; set; }

        [Display(Name = "Some other files")]
        public PluploadFileCollection OtherFiles { get; set; }
    }
}
