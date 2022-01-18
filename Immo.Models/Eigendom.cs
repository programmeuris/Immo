using System;

namespace Immo.Models
{
    public class Eigendom
    {
        // constructor
        public Eigendom(
            int prijs,
            double oppervlakte,
            string adres)
        {
            Prijs = prijs;
            Oppervlakte = oppervlakte;
            Adres = adres;
        }

        // public methods
        public virtual string Overzicht() =>
            $"{this.GetType().Name}\n" +
            $"{nameof(Oppervlakte)}: {Oppervlakte}\n" +
            $"{nameof(Adres)}: {Adres}\n" +
            $"{nameof(Prijs)}: {Prijs:c2}\n";

        // beetje raar dat prijs op 2 cijfers na de komma moet worden afgerond terwijl het een int moet zijn
        public override string ToString() => $"{this.GetType().Name} - {nameof(Prijs)}: {Prijs:c2}";

        public override bool Equals(object obj) => Adres == (obj as Eigendom).Adres;

        // public properties
        public string Adres
        {
            get => _adres;
            set => _adres = value;
        }

        public double Oppervlakte
        {
            get => _oppervlakte;
            set => _oppervlakte = value >= 0 ?
                value : throw new CustomException($"{nameof(Oppervlakte)} is kleiner dan 0");
        }

        public int Prijs
        {
            get => _prijs;
            set => _prijs = value >= 0 ?
                value : throw new CustomException($"{nameof(Prijs)} is kleiner dan 0");
        }

        // private fields
        private string _adres;
        private double _oppervlakte;
        private int _prijs;
    }
}
