using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class HighscoreRecord : IComparable<HighscoreRecord>
{
	public string playerName = "";
	public int score = 0;

	public int CompareTo(HighscoreRecord compareScore)
	{
		if (compareScore == null)
		{
			return 1;
		}
		else
		{
			return this.score.CompareTo(compareScore.score);
		}
	}
}
