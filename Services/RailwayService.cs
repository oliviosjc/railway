using RailwayManager.Models;

namespace RailwayManager.Services;

public class RailwayService
{
    private List<RailwayLine> _lines = new();
    private Random _random = new();
    
    public event Action? OnStateChanged;
    
    public List<RailwayLine> GetLines() => _lines;
    
    public RailwayService()
    {
        InitializeLines();
        GenerateMockTrains();
    }
    
    private void InitializeLines()
    {
        var lineColors = GenerateColors(50);
        
        for (int i = 0; i < 50; i++)
        {
            _lines.Add(new RailwayLine
            {
                Id = i + 1,
                Name = $"Linha {i + 1}",
                Color = lineColors[i],
                ConnectedLines = GenerateConnections(i, 50)
            });
        }
    }
    
    private string[] GenerateColors(int count)
    {
        var colors = new string[count];
        for (int i = 0; i < count; i++)
        {
            int hue = (i * 360 / count) % 360;
            colors[i] = $"hsl({hue}, 70%, 50%)";
        }
        return colors;
    }
    
    private List<int> GenerateConnections(int lineIndex, int totalLines)
    {
        var connections = new List<int>();
        
        if (lineIndex > 0) connections.Add(lineIndex);
        if (lineIndex < totalLines - 1) connections.Add(lineIndex + 2);
        
        if (_random.Next(100) < 60 && lineIndex + 3 < totalLines)
            connections.Add(lineIndex + 3);
        
        if (_random.Next(100) < 50 && lineIndex > 2)
            connections.Add(lineIndex - 2);
            
        if (_random.Next(100) < 40 && lineIndex + 5 < totalLines)
            connections.Add(lineIndex + 5);
            
        if (_random.Next(100) < 30 && lineIndex > 4)
            connections.Add(lineIndex - 4);
            
        return connections.Distinct().ToList();
    }
    
    private void GenerateMockTrains()
    {
        var trainTypes = new[] { "Passageiros", "Carga", "Expresso", "Regional", "Metropolitano", "Minério", "Container", "Tanque" };
        var trainColors = GenerateTrainColors(20);
        
        int trainCounter = 1;
        int totalTrains = 0;
        int targetTrains = 3500; // 3500 TRENS
        
        // Distribuir trens uniformemente
        int trainsPerLine = targetTrains / _lines.Count;
        int extraTrains = targetTrains % _lines.Count;
        
        foreach (var line in _lines)
        {
            int trainCount = trainsPerLine + (extraTrains > 0 ? 1 : 0);
            if (extraTrains > 0) extraTrains--;
            
            for (int j = 0; j < trainCount; j++)
            {
                int wagons = _random.Next(15, 40); // Cada trem tem 15-40 vagões
                
                var train = new Train
                {
                    Code = $"T{trainCounter:D4}",
                    Type = trainTypes[_random.Next(trainTypes.Length)],
                    Wagons = wagons,
                    Color = trainColors[_random.Next(trainColors.Length)],
                    CurrentLineId = line.Id,
                    Position = _random.Next(0, 100) // Posição aleatória
                };
                
                // Gerar vagões para cada trem
                GenerateWagons(train, wagons);
                
                line.Trains.Add(train);
                trainCounter++;
                totalTrains++;
                
                if (totalTrains >= targetTrains) return;
            }
        }
    }
    
    private void GenerateWagons(Train train, int count)
    {
        var wagonTypes = new[] { "Passageiros", "Carga Geral", "Container", "Tanque", "Frigorífico", "Graneleiro", "Plataforma", "Gôndola" };
        var cargoTypes = new[] { "Vazio", "Minério", "Grãos", "Combustível", "Produtos", "Passageiros", "Containers", "Veículos" };
        var statusTypes = new[] { "Operacional", "Manutenção", "Carregado", "Descarregando", "Em Trânsito" };
        
        for (int i = 0; i < count; i++)
        {
            var wagonType = wagonTypes[_random.Next(wagonTypes.Length)];
            train.WagonList.Add(new Wagon
            {
                Code = $"{train.Code}-V{i + 1:D2}",
                Type = wagonType,
                Weight = _random.Next(15, 35) + _random.NextDouble(),
                Capacity = _random.Next(40, 100) + _random.NextDouble(),
                Cargo = cargoTypes[_random.Next(cargoTypes.Length)],
                Status = statusTypes[_random.Next(statusTypes.Length)]
            });
        }
    }
    
    private string[] GenerateTrainColors(int count)
    {
        var colors = new string[count];
        for (int i = 0; i < count; i++)
        {
            int value = 30 + (i * 40 / count);
            colors[i] = $"hsl(0, 0%, {value}%)";
        }
        return colors;
    }
    
    public void MoveTrain(string trainId, int fromLineId, int toLineId)
    {
        var fromLine = _lines.FirstOrDefault(l => l.Id == fromLineId);
        var toLine = _lines.FirstOrDefault(l => l.Id == toLineId);
        
        if (fromLine == null || toLine == null) return;
        
        var train = fromLine.Trains.FirstOrDefault(t => t.Id == trainId);
        if (train == null) return;
        
        // Remover restrição de conexão - permite mover para qualquer linha
        fromLine.Trains.Remove(train);
        train.CurrentLineId = toLineId;
        toLine.Trains.Add(train);
        
        OnStateChanged?.Invoke();
    }
}