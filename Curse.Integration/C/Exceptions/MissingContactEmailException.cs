using System;

namespace Curse.Integration.C.Exceptions;

internal class MissingContactEmailException : Exception
{
    public MissingContactEmailException(string message) : base(message)
    {
    }
}