using Microsoft.AspNetCore.Http;

namespace Group4Flight.Models
{
    // Session wrapper — manages filter criteria and flight selection IDs in session
    public class FlightSession
    {
        private const string FilterKey    = "FlightFilter";
        private const string SelectionKey = "FlightSelections";

        private readonly ISession _session;

        public FlightSession(ISession session)
        {
            _session = session;
        }

        // --- Filter ---

        public FilterCriteria GetFilter()
            => _session.GetObject<FilterCriteria>(FilterKey) ?? new FilterCriteria();

        public void SetFilter(FilterCriteria criteria)
            => _session.SetObject(FilterKey, criteria);

        // --- Selections ---

        public List<int> GetSelections()
            => _session.GetObject<List<int>>(SelectionKey) ?? new List<int>();

        public void SetSelections(List<int> ids)
            => _session.SetObject(SelectionKey, ids);

        public bool AddSelection(int flightId)
        {
            var list = GetSelections();
            if (list.Contains(flightId)) return false;
            list.Add(flightId);
            SetSelections(list);
            return true;
        }

        public bool RemoveSelection(int flightId)
        {
            var list = GetSelections();
            if (!list.Contains(flightId)) return false;
            list.Remove(flightId);
            SetSelections(list);
            return true;
        }

        public void ClearSelections()
            => _session.SetObject(SelectionKey, new List<int>());

        public int SelectionCount => GetSelections().Count;

        // Seeds session from cookie on first visit to restore selections
        public void SeedFromCookie(List<int> cookieIds)
        {
            if (!_session.Keys.Contains(SelectionKey))
                SetSelections(cookieIds);
        }
    }

    // Holds filter state stored in session
    public class FilterCriteria
    {
        public string? From  { get; set; }
        public string? To    { get; set; }
        public DateTime? Date { get; set; }
        public string? Cabin { get; set; }
    }
}
