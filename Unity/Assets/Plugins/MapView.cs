using UnityEngine;
using System.Collections;

public class MapView : MonoBehaviour
{
	public bool locationTest = false;
	public GUISkin skin;
	private bool flag = false;
	AndroidJavaObject activity;
	void Start ()
	{
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
	}



	void Update ()
	{
		if (Input.touches.Length == 3 && !locationTest)
			locationTest = true;
		
		
		if (Input.touchCount != 0 && !flag && !locationTest) {
			StartCoroutine (SetLocation ());
		}
	}
	float latitude;
	float longitude;
	IEnumerator SetLocation ()
	{
		flag = true;
		iPhoneSettings.StartLocationServiceUpdates ();
		while (iPhoneSettings.locationServiceStatus.Equals (LocationServiceStatus.Initializing)) {
			yield return new WaitForEndOfFrame ();
		}
		latitude = iPhoneInput.lastLocation.latitude;
		longitude = iPhoneInput.lastLocation.longitude;
		iPhoneSettings.StopLocationServiceUpdates ();
		activity.Call ("setLocation", latitude, longitude);
		flag = false;
	}

	void SetLocationTest (float latitude, float longitude)
	{
		activity.Call ("setLocation", latitude, longitude);
	}
	void OnGUI ()
	{
		GUI.skin = skin;
		
		if (locationTest)
			LocationTest ();
		else {
			GUILayout.BeginArea (new Rect (0, Screen.height / 2 - 100, Screen.width, 200));
			GUILayout.Label ("Please touch the screen");
			GUILayout.Box ("latitude : " + latitude);
			GUILayout.Box ("longitude : " + longitude);
			GUILayout.Label ("Test mode by tap with three fingers");
			GUILayout.EndArea ();
		}
	}
	void LocationTest ()
	{
		GUILayout.Space (10);
		if (GUILayout.Button ("Back"))
			locationTest = false;
		
		GUILayout.BeginArea (new Rect (20, Screen.height / 2 - 85, Screen.width - 20, Screen.height - 40));
		GUILayout.Label ("Test Mode");
		GUILayout.BeginHorizontal ();
		LocationTestButton ("Okinawa", 26.212358f, 127.680874f);
		LocationTestButton ("Fukuoka", 33.590421f, 130.401664f);
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		LocationTestButton ("Osaka", 34.69378f, 135.501938f);
		LocationTestButton ("Tokyo", 35.689579f, 139.691763f);
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		LocationTestButton ("Hokkaido", 43.064373f, 141.346922f);
		LocationTestButton ("UnityTechnologies", 37.797965f, -122.402954f);
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
	}

	void LocationTestButton (string locationName, float latitude, float longitude)
	{
		if (GUILayout.Button (locationName, GUILayout.Width (Screen.width / 2 - 20), GUILayout.Height (50)))
			SetLocationTest (latitude, longitude);
	}
}
