using Tutorial3.Exceptions;

namespace Tutorial3.Containers;

public class L : Container, IHazardNotifier
{
    private bool hazardous;

    public L(double height, double containerWeight,
        double depth, double maximumPayload, bool hazardous) :
        base("L", height, containerWeight, depth, maximumPayload)
    {
        this.hazardous = hazardous;
    }
    
    protected override bool CanLoad(double newCargoMass)
    {
        double currentLimit = !hazardous ? maximumPayload * 0.9 : maximumPayload * 0.5;
        
        if (cargoWeight + newCargoMass > currentLimit)
        {
            throw new OverfillException($"Cannot load {newCargoMass}kg. Exceeds {currentLimit}kg limit.");
        }
        
        return true;
    }

    public override string ToString()
    {
        return base.ToString() +
               $"Hazardous: {hazardous}\n" +
               $"{sendMessage()}\n";
    }
    
    public string sendMessage()
    {
        return $"In liquid container {serialNumber} hazardous event occured!";
    }
}