using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public GameObject health;

	private SpriteRenderer healthBar;
	private Vector3 healthBarScale;

	void Start () {
		getHealthBar();
	}

	void getHealthBar () {
		healthBar = health.GetComponent<SpriteRenderer>();
		healthBarScale = healthBar.transform.localScale;
	}

	public void ApplyDamage (float health) {
		healthBar.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
		healthBar.transform.localScale = new Vector3(healthBarScale.x * health * 0.01f, 1, 1);
	}
}
