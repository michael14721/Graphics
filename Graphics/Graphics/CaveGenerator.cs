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

			_rng = new Random();
		}

	    public CaveLine GenerateLine()
	    {
		    if (_rng.Next(0, 100) <= _caveWind)
		    {
			    _caveStart += _rng.Next(-20, 20) / 10;

			    if (_caveStart < 0)
				    _caveStart = 0;
			    if (_caveStart > _caveMaxWidth - _caveMinWidth)
					_caveStart = _caveMaxWidth - _caveMinWidth;

		    }
			
		    if (_rng.Next(0, 100) <= _caveRough)
		    {
			    _caveW += _rng.Next(-20, 20) / 10;

			    if (_caveW < _caveMinWidth)
				    _caveW = _caveMinWidth;
			    if (_caveW > _caveMaxWidth - _caveStart)
				    _caveW = _caveMaxWidth - _caveStart;

		    }
			
		    return new CaveLine
		    {
			    StartPosition = _caveStart,
			    EndPosition = _caveStart + _caveW
		    };
		}

	    public class CaveLine
	    {
		    public int StartPosition { get; set; }
			public int EndPosition { get; set; }
	    }
    }
}
