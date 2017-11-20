using UnityEngine;

public class TimeDestroyable {
	public static float TTL = 500;

	public GameObject obj { get; }
	public System.DateTime started { get; }
	public int duration { get; }

	public TimeDestroyable(GameObject obj, int duration) {
		this.obj = obj;
		this.started = System.DateTime.Now;
		this.duration = duration;
	}

	public bool Destroyable() {
		return (System.DateTime.Now - started).TotalMilliseconds > TTL;
	}
}
