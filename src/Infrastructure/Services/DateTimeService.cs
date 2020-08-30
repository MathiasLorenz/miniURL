using System;
using MiniURL.Application.Common.Interfaces;

namespace MiniURL.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}