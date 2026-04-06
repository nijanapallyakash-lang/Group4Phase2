using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Group4Flight.Models
{
    // Cookie wrapper — persists flight selection IDs for 2 weeks
    public class FlightCookies
    {
        private const string CookieKey = "FlightSelections";

        private readonly HttpContext _context;

        public FlightCookies(HttpContext context)
        {
            _context = context;
        }

        public List<int> GetSelections()
        {
            var raw = _context.Request.Cookies[CookieKey];
            if (string.IsNullOrEmpty(raw)) return new List<int>();
            return JsonConvert.DeserializeObject<List<int>>(raw) ?? new List<int>();
        }

        public void SetSelections(List<int> ids)
        {
            _context.Response.Cookies.Append(CookieKey, JsonConvert.SerializeObject(ids),
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(14),
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax
                });
        }

        public void AddSelection(int flightId)
        {
            var list = GetSelections();
            if (!list.Contains(flightId)) list.Add(flightId);
            SetSelections(list);
        }

        public void RemoveSelection(int flightId)
        {
            var list = GetSelections();
            list.Remove(flightId);
            SetSelections(list);
        }

        public void ClearSelections() => SetSelections(new List<int>());
    }
}
