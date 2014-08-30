using System;
using System.Collections.Generic;
using System.Linq;

namespace XperiCode.PluploadMvc.Sample.Models
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel()
        {
            UploadReference = Guid.NewGuid();
        }

        public Guid UploadReference { get; set; }
    }
}