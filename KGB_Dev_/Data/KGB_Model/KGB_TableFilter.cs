namespace KGB_Dev_.Data.KGB_Model
{
    public class KGB_TableFilter
    {
        public string? User { get; set; }
        public string? Category { get; set; }
        public int Fk_Category { get; set; }
        public int Fk_Subcategory { get; set; }

        public string? Subcategory { get; set; }
        public DateTime d_ins { get; set; }
        public DateTime d_upd { get; set; }
    }
}
