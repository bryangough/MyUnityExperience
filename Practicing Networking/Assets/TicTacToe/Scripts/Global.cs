public enum Team
{
	none,
	blue,
	red
}
[System.Serializable]
public struct SquareModel
{
	public SquareModel(int x, int y, Team team)
	{
		this.x = x;
		this.y = y;
		this.team = team;
	}
	public int x;
	public int y;
	public Team team;
};