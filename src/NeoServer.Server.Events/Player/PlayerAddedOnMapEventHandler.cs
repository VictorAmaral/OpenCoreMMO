﻿using NeoServer.Enums.Creatures.Enums;
using NeoServer.Game.Contracts;
using NeoServer.Game.Contracts.Creatures;
using NeoServer.Game.Contracts.World;
using NeoServer.Networking.Packets.Outgoing;
using NeoServer.Server.Contracts;
using NeoServer.Server.Contracts.Network;
using NeoServer.Server.Model.Players.Contracts;

namespace NeoServer.Server.Events
{
    public class PlayerAddedOnMapEventHandler : IEventHandler
    {
        private readonly IMap map;
        private readonly Game game;

        public PlayerAddedOnMapEventHandler(IMap map, Game game)
        {
            this.map = map;
            this.game = game;
        }
        public void Execute(IWalkableCreature creature, ICylinder cylinder)
        {
            cylinder.ThrowIfNull();
            cylinder.TileSpectators.ThrowIfNull();
            creature.ThrowIfNull();

            var tile = cylinder.ToTile;
            tile.ThrowIfNull();

            foreach (var cylinderSpectator in cylinder.TileSpectators)
            {
                var spectator = cylinderSpectator.Spectator;

                IConnection connection;
                if (!game.CreatureManager.GetPlayerConnection(spectator.CreatureId, out connection))
                {
                    continue;
                }

                var isMyself = creature.CreatureId == spectator.CreatureId;

                if (isMyself)
                {
                    SendPacketsToPlayer(creature as IPlayer, connection);
                }
                else
                {
                    if (!(spectator is IPlayer spectatorPlayer))
                    {
                        continue;
                    }

                    SendPacketsToSpectator(spectatorPlayer, creature, connection, cylinderSpectator.ToStackPosition);
                }           
                connection.Send();
            }
        }

        private void SendPacketsToSpectator(IPlayer playerToSend, IWalkableCreature creatureAdded, IConnection connection, byte stackPosition)
        {
            connection.OutgoingPackets.Enqueue(new AddAtStackPositionPacket(creatureAdded, stackPosition));
            connection.OutgoingPackets.Enqueue(new AddCreaturePacket(playerToSend, creatureAdded));
            connection.OutgoingPackets.Enqueue(new MagicEffectPacket(creatureAdded.Location, EffectT.BubbleBlue));
        }

        private void SendPacketsToPlayer(IPlayer player, IConnection connection)
        {
            connection.OutgoingPackets.Enqueue(new SelfAppearPacket(player));
            connection.OutgoingPackets.Enqueue(new MapDescriptionPacket(player, map));
            connection.OutgoingPackets.Enqueue(new MagicEffectPacket(player.Location, EffectT.BubbleBlue));
            connection.OutgoingPackets.Enqueue(new PlayerInventoryPacket(player.Inventory));
            connection.OutgoingPackets.Enqueue(new PlayerStatusPacket(player));
            connection.OutgoingPackets.Enqueue(new PlayerSkillsPacket(player));

            connection.OutgoingPackets.Enqueue(new WorldLightPacket(game.LightLevel, game.LightColor));

            connection.OutgoingPackets.Enqueue(new CreatureLightPacket(player));

            connection.OutgoingPackets.Enqueue(new PlayerConditionsPacket());
        }
    }
}
