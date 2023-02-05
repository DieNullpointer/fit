using System;
using System.Runtime.Serialization;

namespace FitManager.Application.Services
{
    [Serializable]
    internal class PdfGeneratorException : Exception
    {
        public PdfGeneratorException()
        {
        }

        public PdfGeneratorException(string? message) : base(message)
        {
        }

        public PdfGeneratorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PdfGeneratorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}