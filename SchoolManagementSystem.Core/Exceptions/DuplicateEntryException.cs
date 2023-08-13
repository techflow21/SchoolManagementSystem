using System;
namespace SchoolManagementSystem.Core.Exceptions
{
    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException(string msg) : base(msg)
        {
        }

        public DuplicateEntryException(string msg, Exception innerException) : base(msg,innerException)
        {
        }
    }
}

