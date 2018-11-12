using System;
using System.Collections.Generic;
using System.ServiceModel;
using Common;

namespace Client
{
    public class FailOverClient:IDataBaseManagement
    {
        private int nextTry = 0;
        private List<ChannelFactory<IDataBaseManagement>> _dataFactories = new List<ChannelFactory<IDataBaseManagement>>();
        private IDataBaseManagement _dataService;
        private bool _isClientConnected = false;

        public FailOverClient()
        {
            _dataFactories.Add(new ChannelFactory<IDataBaseManagement>("primary"));
            _dataFactories.Add(new ChannelFactory<IDataBaseManagement>("secondary"));
        }

        public void AddAll(Dictionary<string, Consumer> data)
        {
            throw new NotImplementedException();
        }

        public void AddConsumer(Consumer c)
        {
            throw new NotImplementedException();
        }

        public void ArchiveConsumation()
        {
            throw new NotImplementedException();
        }

        public double CityConsumtion(string city)
        {
            throw new NotImplementedException();
        }

        public void Connect()
        {
            while (!_isClientConnected)
            {
                _dataService = _dataFactories[nextTry].CreateChannel();
                try
                {
                    _isClientConnected = true;
                    Console.WriteLine("Client conneced to server at: {0}", _dataFactories[nextTry].Endpoint.Name);
                }
                catch (EndpointNotFoundException)
                {
                    Console.WriteLine("Client could not connect to server at: {0}", _dataFactories[nextTry].Endpoint.Name);
                    nextTry = (nextTry + 1) % 2;
                    _isClientConnected = false;
                }
            }
        }

        public void CreateFile()
        {
            bool written = false;
            do
            {
                try
                {
                    _dataService.CreateFile();
                    written = true;
                }
                catch (Exception)
                {
                    _isClientConnected = false;
                    Connect();
                }
            } while (!written);
        }

        public double MaxRegionConsumation(string region)
        {
            throw new NotImplementedException();
        }

        public void ModificationConsumer(string ID, string region, string city, string year, double consamption)
        {
            throw new NotImplementedException();
        }

        public double RegionConsumtion(string region)
        {
            throw new NotImplementedException();
        }

        public void RemoveConsumation()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, Consumer> UzmiSve()
        {
            throw new NotImplementedException();
        }
    }
}
