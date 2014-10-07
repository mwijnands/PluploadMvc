using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace XperiCode.PluploadMvc.Sample.Models
{
    public class SampleForm2Model : IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!OtherFiles.Any())
            {
                yield return new ValidationResult("The Some other files field is required.", new[] { "OtherFiles" });
            }
        }
    }
}
