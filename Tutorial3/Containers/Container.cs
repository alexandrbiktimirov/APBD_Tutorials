using Tutorial3.Exceptions;

namespace Tutorial3.Containers;

public abstract class Container
{
    private static int containerCounter;
    
    private double height;
    private double containerWeight;
    private double depth;
    
    public string serialNumber;
    
    public double maximumPayload { get; }
    public double cargoWeight { get; set; }
    public double totalMass => cargoWeight + containerWeight;

    protected Container(string containerType, double height, double containerWeight,
        double depth, double maximumPayload)
    {
        serialNumber = GenerateSerialNumber(containerType);
        this.height = height;
        this.containerWeight = containerWeight;
        this.depth = depth;
        this.maximumPayload = maximumPayload;
    }

    private static string GenerateSerialNumber(string containerType)
    {
        return $"KON-{containerType}-{++containerCounter}";
    }

    public virtual void EmptyCargo()
    {
        cargoWeight = 0;
    }

    public virtual void LoadCargo(double newCargoWeight)
    {
        if (CanLoad(newCargoWeight))
        {
            cargoWeight += newCargoWeight;
        }
    }
    
    protected virtual bool CanLoad(double newCargoMass)
    {
        if (cargoWeight + newCargoMass > maximumPayload)
        {
            throw new OverfillException($"Cannot load {newCargoMass}kg. Exceeds {maximumPayload}kg limit.");
        }
        
        return true;
    }

    public override string ToString()
    {
        return $"Container {serialNumber}\n" +
               $"Type: {GetType().Name}\n" +
               $"Height: {height} cm\n" +
               $"Container Weight: {containerWeight} kg\n" +
               $"Depth: {depth} cm\n" +
               $"Max Payload: {maximumPayload} kg\n" +
               $"Current Cargo Weight: {cargoWeight} kg\n" +
               $"Total Mass: {totalMass} kg\n";
    }
}