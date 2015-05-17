using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PackmanUI : MonoBehaviour {

	public PackmanLogic packmanLogic;
	public Text points;
	public Text lives;
	public GameObject pause;

	void Start () {
		packmanLogic.changed += OnPackmanChanged;
	}

	public void showPausedUI()
	{
		pause.SetActive(true);
	}
	public void hidePausedUI()
	{
		pause.SetActive(false);
	}
	private void OnPackmanChanged(object sender, System.EventArgs e)
	{
		points.text = "Очки: " + packmanLogic.Points.ToString();
		lives.text = "Жизни: " + packmanLogic.Lives.ToString();
	}

}
