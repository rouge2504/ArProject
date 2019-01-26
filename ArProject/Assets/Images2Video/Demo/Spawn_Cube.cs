using UnityEngine;

public class Spawn_Cube : MonoBehaviour
{
	public GameObject cube;
	public bool keepGoing;

	private Vector3 spawnPosition;
	private float timer = 0.0f;

	void SpawnCube ()
	{
		int xPosition = UnityEngine.Random.Range(-1, 1);
		spawnPosition = new Vector3 (xPosition, 2, 0);

		GameObject tempSpawnCube = (GameObject)Instantiate (cube, spawnPosition, Quaternion.identity);
	}

	void Update() {
		if (keepGoing == false)
			return;
		
		timer += Time.deltaTime;

		if (timer > 1) {
			SpawnCube ();
			timer = 0.0f;
		}
	}
}
