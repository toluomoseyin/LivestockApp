using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class CarouselViewModel
    {
        public string StartText { get; set; }
        public string ColouredText { get; set; }
        public string EndText { get; set; }
        public string LearnMoreUrl { get; set; }
        public string JoinNow { get; set; }
        public string ImageUrl { get; set; }
        public string AltText { get; set; }
        public bool Tracked { get; set; } = false;
    }

    public class ListOfSlides
    {
        public List<CarouselViewModel> Slides { get; set; }

        public ListOfSlides()
        {
            Slides = new List<CarouselViewModel>{
                new CarouselViewModel
                {
                    StartText = "Order your ",
                    ColouredText = "Livestock",
                    EndText = " and Get Them Delivered at your Doorstep",
                    LearnMoreUrl = "#",
                    JoinNow = "#",
                    ImageUrl= "https://res.cloudinary.com/binaryreality/image/upload/v1623591698/LIvestock1_x6owwc.jpg",
                    AltText = "Alternate text"

                },
                new CarouselViewModel
                {
                    StartText = "Join The Largest Community ",
                    ColouredText = "With Quality Livestocks for slaughter, Rearing",
                    EndText = "  e.t.c",
                    LearnMoreUrl = "#",
                    JoinNow = "#",
                    ImageUrl= "https://res.cloudinary.com/binaryreality/image/upload/v1623591885/Livestock3_be9iye.jpg",
                    AltText = "Alternate text"

                },
                new CarouselViewModel
                {
                    StartText = "Order your ",
                    ColouredText = "Poultry",
                    EndText = " and Get Them Delivered at your Doorstep",
                    LearnMoreUrl = "#",
                    JoinNow = "#",
                    ImageUrl= "https://res.cloudinary.com/binaryreality/image/upload/v1623591698/Livestock2_sxz9t4.jpg",
                    AltText = "Alternate text"

                },
                new CarouselViewModel
                {
                    StartText = "Buy ",
                    ColouredText = "Fit-for-Slaughter",
                    EndText = " and Traceable Livestock in Nigeria.",
                    LearnMoreUrl = "#",
                    JoinNow = "#",
                    ImageUrl= "https://res.cloudinary.com/binaryreality/image/upload/v1623867443/LIvestock4_cgono9.jpg",
                    AltText = "Alternate text",
                    Tracked = true

                },
            };
        }
    }
}
