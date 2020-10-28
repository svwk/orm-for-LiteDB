namespace entitymodel
{
    public class dogc : entity
    {
        #region Fields

        private string _predmet  = "";
        private string _document = "";

        #endregion

        #region Constructors

        public dogc() => Init();
        public dogc(int newId) : base(newId) => Init();
        private void Init()
        { }

        #endregion

        #region Comparer



        #endregion

        #region Propeties

        public override EntityType Type => EntityType.dogc;
        public string Predmet
        {
            get => _predmet;
            set
            {
                value = conv.ClearString(value);
                if (_predmet != value && Changed == Status.not_changed) Changed = Status.edited;
                _predmet = value;
                OnPropertyChanged(Predmet);
            }
        }

        public string Document
        {
            get => _document;
            set
            {
                value = conv.ClearString(value);
                if (_document != value && Changed == Status.not_changed) Changed = Status.edited;
                _document = value;
                OnPropertyChanged(Document);
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Predmet;
        }

        #endregion

    }
}