﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace XperiCode.PluploadMvc
{
    public class PluploadContext : IPluploadContext, IDisposable
    {
        private readonly HttpContextBase _httpContext;
        private readonly IDictionary<string, PluploadFile> _files;

        internal PluploadContext(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
            _files = new Dictionary<string, PluploadFile>(StringComparer.OrdinalIgnoreCase);

            _httpContext.DisposeOnPipelineCompleted(this);
        }

        public void SaveChunk(HttpPostedFileBase file, string reference, string fileName, int chunk, int chunks)
        {
            GuardInvalidReference(reference);

            string uploadPath = GetUploadPath(reference);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string fileSavePath = Path.Combine(uploadPath, Path.GetFileName(fileName));

            string partialFileSavePath = string.Concat(fileSavePath, PluploadFile.PartialFileExtension);
            using (var fileStream = chunk == 0 ? File.Create(partialFileSavePath) : File.Open(partialFileSavePath, FileMode.Append))
            {
                file.InputStream.Seek(0, SeekOrigin.Begin);
                file.InputStream.CopyTo(fileStream);
            }

            if (chunk == chunks - 1)
            {
                if (File.Exists(fileSavePath))
                {
                    File.Delete(fileSavePath);
                }

                File.Move(partialFileSavePath, fileSavePath);

                string contentTypeSavePath = string.Concat(fileSavePath, PluploadFile.ContentTypeExtension);
                File.WriteAllText(contentTypeSavePath, MimeMapping.GetMimeMapping(fileName));
            }
        }

        public void SaveFile(HttpPostedFileBase file, string reference)
        {
            GuardInvalidReference(reference);

            string uploadPath = GetUploadPath(reference);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string fileSavePath = Path.Combine(uploadPath, Path.GetFileName(file.FileName));
            using (var fileStream = File.Create(fileSavePath))
            {
                file.InputStream.Seek(0, SeekOrigin.Begin);
                file.InputStream.CopyTo(fileStream);
            }

            string contentTypeSavePath = string.Concat(fileSavePath, PluploadFile.ContentTypeExtension);
            File.WriteAllText(contentTypeSavePath, MimeMapping.GetMimeMapping(file.FileName));
        }

        public IEnumerable<HttpPostedFileBase> GetFiles(string reference)
        {
            GuardInvalidReference(reference);

            string uploadPath = GetUploadPath(reference);
            if (!Directory.Exists(uploadPath))
            {
                yield break;
            }

            var fileNamePaths = Directory.GetFiles(uploadPath).Where(p => !p.EndsWith(PluploadFile.ContentTypeExtension) && !p.EndsWith(PluploadFile.PartialFileExtension));
            foreach (var fileNamePath in fileNamePaths)
            {
                PluploadFile file;

                if (_files.ContainsKey(fileNamePath))
                {
                    file = _files[fileNamePath];
                }
                else
                {
                    file = new PluploadFile(fileNamePath, reference);
                    _files.Add(fileNamePath, file);
                }

                yield return file;
            }
        }

        public void DeleteFiles(string reference)
        {
            GuardInvalidReference(reference);

            var files = _files.Where(f => f.Value.Reference == reference).ToArray();
            foreach (var file in files)
            {
                file.Value.Dispose();
                _files.Remove(file);
            }

            string uploadPath = GetUploadPath(reference);
            if (!Directory.Exists(uploadPath))
            {
                return;
            }

            var fileNamePaths = Directory.GetFiles(uploadPath);
            foreach (var fileNamePath in fileNamePaths)
            {
                try
                {
                    File.Delete(fileNamePath);
                }
                catch (IOException)
                {
                    // Files could always be in use by virusscanners and what not.. So ignore it.
                }
            }

            try
            {
                Directory.Delete(uploadPath, true);
            }
            catch (IOException)
            {
                // Files could always be in use by virusscanners and what not.. So ignore it.
            }
        }

        public void DeleteFiles(PluploadFileCollection collection)
        {
            this.DeleteFiles(collection.Reference);
        }

        public void DeleteFile(string reference, string fileName)
        {
            GuardInvalidReference(reference);

            var files = _files.Where(f => f.Value.Reference == reference && f.Value.FileName == fileName).ToArray();
            foreach (var file in files)
            {
                file.Value.Dispose();
                _files.Remove(file);
            }

            string uploadPath = GetUploadPath(reference);
            if (!Directory.Exists(uploadPath))
            {
                return;
            }

            var fileNamePaths = Directory.GetFiles(uploadPath).Where(p => !p.EndsWith(PluploadFile.ContentTypeExtension) && !p.EndsWith(PluploadFile.PartialFileExtension));
            foreach (var fileNamePath in fileNamePaths.Where(x => Path.GetFileName(x) == fileName))
            {
                try
                {
                    File.Delete(fileNamePath);
                    File.Delete(string.Concat(fileNamePath, PluploadFile.ContentTypeExtension));
                }
                catch (IOException)
                {
                    // Files could always be in use by virusscanners and what not.. So ignore it.
                }
            }
        }

        public void DeleteFile(PluploadFileCollection collection, string fileName)
        {
            DeleteFile(collection.Reference, fileName);
        }

        public void DeleteFile(PluploadFile file)
        {
            DeleteFile(file.Reference, file.FileName);
        }

        public static void CleanupFiles()
        {
            string uploadPath = PluploadConfiguration.UploadPath;
            if (uploadPath[0] == '~')
            {
                uploadPath = HostingEnvironment.MapPath(uploadPath);
            }
            if (!Directory.Exists(uploadPath))
            {
                return;
            }

            var fileNamePaths = Directory.GetFiles(uploadPath, "*", SearchOption.AllDirectories);
            foreach (var fileNamePath in fileNamePaths)
            {
                try
                {
                    File.Delete(fileNamePath);
                }
                catch (IOException)
                {
                    // Files could always be in use by virusscanners and what not.. So ignore it.
                }
            }

            var directories = Directory.GetDirectories(uploadPath, "*", SearchOption.TopDirectoryOnly);
            foreach (var directory in directories)
            {
                try
                {
                    Directory.Delete(directory, true);
                }
                catch (IOException)
                {
                    // Files could always be in use by virusscanners and what not.. So ignore it.
                }
            }
        }

        public static bool ValidateReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                return false;
            }
            return !reference.Any(c => Path.GetInvalidFileNameChars().Contains(c));
        }

        public static void GuardInvalidReference(string reference)
        {
            if (!ValidateReference(reference))
            {
                throw new ArgumentException("reference cannot be empty or contain invalid filename chars.", "reference");
            }
        }

        internal string GetUploadPath(string reference)
        {
            string uploadPath = PluploadConfiguration.UploadPath;
            if (uploadPath[0] == '~')
            {
                uploadPath = _httpContext.Server.MapPath(uploadPath);
            }

            return Path.Combine(uploadPath, reference);
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
                foreach (var file in _files)
                {
                    file.Value.Dispose();
                }
                _files.Clear();
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
