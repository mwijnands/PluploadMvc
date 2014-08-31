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

        private readonly string _fileNamePath;
        private readonly string _contentTypeFileNamePath;
        private readonly string _fileName;
        private readonly FileStream _fileStream;
        private readonly string _contentType;
        private readonly int _contentLength;

        public PluploadFile(string fileNamePath)
        {
            this._fileNamePath = fileNamePath;
            this._contentTypeFileNamePath = string.Concat(fileNamePath, ContentTypeExtension);
            this._fileName = Path.GetFileName(fileNamePath);
            this._fileStream = File.OpenRead(fileNamePath);
            this._contentLength = Convert.ToInt32(this._fileStream.Length);

            if (File.Exists(this._contentTypeFileNamePath))
            {
                this._contentType = File.ReadAllText(this._contentTypeFileNamePath);
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

                if (File.Exists(this._fileNamePath))
                {
                    try
                    {
                        File.Delete(this._fileNamePath);
                    }
                    catch (Exception)
                    {
                    }
                }

                if (File.Exists(this._contentTypeFileNamePath))
                {
                    try
                    {
                        File.Delete(this._contentTypeFileNamePath);
                    }
                    catch (Exception)
                    {
                    }
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
