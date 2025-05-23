namespace RailwayManager.Models;

public class RailwayLine
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Train> Trains { get; set; } = new();
    public List<int> ConnectedLines { get; set; } = new();
    public string Color { get; set; } = "";
}