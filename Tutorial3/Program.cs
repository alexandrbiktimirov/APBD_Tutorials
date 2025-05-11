using Tutorial3;
using Tutorial3.Containers;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Creating container ship 1...");
        ContainerShip ship1 = new ContainerShip(25,5,50000.0);
        Console.WriteLine("Creating container ship 2...");
        ContainerShip ship2 = new ContainerShip(30, 3, 30000.0);

        Console.WriteLine("\nCreating a hazardous liquid container (L)...");
        L liquidContainer = new L(300, 1000, 250, 20000, true);
        Console.WriteLine("\nCreating a gas container (G)...");
        G gasContainer = new G(300, 1200, 250, 15000, 5.5);
        Console.WriteLine("\nCreating a refrigerated container (C) for Fish...");
        C refrigeratedContainer = new C(300, 1500,250, 10000, "Fish", 2.0);

        Console.WriteLine("\nLoading cargo into the containers...");
        try
        {
            liquidContainer.LoadCargo(8000);
            Console.WriteLine("Liquid container loaded with 8000 kg.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        try
        {
            gasContainer.LoadCargo(10000);
            Console.WriteLine("Gas container loaded with 10000 kg.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        try
        {
            refrigeratedContainer.LoadCargo(5000, "Fish");
            Console.WriteLine("Refrigerated container loaded with 5000 kg of Fish.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\nAdding containers to container ship 1...");
        try
        {
            ship1.AddContainer(liquidContainer);
            ship1.AddContainer(gasContainer);
            ship1.AddContainer(refrigeratedContainer);
            Console.WriteLine("All containers added to container ship 1.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\nContainer ship 1 details:");
        Console.WriteLine(ship1);
        Console.WriteLine("Current total cargo weight: " + ship1.CurrentWeight() + " kg");

        Console.WriteLine("\nRemoving the gas container from container ship 1...");
        try
        {
            ship1.RemoveContainer(gasContainer);
            Console.WriteLine("Gas container removed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\nContainer ship 1 details after removal:");
        Console.WriteLine(ship1);

        Console.WriteLine("\nEmptying cargo from the refrigerated container...");
        refrigeratedContainer.EmptyCargo();
        Console.WriteLine("Refrigerated container details after emptying cargo:");
        Console.WriteLine(refrigeratedContainer);

        Console.WriteLine("\nReplacing the liquid container with a new liquid container on container ship 1...");
        L newLiquidContainer = new L(300, 1000, 250, 20000, false);
        try
        {
            newLiquidContainer.LoadCargo(15000);
            ship1.ReplaceContainer(liquidContainer, newLiquidContainer);
            Console.WriteLine("Liquid container replaced.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\nContainer ship 1 details after replacement:");
        Console.WriteLine(ship1);

        Console.WriteLine("\nReplacing a container by serial number on container ship 1...");
        L anotherLiquidContainer = new L( 300, 1000, 250, 20000,false);
        try
        {
            anotherLiquidContainer.LoadCargo(14000);
            ship1.ReplaceContainer(newLiquidContainer.serialNumber, anotherLiquidContainer);
            Console.WriteLine("Container replaced using its serial number.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\nContainer ship 1 details after serial number replacement:");
        Console.WriteLine(ship1);

        Console.WriteLine("\nCreating and adding additional containers to container ship 2...");
        G gasContainer2 = new G(300,1100,250, 15000,6.0);
        try
        {
            gasContainer2.LoadCargo(7000);
            Console.WriteLine("Second gas container loaded with 7000 kg.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        C refrigeratedContainer2 = new C(300, 1400, 250,12000,"Fish",2.0);
        try
        {
            refrigeratedContainer2.LoadCargo(8000, "Fish");
            Console.WriteLine("Second refrigerated container loaded with 8000 kg of Fish.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        try
        {
            ship2.AddContainer(gasContainer2);
            ship2.AddContainer(refrigeratedContainer2);
            Console.WriteLine("Containers added to container ship 2.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\nContainer ship 2 details:");
        Console.WriteLine(ship2);
        Console.WriteLine("Current total cargo weight: " + ship2.CurrentWeight() + " kg");

        Console.WriteLine("\nTransferring the second refrigerated container from container ship 2 to container ship 1...");
        try
        {
            ContainerShip.TransferContainer(ship2, ship1, refrigeratedContainer2.serialNumber);
            Console.WriteLine("Transfer successful.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("Container ship 1 details after transfer:");
        Console.WriteLine(ship1);
        Console.WriteLine("Container ship 2 details after transfer:");
        Console.WriteLine(ship2);

        Console.WriteLine("Printing information about the latest liquid container:");
        Console.WriteLine(anotherLiquidContainer);
    }
}