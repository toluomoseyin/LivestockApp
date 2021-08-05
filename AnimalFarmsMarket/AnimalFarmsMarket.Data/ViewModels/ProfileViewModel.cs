using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class ProfileViewModel
    {
        public UpdateProfileViewModel UpdateProfileViewModel { get; set; }

        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
    }
}
