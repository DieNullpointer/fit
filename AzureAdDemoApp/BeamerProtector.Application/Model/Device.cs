using System;

namespace BeamerProtector.Application.Model
{
    public class Device
    {
        public Device(Guid guid)
        {
            Guid = guid;
        }

#pragma warning disable CS8618

        protected Device()
        { }

#pragma warning restore CS8618
        public int Id { get; private set; }
        public Guid Guid { get; set; }
    }
}