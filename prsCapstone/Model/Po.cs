namespace prsCapstone.Model
{
    public class Po
    {
        public Vendor Vendor { get; set; }
        public IEnumerable<Poline> Polines { get; set; }
        public decimal PoTotal { get; set; }
        public Po(Vendor vendor, List<Poline> polines, decimal poTotal)
        {
            Vendor = vendor;
            Polines = polines;
            PoTotal = poTotal;
        }
    }
}
