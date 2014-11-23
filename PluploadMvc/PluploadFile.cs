using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace XperiCode.PluploadMvc
{
    public class PluploadFile : HttpPostedFileBase, IDisposable
    {
        public const string ContentTypeExtension = ".contenttype";
        public const string PartialFileExtension = ".partial";

        private readonly string _fileName;
        private readonly FileStream _fileStream;
        private readonly string _contentType;
        private readonly int _contentLength;
        private readonly string _reference;

        public PluploadFile(string fileNamePath, string reference)
        {
            PluploadContext.GuardInvalidReference(reference);

            if (!File.Exists(fileNamePath))
            {
                return;
            }

            this._reference = reference;
            this._fileName = Path.GetFileName(fileNamePath);
            this._fileStream = File.OpenRead(fileNamePath);
            this._contentLength = Convert.ToInt32(this._fileStream.Length);

            string contentTypeFileNamePath = string.Concat(fileNamePath, ContentTypeExtension);
            if (File.Exists(contentTypeFileNamePath))
            {
                this._contentType = File.ReadAllText(contentTypeFileNamePath);
            }
        }

        internal string Reference 
        { 
            get
            {
                return _reference;
            }
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

        public override void SaveAs(string filename)
        {
            using (var fileStream = File.Create(filename))
            {
                this._fileStream.Seek(0, SeekOrigin.Begin);
                this._fileStream.CopyTo(fileStream);
            }
        }

        #region IDisposable Members

        private Boolean _disposed;

        private void Dispose(Boolean disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this._fileStream != null)
                {
                    this._fileStream.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
