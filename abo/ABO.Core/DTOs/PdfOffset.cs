using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Core.DTOs
{
    public class PdfOffset
    {
        public PdfOffset(float x, float y)
        {
            X = x;
            Y = y;
            Scale = 0.3f;
        }

        public PdfOffset(float x, float y, float scale)
        {
            X = x;
            Y = y;
            Scale = scale;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Scale { get; set; }
    }
}
