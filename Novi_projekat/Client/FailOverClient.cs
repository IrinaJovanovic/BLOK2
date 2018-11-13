using System;
using System.Collections.Generic;
using System.ServiceModel;
using Common;

namespace Client
{
    public class FailOverClient : IDataBaseManagement
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

        public bool AddConsumer(Consumer c)
        {
            bool isSuccess = false;
            bool isAdded = false;
            do
            {
                try
                {
                    isAdded =_dataService.AddConsumer(c);
                    isSuccess = true;
                }
                catch
                {
                    _isClientConnected = false;
                    Connect();
                    //opciono logovanje
                }
            } while (!isSuccess);

            return isAdded;
        }

        public void ArchiveConsumation()
        {
            bool isSuccess = false;
            do
            {
                try
                {
                    _dataService.ArchiveConsumation();
                    isSuccess = true;
                }
                catch
                {
                    _isClientConnected = false;
                    Connect();
                }
            }
            while (!isSuccess);
        }

        public double CityConsumtion(string city)
        {
            bool isSuccess = false;
            double value = 0;
            do
            {
                try
                {
                    value = _dataService.CityConsumtion(city);
                    isSuccess = true;
                }
                catch
                {
                    _isClientConnected = false;
                    Connect();
                }
            }
            while (!isSuccess);
            return value;
        }

        public void Connect()
        {
            while (!_isClientConnected)
            {
                _dataService = _dataFactories[nextTry].CreateChannel();
                try
                {
                    _dataService.CreateFile();
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

        public bool CreateFile()
        {
            bool isCreated = false;
            bool isSuccess = false;
            do
            {
                try
                {
                    isCreated =_dataService.CreateFile();
                    isSuccess = true;
                }
                catch (Exception)
                {
                    _isClientConnected = false;
                    Connect();
                    
                }
            } while (!isSuccess);
            return isCreated;
        }

        public double MaxRegionConsumation(string region)
        {
            double value = 0;
            bool isSuccess = false;
            do
            {
                try
                {
                    value = _dataService.MaxRegionConsumation(region);
                    isSuccess = true;
                }
                catch (Exception)
                {
                    _isClientConnected = false;
                    Connect();
                }
            } while (!isSuccess);
            return value;
        }

        public bool ModificationConsumer(Consumer consumer)
        {
            bool isSuccess = false;
            bool isModified = false;
            do
            {
                try
                {
                    isModified = _dataService.ModificationConsumer(consumer);
                    isSuccess = true;
                }
                catch (Exception)
                {
                    _isClientConnected = false;
                    Connect();
                }
            } while (!isSuccess);
            return isModified;
        }

        public double RegionConsumtion(string region)
        {
            double value = 0;
            bool isSuccess = false;
            do
            {
                try
                {
                    value = _dataService.RegionConsumtion(region);
                    isSuccess = true;
                }
                catch (Exception)
                {
                    _isClientConnected = false;
                    Connect();
                }
            } while (!isSuccess);
            return value;
        }

        public void RemoveConsumation()
        {
            bool isSuccess = false;
            do
            {
                try
                {
                    _dataService.RemoveConsumation();
                    isSuccess = true;
                }
                catch
                {
                    _isClientConnected = false;
                    Connect();
                }
            } while (!isSuccess);
        }
    }
}
