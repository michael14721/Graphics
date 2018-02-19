using System;
using System.Collections.Generic;
using System.Text;

namespace Graphics
{
    internal static class GraphicExtensions
    {
	    public static void Set(this Graphic g, int x, int y, short attr, char c)
	    {
		    g.At(x, y).Attributes = attr;
		    g.At(x, y).Char.UnicodeChar = c;
	    }

	    public static void Set(this Graphic g, int x, int y, short attr, byte c)
	    {
		    g.At(x, y).Attributes = attr;
		    g.At(x, y).Char.AsciiChar = c;
	    }
	}
}
