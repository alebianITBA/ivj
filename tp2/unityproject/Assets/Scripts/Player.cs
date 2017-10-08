using System;

public class Player {
	public string Name { get; private set; }
	public int Score { get; private set; }
	public int Movements { get; private set; }
	public PocketCollider.Pocket LastPocket { get; private set; }
	public Ball.BallTypes BallType { get; private set; }

	public Player (string name) {
		this.Name = name;
		this.Score = 0;
		this.Movements = 0;
		this.BallType = Ball.BallTypes.None;
	}

	public override bool Equals(Object obj) {
		// Check for null values and compare run-time types.
		if (obj == null || GetType() != obj.GetType()) {
			return false;
		}

		Player p = (Player)obj;
		return p.Name == this.Name;
	}

	public string DescriptionText(bool playing) {
		string ans = ToString();
		ans += " | Score " + Score.ToString();
		ans += " | Ball Type " + BallType.ToString();
		if (playing) {
			ans += " | Movements " + Movements.ToString();
		}
		return ans;
	}

	public string ToString() {
		return "Player " + this.Name;
	}

	public void IncreaseScore(Ball.BallTypes type) {
		if (type == this.BallType) {
			this.Score++;
		}
	}

	public void IncreaseMovements() {
		this.Movements++;
	}

	public void DecreaseMovements() {
		this.Movements--;
	}

	public void SetBallType(Ball.BallTypes type) {
		this.BallType = type;
	}

	public void SetLastPocket(PocketCollider.Pocket pocket) {
		this.LastPocket = pocket;
	}
}
