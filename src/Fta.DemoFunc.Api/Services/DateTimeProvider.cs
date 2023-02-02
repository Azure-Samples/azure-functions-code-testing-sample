using Fta.DemoFunc.Api.Interfaces;
using System;

namespace Fta.DemoFunc.Api.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
