using System;
using System.Runtime.Serialization;

[Serializable()]
public class Score{
	public string name { get; }
	public int score { get; }

	public Score(string name, int score) {
		this.name = name;
		this.score = score;
	}
}


