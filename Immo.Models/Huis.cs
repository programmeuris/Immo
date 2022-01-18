using System;
using System.Collections.Generic;
using System.Text;

namespace Immo.Models
{
    public class Huis : Eigendom
    {
        // constructor
        public Huis(
            int prijs,
            double oppervlakte,
            string adres,
            int bouwjaar,
            int bewoonbareOppervlakte) : base(
                prijs,
                oppervlakte,
                adres)
        {
            BewoonbareOppervlakte = bewoonbareOppervlakte;
            Bouwjaar = bouwjaar;
        }

        // public methods
        public override string Overzicht() => base.Overzicht() +
            $"{nameof(Bouwjaar)}: {Bouwjaar}\n" +
            $"Bewoonbare oppervlakte: {BewoonbareOppervlakte}\n";

        // in mijn geval moet ik deze niet overriden om de correcte output te bekomen
        // omdat ik in de base method met this.GetType().Name werk
        // public override string ToString() => throw new NotImplementedException();

        // public properties                
        public int BewoonbareOppervlakte
        {
            get => _bewoonbareOppervlakte;
            set => _bewoonbareOppervlakte = value;
        }

        public int Bouwjaar
        {
            get => _bouwjaar;
            set => _bouwjaar = value >= 0 ?
                value : throw new CustomException("Bouwjaar is kleiner dan 0");
        }

        // private fields
        private int _bewoonbareOppervlakte;
        private int _bouwjaar;
    }
}
