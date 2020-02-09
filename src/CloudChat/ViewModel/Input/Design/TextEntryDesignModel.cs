namespace CloudChat   
{
    public class TextEntryDesignModel:TextEntryViewModel
    {
        public static TextEntryDesignModel Instance = new TextEntryDesignModel();
        public TextEntryDesignModel()
        {
            Label = "Name";
            OriginalText = "Sayfiddinov Quvondiq";
            EditedText = "Sayfiddinov Quvondiq";
            Working = false;
            Editing = false;
        }
    }
}
