namespace Tutorial3.Containers;

public class G : Container, IHazardNotifier
{
    private double pressure;
    
    public G(double height, double containerWeight,
        double depth, double maximumPayload, double pressure) :
        base("G", height, containerWeight, depth, maximumPayload)
    {
        this.pressure = pressure;
    }
    
    public override void EmptyCargo()
    {
        cargoWeight *= 0.05;
    }

    public override string ToString()
    {
        return base.ToString() +
               $"Pressure: {pressure}atm \n" +
               $"{sendMessage()}\n";
    }
    
    public string sendMessage()
    {
        return $"In gas container {serialNumber} hazardous event occured!";
    }
}