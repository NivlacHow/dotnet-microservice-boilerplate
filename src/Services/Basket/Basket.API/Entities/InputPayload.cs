﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileProcessor.Entities
{
    public class InputPayload
    {
        public string FileLocation { get; set; }
        public string TargetFileExt { get; set; }
    }
}