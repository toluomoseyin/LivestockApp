using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnimalFarmsMarket.Data.Enum
{
    public enum Coverage
    {
        [Display(Name = "Within State")]
        WithinState,

        [Display(Name = "Outside State")]
        OutsideState
    }
}
