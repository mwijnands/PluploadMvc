using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XperiCode.PluploadMvc
{
    public class PluploadFileCollection : ReadOnlyCollection<PluploadFile>
    {
        public PluploadFileCollection() : this(new List<PluploadFile>(), Guid.NewGuid().ToString())
        {
        }

        public PluploadFileCollection(string reference) : this(new List<PluploadFile>(), reference)
        {
        }

        public PluploadFileCollection(IList<PluploadFile> files, string reference) : base(files)
        {
            this.Reference = reference;
        }

        internal string Reference { get; set; }

        public override string ToString()
        {
            return this.Reference;
        }
    }
}
