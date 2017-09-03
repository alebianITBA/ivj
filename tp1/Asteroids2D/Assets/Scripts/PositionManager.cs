using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour {

	static float MAX_X = 6.9f;
	static float MIN_X = -1 * MAX_X;
	static float MAX_Y = 5.5f;
	static float MIN_Y = -1 * MAX_Y;

	public static Vector2 Reposition(Vector2 pos) {
		Vector2 ans = new Vector2 (pos.x, pos.y);
		if (pos.x > MAX_X) {
			ans.x = MIN_X;
			ans.y = pos.y * -1;
		}
		if (pos.x < MIN_X) {
			ans.x = MAX_X;
			ans.y = pos.y * -1;
		}
		if (pos.y > MAX_Y) {
			ans.y = MIN_Y;
			ans.x = pos.x * -1;
		}
		if (pos.y < MIN_Y) {
			ans.y = MAX_Y;
			ans.x = pos.x * -1;
		}
		return ans;
	}
}
