// Company: INNOVT
// Project: Innovt.Common
// Created By: Michel Borges
// Date: 2016/10/19

using System;

namespace Innovt.Core.Exceptions
{
    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "<Pending>")]
    public class FatalException : BaseException
    {
        public FatalException(string message):base(message)
        {
            
        }

        public FatalException(string message,Exception ex) : base(message,ex)
        {

        }

        public FatalException(Exception ex) : base(ex)
        {

        }
    }
}