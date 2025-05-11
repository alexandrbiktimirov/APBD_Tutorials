using Tutorial3.Containers;
using Tutorial3.Exceptions;

public class C : Container
{
    private static Dictionary<string, double> ProductTemperatureMap = new()
    {
        { "Bananas", 13.3 },
        { "Chocolate", 18.0 },
        { "Fish", 2.0 },
        { "Meat", -15.0 },
        { "Ice cream", -18.0 },
        { "Frozen pizza", -30.0 },
        { "Cheese", 7.2 },
        { "Sausages", 5.0 },
        { "Butter", 20.5 },
        { "Eggs", 19.0 }
    };
    
    private string productType;
    private double temperature;

    public C(double height, double containerWeight,
        double depth, double maximumPayload, string productType, double temperature) :
        base("C", height, containerWeight, depth, maximumPayload)
    {
        if (ProductTemperatureMap[productType] > temperature)
        {
            throw new TemperatureMismatchException(
                "Temperature of the container cannot be lower than the temperature required by a given type of product!" +
                $" ({productType} requires {ProductTemperatureMap[productType]} < given {temperature})");
        }
        
        this.productType = productType;
        this.temperature = temperature;
    }

    public override void LoadCargo(double newCargoWeight)
    {
        throw new Exception("Use LoadCargo(double newCargoWeight, string productType) instead.");
    }
    
    public void LoadCargo(double newCargoWeight, string productType)
    {
        if (productType != this.productType)
        {
            throw new ProductTypeMismatchException($"You can load only products of type: {this.productType}.");
        }
        
        base.LoadCargo(newCargoWeight);
    }

    public override string ToString()
    {
        return base.ToString() +
               $"ProductType: {productType}\n" +
               $"Maintained temperature: {temperature}°C\n";
    }
}