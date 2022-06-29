namespace KGB_Dev_.Data.KGB_Model
{
    public class KGB_SubcategoryViewModel
    {
        public int Id { get; set; }
        public int Fk_Kategorija { get; set; }
        public string? Naziv_Potkategorije { get; set; }
    }
}
