﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.IE.Qwiq.Exceptions
{
    internal class AggregateExceptionExploder : IExceptionExploder
    {
        public IEnumerable<Exception> Explode(Exception exception)
        {
            return new [] {exception}.OfType<AggregateException>().SelectMany(ae => ae.InnerExceptions);
        }
    }
}