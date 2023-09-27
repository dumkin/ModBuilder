using System;

namespace Curse.Integration.C.Exceptions;

internal class MissingApiKeyException : Exception
{
    public MissingApiKeyException(string message) : base(message)
    {
    }
}