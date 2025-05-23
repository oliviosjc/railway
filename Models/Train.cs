namespace RailwayManager.Models;

public class Train
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Code { get; set; } = "";
    public string Type { get; set; } = "";
    public int Wagons { get; set; }
    public string Color { get; set; } = "";
    public int CurrentLineId { get; set; }
    public double Position { get; set; }
    public List<Wagon> WagonList { get; set; } = new();
}

public class Wagon
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Code { get; set; } = "";
    public string Type { get; set; } = "";
    public double Weight { get; set; }
    public double Capacity { get; set; }
    public string Cargo { get; set; } = "";
    public string Status { get; set; } = "";
}