﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Steeltoe.Management.Census.Stats
{
    [Obsolete("Use OpenCensus project packages")]
    public interface IStatsRecorder
    {
        IMeasureMap NewMeasureMap();
    }
}
