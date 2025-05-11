using System.Text;
using Tutorial3.Containers;

namespace Tutorial3;

public class ContainerShip
{
    private static int shipCounter;

    public string shipName;
    public int maxSpeed;
    public int maxContainerAmount;
    public double maxWeightCapacity;
    public Dictionary<string, Container> containers = new();

    public ContainerShip(int maxSpeed, int maxContainerAmount, double maxWeightCapacity)
    {
        shipName = $"Ship-{++shipCounter}";
        this.maxSpeed = maxSpeed;
        this.maxContainerAmount = maxContainerAmount;
        this.maxWeightCapacity = maxWeightCapacity;
    }

    public void AddContainer(Container container)
    {
        if (containers.Count >= maxContainerAmount)
        {
            throw new Exception($"Cannot add container {container.serialNumber}. Ship has reached max capacity of {maxContainerAmount} containers!");
        }

        double currentWeight = containers.Values.Sum(c => c.totalMass);
        
        if (currentWeight + container.totalMass > maxWeightCapacity)
        {
            throw new Exception($"Cannot add container {container.serialNumber}. Ship would exceed max weight ({maxWeightCapacity} kg)!");
        }

        if (!containers.TryAdd(container.serialNumber, container))
        {
            throw new Exception($"Container {container.serialNumber} is already on this ship!");
        }
    }

    public void RemoveContainer(Container container)
    {
        RemoveContainer(container.serialNumber);
    }

    public void RemoveContainer(string containerSerialNumber)
    {
        if (!containers.Remove(containerSerialNumber))
        {
            throw new Exception($"Container {containerSerialNumber} not found on {shipName}.");
        }
    }

    public void ReplaceContainer(Container remove, Container add)
    {
        RemoveContainer(remove);
        AddContainer(add);
    }

    public void ReplaceContainer(string containerReplaceNumber, Container container)
    {
        RemoveContainer(containerReplaceNumber);
        AddContainer(container);
    }

    public static void TransferContainer(ContainerShip from, ContainerShip to, string containerSerialNumber)
    {
        if (!from.containers.TryGetValue(containerSerialNumber, out var movedContainer))
        {
            throw new Exception($"Container {containerSerialNumber} not found on {from.shipName}.");
        }

        from.RemoveContainer(movedContainer);
        to.AddContainer(movedContainer);
    }

    public double CurrentWeight()
    {
        return containers.Values.Sum(c => c.totalMass);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder()
            .AppendLine($"Ship: [{shipName}]")
            .AppendLine($"Max Speed: {maxSpeed} knots")
            .AppendLine($"Max Containers: {maxContainerAmount}")
            .AppendLine($"Max Weight Capacity: {maxWeightCapacity} kg")
            .AppendLine($"Current Load: {containers.Values.Sum(c => c.totalMass)} kg / {maxWeightCapacity} kg")
            .AppendLine("-------------------------");

        if (containers.Count == 0)
        {
            sb.AppendLine("No containers on this ship.");
        }
        
        else
        {
            foreach (var container in containers.Values)
            {
                sb.AppendLine(container.ToString());
            }
        }

        return sb.ToString();
    }
}