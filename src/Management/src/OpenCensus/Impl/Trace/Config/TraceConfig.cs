﻿// Copyright 2017 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

namespace Steeltoe.Management.Census.Trace.Config
{
    [Obsolete("Use OpenCensus project packages")]
    public sealed class TraceConfig : TraceConfigBase
    {
        public TraceConfig()
        {
            _activeTraceParams = TraceParams.DEFAULT;
        }

        private ITraceParams _activeTraceParams;

        public override ITraceParams ActiveTraceParams
        {
            get
            {
                return _activeTraceParams;
            }
        }

        public override void UpdateActiveTraceParams(ITraceParams traceParams)
        {
            _activeTraceParams = traceParams;
        }
    }
}