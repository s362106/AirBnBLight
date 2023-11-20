using System;
using Group_Project_2.Models;

namespace Group_Project_2.ViewModels
{
    public class HouseListViewModel
    {
        public IEnumerable<House> Houses;
        public string? CurrentViewName;

        public HouseListViewModel(IEnumerable<House> houses, string? currentViewName)
        {
            Houses = houses;
            CurrentViewName = currentViewName;
        }
	}
}
