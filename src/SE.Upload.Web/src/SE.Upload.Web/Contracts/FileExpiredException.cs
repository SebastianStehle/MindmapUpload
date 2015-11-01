// ==========================================================================
// FileExpiredException.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;
using System.Runtime.Serialization;

namespace SE.Upload.Web.Contracts
{
    public class FileExpiredException : Exception
    {
        public FileExpiredException()
        {
        }

        public FileExpiredException(string message)
            : base(message)
        {
        }

        public FileExpiredException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FileExpiredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
