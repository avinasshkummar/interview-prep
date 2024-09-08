using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;


public enum VehicleType
{
    Car,
    Bike,
    Truck
}
public abstract class ParkingSpot
{
    public int SpotId { get; private set;}
    public bool IsAvailable { get; private set;}
    public double Price { get; protected set;}

    public ParkingSpot(int spotId)
    {
        SpotId = spotId;
        IsAvailable = true;
    }
    public void BookSpot()
    {
        IsAvailable = false;
    }
    public void LeaveSpot()
    {
        IsAvailable = true;
    }
    public abstract void SetPrice();
    public abstract bool CanFitVehicle(Vehicle vehicle); 

}
class SmallSpot : ParkingSpot
{
    public SmallSpot(int spotId) : base(spotId){ SetPrice(); }
    public override void SetPrice()
    {
        Price = 10;
    }
    public override bool CanFitVehicle(Vehicle vehicle)
    {
        return vehicle.VehicleType == VehicleType.Bike;
    }

}
class MediumSpot : ParkingSpot
{
    public MediumSpot(int spotId) : base(spotId){ SetPrice(); }
    public override void SetPrice()
    {
        Price = 20;
    }
    public override bool CanFitVehicle(Vehicle vehicle)
    {
        return vehicle.VehicleType == VehicleType.Car;
    }
}
class LargeSpot : ParkingSpot
{
    public LargeSpot(int spotId) : base(spotId){ SetPrice(); }
    public override void SetPrice()
    {
        Price = 30;
    }
    public override bool CanFitVehicle(Vehicle vehicle)
    {
        return vehicle.VehicleType == VehicleType.Truck;
    }
}

public abstract class Vehicle
{
    public string VehicleNumber { get; private set; }
    public VehicleType VehicleType { get; private set;}
    public Vehicle(string vehicleNumber, VehicleType vehicleType)
    {
        VehicleNumber = vehicleNumber;
        VehicleType = vehicleType;
    }
}
class Car: Vehicle
{
    public Car(string carNumber) : base(carNumber, VehicleType.Car) { }
}
class Bike: Vehicle
{
    public Bike(string bikeNumber) : base(bikeNumber, VehicleType.Bike) { }
}

class Truck: Vehicle
{
    public Truck(string truckNumber) : base(truckNumber, VehicleType.Truck) { }
}

class ParkingTicket
{
    public DateTime ParkedTime{ get; private set; }
    public ParkingSpot ParkingSpot { get; private set; }
    public bool Paid { get; private set; }
    public bool IsValid { get; private set; }  // New flag for ticket validity

    public ParkingTicket(ParkingSpot parkingSpot)
    {
        ParkingSpot = parkingSpot;
        ParkedTime = DateTime.Now;
        Paid = false;
        IsValid = true;
    }
    public void MarkAsPaid()
    {
        Paid = true;
    }
    public void InvalidateTicket()  // Method to invalidate the ticket
    {
        IsValid = false;
    }
    public double GetParkedTime() //for now in seconds
    {
        return (DateTime.Now - ParkedTime).TotalSeconds;
    }
}
class ParkingLot
{
    List<ParkingSpot> parkingSpots;

    public ParkingLot(int smallSpots, int mediumSpots, int largeSpots)
    {
        parkingSpots = new List<ParkingSpot>();
        int parkingSpotCount = 0;
        for (int i = 0; i < smallSpots; i++)
        {
            parkingSpots.Add(new SmallSpot(parkingSpotCount++));
        }
        for (int i = 0; i < mediumSpots; i++)
        {
            parkingSpots.Add(new MediumSpot(parkingSpotCount++));
        }
        for (int i = 0; i < largeSpots; i++)
        {
            parkingSpots.Add(new LargeSpot(parkingSpotCount++));
        }
    }
    public ParkingSpot FindAvailableSpot(Vehicle vehicle)
    {
        foreach (var parkingSpot in parkingSpots)
        {
            if (parkingSpot.IsAvailable && parkingSpot.CanFitVehicle(vehicle))
            {
                return parkingSpot;
            }
        }
        return null;
    }
    public void VacateOccupiedSpot(ParkingSpot parkingSpot)
    {
        parkingSpot.LeaveSpot();
    }
    public void CheckSpotsStatus()
    {
        Console.WriteLine("---------------------");
        foreach (var parkingSpot in parkingSpots)
        {
            string spotType = parkingSpot.GetType().Name;
            Console.WriteLine($"Spot ID: {parkingSpot.SpotId}, Status: {(parkingSpot.IsAvailable ? "Available" : "Occupied")}, Spot Type: {spotType}");
        }
    }
}


class ParkingAttendant
{
    public ParkingTicket BookSpot(ParkingLot parkingLot, Vehicle vehicle)
    {
        var parkingSpot = parkingLot.FindAvailableSpot(vehicle);
        if (parkingSpot == null)
        {
            Console.WriteLine($"No available spot for vehicle {vehicle.VehicleNumber} of type {vehicle.VehicleType}.");
            return null;
        }
        parkingSpot.BookSpot();
        ParkingTicket parkingTicket = new ParkingTicket(parkingSpot);
        Console.WriteLine($"Spot {parkingSpot.SpotId} booked for vehicle {vehicle.VehicleNumber} of type {vehicle.VehicleType}.");
        return parkingTicket;
    }
    public void GenerateBill(ParkingTicket parkingTicket)
    {
        if (parkingTicket == null)
        {
            Console.WriteLine("Invalid ticket. Cannot generate a bill.");
            return;
        }
        if(!parkingTicket.IsValid)
        {
            Console.WriteLine("This ticket is already invalid. Bill has been generated.");
            return;
        }
        parkingTicket.MarkAsPaid();
        parkingTicket.InvalidateTicket();
        parkingTicket.ParkingSpot.LeaveSpot();
        Console.WriteLine($"Spot {parkingTicket.ParkingSpot.SpotId} has been left.");
        Console.WriteLine($" Bill of $ {parkingTicket.ParkingSpot.Price * parkingTicket.GetParkedTime()}");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        

        Console.WriteLine($"Hello World {VehicleType.Bike} ");

        ParkingLot parkingLot= new ParkingLot(2,2,2);
        parkingLot.CheckSpotsStatus();
        ParkingAttendant parkingAttendant = new ParkingAttendant();

        Vehicle car1 = new Car("CAR123");
        Vehicle car2 = new Car("CAR456");

        Vehicle bike1 = new Bike("BIKE123");

        var ticket1 = parkingAttendant.BookSpot(parkingLot, car1);
        var ticket2 = parkingAttendant.BookSpot(parkingLot, car2);

        parkingLot.CheckSpotsStatus();

        var ticket3 = parkingAttendant.BookSpot(parkingLot, bike1);
        
        parkingLot.CheckSpotsStatus();

        Thread.Sleep(2000);

        parkingAttendant.GenerateBill(ticket1);
        parkingAttendant.GenerateBill(ticket3);
        parkingLot.CheckSpotsStatus();
        parkingAttendant.GenerateBill(ticket2);
        parkingAttendant.GenerateBill(ticket2);

    }
}