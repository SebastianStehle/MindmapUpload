// ==========================================================================
// IFileStorage.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;
using System.Threading.Tasks;

namespace SE.Upload.Web.Contracts
{
    public interface IFileStorage
    {
        Task<string> UploadAsync(FileUpload file);

        Task<FileUpload> GetFileAsync(string id);
    }
}
