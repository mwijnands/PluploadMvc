using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XperiCode.PluploadMvc
{
    public class PluploadFileCollection : ReadOnlyCollection<PluploadFile>
    {
        public PluploadFileCollection() : this(new List<PluploadFile>())
        {
        }

        public PluploadFileCollection(IList<PluploadFile> list) : base(list)
        {
            this.Reference = Guid.NewGuid();
        }

        public Guid Reference { get; set; }
    }
}
