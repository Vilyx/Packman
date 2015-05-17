using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ObjectToStoreStringUtil
{
	public static string HighscoreToString(List<HighscoreRecord> scoreRecords)
	{
		string higscoreString = "";
		bool isFirst = true;
		foreach (HighscoreRecord record in scoreRecords)
		{
			if (!isFirst)
			{
				higscoreString += "|";
			}
			else
			{
				isFirst = false;
			}
			higscoreString += record.playerName + "," + record.score;
		}
		return higscoreString;
	}
	public static List<HighscoreRecord> StringToHighscore(string scoreString)
	{
		List<HighscoreRecord> scoreRecords = new List<HighscoreRecord>();
		string[] strs = scoreString.Split('|');
		foreach (string strRec in strs)
		{
			HighscoreRecord record = new HighscoreRecord();
			string[] props = strRec.Split(',');
			record.playerName = props[0];
			record.score = int.Parse(props[1]);
			scoreRecords.Add(record);
		}
		return scoreRecords;
	}
}
