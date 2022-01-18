using System;
using System.Collections.Generic;
using System.Text;

namespace Immo.Models
{
    public class Appartement : Huis
    {
        // constructor
        public Appartement(
            int prijs,
            double oppervlakte,
            string adres,
            int bouwjaar,
            int bewoonbareOppervlakte) : base(
                prijs,
                oppervlakte,
                adres,
                bouwjaar,
                bewoonbareOppervlakte)
        {
            Huurprijs = (double)prijs;
        }

        // public methods
        public override string Overzicht() => base.Overzicht() +
            $"{nameof(Huurprijs)}: {Huurprijs:c2}\n";

        public override string ToString() =>
            $"{this.GetType().Name} - {nameof(Huurprijs)}: {Huurprijs:c2}";

        // public properties
        // not throwing an exception here because Prijs already triggers one in base class
        public double Huurprijs
        {
            get => _huurprijs;
            set => _huurprijs = value;
        }

        // private fields
        private double _huurprijs;
    }
}
