// ==========================================================================
// EntityNotValidException.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;

namespace SE.Upload.Web.Pipeline
{
    public class EntityNotValidException : Exception
    {
        public EntityNotValidException()
        {
        }

        public EntityNotValidException(string message)
            : base(message)
        {
        }

        public EntityNotValidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}