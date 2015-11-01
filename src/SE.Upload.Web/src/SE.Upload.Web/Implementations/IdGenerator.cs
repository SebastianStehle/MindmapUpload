// ==========================================================================
// IdGenerator.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;

namespace SE.Upload.Web.Implementations
{
    public static class IdGenerator
    {
        public static string GenerateId()
        {
            return Encode(Guid.NewGuid());
        }

        public static string Encode(Guid guid)
        {
            string encoded = Convert.ToBase64String(guid.ToByteArray());

            encoded = encoded
              .Replace("/", "_")
              .Replace("+", "-");

            return encoded.Substring(0, 22);
        }
    }
}
