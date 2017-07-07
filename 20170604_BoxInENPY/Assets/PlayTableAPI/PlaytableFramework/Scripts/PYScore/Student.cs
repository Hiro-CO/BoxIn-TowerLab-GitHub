using UnityEngine;
using System.Collections;
using System;

namespace Playmove
{
	[Serializable]
	public class Student
	{

		public const int numberOfDificulties = 3;

		public string Name;
		/// <summary>
		/// Exemplo para quando o placar funcionar como inteiro único
		/// </summary>
		public int[] Score;
		//public DateTime Date;
		public string Achievements;

		public override string ToString()
		{
			return string.Format("Name: {0} \nScore:{1}-{2}-{3} \nDate: ", Name, Score[0], Score[1], Score[2]/*, Date*/);
		}

		public Student()
		{
			Name = "";
			Score = new int[numberOfDificulties];
			for (int i = 0; i < Score.Length; i++) {
				Score [i] = 0;
			}
			//Date = DateTime.Now;
			Achievements = "";
		}

		public Student(string name, int score, string achievements)
		{
			Name = name;
			Score = new int[numberOfDificulties];
			for (int i = 0; i < Score.Length; i++) {
				Score [i] = 0;
			}
			Achievements = achievements;
		}

		public Student(string name, int score, TagManager.GameDifficulty difficulty = TagManager.GameDifficulty.Easy)
			: base()
		{
			Name = name;
			Score = new int[numberOfDificulties];
			for (int i = 0; i < Score.Length; i++) {
				Score [i] = 0;
			}
			SetScore(score, difficulty);
			Achievements = "";
		}

		public void SetScoreByDifficultNumber(int score, int difficulty)
		{
			Score[difficulty] = score;
		}

		public void SetScore(int score, TagManager.GameDifficulty difficulty = TagManager.GameDifficulty.Easy)
		{
			switch (difficulty)
			{
			case TagManager.GameDifficulty.Easy:
				Score[0] = score;
				break;
			case TagManager.GameDifficulty.Normal:
				Score[1] = score;
				break;
			case TagManager.GameDifficulty.Hard:
				Score[2] = score;
				break;
			}
		}
	}
}