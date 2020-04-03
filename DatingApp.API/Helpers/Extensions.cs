using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddAppilcationError(this HttpResponse response, string message)
        {
            var mess = "Application-Error";
            response.Headers.Add(mess, message);
            response.Headers.Add("Access-Control-Expose-Headers", mess);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;
            return age;
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalpages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalpages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader,camelCaseFormatter));
             response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}