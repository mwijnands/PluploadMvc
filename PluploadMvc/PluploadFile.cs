using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XperiCode.PluploadMvc
{
    public class PluploadFile : HttpPostedFileBase
    {
        private readonly string _fileName;
        private readonly Stream _fileStream;
        private readonly string _contentType;
        private readonly int _contentLength;

        public PluploadFile(string fileName, Stream fileStream, string contentType)
        {
            this._fileName = fileName;
            this._fileStream = fileStream;
            this._contentType = contentType;
            this._contentLength = Convert.ToInt32(fileStream.Length);
        }

        public override string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public override Stream InputStream
        {
            get
            {
                return _fileStream;
            }
        }

        public override int ContentLength
        {
            get
            {
                return _contentLength;
            }
        }

        public override string ContentType
        {
            get
            {
                return _contentType;
            }
        }
        // TODO: override contentlength and contenttype, save these values to a json file when uploaded (next te uploaded file)
        //       check SaveAs method.
    }
}
