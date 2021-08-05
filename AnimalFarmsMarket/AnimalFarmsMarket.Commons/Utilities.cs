using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimalFarmsMarket.Commons
{
    public class Utilities
    {
        public static string ChangeToTitleCase(string str)
        {
            var covertedString = str.ToCharArray()[0].ToString().ToUpper() + str.Substring(1);
            return covertedString;
        }
        public static PageMetaData Paginate(int page, int per_page, int total)
        {
            int total_page = total % per_page == 0 ? total / per_page : total / per_page + 1;
            var result = new PageMetaData()
            {
                Page = page,
                PerPage = per_page,
                Total = total,
                TotalPages = total_page
            };
            return result;
        }

        public static ResponseDto<T> CreateResponse<T>(string message, 
            ModelStateDictionary errs, T data)
        {
            var errors = new Dictionary<string, string>();
            if(errs != null)
            {
                foreach (var err in errs)
                {
                    var counter = 0;
                    var key = err.Key;
                    var errVals = err.Value;
                    foreach (var errMsg in errVals.Errors)
                    {
                        errors.Add($"{(counter + 1)} - {key}", errMsg.ErrorMessage);
                        counter++;
                    }
                }
            }

            var obj = new ResponseDto<T>()
            {
                Message = message,
                Errs = errors,
                Data = data
            };
            return obj;
        }

        public static bool ValidNIN(string nin)
        {
            if (nin.Length == 11 && nin.All(char.IsDigit))
            {
                return true;
            }
            return false;
        }

        public class PictureFilter
        {
            public static string CheckPictureTypeAndSize(IFormFile picture)
            {
                var extensions = new List<string>() { ".jpg", ".jpeg", ".png" };
                String format = null;
                //check if picture is more than 1MB
                if (picture.Length > 1000000)
                {
                    format = "SizeError";
                    return format;
                }
                // check if picture has a valid extension
                foreach (var ext in extensions)
                {
                    if (picture.FileName.ToLower().EndsWith(ext))
                    {
                        format = "CorrectFormat";
                        break;
                    }
                }
                return format;
            }

           
        }
    }
}