public enum Team
{
	none,
	blue,
	red
}
public struct BoardModel
{
	public BoardModel(int x, int y, Team team)
	{
		this.x = x;
		this.y = y;
		this.team = team;
	}
	public int x;
	public int y;
	public Team team;
};