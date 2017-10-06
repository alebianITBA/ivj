using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public enum BallTypes { White, Black, Striped, Solid };
	public string id;
	public BallTypes type;
}
