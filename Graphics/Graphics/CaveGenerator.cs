using System;

namespace Graphics
{
    internal class CaveGenerator
    {
	    private readonly int _caveMaxWidth;
	    private readonly int _caveRough;
	    private readonly int _caveWind;
	    private readonly int _caveMinWidth;
		private int _caveW;

	    private readonly Random _rng;
	    private int _caveStart;

	    public CaveGenerator(int caveStart, int caveMinWidth, int caveMaxWidth, int caveRoughness, int caveWindyness)
		{
			_caveStart = caveStart;
			_caveMinWidth = caveMinWidth;
			_caveMaxWidth = caveMaxWidth;
			_caveRough = caveRoughness;
			_caveWind = caveWindyness;
			
			_caveW = caveMinWidth;

			_rng = new Random((int)DateTime.Now.Ticks);
		}

	    public CaveLine GenerateLine()
	    {
			var line = new CaveLine();

		    if (_rng.Next(0, 100) <= _caveWind)
		    {
			    _caveStart += _rng.Next(-2, 2);

			    if (_caveStart < 0)
				    _caveStart = 0;
			    if (_caveStart > _caveMaxWidth - _caveMinWidth)
					_caveStart = _caveMaxWidth - _caveMinWidth;

		    }

			line.StartPosition = _caveStart;

		    if (_rng.Next(0, 100) <= _caveRough)
		    {
			    _caveW += _rng.Next(-2, 2);

			    if (_caveW < _caveMinWidth)
				    _caveW = _caveMinWidth;
			    if (_caveW > _caveMaxWidth - line.StartPosition)
				    _caveW = _caveMaxWidth - line.StartPosition;

		    }

		    line.EndPosition = line.StartPosition + _caveW;

			return line;
	    }

	    public class CaveLine
	    {
		    public int StartPosition { get; set; }
			public int EndPosition { get; set; }
	    }
    }
}
