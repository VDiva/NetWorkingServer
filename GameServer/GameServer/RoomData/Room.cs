using NetWorkingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.RoomData
{
    public class Room
    {
        public int MaxPeople;
        List<Client<Message>> clients;
        public Room() { 
            clients = new List<Client<Message>>();
        }

        public void Init(int MaxPeople)
        {
            this.MaxPeople = MaxPeople;
        }

        public bool JoinRoom(Client<Message> client)
        {
            if(clients.Count>MaxPeople) return false;
            client.IsJoinRoom = true;
            clients.Add(client);
            return true;

        }

        public void BlackRoom(Client<Message> client)
        {
            try
            {
                client.IsJoinRoom=false;
                clients.Remove(client);
            }catch (Exception ex)
            {
                DebugLog.LogError(ex.Message);
            }
        }

        public void CliearRoom()
        {
            clients.Clear();
        }




    }
}
