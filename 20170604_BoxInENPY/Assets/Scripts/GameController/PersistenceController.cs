using UnityEngine;

using System;
//#if UNITY_WP8
//To use the base64 on the player prefs, because serialization doesnt work on windows phone
using System.Text;
//#else
//To serialize needs these 2 libraries and the "System" library
/*
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
*/
//#endif


public class PersistenceController : MonoBehaviour {

	//Static so you can access the instance in any other class calling PersistenceController.control
	//This is a singleton, so there is only one object of this class instanced.
	//It make sure there is only one on the Awake()
	public static PersistenceController control;

	public int maxCoinsHighScore;
	public int totalCoins;
	public int maxPointsHighScore;
	public int maxPointsHighScore2;
	public int maxPointsHighScore3;
	public int totalPoints;

	//#if UNITY_WP8
	private const string maxCoinsHighScoreKeyToPrefs = "mchs";
	private const string totalCoinsKeyToPrefs = "tc";
	private const string maxPointsHighScoreKeyToPrefs = "mphs";
	private const string maxPointsHighScoreKeyToPrefs2 = "mphs2";
	private const string maxPointsHighScoreKeyToPrefs3 = "mphs3";
	private const string totalPointsKeyToPrefs = "tp";
	//#else
	/*
	public string fileName = "persistentdata.dat";
	private string completeFilePath;
	*/
	//#endif

	private const string playedTutorial = "PlayedTutorial";


	void Awake() {
		//DontDestroyOnLoad make the object available even when you change the scene.
		//The problem is that you must enter a scene that have the object first
		//The solution is to have one copy of the object on each scene, but when you change scenes
		//the object from previous scene is there too. So you make sure you Destroy this if there is another 
		//object from the class instanced.
		//It can be verified because the variable "control" is static and is unique for every object on the class.
		//So you can verify if any object of the class is in the control static variable.
		if(control == null){
			DontDestroyOnLoad(gameObject);
			control = this;
			control.Load ();
		}else if(control != this){
			Destroy (gameObject);
		}

		//#if !UNITY_WP8
		/*
		completeFilePath = Application.persistentDataPath + "/"+fileName;
		*/
		//#endif
	}


	//Application.persistentDataPath is an automatic path unity makes to store data.
	//It works even on mobile, it doesnt work only in webplayer (because webplayer dont have anywhere to write files)
	public void Save(){
		//#if UNITY_WP8
		PlayerPrefs.SetString( maxCoinsHighScoreKeyToPrefs, EncodeInt(maxCoinsHighScore) );
		PlayerPrefs.SetString( totalCoinsKeyToPrefs, EncodeInt(maxCoinsHighScore) );
		PlayerPrefs.SetString( maxPointsHighScoreKeyToPrefs, EncodeInt(maxPointsHighScore) );
		PlayerPrefs.SetString( maxPointsHighScoreKeyToPrefs2, EncodeInt(maxPointsHighScore2) );
		PlayerPrefs.SetString( maxPointsHighScoreKeyToPrefs3, EncodeInt(maxPointsHighScore3) );
		PlayerPrefs.SetString( totalPointsKeyToPrefs, EncodeInt(totalPoints) );
		//#else
		/*
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(completeFilePath);

		PersistentData data = new PersistentData();

		data.maxCoinsHighScore = maxCoinsHighScore;
		data.totalCoins = totalCoins;
		data.maxPointsHighScore = maxPointsHighScore;
		data.maxPointsHighScore2 = maxPointsHighScore2;
		data.maxPointsHighScore3 = maxPointsHighScore3;
		data.totalPoints = totalPoints;

		bf.Serialize(file, data);
		file.Close();
		*/
		//#endif
	}

	public void Load(){
		//#if UNITY_WP8
		if(PlayerPrefs.GetString(maxCoinsHighScoreKeyToPrefs) != ""){
			maxCoinsHighScore = DecodeInt(PlayerPrefs.GetString(maxCoinsHighScoreKeyToPrefs));
		}
		if(PlayerPrefs.GetString(totalCoinsKeyToPrefs) != ""){
			totalCoins = DecodeInt(PlayerPrefs.GetString(totalCoinsKeyToPrefs));
		}
		if(PlayerPrefs.GetString(maxPointsHighScoreKeyToPrefs) != ""){
			maxPointsHighScore = DecodeInt(PlayerPrefs.GetString(maxPointsHighScoreKeyToPrefs));
		}
		if(PlayerPrefs.GetString(maxPointsHighScoreKeyToPrefs2) != ""){
			maxPointsHighScore2 = DecodeInt(PlayerPrefs.GetString(maxPointsHighScoreKeyToPrefs2));
		}
		if(PlayerPrefs.GetString(maxPointsHighScoreKeyToPrefs3) != ""){
			maxPointsHighScore3 = DecodeInt(PlayerPrefs.GetString(maxPointsHighScoreKeyToPrefs3));
		}
		if(PlayerPrefs.GetString(totalPointsKeyToPrefs) != ""){
			totalPoints = DecodeInt(PlayerPrefs.GetString(totalPointsKeyToPrefs));
		}
		//#else
		/*
		if(File.Exists(completeFilePath)){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(completeFilePath, FileMode.Open);

			PersistentData data = (PersistentData)bf.Deserialize(file);
			file.Close();

			maxCoinsHighScore = data.maxCoinsHighScore;
			totalCoins = data.totalCoins;
			maxPointsHighScore = data.maxPointsHighScore;
			maxPointsHighScore2 = data.maxPointsHighScore2;
			maxPointsHighScore3 = data.maxPointsHighScore3;
			totalPoints = data.totalPoints;
		}
		*/
		//#endif
	}

	//#if UNITY_WP8
	string EncodeInt(int intToEncode){
		byte[] bytesToEncode = Encoding.UTF8.GetBytes (intToEncode.ToString());
		string encodedText = Convert.ToBase64String (bytesToEncode);
		return encodedText;
	}

	int DecodeInt(string textToDecode){
		byte[] decodedBytes = Convert.FromBase64String (textToDecode);
		//the next line didnt worked with only decodedBytes on windows phone
		string decodedText = Encoding.UTF8.GetString (decodedBytes, 0, decodedBytes.Length);
		return int.Parse(decodedText);
	}
	//#endif

	public void setPlayedTutorial(int playedTheTutorial){
		PlayerPrefs.SetInt(playedTutorial, playedTheTutorial);
	}

	public void FinishedTheTutorial(){
		PlayerPrefs.SetInt(playedTutorial, 1);
	}

	public int getPlayedTutorial(){
		return PlayerPrefs.GetInt(playedTutorial);
	}

	public bool getFinishedTheTutorial(){
		return PlayerPrefs.GetInt(playedTutorial) == 1;
	}
}

//#if !UNITY_WP8
//To serialize we need a class with only the variables that we will serialize and with [Serializable] before the class name
//Serializing a class with methods can cause problems (something to do with monodevelop compiling or something like that)
/*
[Serializable]
class PersistentData{
	public int maxCoinsHighScore;
	public int totalCoins;
	public int maxPointsHighScore;
	public int maxPointsHighScore2;
	public int maxPointsHighScore3;
	public int totalPoints;
}
*/
//#endif