using UnityEngine;
using System.Collections;

public class ExitGamePlayTable : MonoBehaviour {

	public void CloseApp(){
		System.Diagnostics.Process.GetCurrentProcess().Kill();
	}
}
