using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lufthansa.Data;
using Lufthansa.Logic;
using Lufthansa.Repository;
using Lufthansa.WpfApp.ViewModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;

namespace Lufthansa.WpfApp
{
    public class MainViewModel
    {
        public IAirplaneLogic _logic;

        public MainViewModel()
        {
            _logic = Ioc.Default.GetService<IAirplaneLogic>();
            if (_logic == null)
            {
                System.Windows.MessageBox.Show("Please call technical support");
                Application.Current.Shutdown();

                //Environment.Exit(-1);
            }

            _logic.CreateTestData();

            GetAllCommand = new RelayCommand(GetAll);
            IncrementFlowDistanceCommand = new RelayCommand(IncrementFlowDistance);
            Airplanes = new ObservableCollection<AirplaneVM>();
        }

        public AirplaneVM SelectedItem { get; set; }

        public ObservableCollection<AirplaneVM> Airplanes { get; set; }

        public RelayCommand GetAllCommand { get; set; }

        public RelayCommand IncrementFlowDistanceCommand { get; set; }

        public int DistanceInKm { get; set; } = 0;

        public void GetAll()
        {
            // Init Airplanes Collection

            var list =  _logic.GetAllAirplane();
            
            Airplanes.Clear();

            foreach (var airplane in list)
            {
                Airplanes.Add(new AirplaneVM(airplane));
            }
        }

        public void IncrementFlowDistance()
        {
            if (SelectedItem == null)
            {
                return;
            }

            _logic.IncrementAggregatedDistance(DistanceInKm, SelectedItem.Id);
            SelectedItem.AggregatedFlownDistance += DistanceInKm;
        }

        
    }
}
