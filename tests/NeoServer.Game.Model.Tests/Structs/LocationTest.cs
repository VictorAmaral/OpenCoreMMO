﻿using NeoServer.Game.Common.Location;
using NeoServer.Game.Common.Location.Structs;
using NeoServer.Game.Common.Players;
using Xunit;

namespace NeoServer.Game.Model.Tests.Structs
{
    public class LocationTest
    {
        [Theory]
        [InlineData(100, 100, 105, 103, 8)]
        [InlineData(105, 103, 100, 100, 8)]
        [InlineData(100, 0, 0, 100, 200)]
        public void GetSqmDistance_Returns_Sum_Of_Sqm_Distance(int fromX, int fromY, int toX, int toY, ushort total)
        {
            var fromLocation = new Location((ushort)fromX, (ushort)fromY, 7);
            var toLocation = new Location((ushort)toX, (ushort)toY, 7);

            Assert.Equal(total, fromLocation.GetSqmDistance(toLocation));
        }

        [Theory]
        [InlineData(100, 100, 100, 100, Direction.None)]
        [InlineData(100, 100, 101, 100, Direction.East)]
        [InlineData(100, 100, 99, 100, Direction.West)]
        [InlineData(100, 100, 100, 101, Direction.South)]
        [InlineData(100, 100, 100, 99, Direction.North)]
        [InlineData(100, 100, 101, 99, Direction.East)]
        [InlineData(100, 100, 99, 101, Direction.West)]
        [InlineData(100, 100, 102, 101, Direction.East)]
        [InlineData(100, 100, 102, 103, Direction.South)]
        [InlineData(100, 100, 98, 101, Direction.West)]
        [InlineData(100, 100, 102, 97, Direction.North)]

        public void DirectionTo_Returns_TargetDirection(int fromX, int fromY, int toX, int toY, Direction direction)
        {
            var fromLocation = new Location((ushort)fromX, (ushort)fromY, 7);
            var toLocation = new Location((ushort)toX, (ushort)toY, 7);

            Assert.Equal(direction, fromLocation.DirectionTo(toLocation));
        }

        [Theory]
        [InlineData(Slot.Legs)]
        [InlineData(Slot.Body)]
        [InlineData(Slot.Backpack)]
        [InlineData(Slot.Ammo)]
        [InlineData(Slot.Head)]
        [InlineData(Slot.Left)]
        [InlineData(Slot.Feet)]
        [InlineData(Slot.Necklace)]
        [InlineData(Slot.Right)]
        [InlineData(Slot.Ring)]

        public void Inventory_Returns_Location_As_Slot(Slot slot)
        {
            var location = Location.Inventory(slot);

            Assert.Equal(LocationType.Slot, location.Type);
            Assert.Equal(slot, location.Slot);
        }

        [Theory]
        [InlineData(1,1)]
        [InlineData(2, 2)]
        [InlineData(3, 5)]
        [InlineData(4, 10)]
        [InlineData(5, 11)]
        [InlineData(6, 13)]
        [InlineData(7, 15)]
        [InlineData(8, 18)]
        [InlineData(9, 19)]
        [InlineData(10, 20)]
        [InlineData(13, 25)]
        [InlineData(15, 6)]
        public void Container_Returns_Type_As_Container(int id, int slot)
        {
            var location = Location.Container(id, (byte)slot);

            Assert.Equal(LocationType.Container, location.Type);
            Assert.Equal(id, location.ContainerId);
            Assert.Equal(slot, location.ContainerSlot);
        }
     
    }
}
