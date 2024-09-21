# Parking Lot System Overview

The parking lot system is designed to manage parking spaces for different types of vehicles, such as cars, bikes, and trucks. Below is a breakdown of the key components and their relationships.

## Class Diagram

![Class Diagram](@class-diagram.png)

## Key Components

1. **ParkingLot**:

   - Manages a list of parking spots.
   - Methods:
     - `FindAvailable(Vehicle vehicle)`: Finds an available parking spot for a vehicle.
     - `VacateOccupiedSpot(ParkingSpot parkingSpot)`: Marks a spot as available when a vehicle leaves.
     - `CheckSpotsStatus()`: Displays the status of all parking spots.

2. **ParkingSpot**:

   - Represents a single parking space.
   - Attributes:
     - `SpotId`: Unique identifier for the spot.
     - `IsAvailable`: Indicates if the spot is free.
     - `Price`: Cost to park in this spot.
   - Methods:
     - `BookSpot()`: Marks the spot as occupied.
     - `LeaveSpot()`: Marks the spot as available.
     - `SetPrice()`: Sets the price for parking.
     - `CanFitVehicle(Vehicle vehicle)`: Checks if a vehicle can fit in this spot.

3. **Vehicle**:

   - Represents a vehicle with attributes like `VehicleNumber` and `VehicleType`.

4. **ParkingTicket**:

   - Issued when a vehicle parks.
   - Attributes:
     - `ParkedTime`: Time when the vehicle was parked.
     - `ParkingSpot`: The spot where the vehicle is parked.
     - `Paid`: Indicates if the parking fee has been paid.
     - `IsValid`: Indicates if the ticket is still valid.
   - Methods:
     - `MarkAsPaid()`: Marks the ticket as paid.
     - `InvalidateTicket()`: Invalidates the ticket if necessary.
     - `GetParkedTime()`: Calculates the duration the vehicle has been parked.

5. **Spot Types**:
   - **SmallSpot**, **MediumSpot**, **LargeSpot**: Different classes for parking spots that can accommodate different vehicle sizes. Each has methods to set prices and check if a vehicle can fit.

## Summary

This system helps organize parking efficiently, ensuring that vehicles are parked appropriately and that payments are handled smoothly. It benefits both parking lot operators and vehicle owners.
