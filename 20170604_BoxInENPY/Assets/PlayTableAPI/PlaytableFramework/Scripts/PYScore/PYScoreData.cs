using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

/// <summary>
/// Script criado para controlar todo gerenciamento de jogadores, possui todos os métodos necessários para manipulação por todo o jogo
/// Com os métodos estáticos essa classe possui toda interação necessária para manipular e responder os dados referentes ao jogadores
/// </summary>
namespace Playmove
{
	public class PYScoreData
	{
		private static List<Student> _students;
		public static List<Student> Students
		{
			get
			{
				if (_students == null)
					_students = new List<Student>();
				return _students;
			}
			private set
			{
				_students = value;
			}
		}

		private static GameObject achievementVisualObject;
		private static AchievementVisualController achievementVisualController;

		public static bool synchronizedTheScores = false;

		public static void Initialize()
		{
			//PYNamesManager.DeleteAll();
			//DeleteAll();

			PYNamesManager.Initialize();
			Load();

			achievementVisualObject = GameObject.FindGameObjectWithTag (Tags.achievementVisualObject);

			if (achievementVisualObject != null) {
				achievementVisualController = achievementVisualObject.GetComponent<AchievementVisualController> ();
			}

			synchronizedTheScores = true;
		}

		public static void Save()
		{
			BinaryFormatter b = new BinaryFormatter();
			MemoryStream m = new MemoryStream();

			b.Serialize(m, Students);

			//PlaytableWin32.Instance.SetScores(PlaytableWin32.GameName, 0, Convert.ToBase64String(m.GetBuffer()));

			using (Stream stream = File.Open(string.Format("{0}/data.bin", Application.persistentDataPath), FileMode.Create))
			{
				BinaryFormatter bin = new BinaryFormatter();
				bin.Serialize(stream, Students);
			}
		}

		public static void Load()
		{
			//PlaytableWin32.Instance.GetScores(PlaytableWin32.GameName, 0, SynchronizeScores);

			if (File.Exists(string.Format("{0}/data.bin", Application.persistentDataPath)))
			{
				try
				{
					using (Stream stream = File.Open(string.Format("{0}/data.bin", Application.persistentDataPath), FileMode.Open))
					{
						BinaryFormatter bin = new BinaryFormatter();
						Students = (List<Student>)bin.Deserialize(stream);
					}
				}
				catch (Exception)
				{
					Students = new List<Student>();
					Save();
					Debug.LogWarning("Students rebuild!");
				}
			}
			else
			{
				Students = new List<Student>();
			}
		}

		private static void SynchronizeScores(string data)
		{
			if (!string.IsNullOrEmpty(data))
			{
				var b = new BinaryFormatter();
				var m = new MemoryStream(Convert.FromBase64String(data));
				Students = (List<Student>)b.Deserialize(m);

				foreach (var student in Students)
				{
					Debug.Log(student);
				}
			}
		}

		/// <summary>
		/// Retorna a posição do aluno no vetor de alunos do JOGO
		/// Retorna -1 se não existir
		/// </summary>
		/// <param name="studentName"></param>
		/// <returns></returns>
		public static int GetStudentIndex(string studentName)
		{
			for (int i = 0; i < Students.Count; i++)
			{
				if (Students[i].Name == studentName)
					return i;
			}
			return -1;
		}

		public static Student GetStudentByName(string name)
		{
			return Students.FirstOrDefault((n) => n.Name == name);
		}

		/*
		public static List<Student> GetStudents()
		{
			OrganizeScores();
			return Students != null ? Students[Difficulty] : new List<Student>();
		}
		*/

		public static void RegisterStudent(string name, string achievements, int score = 0, TagManager.GameDifficulty difficulty = TagManager.GameDifficulty.Easy, int classId = 0)
		{
			Student student = Students.Find(s => s.Name == name);
			if (student == null)
				Students.Add(new Student(name, score, difficulty));
			else
				student.SetScore(score, difficulty);

			PYNamesManager.SaveName(name, classId);
			Save();
		}

		public static void RegisterStudentByDifficulty(string name, int score = 0, int difficulty = 0, int classId = 0)
		{
			Student student = Students.Find(s => s.Name == name);
			if (student == null)
				Students.Add(new Student(name, score, TagManager.GameDifficulty.Easy));
			else
				student.SetScoreByDifficultNumber(score, difficulty);

			PYNamesManager.SaveName(name, classId);
			Save();
		}

		public static void RegisterStudent(string name, int score = 0, TagManager.GameDifficulty difficulty = TagManager.GameDifficulty.Easy, int classId = 0)
		{
			Student student = Students.Find(s => s.Name == name);
			if (student == null)
				Students.Add(new Student(name, score, difficulty));
			else
				student.SetScore(score, difficulty);

			PYNamesManager.SaveName(name, classId);
			Save();
		}
		public static void RegisterStudent(string name, int classId, int score = 0)
		{
			Student student = Students.Find(s => s.Name == name);
			if (student == null)
				Students.Add(new Student(name, score));
			else
				student.SetScore(score);

			PYNamesManager.SaveName(name, classId);
			Save();
		}
		public static void RegisterStudent(Student student, int classId)
		{
			Student st = Students.Find(s => s.Name == student.Name);
			if (st == null)
				Students.Add(student);
			//else
			//student.Update(student);

			PYNamesManager.SaveName(student.Name, classId);
			Save();
		}

		public static int GetStudentScore(string name, TagManager.GameDifficulty difficulty)
		{
			int diff = SwitchDifficulty(difficulty);
			return Students.Find(s => s.Name == name).Score[Mathf.Abs(diff)];
		}

		public static void OrganizeScores(TagManager.GameDifficulty difficulty = TagManager.GameDifficulty.None)
		{
			if (Students.Count == 0) return;
			int diff = SwitchDifficulty(difficulty);
			if (diff == -1)
			{
				//Debug.Log("Organize Scores>ByName");
				Students = Students.OrderByDescending(student => student.Name).ToList();
			}
			else
			{
				//Debug.Log("Organize Scores>ByScore: difficulty " + diff);
				Students = Students.OrderByDescending((student) => student.Score[diff]).ThenBy(student => student.Name).ToList();
			}
		}

		private static int SwitchDifficulty(TagManager.GameDifficulty difficulty)
		{
			switch (difficulty)
			{
			case TagManager.GameDifficulty.Easy:
				return 0;
			case TagManager.GameDifficulty.Normal:
				return 1;
			case TagManager.GameDifficulty.Hard:
				return 2;
			}
			return -1;
		}

		public static string[] GetStudentNames()
		{
			return PYNamesManager.GetNames();
		}

		public static void DeleteAll()
		{
			Students = new List<Student>();
			PYNamesManager.DeleteAll();
			Save();
		}
	}
}