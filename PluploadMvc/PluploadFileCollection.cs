using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XperiCode.PluploadMvc
{
    public class PluploadFileCollection : ReadOnlyCollection<PluploadFile>
    {
        public PluploadFileCollection() : this(new List<PluploadFile>(), Guid.NewGuid())
        {
        }

        public PluploadFileCollection(IList<PluploadFile> list, Guid reference) : base(list)
        {
            this.Reference = reference;
        }

        internal Guid Reference { get; set; }

        public override string ToString()
        {
            return this.Reference.ToString();
        }
    }
}
